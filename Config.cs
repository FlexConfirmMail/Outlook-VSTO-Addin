using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace CheckMyMail
{
    public class Config
    {
        const String ROOTPATH = @"C:\ProgramData\CheckMyMail\";

        public List<string> TrustedDomains = new List<string>();

        public void LoadFileSystem()
        {
            ReadDomainList(ROOTPATH + "trusted.txt", TrustedDomains);
        }

        private void ReadDomainList(string path, List<string> list)
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
