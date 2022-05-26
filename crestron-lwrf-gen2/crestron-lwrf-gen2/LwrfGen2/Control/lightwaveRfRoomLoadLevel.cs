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
        public static void lightwaveRfRoomLoadLevel(string roomName, string zoneName, int loadNum, double loadLevel, HTTPSClientResponseCallback response)
        {
            try
            {
                List<HttpsHeader> headers = new List<HttpsHeader>();
                object contentObj = new object(); 
                string content = String.Empty;

                headers.Add(new HttpsHeader("Content-Type: application/json"));
                headers.Add(new HttpsHeader("authorization: bearer " + authObj.tokens.access_token));


                if (loadLevel <= 100)
                {
                    contentObj = new
                    {
                        value = loadLevel
                    };

                    content = JsonConvert.SerializeObject(contentObj);
                }
                else
                    ErrorLog.Error("Error : Value must be 0 to 100");

                List<string> dictValue = lightwaveRfLoadsDimLevel.TryGetValue(zoneName + "-" + roomName, out dictValue) ? dictValue : null;
                if (dictValue != null)
                {
                    if (loadNum < dictValue.Count)
                    {
                        HttpRequests.PostAsync(@"https://publicapi.lightwaverf.com/v1/feature/" + dictValue[(int)loadNum] + @"?", content, headers, (res,err) =>
                        {
                            if (loadLevel > 0)
                                lightwaveRfRoomLoadOn(roomName, zoneName, loadNum, (a, b) => { });
                            else
                                lightwaveRfRoomLoadOff(roomName, zoneName, loadNum, (a, b) => { });
                            response(res, err);
                        });
                    }
                    else
                        return;
                }
                else
                    return;





            }
            catch (Exception exc)
            {
                ErrorLog.Error("Exception in LwrfGen2.lightwaveRfRoomLoadLevel, Message : {0}", exc.Message);
            }
        }
    }
}
