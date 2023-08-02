using System;
using System.Collections.Generic;
using System.Text;

namespace Stormworks_Pallettes_Manager
{
    public class DefinitionEntry
    {
        public List<int> categories = new List<int>();
        public string file_ref;

        public DefinitionEntry (string file)
        {
            file_ref = file;
        }
    }
}
