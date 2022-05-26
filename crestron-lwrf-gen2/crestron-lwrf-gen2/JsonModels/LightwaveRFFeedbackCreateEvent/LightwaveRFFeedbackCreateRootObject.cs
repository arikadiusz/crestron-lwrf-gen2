using System.Collections.Generic;

namespace crestron_lwrf_gen2
{
    internal class LightwaveRFFeedbackCreateRootObject
    {
        public List<LightwaveRFFeedbackCreateEvent> events = new List<LightwaveRFFeedbackCreateEvent>();
        public string url { get; set; }
        public string @ref { get; set; }
    }
}
