using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace HushNow
{
    public class Settings
    {
        public Keys SavedKey { get; set; }
        
        private static string SettingsPath => Path.GetFullPath("./settings.json");

        public static Settings Read()
        {
            if (!File.Exists(SettingsPath))
                File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(new Settings()));
                
            return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(SettingsPath));
        }

        public static void Write(Settings settings)
        {
           File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(settings)); 
        }
    }
}