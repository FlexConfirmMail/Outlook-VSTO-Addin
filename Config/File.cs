using System;
using System.IO;
using System.Collections.Generic;

namespace FlexConfirmMail
{
    public class FileConfig
    {
        public static Dictionary<string, string> ReadDict(string path)
        {
            QueueLogger.Log($"File: {path}");
            try
            {
                using (TextReader sr = File.OpenText(path))
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    string key, val;
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (Tokenize(line.Trim(), out key, out val))
                        {
                            dict[key] = val;
                        }
                    }
                    return dict;
                }
            }
            catch (IOException e)
            {
                QueueLogger.Log($"* {e.GetType().Name}");
            }
            return null;
        }

        public static List<string> ReadList(string path)
        {
            QueueLogger.Log($"File: {path}");
            try
            {
                using (TextReader sr = File.OpenText(path))
                {
                    List<string> list = new List<string>();
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (!IsComment(line))
                        {
                            list.Add(line);
                        }
                    }
                    return list;
                }
            }
            catch (IOException e)
            {
                QueueLogger.Log($"* {e.GetType().Name}");
            }
            return null;
        }

        private static bool IsComment(string line)
        {
            return line.StartsWith("#") || String.IsNullOrEmpty(line);
        }

        private static bool Tokenize(string line, out string key, out string val)
        {
            key = null;
            val = null;

            if (IsComment(line))
            {
                return false;
            }

            int equal = line.IndexOf('=');
            if (equal < 0)
            {
                return false;
            }
            key = line.Substring(0, equal).Trim();
            val = line.Substring(equal + 1).Trim();
            return true;
        }
    }
}
