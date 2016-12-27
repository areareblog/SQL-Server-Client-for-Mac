using System;
using System.Collections.Generic;
using AppKit;
using CoreGraphics;
using Foundation;

namespace SQLClient.Mac
{
	public class TableDefinitionDataSource : NSTableViewDataSource
	{
		private int itemCount;

		public TableDefinitionDataSource(int itemCount)
		{
			this.itemCount = itemCount;
		}

		public override nint GetRowCount(NSTableView tableView)
		{
			return itemCount;
		}
	}
}
