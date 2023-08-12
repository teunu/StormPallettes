using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace Stormworks_Pallettes_Manager
{
    public class DefinitionEntry
    {
        public List<int> categories { get; private set; }  = new List<int>();
        public string file_ref;

        public bool visible;

        public DefinitionEntry (string file)
        {
            file_ref = file;
        }

        public void SetCategory(int i)
        {
            categories.Add(i);
            Data.loaded_categories[i].OnCategoryChanged += CheckVisible;
        }

        public void StripCategory(int i)
        {
            categories.Remove(i);
            Data.loaded_categories[i].OnCategoryChanged -= CheckVisible;
        }

        public void CheckVisible()
        {
            bool visible = false;
            for(int i = categories.Count; i > 0 ; i--)
            {
                if (Data.loaded_categories[categories[i-1]].visible)
                {
                    visible = true;
                }
            }

            SetDefinitionVisibity(visible);
        }

        //Sets flag to deprecation flag 1 or 0. Remember other flags exists, so we only change the 29 index bit 
        public void SetDefinitionVisibity(bool visible)
        {
            string file = File.ReadAllText(file_ref);
            string[] parts = file.Split('\n');

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(parts[0] + parts[1] + "</definition>");

            XmlAttributeCollection collection = doc.ChildNodes[1].Attributes;

            try
            {
                string flagm = collection["flags"].Value;

                int flag_int;

                if (Int32.TryParse(flagm, out flag_int))
                {
                    if (visible)
                        flag_int &= ~(1 << 29); //Set bit index 29 to 0 or "Not Deprecated"
                    else
                        flag_int |= 1 << 29; //Set bit index 29 to 1 or "Deprecated"
                }

                string new_flag = flag_int.ToString();

                //Devs likely concatonated their xml definitions, because it doesn't follow rules. I can spend time making a parser, or we can do it their route and concatenate myself.
                string old_s = $"flags=\"{flagm}\"";
                string new_s = $"flags=\"{new_flag}\"";
                string replace = parts[1].Replace(old_s, new_s);
                parts[1] = replace;

                string full = "";
                foreach (string part in parts) { full += part + "\n"; }

                File.WriteAllText(file_ref, full);
            }
            catch
            {

            }
        }
    }
}

/*
*/