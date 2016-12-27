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
	[Register ("LoginViewController")]
	partial class LoginViewController
	{
		[Outlet]
		AppKit.NSButton connectButton { get; set; }

		[Outlet]
		AppKit.NSTextField login { get; set; }

		[Outlet]
		AppKit.NSTextField messageText { get; set; }

		[Outlet]
		AppKit.NSSecureTextField password { get; set; }

		[Outlet]
		AppKit.NSTextField server { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (connectButton != null) {
				connectButton.Dispose ();
				connectButton = null;
			}

			if (login != null) {
				login.Dispose ();
				login = null;
			}

			if (password != null) {
				password.Dispose ();
				password = null;
			}

			if (server != null) {
				server.Dispose ();
				server = null;
			}

			if (messageText != null) {
				messageText.Dispose ();
				messageText = null;
			}
		}
	}
}
