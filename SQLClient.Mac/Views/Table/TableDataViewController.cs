using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

using System.Data.SqlClient;

namespace SQLClient.Mac
{
	public partial class TableDataViewController : AppKit.NSViewController
	{
		#region Constructors

		// Called when created from unmanaged code
		public TableDataViewController(IntPtr handle) : base(handle)
		{
			Initialize();
		}

		// Called when created directly from a XIB file
		[Export("initWithCoder:")]
		public TableDataViewController(NSCoder coder) : base(coder)
		{
			Initialize();
		}

		// Call to load from the XIB/NIB file
		public TableDataViewController() : base("TableDataView", NSBundle.MainBundle)
		{
			Initialize();
		}

		// Shared initialization code
		void Initialize()
		{
		}

		#endregion

		//strongly typed view accessor
		public new TableDataView View
		{
			get
			{
				return (TableDataView)base.View;
			}
		}

		public string DbName
		{
			get;
			set;
		}
		public string TableName
		{
			get;
			set;
		}
		public List<TableDefinitionItem> TableDefinitionItems
		{
			get;
			set;
		}


		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			SetUpTableColumns();
			var data = Query();

			this.tableView.DataSource = new TableDataSource(data.Count);
			this.tableView.Delegate = new TableDataDelegate(data);
		}

		private void SetUpTableColumns()
		{
			for (var i = 0; i < TableDefinitionItems.Count; i++)
			{
				var item = TableDefinitionItems[i];
				this.tableView.AddColumn(new NSTableColumn($"{i}") { Title = item.ColumnName });
			}
		}

		private string CreateSelectSql()
		{
			var sql = "SELECT TOP 200 ";

			for (var i = 0; i < TableDefinitionItems.Count; i++)
			{
				if (i != 0)
				{
					sql += ",";
				}

				sql += $"{TableDefinitionItems[i].ColumnName}";

			}

			sql += $" FROM {TableName}";

			return sql;
		}

		private List<List<object>> Query()
		{
			var sql = CreateSelectSql();
			var data = new List<List<object>>();

			AppDelegate.Model.Connection.ChangeDatabase(DbName);
			using (var command = new SqlCommand(sql, AppDelegate.Model.Connection))
			{
				var reader = command.ExecuteReader();

				while (reader.Read())
				{
					var row = new List<object>();

					for (var i = 0; i < TableDefinitionItems.Count; i++)
					{
						row.Add(reader[i]);
					}

					data.Add(row);
				}
			}

			return data;
		}
	}

	class TableDataDelegate : NSTableViewDelegate
	{
		public TableDataDelegate(List<List<object>> data)
		{
			this.Data = data;
		}

		public List<List<object>> Data
		{
			get;
			set;
		}

		const string identifer = "myCellIdentifier";

		public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
		{
			NSTextField view = (NSTextField)tableView.MakeView(identifer, this);
			if (view == null)
			{
				view = new NSTextField()
				{
					Identifier = identifer,
					Bordered = false,
					Selectable = false,
					Editable = false,
					BackgroundColor = NSColor.Clear
				};
			}

			var colIndex = int.Parse(tableColumn.Identifier);
			var item = Data[(int)row];
			view.StringValue = $"{item[colIndex]}";

			return view;
		}
	}

	public class TableDataSource : NSTableViewDataSource
	{
		public TableDataSource(int itemCount)
		{
			this.itemCount = itemCount;
		}

		public int itemCount
		{
			get;
			set;
		}

		public override nint GetRowCount(NSTableView tableView)
		{
			return itemCount;
		}
	}
}
