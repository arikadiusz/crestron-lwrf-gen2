using System.Collections.Generic;

namespace crestron_lwrf_gen2
{
    internal class LightwaveRFFeedbackUpdateRootObject
    {
        public List<LightwaveRFFeedbackUpdateEvent> events = new List<LightwaveRFFeedbackUpdateEvent>();
        public string url { get; set; }
        public string @ref { get; set; }
        public int version { get; set; }
    }
}
