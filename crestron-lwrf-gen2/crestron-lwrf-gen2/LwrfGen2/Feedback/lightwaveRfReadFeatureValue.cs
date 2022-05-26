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
        internal static LightwaveRfFeatureReadRootObject lightwaveRfReadFeatureValue(string featureId)
        {
            try
            {
                LightwaveRfFeatureReadRootObject featureObj = new LightwaveRfFeatureReadRootObject();
                List<HttpsHeader> headers = new List<HttpsHeader>();
                object contentObj = new object();
                string content = String.Empty;
                string response = String.Empty;

                headers.Add(new HttpsHeader("Content-Type: application/json"));
                headers.Add(new HttpsHeader("authorization: bearer " + authObj.tokens.access_token));

                response = HttpRequests.Get(@"https://publicapi.lightwaverf.com/v1/feature/" + featureId + @"?", headers);

                featureObj = JsonConvert.DeserializeObject<LightwaveRfFeatureReadRootObject>(response);
                
                return featureObj;
            }
            catch (Exception exc)
            {
                ErrorLog.Error("Exception in lightwaveRfReadFeatureValue, Message : {0}", exc.Message);
                return null;
            }
        }
    }
}
 