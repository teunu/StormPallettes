using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Stormworks_Pallettes_Manager
{
    /// <summary>
    /// Interaction logic for Category.xaml
    /// </summary>
    public partial class CategoryLabel : UserControl
    {
        public int category_index;

        public CategoryLabel(int index)
        {
            InitializeComponent();

            category_index = index;

            TextBlock label_name = (TextBlock)FindName("Cat_Name");
            label_name.Text = Data.loaded_categories[category_index].name;
            TextBlock label_auth = (TextBlock)FindName("Author");
            label_auth.Text = Data.loaded_categories[category_index].author;

            Button vis_btn = (Button)FindName("Visibility");
            if (Data.loaded_categories[index].visible)
                vis_btn.Content = "VISIBILE";
            else
                vis_btn.Content = "HIDDEN";
        }

        private void Visibility_Click(object sender, RoutedEventArgs e)
        {
            Data.loaded_categories[category_index].ToggleCategoryVisibility();

            Button vis_btn = (Button)FindName("Visibility");
            if (Data.loaded_categories[category_index].visible)
                vis_btn.Content = "VISIBILE";
            else
                vis_btn.Content = "HIDDEN";
        }
    }
}
