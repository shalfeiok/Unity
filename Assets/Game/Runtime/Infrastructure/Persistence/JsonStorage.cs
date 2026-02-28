using System.IO;

namespace Game.Infrastructure.Persistence
{
    public sealed class JsonStorage
    {
        public void Save(string path, string json)
        {
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(path, json);
        }

        public bool TryLoad(string path, out string json)
        {
            if (!File.Exists(path))
            {
                json = string.Empty;
                return false;
            }

            json = File.ReadAllText(path);
            return true;
        }
    }
}
