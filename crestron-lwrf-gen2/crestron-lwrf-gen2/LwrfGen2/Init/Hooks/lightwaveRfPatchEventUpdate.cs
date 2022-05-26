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
        internal static void lightwaveRfPatchEventUpdate(string eventId, LightwaveRFFeedbackUpdateRootObject eventObj)
        {
            try
            {
                List<HttpsHeader> headers = new List<HttpsHeader>();
                string content = String.Empty;

                headers.Add(new HttpsHeader("Content-Type: application/json"));
                headers.Add(new HttpsHeader("authorization: bearer " + authObj.tokens.access_token));


                content = JsonConvert.SerializeObject(eventObj);


                string response = HttpRequests.Patch(@"https://publicapi.lightwaverf.com/v1/events/" + eventId, content, headers);

                CrestronConsole.PrintLine("HTTP Response: {0}", response);
            }
            catch (Exception exc)
            {
                ErrorLog.Error("Exception in LwrfGen2.lightwaveRfPatchEventUpdate, Message : {0}", exc.Message);
            }
        }
    }
}
