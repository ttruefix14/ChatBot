namespace ChatBotNew
{
    class WordStorage
    {
        private string _path;
        public Dictionary<string, string> GetAllWords()
        {
            Dictionary<string, string> words = new Dictionary<string, string>();
            try
            {
                using (StreamReader sr = new StreamReader(_path))
                {
                    string? line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        try
                        {
                            var item = line.Trim().Split('|');
                            words.Add(item[0], item[1]);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Некорректный формат: {line}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось обработать файл: {_path}");
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
            try
            {
                using (var writer = new StreamWriter(_path, true))
                {
                    writer.WriteLine(word + "|" + translate);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Слово не было добавлено в словарь:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
