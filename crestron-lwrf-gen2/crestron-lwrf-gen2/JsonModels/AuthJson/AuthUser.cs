using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crestron_lwrf_gen2
{
    internal class AuthUser
    {
        public string givenName { get; set; }
        public string email { get; set; }
        public AuthLightwaveRfPublic lightwaveRfPublic { get; set; }
        public AuthProviders providers { get; set; }
        public string _id { get; set; }
        public long created { get; set; }
        public long modified { get; set; }
    }
}
