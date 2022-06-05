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
    }
}
