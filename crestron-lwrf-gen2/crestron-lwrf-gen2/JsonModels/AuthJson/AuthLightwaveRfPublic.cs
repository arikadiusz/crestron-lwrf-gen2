using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crestron_lwrf_gen2
{
    internal class AuthLightwaveRfPublic
    {
        public int user_id { get; set; }
        public int active { get; set; }
        public int active_campaign_id { get; set; }
        public int t_and_c_consent { get; set; }
        public AuthMyClient myClient { get; set; }
    }
}
