using Crestron.SimplSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crestron_lwrf_gen2
{
    public static partial class LwrfGen2
    {
        public static void lightwaveRfRoomLoadLevelFeedback(string featureId, double loadLevel)
        {
            try
            {
                foreach (var device in structuresAllObj.devices)
                {
                    foreach (var featureSets in device.featureSets)
                    {
                        foreach (var feature in featureSets.features)
                        {
                            if (feature.featureId == featureId)
                            {
                                foreach (var zone in zoneListObj.zone)
                                {
                                    foreach (var room in roomListObj)
                                    {
                                        int i = 0; 
                                        if (zone.groupId == room.parentGroups[0])
                                        {
                                            foreach (var item in room.order)
                                            {

                                                if(featureSets != null && item != null)
                                                {
                                                    if (item.ToString() == featureSets.featureSetId)
                                                    {
                                                        if (feature.type == "switch")
                                                        {
                                                            if (loadLevel > 0)
                                                            {
                                                                eventList[zone.name + "-" + room.name].Invoke(room.name, zone.name, i + 1, feature.type, loadLevel);
                                                                if (!lightwaveRfGetLoadDimmability(room.name, zone.name, i + 1))
                                                                {
                                                                    eventList[zone.name + "-" + room.name].Invoke(room.name, zone.name, i + 1, "dimLevel", 100);
                                                                }
                                                                else
                                                                {
                                                                    foreach (var searchFeature in featureSets.features)
                                                                    {
                                                                        if (searchFeature.type == "dimLevel")
                                                                        {
                                                                            LightwaveRfFeatureReadRootObject value = lightwaveRfReadFeatureValue(searchFeature.featureId);
                                                                            eventList[zone.name + "-" + room.name].Invoke(room.name, zone.name, i + 1, "dimLevel", (uint)value.value);
                                                                        }
                                                                    }
                                                                }

                                                            }
                                                            else
                                                            {
                                                                eventList[zone.name + "-" + room.name].Invoke(room.name, zone.name, i + 1, feature.type, loadLevel);
                                                                eventList[zone.name + "-" + room.name].Invoke(room.name, zone.name, i + 1, "dimLevel", 0);
                                                            }
                                                        }
                                                        else if (feature.type == "dimLevel")
                                                        {
                                                            eventList[zone.name + "-" + room.name].Invoke(room.name, zone.name, i + 1, feature.type, loadLevel);
                                                        }
                                                        break;
                                                    }
                                                }

                                                i++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception exc)
            {
                ErrorLog.Error("Exception in LwrfGen2.lightwaveRfRoomLoadLevelFeedback, Message : {0}", exc.Message);
            }
        }
    }
}
