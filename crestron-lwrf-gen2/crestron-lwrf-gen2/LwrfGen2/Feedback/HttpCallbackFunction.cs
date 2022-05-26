using Crestron.SimplSharp;
using Crestron.SimplSharp.Net;
using Crestron.SimplSharp.Net.Http;
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
        public static void HttpCallbackFunction(object sender, OnHttpRequestArgs e)
        {
            try
            {
                if (e.Response.Code == 200)
                {
                    //CrestronConsole.PrintLine("Path = {0}", HttpUtility.UrlDecode(e.Request.Path));
                    //CrestronConsole.PrintLine("Message = {0}", e.Request.ContentString);

                    if (e.Request.Path == "/endpoint" && INITIALIZED)
                    {
                        httpServerQueue.Enqueue(new QueueItem()
                        {
                            ContentString = e.Request.ContentString
                        });
                    }

                    e.Response.Header.SetHeaderValue("Content-Type", "text/html");
                    e.Response.ResponseText = "OK"; 
                    e.Response.Code = 200;
                }
                else
                {
                    CrestronConsole.PrintLine(e.Response.ContentString);
                }
            }
            catch (Exception exc)
            {
                ErrorLog.Error("Error in LwrfGen2.HttpCallback: {0}", exc.Message);
            }
        }

        public static void QueueTimerHandler(object o)
        {
            try
            {
                WebHookRootObject responseObj = new WebHookRootObject();

                if (httpServerQueue != null)
                {
                    if (httpServerQueue.Count > 0)
                    {
                        QueueItem item = httpServerQueue.Dequeue();

                        if (item != null)
                        {
                            responseObj = JsonConvert.DeserializeObject<WebHookRootObject>(item.ContentString);
                            lightwaveRfRoomLoadLevelFeedback(responseObj.triggerEvent.id, (uint)responseObj.payload.value);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in LwrfGen2.QueueTimerHandler: {0}", e.Message);
            }
        }
    }
}
