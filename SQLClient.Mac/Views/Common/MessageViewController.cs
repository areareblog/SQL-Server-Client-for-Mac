using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace SQLClient.Mac
{
	public partial class MessageViewController : AppKit.NSViewController
	{
		#region Constructors

		// Called when created from unmanaged code
		public MessageViewController(IntPtr handle) : base(handle)
		{
			Initialize();
		}

		// Called when created directly from a XIB file
		[Export("initWithCoder:")]
		public MessageViewController(NSCoder coder) : base(coder)
		{
			Initialize();
		}

		// Call to load from the XIB/NIB file
		public MessageViewController() : base("MessageView", NSBundle.MainBundle)
		{
			Initialize();
		}

		// Shared initialization code
		void Initialize()
		{
		}

		#endregion

		//strongly typed view accessor
		public new MessageView View
		{
			get
			{
				return (MessageView)base.View;
			}
		}

		public string MessageTitle { set; get; }
		public string Message { set; get; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			this.titleText.StringValue = this.MessageTitle;
			this.messageText.StringValue = this.Message;

			this.okButton.Activated += (sender, e) => {
				this.View.Window.Close();
			};
		}
	}
}
