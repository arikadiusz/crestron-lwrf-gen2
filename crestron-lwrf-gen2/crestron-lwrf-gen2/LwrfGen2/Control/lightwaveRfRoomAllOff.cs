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
        public static void lightwaveRfRoomAllOff(string roomName, string zoneName, HTTPSClientResponseCallback response)
        {
            try
            {
                FeatureBatchRootObject batch = new FeatureBatchRootObject();

                List<HttpsHeader> headers = new List<HttpsHeader>();
                object contentObj = new object(); 
                string content = String.Empty;

                headers.Add(new HttpsHeader("Content-Type: application/json"));
                headers.Add(new HttpsHeader("authorization: bearer " + authObj.tokens.access_token));
                 

                foreach (var zone in zoneListObj.zone)
                {
                    if (zoneName == zone.name)
                    {
                        foreach (var room in roomListObj)
                        {
                            if (roomName == room.name)
                            {
                                foreach (var device in structuresAllObj.devices)
                                {
                                    foreach (var featureSets in device.featureSets)
                                    {
                                        foreach (var item in room.order)
                                        {
                                            if (item.ToString() == featureSets.featureSetId)
                                            {
                                                uint i = 0;
                                                foreach (var feature in featureSets.features)
                                                {
                                                    if (feature.type == "switch")
                                                    {
                                                        batch.features.Add(new FeatureBatch() { featureId = feature.featureId, value = 0 });
                                                        break;
                                                    }
                                                    i++;
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                }


                HttpRequests.PostAsync(@"https://publicapi.lightwaverf.com/v1/features/write", JsonConvert.SerializeObject(batch), headers, response);

            }
            catch (Exception exc)
            {
                ErrorLog.Error("Exception in LwrfGen2.lightwaveRfRoomAllOff, Message : {0}", exc.Message);
            }
        }
    }
}
