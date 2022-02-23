using Newtonsoft.Json;
using System.IO;

namespace SimonUI
{
    public static class FileIOUtils
    {
        private const string _filename = "Scores.dat";
       
        public static void SaveToJson(ScoreEntry[] scores)
        {
            string json = JsonConvert.SerializeObject(scores, Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(_filename))
            {
                sw.WriteLine(json);
            }

        }

        public static ScoreEntry[] LoadFromJson()
        {
            if (!File.Exists(_filename))
            {
                return new ScoreEntry[0];
            }
            string json;
            using (StreamReader sr = new StreamReader(_filename))
            {
                json = sr.ReadToEnd();
            }
            ScoreEntry[] loadedData = JsonConvert.DeserializeObject<ScoreEntry[]>(json);
            return loadedData;
        }
    }
}
