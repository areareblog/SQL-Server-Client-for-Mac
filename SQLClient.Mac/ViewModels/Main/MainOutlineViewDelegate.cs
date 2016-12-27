using System;
using AppKit;
using Foundation;


namespace SQLClient.Mac
{
	public class MainOutlineViewDelegate : NSOutlineViewDelegate
	{
		const string identifer = "myCellIdentifier";
		public override NSView GetView(NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
		{
			NSTextField view = (NSTextField)outlineView.MakeView(identifer, this);
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

			view.StringValue = ((MainTreeNode)item).Data;
			return view;
		}

		public override bool ShouldSelectItem(NSOutlineView outlineView, NSObject item)
		{

			if (ItemSelected != null)
			{
				ItemSelected(item, null);
			}

			return true;
		}

		public event EventHandler ItemSelected;
	}
}
