using System;
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace SignalR.iOS
{
    public partial class ViewController : UIViewController
    {
        int count = 1;
        ChatModel _model;
        TableSource _source;
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _model = new ChatModel();
            _model.Messages.CollectionChanged += Messages_CollectionChanged;
            _model.Connect();
            _source = new TableSource(_model.Messages);
            MessagesTable.Source = _source;
            // Perform any additional setup after loading the view, typically from a nib.
        }

        void Messages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(
                () =>
                {
                    MessagesTable.ReloadData();
                });
        }

        partial void SendTouched(NSObject sender)
        {
            _model.Message = MessageText.Text;
            _model.SendMessage();
        }

        partial void ReconnectTouch(NSObject sender)
        {
            _model.Connect();
        }

        public override void ViewDidUnload()
        {
            _model.Disconnect();
            base.ViewDidUnload();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.		
        }
    }
}
