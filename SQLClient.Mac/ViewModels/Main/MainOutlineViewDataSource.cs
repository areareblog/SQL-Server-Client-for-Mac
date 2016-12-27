using System;
using AppKit;
using Foundation;

namespace SQLClient.Mac
{
	public class MainOutlineViewDataSource : NSOutlineViewDataSource
	{
		MainTreeNode parentNode;
		public MainOutlineViewDataSource(MainTreeNode parentNode)
		{
			this.parentNode = parentNode;
		}

		public override nint GetChildrenCount(NSOutlineView outlineView, NSObject item)
		{
			item = item == null ? parentNode : item;
			return ((MainTreeNode)item).ChildCount;
		}

		public override NSObject GetChild(NSOutlineView outlineView, nint childIndex, NSObject item)
		{
			item = item == null ? parentNode : item;
			return ((MainTreeNode)item).GetChild((int)childIndex);
		}

		public override bool ItemExpandable(NSOutlineView outlineView, NSObject item)
		{
			item = item == null ? parentNode : item;
			return !((MainTreeNode)item).IsLeaf;
		}
	}
}
