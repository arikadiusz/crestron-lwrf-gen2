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
        public static int lightwaveRfGetLoadsNumber(string roomName, string zoneName)
        {
            int retValue = 0;

            try
            {

                List<string> dictValue = lightwaveRfLoadsSwitch.TryGetValue(zoneName + "-" + roomName, out dictValue) ? dictValue : null;

                if (dictValue != null)
                {
                    foreach (var item in dictValue)
                    {
                        retValue++;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in LwrfGen2.lightwaveRfGetLoadsNumber, Message : {0}", e.Message);
            }

            return retValue;
        }
    }
}
