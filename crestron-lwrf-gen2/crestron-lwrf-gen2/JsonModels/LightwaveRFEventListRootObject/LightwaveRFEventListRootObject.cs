using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crestron_lwrf_gen2
{
    internal class LightwaveRFEventListRootObject
    {
        public string id { get; set; }
        public string type { get; set; }
        public int version { get; set; }
        public bool active { get; set; }
        public string userId { get; set; }
    }
}
