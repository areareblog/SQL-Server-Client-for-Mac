// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace SQLClient.Mac
{
	[Register ("MainViewController")]
	partial class MainViewController
	{
		[Outlet]
		AppKit.NSButton connectButton { get; set; }

		[Outlet]
		AppKit.NSView contentsView { get; set; }

		[Outlet]
		AppKit.NSButton disconnectButton { get; set; }

		[Outlet]
		AppKit.NSOutlineView serverTree { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (connectButton != null) {
				connectButton.Dispose ();
				connectButton = null;
			}

			if (disconnectButton != null) {
				disconnectButton.Dispose ();
				disconnectButton = null;
			}

			if (serverTree != null) {
				serverTree.Dispose ();
				serverTree = null;
			}

			if (contentsView != null) {
				contentsView.Dispose ();
				contentsView = null;
			}
		}
	}
}
