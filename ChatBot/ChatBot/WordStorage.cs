using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace ChatBot
{
    class WordStorage
    {
        private string _path;
        public Dictionary<string, string> GetAllWords()
        {
            Dictionary<string, string> words = new Dictionary<string, string>();
            using (StreamReader sr = new StreamReader(_path))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    var item = line.Trim().Split('|');
                    words.Add(item[0], item[1]);
                }
            }
            return words;
        }
        public WordStorage(string path)
        {
            _path = path;
            if (!File.Exists(_path))
                File.Create(_path).Close();
        }
        public void AddWord(string word, string translate)
        {
            using (var writer = new StreamWriter(_path, true)) 
            {
                writer.WriteLine(word + "|" + translate);
            }
        }
    }
}
