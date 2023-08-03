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
        public void SetDefinitionVisibity(bool visble)
        {
            XmlReader reader = new XmlTextReader(file_ref);
       
            string flag = reader.GetAttribute("flags");

            int flag_int;

            if (Int32.TryParse(flag, out flag_int))
            {
                if (visble)
                    flag_int |= 0 << 29; //Set bit index 29 to 0 or "Not Deprecated"
                else
                    flag_int |= 1 << 29; //Set bit index 29 to 1 or "Deprecated"
            }
            reader.Close();

            var txt = File.ReadAllText(file_ref);
            string[] nodes = txt.Split('>');

        }
    }
}
