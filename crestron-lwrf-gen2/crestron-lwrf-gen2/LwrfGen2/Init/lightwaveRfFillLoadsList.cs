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
        internal static void lightwaveRfFillLoadsList()
        {
            try
            { 
                foreach (var room in roomListObj)
                {
                    List<string> buffSwitch = new List<string>();
                    List<string> buffDimLevel = new List<string>();
                    string zoneBuff = "";

                    foreach (var zone in zoneListObj.zone)
                    {
                        if (room.parentGroups[0] == zone.groupId)
                        {
                            zoneBuff = zone.name;
                        }
                    }

                    foreach (var featuresID in room.order)
                    {
                        foreach (var device in structuresAllObj.devices)
                        {
                            foreach (var featureSet in device.featureSets)
                            {
                                bool isDimmable = false;
                                foreach (var feature in featureSet.features)
                                {
                                    if (featuresID.ToString() == featureSet.featureSetId)
                                    {
                                        if (feature.type == "switch")
                                        {
                                            buffSwitch.Add(feature.featureId);
                                            foreach (var featureSearch in featureSet.features)
                                            {
                                                if (featureSearch.type == "dimLevel")
                                                {
                                                    isDimmable = true;
                                                    buffDimLevel.Add(featureSearch.featureId);
                                                    break;
                                                }
                                            }
                                            if (!isDimmable)
                                            {
                                                buffDimLevel.Add("");
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    lightwaveRfLoadsSwitch.Add(zoneBuff + "-" + room.name, buffSwitch);
                    lightwaveRfLoadsDimLevel.Add(zoneBuff + "-" + room.name, buffDimLevel);
                }
            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in LwrfGen2.lightwaveRfFillLoadsList: " + e.Message);
            }
        }
    }
}
