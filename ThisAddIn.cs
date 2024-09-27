using FlexConfirmMail.Dialog;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Interop;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Collections.Generic;
using System;

namespace FlexConfirmMail
{
    public partial class ThisAddIn
    {
        private Outlook.Explorers Explorers { get; set; } = null;
        private List<Outlook.Explorer> ExplorerList { get; set; } = new List<Outlook.Explorer>();
        private Outlook.Inspectors Inspectors { get; set; } = null;
        private Dictionary<string, Outlook.MailItem> SelectedMailDictionary { get; set; } = new Dictionary<string, Outlook.MailItem>();
        private Dictionary<string, List<RecipientInfo>> EntryIdToOriginalRecipientsDictionary { get; set; } = new Dictionary<string, List<RecipientInfo>>();
        private string OutBoxFolderPath { get; set; } = null;


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
            try
            {
                OutBoxFolderPath = Application.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderOutbox).FolderPath;
            }
            catch
            {
                // If the target folder type does not exist, Outlook raises an error.
                // It is a possible situation, so ignore it.
            }
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

            var mailID = GenerateMailID(mailItem);
            if (string.IsNullOrEmpty(mailID))
            {
                return;
            }
            var originalRecipients = new List<RecipientInfo>();
            foreach (Outlook.Recipient recp in mailItem.Recipients)
            {
                originalRecipients.Add(new RecipientInfo(recp));
            }
            EntryIdToOriginalRecipientsDictionary[mailID] = originalRecipients;
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
            if (!string.IsNullOrEmpty(OutBoxFolderPath) &&
                StringComparer.InvariantCultureIgnoreCase.Equals(acriveExplorer.CurrentFolder.FolderPath, OutBoxFolderPath))
            {
                // When deferred delivery is enabled, a mail is moved to the outbox and delivered after spending the delay time.
                // If we access any of properties of the mail in the outbox, the mail is regarded as "edited" and not delivered
                // even after spending the delay time.
                // So if the folder is the outbox, return before accessing properties of the mail.
                // Note that if a mail in the outbox is opened with a new inspector, it is regarded as "edited", which is Outlook's
                // specification, so we don't add this check for ThisAddIn_NewInspector.
                return;
            }
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
            config.Merge(Loader.LoadFromDir(StandardPath.GetDefaultConfigDir()));
            config.Merge(Loader.LoadFromDir(StandardPath.GetUserDir()));
            var mailID = GenerateMailID(mail);
            List<RecipientInfo> originalRecipients = GetOriginalRecipientsFromDictionary(mailID);

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
                    RemoveRecipientsFromDictionary(mailID);
                    return true;
                }

                if (new CountDialog(config).ShowDialog() == true)
                {
                    RemoveRecipientsFromDictionary(mailID);
                    return true;
                }
            }
            return false;
        }

        private List<RecipientInfo> GetOriginalRecipientsFromDictionary(string mailID)
        {
            if (string.IsNullOrEmpty(mailID))
            {
                return null;
            }
            return EntryIdToOriginalRecipientsDictionary.ContainsKey(mailID) ? EntryIdToOriginalRecipientsDictionary[mailID] : null;
        }

        private void RemoveRecipientsFromDictionary(string mailID)
        {
            if (string.IsNullOrEmpty(mailID))
            {
                return;
            }
            EntryIdToOriginalRecipientsDictionary.Remove(mailID);
        }

        private string GenerateMailID(Outlook.MailItem mailItem)
        {
            if(mailItem == null)
            {
                return null;
            }
            // Create an uniq id for this mail(response). mailItem.EntryID can use as uniq id, but
            // mailItem.EntryID is created when saving, but this mail(response) is not saved yet.
            // We can generate the entry id by executing mailItem.Save(), but mailItem.Save()
            // may do unexpected behavior when a mail item is an inline reply, so we can't use it.
            //
            // https://learn.microsoft.com/en-us/office/vba/api/outlook.mailitem.save
            // > If a mail item is an inline reply, calling Save on that mail item may fail and result in unexpected behavior.
            var index = mailItem.ConversationIndex ?? "";
            var topic = mailItem.ConversationTopic ?? "";

            // For mails that have no index and no topic, we give up to specify an uniq id, and mailId for them becomes "_".
            // Among those mails, recipients of the last mail that passes this method is regarded as original recipients.
            return $"{topic}_{index}";
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
