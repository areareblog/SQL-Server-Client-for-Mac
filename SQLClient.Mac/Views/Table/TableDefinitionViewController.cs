using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace SQLClient.Mac
{
	public partial class TableDefinitionViewController : AppKit.NSViewController
	{
		#region Constructors

		// Called when created from unmanaged code
		public TableDefinitionViewController(IntPtr handle) : base(handle)
		{
			Initialize();
		}

		// Called when created directly from a XIB file
		[Export("initWithCoder:")]
		public TableDefinitionViewController(NSCoder coder) : base(coder)
		{
			Initialize();
		}

		// Call to load from the XIB/NIB file
		public TableDefinitionViewController() : base("TableDefinitionView", NSBundle.MainBundle)
		{
			Initialize();
		}

		// Shared initialization code
		void Initialize()
		{
		}

		#endregion

		//strongly typed view accessor
		public new TableDefinitionView View
		{
			get
			{
				return (TableDefinitionView)base.View;
			}
		}

		public NSTableView TableView{

			get { return tableView;}
		}
	}
}
