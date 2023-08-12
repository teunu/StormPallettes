using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;

namespace Stormworks_Pallettes_Manager
{
    [System.Serializable]
    public class PalletteCategory
    {
        public string name;
        public string description;
        public string author;
        public Color display_color = Color.Aquamarine;

        public bool default_visible = true;
        public bool visible = true;

        public List<string> explicit_sorting = new List<string>();
        public List<string> prefix_sorting = new List<string>();
        public List<string> contents_sorting = new List<string>();

        //Not saved, rather assigned when loaded
        public string def_name { get; private set; }


        public PalletteCategory(string _name)
        {
            name = _name;
            author = "Program";
        }

        public PalletteCategory(string def, string _name)
        {
            string[] parts = def.Split('/', '\\');
            def_name = parts[parts.Length - 1].Replace(".xml", "");
            name = _name;
            author = "Pallettes";
        }

        public delegate void ChangeCategory();
        public ChangeCategory OnCategoryChanged;

        public void ToggleCategoryVisibility()
        {
            visible = !visible;
            OnCategoryChanged?.Invoke();

            Save(this, def_name);
        }

        public void SetCategoryVisibility(bool set)
        {
            visible = set;
            OnCategoryChanged?.Invoke();

            Save(this, def_name);
        }

        public bool MatchAny(string file) { return MatchPrefix(file) || MatchContents(file) || MatchExplicit(file); }

        public bool MatchExplicit(string file) { foreach (string test in explicit_sorting) { if (file == test) return true; } return false; }

        public bool MatchPrefix(string file) { foreach (string test in prefix_sorting) { if (file.StartsWith(test)) return true; } return false; }

        public bool MatchContents(string file) { foreach (string test in contents_sorting) { if (file.Contains(test)) return true; } return false; }

        #region Serialisation

        public static void Save(PalletteCategory data, string save_name)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PCSerializeWrapper));
            if (save_name == null)
                save_name = data.name.ToLower().Replace(' ', '_');

            //Create a file if it's empty. I guess kinda cursed to create-open-close-open-write-close, but I'm in a rush and this works well enough.
            if (!File.Exists(Reference.pallette_path + @"\" + save_name + ".xml"))
            {
                FileStream f = File.Create(Reference.pallette_path + @"\" + save_name + ".xml");
                f.Close();
            }

            using (var writer = new StreamWriter(Reference.pallette_path + @"\" + save_name + ".xml"))
            {
                PCSerializeWrapper candidate = new PCSerializeWrapper(data.name, data.description, data.author, data.display_color, data.visible, data.default_visible,
                    data.explicit_sorting.ToArray(),
                    data.prefix_sorting.ToArray(),
                    data.contents_sorting.ToArray());

                serializer.Serialize(writer, candidate);
            }
        }

        public static string[] file_defs
        {
            get
            {
                string[] files = Directory.GetFiles(Reference.pallette_path);

                return files;
            }
        }

        public static PalletteCategory[] LoadAll()
        {
            string[] files;
            if (Directory.Exists(Reference.pallette_path))
                files = Directory.GetFiles(Reference.pallette_path);
            else return new PalletteCategory[0];

            List<PalletteCategory> loaded = new List<PalletteCategory>();

            foreach (string f in files)
            {
                string file = File.ReadAllText(f);
                XmlSerializer serializer = new XmlSerializer(typeof(PCSerializeWrapper));
                using (TextReader reader = new StringReader(file))
                {
                    PCSerializeWrapper wrapped = (PCSerializeWrapper)serializer.Deserialize(reader);
                    loaded.Add(wrapped.unwrap(f));
                }
            }

            return loaded.ToArray();
        }

        #endregion
    }

    public struct PCSerializeWrapper
    {
        public string name;
        public string description;
        public string author;
        public string color_hex;

        public bool default_visibility;
        public bool visibility;

        public string[] sort_explicit;
        public string[] sort_prefix;
        public string[] sort_content;

        public PCSerializeWrapper(string _name, string _desc, string _auth, Color _colr, bool _show, bool _dshw, string[] _expl, string[] _prfx, string[] _cont)
        {
            name = _name;
            description = _desc;
            author = _auth;
            color_hex = ColorTranslator.ToHtml(_colr);
            visibility = _show;
            default_visibility = _dshw;
            sort_explicit = _expl;
            sort_prefix = _prfx;
            sort_content = _cont;
        }

        public PalletteCategory unwrap(string file_name)
        {
            PalletteCategory value = new PalletteCategory(file_name, name);
            value.author = author;
            value.description = description;
            value.display_color = ColorTranslator.FromHtml(color_hex);

            value.default_visible = default_visibility;
            value.visible = visibility;

            value.explicit_sorting = new List<string>(sort_explicit);
            value.prefix_sorting = new List<string>(sort_prefix);
            value.contents_sorting = new List<string>(sort_content);
            return value;
        }
    }
}
