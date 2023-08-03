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

        string root = @"C:\Program Files (x86)\Steam\steamapps\common\Stormworks\rom\data\definitions";

        public MainWindow()
        {
            InitializeComponent();

            Data.loaded_categories = new List<PalletteCategory>();

            Data.loaded_categories.Add(new PalletteCategory("Connectors"));
            Data.loaded_categories[0].prefix_sorting.Add("connector");
            Data.loaded_categories.Add(new PalletteCategory("Buttons"));
            Data.loaded_categories[1].prefix_sorting.Add("button");
            Data.loaded_categories.Add(new PalletteCategory("Modular Engines"));
            Data.loaded_categories[2].prefix_sorting.Add("modular");

            Data.loaded_defs = new List<DefinitionEntry>();

        }

        private void btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            Data.loaded_defs.Clear();

            StackPanel viewer = (StackPanel)FindName("ViewerContainer");
            viewer.Children.Clear();

            for (int i = 0; i < Data.loaded_categories.Count; i ++)
            {
                CategoryLabel label = new CategoryLabel(i);

                viewer.Children.Add(label);
            }
            
            
            string[] files = Directory.GetFiles(root);
            foreach (string file in files)
            {
                DefinitionEntry candidate = new DefinitionEntry(file);
                for(int i = 0; i < Data.loaded_categories.Count; i++)
                {
                    if (Data.loaded_categories[i].MatchAny(file.Substring(file.IndexOf("definitions")+12))) 
                    {
                        candidate.SetCategory(i);
                    }
                }

                Data.loaded_defs.Add(candidate);

                foreach(int c in candidate.categories)
                {
                    TextBlock block = new TextBlock();
                    block.Text = file.Substring(file.IndexOf("definitions"));

                    CategoryLabel parent = (CategoryLabel)viewer.Children[c];
                    StackPanel panel = (StackPanel)parent.FindName("Contents");

                    panel.Children.Add(block);
                }
            }
            


            Console.WriteLine(Data.loaded_defs.Count);
        }
    }
}
