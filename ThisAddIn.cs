using System.Windows.Forms;
using FlexConfirmMail.Dialog;
using Outlook = Microsoft.Office.Interop.Outlook;
using FlexConfirmMail.Config;

namespace FlexConfirmMail
{
    public partial class ThisAddIn
    {

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            this.Application.ItemSend += new Microsoft.Office.Interop.Outlook.ApplicationEvents_11_ItemSendEventHandler(ThisAddIn_ItemSend);
            _banner();
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        private void ThisAddIn_ItemSend(object Item, ref bool Cancel)
        {
            Outlook.MailItem mail = (Outlook.MailItem)Item;

            // First of all, enable Cancel flag. This makes Outlook
            // NOT to send the mail even if something goes awry.
            Cancel = true;

            try
            {
                QueueLogger.Log("Start handling ItemSend");

                ConfigData config = new ConfigData();
                _doLoadConfig(config);

                MainDialog mainDialog = new MainDialog(config);
                mainDialog.LoadMail(mail);
                if (mainDialog.ShowDialog() == true)
                {
                    if (config.GetBool("CountEnabled"))
                    {
                        CountDialog countDialog = new CountDialog(config);
                        if (countDialog.ShowDialog() == true)
                        {
                            Cancel = false;
                        }
                    }
                    else
                    {
                        Cancel = false;
                    }
                }
            }
            catch (System.Exception e)
            {
                QueueLogger.Log(e);
            }
            // Never send a message with a debug build!
#if DEBUG
            Cancel = true;
#endif
        }

        private void _banner()
        {
            QueueLogger.Log($"Start {Global.AppName} {Global.Version}");
            QueueLogger.Log($" - Outlook {Application.Version}");
            QueueLogger.Log($" - {System.Environment.OSVersion.VersionString}");
        }

        private bool _doLoadConfig(ConfigData config)
        {
            FileLoader loader = new FileLoader(config);
            loader.TryOptionFile(StandardPath.GetUserDir(), ConfigFile.Common);
            loader.TryListFile(StandardPath.GetUserDir(), ConfigFile.TrustedDomains);
            loader.TryListFile(StandardPath.GetUserDir(), ConfigFile.UnsafeDomains);
            loader.TryListFile(StandardPath.GetUserDir(), ConfigFile.UnsafeFiles);
            return true;
        }

        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            return new Ribbon();
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
