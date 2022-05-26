using System.Collections.Generic;

namespace crestron_lwrf_gen2
{
    internal class LightwaveRFFeedbackReadMeta
    {
        protected List<LightwaveRFFeedbackReadEvent> events { get; set; }
        protected string url { get; set; }
        protected string @ref { get; set; }
        protected string userId { get; set; }
        protected string clientId { get; set; }
    }
}
