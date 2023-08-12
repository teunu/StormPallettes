using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Stormworks_Pallettes_Manager
{
    /// <summary>
    /// Interaction logic for PopupDefaultPopulation.xaml
    /// </summary>
    public partial class PopupDefaultPopulation : Window
    {
        public PopupDefaultPopulation(string reason)
        {
            InitializeComponent();

            if (reason != "")
                display.Text = reason;
        }

        private void decline_Click(object sender, RoutedEventArgs e)
        {
            //Close tab
            this.Close();
        }

        private async void accept_Click(object sender, RoutedEventArgs e)
        {
            //Add defaults
            PalletteCategory depr = DefaultPallettes.deprecated;
            PalletteCategory.Save(depr, depr.def_name);

            PalletteCategory weap = DefaultPallettes.weapons_dlc;
            PalletteCategory.Save(weap, weap.def_name);

            //Refresh data
            MainWindow.instance.RefreshData();

            //Close tab
            this.Close();
        }
    }
}
