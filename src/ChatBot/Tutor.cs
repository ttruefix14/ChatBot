using static System.Net.Mime.MediaTypeNames;

namespace ChatBot
{
    class Tutor
    {
        private Random _random = new Random();
        private Dictionary<string, string> _words = new Dictionary<string, string>();
        public Dictionary<string, string> Words { get { return _words; } }
        private WordStorage _storage;
        public Tutor(string path)
        {
            _storage = new WordStorage(path);
            _words = _storage.GetAllWords();
        }
        public void AddWord(string word, string translate)
        {
            if (!_words.ContainsKey(word))
            {
                _words.Add(word, translate);
                _storage.AddWord(word, translate);
            }
        }
        public string AddWord(string[] msgArgs)
        {
            if (msgArgs.Length < 3)
            {
                return "Неверное количество аргументов";
            }
            AddWord(msgArgs[1], msgArgs[2]);
            return "Слово добавлено в словарь";
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
        public string CheckWord(string[] msgArgs)
        {
            string text;
            if (msgArgs.Length < 3)
            {
                return "Неверное количество аргументов";
            }
            text = CheckWord(msgArgs[1], msgArgs[2]) switch
            {
                CheckWordResult.Unknown   => $"Слово \"{msgArgs[1]}\" отсутствует в словаре",
                CheckWordResult.Incorrect => $"Правильный ответ: \"{Translate(msgArgs[1])}\"",
                CheckWordResult.Correct   => "Верно!",
                _ => "Неизвестно"
            };
            return text;
        }
        public string? Translate(string word)
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
        public string? GetRandomEngWord()
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
