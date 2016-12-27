using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace SQLClient.Mac
{
	public partial class LoginViewController : AppKit.NSViewController
	{
		#region Constructors

		// Called when created from unmanaged code
		public LoginViewController(IntPtr handle) : base(handle)
		{
			Initialize();
		}

		// Called when created directly from a XIB file
		[Export("initWithCoder:")]
		public LoginViewController(NSCoder coder) : base(coder)
		{
			Initialize();
		}

		// Call to load from the XIB/NIB file
		public LoginViewController() : base("LoginView", NSBundle.MainBundle)
		{
			Initialize();
		}

		// Shared initialization code
		void Initialize()
		{
		}

		#endregion

		public LoginModel Model = new LoginModel();

		//strongly typed view accessor
		public new LoginView View
		{
			get
			{
				return (LoginView)base.View;
			}
		}

		public event EventHandler ConnectionSucceeded;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			this.Title = "Login";

			this.connectButton.Activated += (sender, e) => {

				var s = server.StringValue;
				var l = login.StringValue;
				var p = password.StringValue;

				try
				{
					AppDelegate.Model.Connection = Model.Login(s, l, p);

					if (ConnectionSucceeded != null)
					{
						ConnectionSucceeded(sender, e);
					}

					this.View.Window.Close();
				}
				catch (Exception ex)
				{
					var message = ex.Message;

					if (ex.InnerException != null)
					{
						message += $"\n{ex.InnerException.Message}";
					}

					this.messageText.StringValue = message;
				}
			};

			this.connectButton.Enabled = false;

			this.server.Changed += TextField_Changed;
			this.login.Changed += TextField_Changed;
			this.password.Changed += TextField_Changed;
		}


		void TextField_Changed(object sender, EventArgs e)
		{
			this.connectButton.Enabled =
				server.StringValue.Length > 0
				&& login.StringValue.Length > 0
				&& password.StringValue.Length > 0;

			this.messageText.StringValue = string.Empty;
		}

	}
}
