using FlexConfirmMail.Dialog;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Interop;
using Office = Microsoft.Office.Core;

namespace FlexConfirmMail
{
    [ComVisible(true)]
    public class Ribbon : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI ribbon;

        public Ribbon()
        {
        }

        public virtual object Ribbon_LoadImage(string imageName)
        {
            switch (imageName)
            {
                case "logo.png":
                    return Properties.Resources.logo;
            }
            return null;
        }

        public string Ribbon_LoadLabel(Office.IRibbonControl Control)
        {
            switch (Control.Id)
            {
                case "GroupFlexConfirmMail":
                    return Properties.Resources.RibbonGroupFlexConfirmMail;
                case "ButtonFlexConfirmMail":
                    return Properties.Resources.RibbonButtonFlexConfirmMail;
            }
            return "";
        }

        #region IRibbonExtensibility のメンバー

        public string GetCustomUI(string ribbonID)
        {
            if (ribbonID == "Microsoft.Outlook.Explorer")
            {
                return GetResourceText("FlexConfirmMail.Ribbon.xml");
            }
            return "";
        }

        #endregion

        #region リボンのコールバック
        //ここでコールバック メソッドを作成します。コールバック メソッドの追加について詳しくは https://go.microsoft.com/fwlink/?LinkID=271226 をご覧ください

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
        }

        #endregion
        public void OnClickConfig(Office.IRibbonControl control)
        {
            // Some users reported that Intel Graphic + Win10 causes
            // a blank screen. Diable Hardware Accerelation.
            RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;

            new ConfigDialog().ShowDialog();
        }
        #region ヘルパー

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
