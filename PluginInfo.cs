using PluginInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDIPaint
{
    public class PluginInfo
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Version { get; set; }
        public string Path { get; set; }
        public bool Enabled { get; set; }
        public IPlugin Instance { get; set; }
    }
}
