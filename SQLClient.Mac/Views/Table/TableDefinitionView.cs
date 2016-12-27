using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace SQLClient.Mac
{
	public partial class TableDefinitionView : AppKit.NSView
	{
		#region Constructors

		// Called when created from unmanaged code
		public TableDefinitionView(IntPtr handle) : base(handle)
		{
			Initialize();
		}

		// Called when created directly from a XIB file
		[Export("initWithCoder:")]
		public TableDefinitionView(NSCoder coder) : base(coder)
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
