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
        internal static void lightwaveRfPostAuth(string url_string, string login, string password, HTTPSClientResponseCallback response)
        {
            try 
            {
                List<HttpsHeader> headers = new List<HttpsHeader>();
                object contentObj = new object();
                string content = String.Empty;

                headers.Add(new HttpsHeader("Content-Type: application/json"));
                headers.Add(new HttpsHeader("cache-control: no-cache"));
                headers.Add(new HttpsHeader("x-lwrf-appid: ios-01"));

                contentObj = new
                {
                    email = login, 
                    password = password,
                    version = "2.0"
                };

                content = JsonConvert.SerializeObject(contentObj);

                HttpRequests.PostAsync(url_string, content, headers, response);
            }
            catch (Exception exc)
            {
                ErrorLog.Error("Exception in LwrfGen2.lightwaveRfPostAuth: {0}", exc.Message);
            }
        }
    }
}