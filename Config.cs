using System;
using System.Collections.Generic;

using System.IO;

namespace CheckMyMail
{
    public class Config
    {
        const String ROOTPATH = @"C:\ProgramData\CheckMyMail\";
        public const String DOMAIN_EXCHANGE = "Exchange";

        public HashSet<string> TrustedDomains = new HashSet<string>();

        public void LoadFileSystem()
        {
            TrustedDomains.Add(DOMAIN_EXCHANGE);
            ReadDomainList(ROOTPATH + "trusted.txt", TrustedDomains);
        }

        private void ReadDomainList(string path, HashSet<string> list)
        {
            StreamReader sr;
            try
            {
                sr = File.OpenText(path);
            }
            catch (Exception)
            {
                return;
            }

            string s;
            while ((s = sr.ReadLine()) != null)
            {
                if (!s.StartsWith("#"))
                {
                    list.Add(s.Trim());
                }
            }
            sr.Close();
        }
    }
}
