using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crestron_lwrf_gen2
{
    internal class AuthRootObject
    {
        public AuthUser user { get; set; }
        public AuthTokens tokens { get; set; }
    }
}
