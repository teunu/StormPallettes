using System;
using System.Collections.Generic;
using System.Text;

namespace Stormworks_Pallettes_Manager
{
    public class PalletteCategory
    {
        public string name;
        public string description;
        public string author;

        public bool visible = true;

        public List<string> explicit_sorting = new List<string>();
        public List<string> prefix_sorting = new List<string>();
        public List<string> contents_sorting = new List<string>();

        public PalletteCategory (string _name)
        {
            name = _name;
            author = "Program";
        }

        public delegate void ChangeCategory();
        public ChangeCategory OnCategoryChanged;

        public void ToggleCategoryVisibility()
        {
            visible = !visible;
            OnCategoryChanged?.Invoke();
        }

        public void SetCategoryVisibility(bool set)
        {
            visible = set;
            OnCategoryChanged?.Invoke();
        }

        public bool MatchAny(string file) { return MatchPrefix(file) || MatchContents(file) || MatchExplicit(file); }

        public bool MatchExplicit(string file) { foreach (string test in explicit_sorting) { if (file == test) return true; } return false; }

        public bool MatchPrefix(string file) { foreach (string test in prefix_sorting) { if (file.StartsWith(test)) return true; } return false; }

        public bool MatchContents(string file) { foreach (string test in contents_sorting) { if (file.Contains(test)) return true; } return false; }
    }
}
