using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.CompilerServices;

namespace Stormworks_Pallettes_Manager
{
    public static class Data
    {
        public static List<PalletteCategory> loaded_categories;
        public static List<DefinitionEntry> loaded_defs;
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Settings settings;

        public static MainWindow instance;

        public MainWindow()
        {
            InitializeComponent();
            instance = this;

            //Check if settings can be loaded, if not create it
            if (Settings.Load(out settings))
            {
                if (settings.sw_install_folder == null || !System.IO.File.Exists(settings.sw_install_folder+ @"\stormworks.exe"))
                {
                    //Pop up a box saying "we've not found an install folder"

                    //Pop up the "Please put Directory" prompt
                    Window askpath = new PopupSetPaths();
                    askpath.Show();
                    return;
                } 
                else
                    RefreshData();
            }
            else
            {
                settings = new Settings();

                //Pop up the "Please put Directory" prompt
                Window askpath = new PopupSetPaths();
                askpath.Show();
            }
        }

        public void RefreshData()
        {
            Data.loaded_defs = new List<DefinitionEntry>();

            Data.loaded_categories = new List<PalletteCategory>(PalletteCategory.LoadAll());
            if (Data.loaded_categories.Count > 0)
            {
                //Refresh
                RefreshView();
            }
            else
            {
                //Ask if the program should populate with defaults?
                Window w = new PopupDefaultPopulation("");
                w.Show();
            }
        }

        public void RefreshView()
        {
            Data.loaded_defs.Clear();

            StackPanel viewer = (StackPanel)FindName("ViewerContainer");
            viewer.Children.Clear();

            for (int i = 0; i < Data.loaded_categories.Count; i++)
            {
                CategoryLabel label = new CategoryLabel(i);

                viewer.Children.Add(label);
            }

            string[] files = Directory.GetFiles(Reference.definitions_path);
            foreach (string file in files)
            {
                DefinitionEntry candidate = new DefinitionEntry(file);
                for (int i = 0; i < Data.loaded_categories.Count; i++)
                {
                    if (Data.loaded_categories[i].MatchAny(file.Substring(file.IndexOf("definitions") + 12)))
                    {
                        candidate.SetCategory(i);
                    }
                }
            }
        }

        private void btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshView();
        }
    
        private void btn_OpenSettings(object sender, RoutedEventArgs e)
        {
            var p = new ProcessStartInfo(@"Settings.xml")
            {
                UseShellExecute = true
            };
            Process.Start(p);
        }

        private void btn_ResetDefaults(object sender, RoutedEventArgs e)
        {
            foreach(PalletteCategory cat in Data.loaded_categories)
            {
                cat.SetCategoryVisibility(cat.default_visible);
            }
        }

        private void btn_Git(object sender, RoutedEventArgs e)
        {
            var p = new ProcessStartInfo()
            {
                FileName = "https://github.com/teunu/StormPallettes",
                UseShellExecute = true
            };
            Process.Start(p);
        }

        private void btn_Portfolio(object sender, RoutedEventArgs e)
        {
            var p = new ProcessStartInfo()
            {
                FileName = "https://teunu.com/about",
                UseShellExecute = true
            };
            Process.Start(p);
        }

        private void btn_OpenCategoryFolder(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(Reference.pallette_path))
                Process.Start("explorer.exe", Reference.pallette_path);
        }

        private void Main_GotFocus(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }
    }
}
