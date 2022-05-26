using System.Collections.Generic;

namespace crestron_lwrf_gen2
{
    internal class StructuresAllDevice
    {
        public string deviceId { get; set; }
        public string name { get; set; }
        public string productCode { get; set; }
        public List<StructuresAllFeatureSet> featureSets { get; set; }
        public string product { get; set; }
        public string device { get; set; }
        public string desc { get; set; }
        public string type { get; set; }
        public string cat { get; set; }
        public int gen { get; set; }
    }
}
