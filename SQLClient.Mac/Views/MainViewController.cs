using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

using System.Data.SqlClient;
using CoreGraphics;

namespace SQLClient.Mac
{
	public partial class MainViewController : AppKit.NSViewController
	{
		#region Constructors

		// Called when created from unmanaged code
		public MainViewController(IntPtr handle) : base(handle)
		{
			Initialize();
		}

		// Called when created directly from a XIB file
		[Export("initWithCoder:")]
		public MainViewController(NSCoder coder) : base(coder)
		{
			Initialize();
		}

		// Call to load from the XIB/NIB file
		public MainViewController() : base("MainView", NSBundle.MainBundle)
		{
			Initialize();
		}

		// Shared initialization code
		void Initialize()
		{
		}

		#endregion

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			SetConnectButtonsEnabled(false);

			connectButton.Activated += (sender, e) => {

				var login = new LoginViewController();
				login.ConnectionSucceeded += Login_ConnectionSucceeded;

				PresentViewControllerAsModalWindow(login);
			};

			disconnectButton.Activated += (sender, e) => {
				AppDelegate.Model.CloseConnection();
				SetConnectButtonsEnabled(AppDelegate.Model.HasConnection);
				serverTree.DataSource = null;
				serverTree.ReloadData();
				ClearContensView();
			};

		}

		private void SetConnectButtonsEnabled(bool connected)
		{
			connectButton.Enabled = !connected;
			disconnectButton.Enabled = connected;
		}

		void Login_ConnectionSucceeded(object sender, EventArgs e)
		{
			SetConnectButtonsEnabled(AppDelegate.Model.HasConnection);

			var outLineDelegate = new MainOutlineViewDelegate();
			outLineDelegate.ItemSelected += OutLineDelegate_ItemSelected;;
			serverTree.Delegate = outLineDelegate;

			var sql = "select @@servername";
			var serverName = "";
			using (var command = new SqlCommand(sql, AppDelegate.Model.Connection))
			{
				serverName = command.ExecuteScalar() as string;
			}

			var rootNode = new MainTreeNode("Root");
			var serverNode = new MainTreeNode(serverName);


			var systemDbNames = new List<string> { "master", "tempdb", "model", "msdb" };
			var dbNames = new List<string>();

			sql = @"SELECT name, database_id, create_date FROM sys.databases";

			using (var command = new SqlCommand(sql, AppDelegate.Model.Connection))
			{
				var reader = command.ExecuteReader();

				while (reader.Read())
				{
					dbNames.Add(reader[0] as string);
				}
			}


			var dbFolderNode = new MainTreeNode("データベース") { NodeType = MainTreeNodeType.Folder };
			var sysFolderDbNode = new MainTreeNode("システムデータベース") { NodeType = MainTreeNodeType.Folder };
			dbFolderNode.Children.Add(sysFolderDbNode);

			foreach (var dbName in dbNames)
			{
				var dbNode = new MainTreeNode(dbName) { NodeType = MainTreeNodeType.Database };

				var tableFolderNode = new MainTreeNode("テーブル") {
					NodeType = MainTreeNodeType.Folder
				};
				AppDelegate.Model.Connection.ChangeDatabase(dbName);

				sql = "SELECT * FROM Sys.Tables";
				using (var command = new SqlCommand(sql, AppDelegate.Model.Connection))
				{
					var reader = command.ExecuteReader();

					while (reader.Read())
					{

						var tableName = reader[0] as string;
						tableFolderNode.Children.Add(new MainTreeNode(tableName) { 
							NodeType = MainTreeNodeType.Table,
							Information = dbName 
						});
					}
				}
				dbNode.Children.Add(tableFolderNode);


				if (systemDbNames.Contains(dbName))
				{
					sysFolderDbNode.Children.Add(dbNode);
				}
				else
				{
					dbFolderNode.Children.Add(dbNode);
				}
			}

			serverNode.Children.Add(dbFolderNode);

			rootNode.Children.Add(serverNode);
			serverTree.DataSource = new MainOutlineViewDataSource(rootNode);
		}

