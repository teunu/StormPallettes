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

namespace Stormworks_Pallettes_Manager
{
    public static class Data
    {
        public static List<PalletteCategory> loaded_categories;
        public static List<DefinitionEntry> loaded_defs;
    }

    public static class DefaultPallettes
    {
        public static PalletteCategory deprecated
        {
            get
            {
                var value = new PalletteCategory("V Deprecated Blocks");
                value.default_visible = false;
                value.display_color = ColorTranslator.FromHtml("#249ED6");
                value.explicit_sorting = new List<string> {
                    "torque_gearbox.xml",
                    "fluid_filter.xml",
                    "gate_torque_add.xml",
                    "gate_torque_multimeter.xml",
                    "passenger_seat.xml",
                    "radar_dish.xml",
                    "radar_huge.xml",
                    "radar_large.xml",
                    "radar_sonar_small.xml",
                    "radar_sonar.xml",
                    "radar.xml",
                    "rx_huge.xml",
                    "rx_large.xml",
                    "rx_med.xml",
                    "rx_small.xml",
                    "torque_gearbox2.xml",
                    "water_hose.xml",
                    "water_outlet.xml",
                    "winch_a.xml",
                    "winch_electric.xml",
                    "winch_huge_a.xml",
                    "winch_large_a.xml",
                    "gate_train_junction.xml"
                };
                return value;
            }
        }
        public static PalletteCategory weapons_dlc
        {
            get
            {
                var value = new PalletteCategory("V Weapons Dlc");
                value.default_visible = true;
                value.display_color = ColorTranslator.FromHtml("#3085E6");
                value.explicit_sorting = new List<string> {
                    "inventory_equipment_rifle.xml",
                    "inventory_equipment_rifle_ammo.xml",
                    "inventory_equipment_pistol.xml",
                    "inventory_equipment_pistol_ammo.xml",
                    "inventory_equipment_c4.xml",
                    "inventory_equipment_c4_detonator.xml",
                };
                value.prefix_sorting = new List<string> {
                    "gun_",
                    "warhead_"
                };
                return value;
            }
        }

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
                LateInitialize();
            }
            else
            {
                settings = new Settings();

                //Pop up the "Please put Directory" prompt
                Window askpath = new PopupSetPaths();
                askpath.Show();

                //Pop up the "likely first time, please add"
                Window askpop = new PopupDefaultPopulation("It might be that you're opening this tool for the first time. Do you want to automatically add the default pallettes?");
                askpop.Show();
                return;
            }
        }

        public void LateInitialize()
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
    }
}
