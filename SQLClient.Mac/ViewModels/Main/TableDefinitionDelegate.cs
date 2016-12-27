using System;
using System.Collections.Generic;
using AppKit;
using CoreGraphics;
using Foundation;

namespace SQLClient.Mac
{
	public class TableDefinitionDelegate : NSTableViewDelegate
	{
		private List<TableDefinitionItem> items;

		public TableDefinitionDelegate(List<TableDefinitionItem> items)
		{
			this.items = items;
		}

		const string identifer = "myCellIdentifier";

		public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
		{
			var view = (NSTextField)tableView.MakeView(identifer, this);
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


			var splited = tableColumn.Identifier.Split('.');
			var colIndex = int.Parse(splited[splited.Length-1]);

			var item = items[(int)row];

			view.StringValue = "";

			switch (colIndex)
			{
				case 0:
					if(item.IsPrimaryKey)
						view.StringValue = "*";
					break;
				case 1:
					view.StringValue = item.ColumnName;
					break;
				case 2:
					view.StringValue = item.DataType;
					break;
				case 3:
					if (item.IsNullable)
						view.StringValue = "*";
					break;

			}

			return view;
		}
	}
}
