using Crestron.SimplSharp;
using Crestron.SimplSharp.Net.Https;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crestron_lwrf_gen2
{
    public static partial class LwrfGen2
    {
        internal static void lightwaveRfGetStructures(HTTPSClientResponseCallback response)
        {
            try
            {
                List<HttpsHeader> headers = new List<HttpsHeader>();
                object contentObj = new object();
                string content = String.Empty; 

                headers.Add(new HttpsHeader("Content-Type: application/json"));
                headers.Add(new HttpsHeader("authorization: bearer " + authObj.tokens.access_token));


                HttpRequests.GetAsync(@"https://publicapi.lightwaverf.com/v1/structures", headers, response);

            }
            catch (Exception exc)
            {
                ErrorLog.Error("Exception in LwrfGen2.lightwaveRfPostStructures, Message : {0}", exc.Message);
            }
        }
    }
}
