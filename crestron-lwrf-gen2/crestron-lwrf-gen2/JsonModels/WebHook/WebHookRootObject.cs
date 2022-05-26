using System.Collections.Generic;

namespace crestron_lwrf_gen2
{
    internal class WebHookRootObject
    {
        public string id { get; set; }
        public string userId { get; set; }
        public WebHookTriggerEvent triggerEvent { get; set; }
        public List<WebHookEvent> events { get; set; }
        public WebHookPayload payload { get; set; }
    }
}
