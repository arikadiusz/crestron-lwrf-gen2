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
        public static void lightwaveRfGetLoadValue(string roomName, string zoneName, int loadNum)
        {
            try
            {
                List<string> dictValue = lightwaveRfLoadsDimLevel.TryGetValue(zoneName + "-" + roomName, out dictValue) ? dictValue : null;
                if (dictValue != null)
                {
                    if (loadNum < dictValue.Count)
                    {
                        LightwaveRfFeatureReadRootObject value = lightwaveRfReadFeatureValue(dictValue[(int)loadNum]);
                        eventList[zoneName + "-" + roomName].Invoke(roomName, zoneName, loadNum, "dimLevel", (uint)value.value);
                    }
                    else
                        return;
                }
                else
                    return;

            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in LwrfGen2.lightwaveRfGetLoadValue, Message : {0}", e.Message);
            }
        }
    }
}
