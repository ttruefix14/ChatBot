using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot
{
    class Tutor
    {
        private Random _random = new Random();
        private Dictionary<string, string> _words = new Dictionary<string, string>();
        public Dictionary<string, string> Words { get { return _words; } }
        public void AddWord(string word, string translate)
        {
            _words.Add(word, translate);
        }

        public CheckWordResult CheckWord(string word, string translate)
        {
            if (!_words.ContainsKey(word))
            {
                return CheckWordResult.Unknown;
            }
            else if (_words[word].ToLower() == translate.ToLower())
            {
                return CheckWordResult.Correct;
            }
            else
            {
                return CheckWordResult.Incorrect;
            }
        }
        public string Translate(string word)
        {
            if (_words.ContainsKey(word))
            {
                return _words[word];
            }
            else
            {
                return null;
            }
        }
        public string GetRandomEngWord()
        {
            if (_words.Count == 0)
            {
                return null;
            }
            string randomWord = _words.Keys.ElementAt<string>(_random.Next(0, _words.Count));
            return randomWord;
        }
    }
}
