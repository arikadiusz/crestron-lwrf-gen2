using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crestron_lwrf_gen2
{
    internal class Zone
    {
        public string groupId { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public List<string> parentGroups { get; set; }
        public bool visible { get; set; }
        public List<string> order { get; set; }
        public List<string> rooms { get; set; }
    }
}
