/*MIT License

Copyright(c) [year]
[fullname]

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/

using Crestron.SimplSharp;
using Crestron.SimplSharp.Net.Http;
using Crestron.SimplSharp.Net.Https;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace crestron_lwrf_gen2
{
    public static partial class LwrfGen2
    {
        public delegate void LightwaveRfLoadChangedEventHandler(string room, string zone, int channel, string type, double value);
        public delegate void LightwaveRfInitializedHandler();
        /// <summary>
        /// Events trigger per room. Subscribe to all or indyvidualy using key, where key is `zone-room`
        /// </summary>
        public static Dictionary<string, LightwaveRfLoadChangedEventHandler> eventList = new Dictionary<string, LightwaveRfLoadChangedEventHandler>();

        public static HttpServer myServer { get; set; }
        public static int serverPort { get; set; }
        public static string feedbackServerIp { get; set; }
        public static int feedbackServerPort { get; set; }

        public static bool INITIALIZED { get; set; }


        internal static AuthRootObject authObj { get; set; }
        internal static StructuresMainRootObject structuresMainObj { get; set; }
        internal static StructuresAllRootObject structuresAllObj { get; set; }
        internal static List<RoomListRootObject> roomListObj { get; set; }
        internal static ZoneRootObject zoneListObj { get; set; }
        internal static List<LightwaveRFEventListRootObject> eventsListObj { get; set; }
        public static Dictionary<string, List<string>> lightwaveRfLoadsSwitch = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> lightwaveRfLoadsDimLevel = new Dictionary<string, List<string>>();

        internal static CTimer authRefreshTimer { get; set; }

        internal static string authUrl, authLogin, authPassword;

        public static CTimer httpServerQueueTimer { get; set; }
        public static Queue<QueueItem> httpServerQueue { get; set; }

        public static void Initialize(string url, string login, string password, string feedbackServerIp, int feedbackServerPort, LightwaveRfInitializedHandler initializedHandler)
        {
            try
            {
                // Initialize http server for feedback
                httpServerQueue = new Queue<QueueItem>();
                httpServerQueueTimer = new CTimer(QueueTimerHandler, null, 150, 200); 
                
                myServer = new HttpServer();
                myServer.Port = (int)feedbackServerPort;
                myServer.ServerName = "0.0.0.0";
                myServer.OnHttpRequest += new OnHttpRequestHandler(HttpCallbackFunction); 
                myServer.Active = true;
                authUrl = url;
                authLogin = login; 
                authPassword = password;

                LwrfGen2.feedbackServerIp = feedbackServerIp;
                LwrfGen2.feedbackServerPort = feedbackServerPort; 

                lightwaveRfPostAuth(url, login, password, (res1,err1) => 
                {
                    if (err1 == HTTPS_CALLBACK_ERROR.COMPLETED)
                    {
                        try
                        {
                            authObj = JsonConvert.DeserializeObject<AuthRootObject>(res1.ContentString);
                        }
                        catch (Exception e)
                        {
                            ErrorLog.Error("Exception in AuthFailed (check user/password): {0}", e.Message);
                        }

                        lightwaveRfGetStructures((res2, err2) =>
                        {
                            if(err2 == HTTPS_CALLBACK_ERROR.COMPLETED)
                            {
                                structuresMainObj = JsonConvert.DeserializeObject<StructuresMainRootObject>(res2.ContentString);

                                lightwaveRfGetStructuresAll((res3, err3) => 
                                {
                                    if (err3 == HTTPS_CALLBACK_ERROR.COMPLETED)
                                    { 
                                        structuresAllObj = JsonConvert.DeserializeObject<StructuresAllRootObject>(res3.ContentString);

                                        lightwaveRfRoomList((res4, err4) => 
                                        {
                                            if(err4 == HTTPS_CALLBACK_ERROR.COMPLETED)
                                            {
                                                roomListObj = JsonConvert.DeserializeObject<List<RoomListRootObject>>(res4.ContentString);

                                                lightwaveRfZoneList((res5, err5) => 
                                                {
                                                    if(err5 == HTTPS_CALLBACK_ERROR.COMPLETED)
                                                    {
                                                        zoneListObj = JsonConvert.DeserializeObject<ZoneRootObject>(res5.ContentString);


                                                        lightwaveRfFillLoadsList();

                                                        RegisterHooks();

                                                        INITIALIZED = true;

                                                        initializedHandler();

                                                    }
                                                }); 
                                            }
                                        });
                                    }
                                });
                            }
                        });
                    }
                });

                if (authRefreshTimer != null)
                    authRefreshTimer.Dispose();

                authRefreshTimer = new CTimer(authRefreshTimerHandler, null, 60000, 36000000); // refresh every 10h
            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in Initialize: " + e.Message);
            }
        } 

        public static void RegisterHooks()
        {
            try
            {
                foreach (var zone in zoneListObj.zone)
                {
                    foreach (var room in zone.rooms)
                    {
                        string roomName = string.Empty;

                        foreach (var roomObj in LwrfGen2.roomListObj)
                        {
                            if (roomObj.groupId == room)
                                roomName = roomObj.name;
                        }

                        eventList.Add((zone.name + "-" + roomName), (string r, string z, int c, string t, double v) => { });
                        lightwaveRfEventCreateHooks(zone.name, roomName);
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in LwrfGen2.RegisterHook: {0}", e.Message);
            }
        }

        public static List<LwrfRoom> GetLwrfRooms()
        {
            List<LwrfRoom> retValue = new List<LwrfRoom>();
            
            try
            {
                foreach (var zone in zoneListObj.zone)
                {
                    foreach (var room in zone.rooms)
                    {
                        string roomName = string.Empty;

                        foreach (var roomObj in LwrfGen2.roomListObj)
                        {
                            if (roomObj.groupId == room)
                                roomName = roomObj.name;
                        }

                        retValue.Add(new LwrfRoom() { roomName = roomName, zoneName = zone.name });
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in LwrfGen2.GetLwrfRooms: {0}", e.Message);
            }
            return retValue;
        }

        public static List<string> GetLwrfLoadNames(string roomName, string zoneName)
        {
            List<string> retValue = new List<string>();

            try
            {
                for (int i = 1; i <= LwrfGen2.lightwaveRfGetLoadsNumber(roomName, zoneName); i++)
                {
                    retValue.Add(LwrfGen2.lightwaveRfGetLoadName(roomName, zoneName, i));
                }
            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in LwrfGen2.GetLwrfLoadNames: {0}", e.Message);
            }
            return retValue;
        }

        public static List<bool> GetLwrfLoadDimming(string roomName, string zoneName)
        {
            List<bool> retValue = new List<bool>();

            try
            {
                for (int i = 1; i <= LwrfGen2.lightwaveRfGetLoadsNumber(roomName, zoneName); i++)
                {
                    retValue.Add(LwrfGen2.lightwaveRfGetLoadDimmability(roomName, zoneName, i));
                }
            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in LwrfGen2.GetLwrfLoadNames: {0}", e.Message);
            }
            return retValue;
        }

        internal static void authRefreshTimerHandler(object o)
        {

            try
            {
                lightwaveRfPostAuth(authUrl, authLogin, authPassword, lightwaveRfPostAuthHandler);
            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in LwrfGen2.authRefreshTimerHandler: {0}", e.Message);
            }
        }

        internal static void lightwaveRfPostAuthHandler(HttpsClientResponse response, HTTPS_CALLBACK_ERROR error)
        {
            try
            {
                if (error == HTTPS_CALLBACK_ERROR.COMPLETED)
                {
                    authObj = JsonConvert.DeserializeObject<AuthRootObject>(response.ContentString); 
                }
            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in postHandler: {0}", e.Message);
            }
        }
         
        public static void Dispose()
        {
            try
            {
                if (authRefreshTimer != null)
                    authRefreshTimer.Dispose();

                if (myServer != null)
                    myServer.Dispose();

                if (httpServerQueueTimer != null)
                    httpServerQueueTimer.Dispose();
            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in LwrfGen2.Dispose: {0}", e.Message);
            }
        }
    }
}
