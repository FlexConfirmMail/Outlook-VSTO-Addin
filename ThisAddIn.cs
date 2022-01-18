using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace FlexConfirmMail
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            this.Application.ItemSend += new Microsoft.Office.Interop.Outlook.ApplicationEvents_11_ItemSendEventHandler(ThisAddIn_ItemSend);
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        private void ThisAddIn_ItemSend(object Item, ref bool Cancel)
        {
            Outlook.MailItem mail = (Outlook.MailItem)Item;
            Cancel = true;

            try
            {
                Config config = new Config();
                config.LoadFileSystem();

                MainDialog mainDialog = new MainDialog();
                mainDialog.LoadMail(mail, config);
                if (mainDialog.ShowDialog() == true)
                {
                    CountDialog countDialog = new CountDialog();
                    if (countDialog.ShowDialog() == DialogResult.OK)
                    {
#if !DEBUG
                        Cancel = false;
#endif
                    }
                }
            }
            catch { }
        }
        #region VSTO で生成されたコード

        /// <summary>
        /// デザイナーのサポートに必要なメソッドです。
        /// このメソッドの内容をコード エディターで変更しないでください。
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }
}
