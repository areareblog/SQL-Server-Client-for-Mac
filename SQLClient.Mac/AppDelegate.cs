using AppKit;
using Foundation;

namespace SQLClient.Mac
{
	[Register("AppDelegate")]
	public class AppDelegate : NSApplicationDelegate
	{

		public static readonly AppModel Model = new AppModel();

		public AppDelegate()
		{
		}

		public override void DidFinishLaunching(NSNotification notification)
		{
			// Insert code here to initialize your application
		}

		public override void WillTerminate(NSNotification notification)
		{
			// Insert code here to tear down your application
			Model.CloseConnection();
		}
	}
}
