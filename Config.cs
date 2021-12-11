using System;
using System.Collections.Generic;

using System.IO;

namespace CheckMyMail
{
    public class Config
    {
        const String ROOTPATH = @"C:\ProgramData\CheckMyMail\";
        public const String DOMAIN_EXCHANGE = "Exchange";

        public HashSet<string> InternalDomains = new HashSet<string>();

        public void LoadFileSystem()
        {
            InternalDomains.Add(DOMAIN_EXCHANGE);
            ReadDomainList(ROOTPATH + "internal.txt", InternalDomains);
        }

        private void ReadDomainList(string path, HashSet<string> list)
        {
            StreamReader sr;
            try
            {
                sr = File.OpenText(ROOTPATH + "trusted.txt");
            }
            catch (Exception)
            {
                return;
            }

            string s;
            while ((s = sr.ReadLine()) != null)
            {
                list.Add(s.Trim());
            }
            sr.Close();
        }
    }
}
