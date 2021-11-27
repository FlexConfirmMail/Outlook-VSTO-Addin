using System.Windows.Forms;

namespace CheckMyMail
{
    public class CustomListView : ListView
    {
        public void EnableDoubleBuffering()
        {
            this.DoubleBuffered = true;
        }
    }
}
