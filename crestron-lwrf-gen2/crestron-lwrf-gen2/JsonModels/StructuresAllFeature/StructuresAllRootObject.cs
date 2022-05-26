using System.Collections.Generic;

namespace crestron_lwrf_gen2
{
    internal class StructuresAllRootObject
    {
        public string name { get; set; }
        public string groupId { get; set; }
        public List<StructuresAllDevice> devices { get; set; }
    }
} 
