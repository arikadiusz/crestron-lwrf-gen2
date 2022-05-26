using Crestron.SimplSharp.Net.Https;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Crestron.SimplSharp;

namespace crestron_lwrf_gen2
{
    public static partial class LwrfGen2
    {
        public static void lightwaveRfRoomLoadOff(string roomName, string zoneName, int loadNum, HTTPSClientResponseCallback response)
        {
            try
            {
                List<HttpsHeader> headers = new List<HttpsHeader>();
                object contentObj = new object(); 
                string content = String.Empty;

                headers.Add(new HttpsHeader("Content-Type: application/json"));
                headers.Add(new HttpsHeader("authorization: bearer " + authObj.tokens.access_token));

                contentObj = new
                {
                    value = 0
                };

                content = JsonConvert.SerializeObject(contentObj);

                List<string> dictValue = lightwaveRfLoadsSwitch.TryGetValue(zoneName + "-" + roomName, out dictValue) ? dictValue : null;
                if (dictValue != null)
                {
                    if (loadNum < dictValue.Count)
                    {
                        HttpRequests.PostAsync(@"https://publicapi.lightwaverf.com/v1/feature/" + dictValue[(int)loadNum] + @"?", content, headers, response);
                    }
                    else
                        return;
                }
                else
                    return;


            }
            catch (Exception exc)
            {
                ErrorLog.Error("Exception in LwrfGen2.lightwaveRfRoomLoadOff, Message : {0}", exc.Message);
            }
        }
    }
}
