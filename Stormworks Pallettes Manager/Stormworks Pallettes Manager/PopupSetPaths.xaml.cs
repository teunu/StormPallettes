using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Forms;
using System.ComponentModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Stormworks_Pallettes_Manager
{
    /// <summary>
    /// Interaction logic for PopupSetPaths.xaml
    /// </summary>
    public partial class PopupSetPaths : Window
    {
        string path = "C:/";

        public PopupSetPaths()
        {
            InitializeComponent();
            Find_Path.Content = path;

            Closing += Close;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.settings.sw_install_folder = path;
            MainWindow.settings.Save();

            MainWindow.instance.RefreshData();

            this.Close();
        }

        private void Find_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.RootFolder = Environment.SpecialFolder.SystemX86;
                var result = dialog.ShowDialog();
                if (result.ToString() != string.Empty)
                {
                    path = dialog.SelectedPath;
                }

            }

            Find_Path.Content = path;
        }

        private void Close(object sender, CancelEventArgs e)
        {
            //Pop up the "likely first time, please add"
            Window askpop = new PopupDefaultPopulation("It might be that you're opening this tool for the first time. Do you want to automatically add the default pallettes?");
            askpop.Show();
        }
    }
}
