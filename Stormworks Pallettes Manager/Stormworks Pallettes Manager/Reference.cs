using System;
using System.Collections.Generic;
using System.Text;

namespace Stormworks_Pallettes_Manager
{
    public static class Reference
    {
        public static string definitions_path { get { return MainWindow.settings.sw_install_folder + @"\rom\data\definitions\"; } }
        public static string pallette_path { get { return MainWindow.settings.sw_install_folder + @"\rom\pallettes\"; } }
    }
}