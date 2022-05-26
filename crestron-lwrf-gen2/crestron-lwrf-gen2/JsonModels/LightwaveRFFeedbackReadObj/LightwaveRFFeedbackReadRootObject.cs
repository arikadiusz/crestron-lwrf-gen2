using System;

namespace crestron_lwrf_gen2
{
    internal class LightwaveRFFeedbackReadRootObject
    {
        public bool active { get; set; }
        public int version { get; set; }
        public string userId { get; set; }
        public string clientId { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public LightwaveRFFeedbackReadMeta meta { get; set; }
    }
}