		void OutLineDelegate_ItemSelected(object sender, EventArgs e)
		{
			var node = sender as MainTreeNode;

			if (node.NodeType == MainTreeNodeType.Table)
			{
				var dbName = node.Information as string;
				var tableName = node.Data;

				var tableViewCtrl = new TableDefinitionViewController();
				var tableView = tableViewCtrl.View;
				tableView.TranslatesAutoresizingMaskIntoConstraints = false;

				contentsView.AddSubview(tableView);
				contentsView.AddConstraint(NSLayoutConstraint.Create(tableView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, contentsView, NSLayoutAttribute.Width, 1, 0));
				contentsView.AddConstraint(NSLayoutConstraint.Create(tableView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, contentsView, NSLayoutAttribute.Height, 1, 0));

				var items = GetTableDefinition(dbName, tableName);
				tableViewCtrl.TableView.DataSource = new TableDefinitionDataSource(items.Count);
				tableViewCtrl.TableView.Delegate = new TableDefinitionDelegate(items);

				var button = new NSButton();
				button.Title = "データを表示する";
				button.Activated += delegate
				{
					ShowTableData(dbName, tableName);
				};
				button.Frame = new CGRect(22, 22, 100, 44);
				contentsView.AddSubview(button);

				return;
			}

			ClearContensView();
		}

		private void ShowTableData(string dbName, string tableName)
		{
			var tableDataCtrl = new TableDataViewController()
			{
				Title = tableName,
				DbName = dbName,
				TableName = tableName,
				TableDefinitionItems = GetTableDefinition(dbName, tableName)					
			};

			PresentViewControllerAsModalWindow(tableDataCtrl);

		}

		private void ClearContensView()
		{
			foreach (var v in contentsView.Subviews)
			{
				v.RemoveFromSuperview();
			}
		}

		private List<TableDefinitionItem> GetTableDefinition(string dbName, string tableName)
		{
			var items = new List<TableDefinitionItem>();

			var sqlToGetDefinition = $@"
SELECT 
	c.column_id 
	,c.name AS column_name  
    ,t.name AS type_name
	,c.max_length  
    ,c.scale  
	,c.is_nullable
FROM sys.columns AS c   
JOIN sys.types AS t ON c.user_type_id=t.user_type_id  
WHERE c.object_id = OBJECT_ID('{tableName}')  
ORDER BY c.column_id;   			
";

			var sqlToPrimaryKey = $@"
SELECT ic.index_column_id
    ,c.name AS column_name 
    ,i.name AS index_name 
FROM sys.indexes AS i  
INNER JOIN sys.index_columns AS ic   
    ON i.object_id = ic.object_id AND i.index_id = ic.index_id  
INNER JOIN sys.columns AS c   
    ON ic.object_id = c.object_id AND c.column_id = ic.column_id  
WHERE i.is_primary_key = 1   
    AND i.object_id = OBJECT_ID('{tableName}');
";
			AppDelegate.Model.Connection.ChangeDatabase(dbName);
			using (var command = new SqlCommand(sqlToGetDefinition, AppDelegate.Model.Connection))
			{
				var reader = command.ExecuteReader();

				while (reader.Read())
				{
					var item = new TableDefinitionItem();
					item.ColumnName = reader[1] as string;
					item.DataType = reader[2] as string;
					item.IsNullable = (bool)reader[5];

					if (item.DataType.Contains("char"))
					{
						item.DataType += $"({reader[3]})";
					}
					else if (item.DataType.Contains("datetime"))
					{
						item.DataType += $"({reader[4]})";
					}

					items.Add(item);
				}
				
			}

			using (var command = new SqlCommand(sqlToPrimaryKey, AppDelegate.Model.Connection))
			{
				var pKeyCols = new List<string>();
				var reader = command.ExecuteReader();

				while (reader.Read())
				{
					pKeyCols.Add(reader[1] as string);
				}

				for (var i = 0; i < items.Count; i++)
				{
					if (pKeyCols.Contains(items[i].ColumnName))
					{
						items[i].IsPrimaryKey = true;
					}
				}
			}

			return items;
		}

	}
}
