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
        public static bool lightwaveRfGetLoadDimmability(string roomName, string zoneName, int loadNum)
        {
            try
            {
                loadNum = loadNum - 1;


                List<string> dictValue = lightwaveRfLoadsDimLevel.TryGetValue(zoneName + "-" + roomName, out dictValue) ? dictValue : null;
                if (dictValue != null)
                {
                    if (loadNum < dictValue.Count)
                    {
                        if (dictValue[(int)loadNum].Length > 0) 
                            return true; 
                        else
                            return false;
                    }
                }

                return false;
            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in LwrfGen2.lightwaveRfGetLoadDimmability, Message : {0}", e.Message);
                return false;
            }
        }
    }
}
