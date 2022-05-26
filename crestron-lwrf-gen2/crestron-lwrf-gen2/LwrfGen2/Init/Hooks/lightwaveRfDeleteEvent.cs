using Crestron.SimplSharp;
using Crestron.SimplSharp.Net.Https;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crestron_lwrf_gen2
{
    public static partial class LwrfGen2
    {
        internal static void lightwaveRfDeleteEvent(string eventId)
        {
            try
            {

                List<HttpsHeader> headers = new List<HttpsHeader>();
                string content = String.Empty;

                headers.Add(new HttpsHeader("Content-Type: application/json"));
                headers.Add(new HttpsHeader("authorization: bearer " + authObj.tokens.access_token));


                string response = HttpRequests.Delete(@"https://publicapi.lightwaverf.com/v1/events/" + eventId, headers);
            }
            catch (Exception exc)
            {
                ErrorLog.Error("Exception in LwrfGen2.lightwaveRfPatchEventCreate, Message : {0}", exc.Message);
            }
        }
    }
}
