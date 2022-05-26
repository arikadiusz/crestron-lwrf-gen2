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
        public static void lightwaveRfEventCreateHooks(string zoneName, string roomName)
        {
            try
            {
                LightwaveRFFeedbackCreateRootObject lightwaveRfCreateEventObj = new LightwaveRFFeedbackCreateRootObject();
                LightwaveRFFeedbackUpdateRootObject lightwaveRfUpdateEventObj = new LightwaveRFFeedbackUpdateRootObject();

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
                                                foreach (var feature in featureSets.features)
                                                {
                                                    if (feature.type == "switch" || feature.type == "dimLevel")
                                                    {
                                                        LightwaveRFFeedbackReadRootObject readEventObj = new LightwaveRFFeedbackReadRootObject();

                                                        lightwaveRfCreateEventObj.@ref = zoneName.Replace(" ", "_") + "-" + roomName.Replace(" ", "_") + "-" + featureSets.name.Replace(" ", "_") + "-" + feature.type;
                                                        lightwaveRfUpdateEventObj.@ref = zoneName.Replace(" ", "_") + "-" + roomName.Replace(" ", "_") + "-" + featureSets.name.Replace(" ", "_") + "-" + feature.type;
                                                        readEventObj = lightwaveRfGetEventRead(lightwaveRfCreateEventObj.@ref);
                                                        CrestronConsole.PrintLine("Read object ID : " + readEventObj.id);

                                                        if (readEventObj == null)
                                                        {
                                                            CrestronConsole.PrintLine("Object Doesn't Exist, Creating new one");
                                                            lightwaveRfCreateEventObj.url = "http://" + feedbackServerIp + ":" + feedbackServerPort + "/endpoint";
                                                            lightwaveRfCreateEventObj.events.Add(new LightwaveRFFeedbackCreateEvent() { type = "feature", id = feature.featureId });
                                                            lightwaveRfPatchEventCreate(lightwaveRfCreateEventObj);
                                                        }
                                                        else
                                                        {
                                                            CrestronConsole.PrintLine("Object Does Exist, Deleting");
                                                            lightwaveRfDeleteEvent(lightwaveRfUpdateEventObj.@ref);
                                                            CrestronConsole.PrintLine("Then Adding");
                                                            lightwaveRfCreateEventObj.url = "http://" + feedbackServerIp + ":" + feedbackServerPort + "/endpoint";
                                                            lightwaveRfCreateEventObj.events.Add(new LightwaveRFFeedbackCreateEvent() { type = "feature", id = feature.featureId });
                                                            lightwaveRfPatchEventCreate(lightwaveRfCreateEventObj);
                                                        }

                                                        lightwaveRfCreateEventObj = new LightwaveRFFeedbackCreateRootObject();
                                                        lightwaveRfUpdateEventObj = new LightwaveRFFeedbackUpdateRootObject();
                                                    }
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
            }
            catch (Exception exc)
            {
                ErrorLog.Error("Exception in LwrfGen2.lightwaveRfPostEventCreate, Message : {0}", exc.Message);
            }
        }
    }
}
