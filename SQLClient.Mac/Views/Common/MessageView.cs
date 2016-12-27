using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace SQLClient.Mac
{
	public partial class MessageView : AppKit.NSView
	{
		#region Constructors

		// Called when created from unmanaged code
		public MessageView(IntPtr handle) : base(handle)
		{
			Initialize();
		}

		// Called when created directly from a XIB file
		[Export("initWithCoder:")]
		public MessageView(NSCoder coder) : base(coder)
		{
			Initialize();
		}

		// Shared initialization code
		void Initialize()
		{
		}

		#endregion
	}
}
