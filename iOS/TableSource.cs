using System;
using System.Collections.ObjectModel;
using Foundation;  
using UIKit;  
namespace SignalR.iOS
{
    public class TableSource : UITableViewSource
    {
        ObservableCollection<MessageData> list;
        public TableSource() { }
        public TableSource(ObservableCollection<MessageData> list)
        {
            this.list = list;
        }
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = new UITableViewCell(UITableViewCellStyle.Default, "");
            var data = list[indexPath.Row];
            string item = $"{data.User}: {data.Message}";
            cell.TextLabel.Text = item;
            return cell;
        }
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return list.Count;
        }
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var data = list[indexPath.Row];
            string item = $"{data.User}: {data.Message}";
            UIAlertView alert = new UIAlertView("Selected Item", item, null, "OK");
            alert.Show();
        }
    }
}
