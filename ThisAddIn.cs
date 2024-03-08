using FlexConfirmMail.Dialog;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Interop;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Collections.Generic;

namespace FlexConfirmMail
{
    public partial class ThisAddIn
    {
        private Outlook.Explorers Explorers { get; set; } = null;
        private List<Outlook.Explorer> ExplorerList { get; set; } = new List<Outlook.Explorer>();
        private Outlook.Inspectors Inspectors { get; set; } = null;
        private Dictionary<string, Outlook.MailItem> SelectedMailDictionary { get; set; } = new Dictionary<string, Outlook.MailItem>();
        private Dictionary<string, List<RecipientInfo>> EntryIdToOriginalRecipientsDictionary { get; set; } = new Dictionary<string, List<RecipientInfo>>();

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            Application.ItemSend += new Outlook.ApplicationEvents_11_ItemSendEventHandler(ThisAddIn_ItemSend);
            Explorers = Application.Explorers;
            foreach (Outlook.Explorer explorer in Explorers)
            {
                explorer.SelectionChange += new Outlook.ExplorerEvents_10_SelectionChangeEventHandler(ThisAddIn_SelectionChange);
                ExplorerList.Add(explorer);
            }
            Explorers.NewExplorer += new Outlook.ExplorersEvents_NewExplorerEventHandler(ThisAddIn_NewExplorer);
            Inspectors = Application.Inspectors;
            Inspectors.NewInspector += new Outlook.InspectorsEvents_NewInspectorEventHandler(ThisAddIn_NewInspector);
            ShowBanner();
        }

        private void ThisAddIn_NewExplorer(Outlook.Explorer explorer)
        {
            explorer.SelectionChange += new Outlook.ExplorerEvents_10_SelectionChangeEventHandler(ThisAddIn_SelectionChange);
            ExplorerList.Add(explorer);
        }

        private void SaveOriginalRecipients(object response, ref bool cancel)
        {
            Outlook.MailItem mailItem = response as Outlook.MailItem;
            // EntryID is created when saving.
            mailItem.Save();
            var entryID = mailItem.EntryID;
            if (string.IsNullOrEmpty(entryID))
            {
                return;
            }
            var originalRecipients = new List<RecipientInfo>();
            foreach (Outlook.Recipient recp in mailItem.Recipients)
            {
                originalRecipients.Add(new RecipientInfo(recp));
            }
            EntryIdToOriginalRecipientsDictionary[entryID] = originalRecipients;
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        private void ThisAddIn_NewInspector(Outlook.Inspector inspector)
        {
            object item = inspector.CurrentItem;
            if (item is Outlook.MailItem mailItem)
            {
                SaveToMailCacheDictionary(mailItem);
            }
        }

        private void ThisAddIn_SelectionChange()
        {
            Outlook.Explorer acriveExplorer = Application.ActiveExplorer();
            if (acriveExplorer.Selection.Count > 0)
            {
                object item = acriveExplorer.Selection[1];
                if (item is Outlook.MailItem mailItem)
                {
                    SaveToMailCacheDictionary(mailItem);
                }
            }
        }

        private void SaveToMailCacheDictionary(Outlook.MailItem mailItem)
        {
            if (mailItem.Sender == null)
            {
                //NewMail, can't reply
                return;
            }
            var id = mailItem.EntryID;
            if (SelectedMailDictionary.ContainsKey(id))
            {
                return;
            }
            Outlook.ItemEvents_10_Event mailEvent = mailItem;

            mailEvent.Reply += new Outlook.ItemEvents_10_ReplyEventHandler(SaveOriginalRecipients);
            mailEvent.ReplyAll += new Outlook.ItemEvents_10_ReplyAllEventHandler(SaveOriginalRecipients);
            SelectedMailDictionary[id] = mailItem;
        }

        private void ThisAddIn_ItemSend(object Item, ref bool Cancel)
        {
            Outlook.MailItem mail = (Outlook.MailItem)Item;

            // Some users reported that Intel Graphic + Win10 causes
            // a blank screen. Diable Hardware Accerelation.
            RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;

            // First of all, enable Cancel flag. This makes Outlook
            // NOT to send the mail even if something goes awry.
            Cancel = true;

            try
            {
                if (DoCheck(mail))
                {
                    Cancel = false;
                    QueueLogger.Log("Check finished [send=yes]");
                }
                else
                {
                    QueueLogger.Log("Check finished [send=no]");
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

        private void ShowBanner()
        {
            try
            {
                QueueLogger.Log($"Start {Global.AppName}");
                QueueLogger.Log($"* Version {Global.Version} {Global.Edition}");
                QueueLogger.Log($"* Outlook {Application.Version}");
                QueueLogger.Log($"* {System.Environment.OSVersion.VersionString}");
            }
            catch (System.Exception e)
            {
                QueueLogger.Log(e);
            }
        }

        private bool DoCheck(Outlook.MailItem mail)
        {
            QueueLogger.Log($"===== Start DoCheck() ======");

            Config config = new Config();

            if (Global.EnableGPO)
            {
                config.Merge(Loader.LoadFromReg(RegistryPath.DefaultPolicy));
            }
            config.Merge(Loader.LoadFromDir(StandardPath.GetUserDir()));

            List<RecipientInfo> originalRecipients = GetOriginalRecipientsFromDictionary(mail.EntryID);

            MainDialog mainDialog = new MainDialog(config, mail, originalRecipients);
            if (mainDialog.SkipConfirm())
            {
                RemoveRecipientsFromDictionary(mail.EntryID);
                return true;
            }

            if (mainDialog.ShowDialog() == true)
            {
                if (!config.CountEnabled)
                {
                    RemoveRecipientsFromDictionary(mail.EntryID);
                    return true;
                }

                if (new CountDialog(config).ShowDialog() == true)
                {
                    RemoveRecipientsFromDictionary(mail.EntryID);
                    return true;
                }
            }
            return false;
        }

        private List<RecipientInfo> GetOriginalRecipientsFromDictionary(string entryID)
        {
            if (string.IsNullOrEmpty(entryID))
            {
                return null;
            }
            return EntryIdToOriginalRecipientsDictionary.ContainsKey(entryID) ? EntryIdToOriginalRecipientsDictionary[entryID] : null;
        }

        private void RemoveRecipientsFromDictionary(string entryID)
        {
            if (string.IsNullOrEmpty(entryID))
            {
                return;
            }
            EntryIdToOriginalRecipientsDictionary.Remove(entryID);
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
