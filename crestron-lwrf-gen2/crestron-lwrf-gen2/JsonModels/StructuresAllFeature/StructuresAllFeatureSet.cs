using System.Collections.Generic;

namespace crestron_lwrf_gen2
{
    internal class StructuresAllFeatureSet
    {
        public string featureSetId { get; set; }
        public string name { get; set; }
        public List<StructuresAllFeature> features { get; set; }
    }
}
