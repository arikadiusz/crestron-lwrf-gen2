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
        public static string lightwaveRfGetLoadName(string roomName, string zoneName, int loadNum)
        {
            string returnValue = "";

            try
            {

                loadNum = loadNum - 1;


                foreach (var zone in zoneListObj.zone)
                {
                    if (zoneName == zone.name) 
                    {
                        foreach (var item in roomListObj)
                        { 
                            if (roomName == item.name)
                            {
                                foreach (var subItem in structuresAllObj.devices)
                                {
                                    foreach (var subsubItem in subItem.featureSets)
                                    {
                                        foreach (var feature in subsubItem.features)
                                        {
                                            if (feature.type == "switch")
                                            {
                                                List<string> dictValue = lightwaveRfLoadsSwitch.TryGetValue(zoneName + "-" + roomName, out dictValue) ? dictValue : null;
                                                if (dictValue != null)
                                                {
                                                    if (loadNum < dictValue.Count)
                                                    {
                                                        if (dictValue[(int)loadNum] == feature.featureId)
                                                        {
                                                            returnValue = subsubItem.name;
                                                            break;
                                                        }
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

                return returnValue;
            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in LwrfGen2lightwaveRfGetLoadName, Message : {0}", e.Message);
                return "Exception in LwrfGen2.lightwaveRfGetLoadName, Message :" + e.Message;
            }
        }
    }
}
