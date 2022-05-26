using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crestron_lwrf_gen2
{
    internal class RoomListRootObject
    {
        public string groupId { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public List<string> parentGroups { get; set; } 
        public bool visible { get; set; }
        public List<object> order { get; set; }
        public List<object> featureSets { get; set; }
        public List<object> scriptSets { get; set; }
        public List<object> automationSets { get; set; }
    }
}
