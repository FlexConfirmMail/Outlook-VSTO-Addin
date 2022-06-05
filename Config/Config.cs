using System;
using System.Collections.Generic;

namespace FlexConfirmMail
{
    public class Config
    {
        public bool CountEnabled = true;
        public int CountSeconds = 3;
        public bool CountAllowSkip = true;
        public bool SafeBccEnabled = true;
        public int SafeBccThreshold = 4;
        public bool MainSkipIfNoExt = false;
        public List<string> TrustedDomains;
        public List<string> UnsafeDomains;
        public List<string> UnsafeFiles;
        public HashSet<ConfigOption> Modified;

        public Config()
        {
            TrustedDomains = new List<string>();
            UnsafeDomains = new List<string>();
            UnsafeFiles = new List<string>();
            Modified = new HashSet<ConfigOption>();
        }

        public void Merge(Config other)
        {
            if (other == null)
            {
                return;
            }

            if (other.Modified.Contains(ConfigOption.CountEnabled))
            {
                CountEnabled = other.CountEnabled;
            }

            if (other.Modified.Contains(ConfigOption.CountSeconds))
            {
                CountSeconds = other.CountSeconds;
            }

            if (other.Modified.Contains(ConfigOption.CountAllowSkip))
            {
                CountAllowSkip = other.CountAllowSkip;
            }

            if (other.Modified.Contains(ConfigOption.SafeBccEnabled))
            {
                SafeBccEnabled = other.SafeBccEnabled;
            }

            if (other.Modified.Contains(ConfigOption.SafeBccThreshold))
            {
                SafeBccThreshold = other.SafeBccThreshold;
            }

            if (other.Modified.Contains(ConfigOption.MainSkipIfNoExt))
            {
                MainSkipIfNoExt = other.MainSkipIfNoExt;
            }

            if (other.Modified.Contains(ConfigOption.TrustedDomains))
            {
                TrustedDomains.AddRange(other.TrustedDomains);
            }

            if (other.Modified.Contains(ConfigOption.UnsafeDomains))
            {
                UnsafeDomains.AddRange(other.UnsafeDomains);
            }

            if (other.Modified.Contains(ConfigOption.UnsafeFiles))
            {
                UnsafeFiles.AddRange(other.UnsafeFiles);
            }

            Modified.UnionWith(other.Modified);
        }
    }
}
