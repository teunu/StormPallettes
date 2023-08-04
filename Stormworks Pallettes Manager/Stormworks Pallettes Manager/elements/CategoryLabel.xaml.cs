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
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Media;

namespace Stormworks_Pallettes_Manager
{
    /// <summary>
    /// Interaction logic for Category.xaml
    /// </summary>
    public partial class CategoryLabel : UserControl
    {
        public int category_index;

        ImageBrush btn_img_show;
        ImageBrush btn_img_hide;

        public CategoryLabel(int index)
        {
            InitializeComponent();

            category_index = index;

            TextBlock label_name = (TextBlock)FindName("Cat_Name");
            label_name.Text = Data.loaded_categories[category_index].name;
            TextBlock label_auth = (TextBlock)FindName("Author");
            label_auth.Text = Data.loaded_categories[category_index].author;

            Uri img_show_uri = new Uri("../resources/icons/eye.png", UriKind.Relative);
            Uri img_hide_uri = new Uri("../resources/icons/eye-low-vision.png", UriKind.Relative);

            #region visualisation

            StreamResourceInfo stream;
            stream = Application.GetResourceStream(img_show_uri);
            BitmapFrame show_img = BitmapFrame.Create(stream.Stream);

            stream = Application.GetResourceStream(img_hide_uri);
            BitmapFrame hide_img = BitmapFrame.Create(stream.Stream);

            btn_img_show = new ImageBrush(show_img);
            btn_img_show.Opacity = 0.9;
            btn_img_show.Transform.Value.Scale(1, 0.5);
            btn_img_show.Stretch = Stretch.Uniform;

            btn_img_hide = new ImageBrush(hide_img);
            btn_img_hide.Opacity = 0.5;
            btn_img_hide.Stretch = Stretch.Uniform;
            btn_img_hide.Transform.Value.Scale(1, 0.5);

            var draw = Data.loaded_categories[index].display_color;
            Grid g = (Grid)FindName("Base");
            g.Background = new SolidColorBrush(Color.FromArgb(draw.A, draw.R, draw.G, draw.B));

            #endregion

            Button vis_btn = (Button)FindName("Visibility");
            if (Data.loaded_categories[index].visible)
                vis_btn.Background = btn_img_show;
            else
                vis_btn.Background = btn_img_hide;
        }

        private void Visibility_Click(object sender, RoutedEventArgs e)
        {
            Data.loaded_categories[category_index].ToggleCategoryVisibility();

            Button vis_btn = (Button)FindName("Visibility");
            if (Data.loaded_categories[category_index].visible)
                vis_btn.Background = btn_img_show;
            else
                vis_btn.Background = btn_img_hide;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(PalletteCategory.file_defs[category_index])
            {
                UseShellExecute = true
            };
            p.Start();
        }
    }
}
