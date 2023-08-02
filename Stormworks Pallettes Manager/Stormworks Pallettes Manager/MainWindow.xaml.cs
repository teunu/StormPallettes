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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<PalletteCategory> loaded_categories;
        List<DefinitionEntry> loaded_defs;

        string root = @"C:\Program Files (x86)\Steam\steamapps\common\Stormworks\rom\data\definitions";

        public MainWindow()
        {
            InitializeComponent();

            loaded_categories = new List<PalletteCategory>();

            loaded_categories.Add(new PalletteCategory("Connectors"));
            loaded_categories[0].prefix_sorting.Add("connector");
            loaded_categories.Add(new PalletteCategory("Buttons"));
            loaded_categories[1].prefix_sorting.Add("button");

            loaded_defs = new List<DefinitionEntry>();

        }

        private void btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            loaded_defs.Clear();

            StackPanel viewer = (StackPanel)FindName("ViewerContainer");
            viewer.Children.Clear();

            foreach (PalletteCategory cat in loaded_categories)
            {
                CategoryLabel label = new CategoryLabel();
                TextBlock label_name = (TextBlock)label.FindName("Cat_Name");
                label_name.Text = cat.name;
                TextBlock label_auth = (TextBlock)label.FindName("Author");
                label_auth.Text = cat.author;

                viewer.Children.Add(label);
            }
            
            
            string[] files = Directory.GetFiles(root);
            foreach (string file in files)
            {
                DefinitionEntry candidate = new DefinitionEntry(file);
                for(int i = 0; i < loaded_categories.Count; i++)
                {
                    if (loaded_categories[i].MatchAny(file.Substring(file.IndexOf("definitions")+12))) 
                    {
                        candidate.categories.Add(i);
                    }
                }

                loaded_defs.Add(candidate);

                foreach(int c in candidate.categories)
                {
                    TextBlock block = new TextBlock();
                    block.Text = file.Substring(file.IndexOf("definitions"));

                    CategoryLabel parent = (CategoryLabel)viewer.Children[c];
                    StackPanel panel = (StackPanel)parent.FindName("Contents");

                    panel.Children.Add(block);
                }

            }
            


            Console.WriteLine(loaded_defs.Count);
        }
    }
}
