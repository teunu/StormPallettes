using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Stormworks_Pallettes_Manager
{
    [System.Serializable]
    public class Settings
    {
        public string sw_install_folder = "none";

        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            using (var writer = new StreamWriter(@"Settings.xml"))
            {
                serializer.Serialize(writer, this);
            }
        }

        public static bool Load(out Settings set)
        {
            set = null;
            string file;
            if (File.Exists(@"Settings.xml"))
                file = File.ReadAllText(@"Settings.xml");
            else return false;

            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            using (TextReader reader = new StringReader(file))
            {
                set = (Settings)serializer.Deserialize(reader);
            }
            if (set != null)
                return true;
            else return false;
        }
    }
}
