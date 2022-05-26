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
        public static void lightwaveRfGetLoadInfo(string roomName, string zoneName)
        {
            try
            {

                List<string> dictValue = lightwaveRfLoadsSwitch.TryGetValue(zoneName + "-" + roomName, out dictValue) ? dictValue : null;

                int i = 0;

                if (dictValue != null)
                {
                    foreach (var item in dictValue)
                    {
                        LightwaveRfFeatureReadRootObject value = lightwaveRfReadFeatureValue(item);
                        if (value.value > 0)
                        {
                            if (!lightwaveRfGetLoadDimmability(roomName, zoneName, i+1))
                            {
                                eventList[zoneName + "-" + roomName].Invoke(roomName, zoneName, i + 1, "dimLevel", 100);
                            }
                            else
                            {
                                LightwaveRfFeatureReadRootObject dimValue = lightwaveRfReadFeatureValue(lightwaveRfLoadsDimLevel[zoneName + "-" + roomName][(int)i]);
                                eventList[zoneName + "-" + roomName].Invoke(roomName, zoneName, i + 1, "dimLevel", (uint)dimValue.value);
                            }
                        }
                        else
                        {
                            if (!lightwaveRfGetLoadDimmability(roomName, zoneName, i+1))
                            {
                                eventList[zoneName + "-" + roomName].Invoke(roomName, zoneName, i + 1, "dimLevel", 0);
                            }
                        }
                        eventList[zoneName + "-" + roomName].Invoke(roomName, zoneName, i + 1, "switch", (uint)value.value);
                        i++;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in LwrfGen2.lightwaveRfGetLoadInfo, Message : {0}", e.Message);
            }
        }
    }
}
