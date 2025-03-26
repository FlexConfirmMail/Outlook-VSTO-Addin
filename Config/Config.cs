using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

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
        public bool SafeNewDomainsEnabled = true;
        public HashSet<ConfigOption> Modified;

        public string TrustedDomainsPattern = "";
        public string TrustedAddressesPattern = "";
        public string UnsafeDomainsPattern = "";
        public string UnsafeAddressesPattern = "";
        public string UnsafeFilesPattern = "";

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

            if (other.Modified.Contains(ConfigOption.SafeNewDomainsEnabled))
            {
                SafeNewDomainsEnabled = other.SafeNewDomainsEnabled;
            }

            Modified.UnionWith(other.Modified);
        }

        public void RebuildPatterns()
        {
            HashSet<string> trustedAddressList = GetHashSet(TrustedDomains.Where(_ => _.Contains("@")));
            HashSet<string> trustedDomainList = GetHashSet(TrustedDomains.Where(_ => !_.Contains("@")));
            HashSet<string> unsafeAddressList = GetHashSet(UnsafeDomains.Where(_ => _.Contains("@")));
            HashSet<string> unsafeDomainList = GetHashSet(UnsafeDomains.Where(_ => !_.Contains("@")).ToList());

            TrustedDomainsPattern = $"^({string.Join("|", trustedDomainList.Select(ConvertWildCardToRegex))})$";
            TrustedAddressesPattern = $"^({string.Join("|", trustedAddressList.Select(ConvertWildCardToRegex))})$";
            UnsafeDomainsPattern = $"^({string.Join("|", unsafeDomainList.Select(ConvertWildCardToRegex))})$";
            UnsafeAddressesPattern = $"^({string.Join("|", unsafeAddressList.Select(ConvertWildCardToRegex))})$";
            UnsafeFilesPattern = $"({string.Join("|", UnsafeFiles.Select(ConvertWildCardToRegex))})";
        }

        private HashSet<string> GetHashSet(IEnumerable<string> list)
        {
            HashSet<string> ret = new HashSet<string>();
            HashSet<string> exclude = new HashSet<string>();
            foreach (string entry in list)
            {
                if (entry.StartsWith("-"))
                {
                    exclude.Add(entry.Substring(1));
                }
                else
                {
                    ret.Add(entry);
                }
            }
            ret.ExceptWith(exclude);
            return ret;
        }

        private static string ConvertWildCardToRegex(string value)
        {
            return Regex.Escape(value).Replace("\\*", ".*?").Replace("\\?", ".");
        }
    }
}
