using System;
using System.Collections.Generic;

using System.IO;

namespace FlexConfirmMail.Config
{
    public class FileLoader
    {
        private ConfigData _config;

        public FileLoader(ConfigData config)
        {
            _config = config;
        }

        public bool TryListFile(string basedir, string file)
        {

            try
            {
                using (TextReader sr = File.OpenText(Path.Combine(basedir, file)))
                {
                    QueueLogger.Log($"Load a list from {file}");
                    ReadListFile(sr, file);
                    return true;
                }
            }
            catch (IOException e)
            {
                QueueLogger.Log($"Skip {file} ({e.GetType().Name})");
                return false;
            }
        }

        public bool TryOptionFile(string basedir, string file)
        {
            try
            {
                using (TextReader sr = File.OpenText(Path.Combine(basedir, file)))
                {
                    QueueLogger.Log($"Load params from {file}");
                    ReadOptionFile(sr);
                    return true;
                }
            }
            catch (IOException e)
            {
                QueueLogger.Log($"Skip {file} ({e.GetType().Name})");
                return false;
            }
        }
        public string TryRawFile(string basedir, string file)
        {
            try
            {
                return File.ReadAllText(Path.Combine(basedir, file));
            }
            catch (IOException e)
            {
                QueueLogger.Log($"Skip {file} ({e.GetType().Name})");
                return "";
            }
        }

        private void ReadListFile(TextReader sr, string file)
        {
            List<string> list = new List<string>();
            string line;

            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.StartsWith("#") || String.IsNullOrEmpty(line))
                {
                    continue;
                }
                list.Add(line);
                QueueLogger.Log($" - {line}");
            }
            _config.AddList(file, list);
        }

        public void ReadOptionFile(TextReader sr)
        {
            string line;
            string key;
            string val;

            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.StartsWith("#") || String.IsNullOrEmpty(line))
                {
                    continue;
                }
                if (TryParse(line, out key, out val))
                {
                    _config.AddOption(key, val);
                    QueueLogger.Log($" - {key} = {val}");
                }
            }
        }

        private bool TryParse(string line, out string key, out string val)
        {
            key = null;
            val = null;

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
