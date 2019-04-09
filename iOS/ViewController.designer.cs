// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace SignalR.iOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UITableView MessagesTable { get; set; }

		[Outlet]
		UIKit.UITextField MessageText { get; set; }

		[Outlet]
		UIKit.UIButton ReconnectButton { get; set; }

		[Outlet]
		UIKit.UIButton SendButton { get; set; }

		[Action ("ReconnectTouch:")]
		partial void ReconnectTouch (Foundation.NSObject sender);

		[Action ("SendTouched:")]
		partial void SendTouched (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (MessagesTable != null) {
				MessagesTable.Dispose ();
				MessagesTable = null;
			}

			if (MessageText != null) {
				MessageText.Dispose ();
				MessageText = null;
			}

			if (SendButton != null) {
				SendButton.Dispose ();
				SendButton = null;
			}

			if (ReconnectButton != null) {
				ReconnectButton.Dispose ();
				ReconnectButton = null;
			}
		}
	}
}
