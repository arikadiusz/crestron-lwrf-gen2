/*MIT License

Copyright(c) 2022
Arkadiusz Rycyk

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


/*
 * fix this bug, happens on teston 
 * 
 * CP4>err
  1. Error: SimplSharpPro[App01] # 2022-05-25 16:43:58 # Exception in HttpRequests.Get: Object reference not set to an instance of an object.
  2. Error: SimplSharpPro[App01] # 2022-05-25 16:43:58 # Exception in LwrfGen2.lightwaveRfRoomLoadLevelFeedback, Message : Object reference not set to an instance of an object.
Total Errors Logged = 2
*/ 

using System;
using Crestron.SimplSharp;                          	// For Basic SIMPL# Classes
using Crestron.SimplSharpPro;                       	// For Basic SIMPL#Pro classes
using Crestron.SimplSharpPro.CrestronThread;        	// For Threading
using Crestron.SimplSharpPro.Diagnostics;		    	// For System Monitor Access
using Crestron.SimplSharpPro.DeviceSupport;         	// For Generic Device Support
using crestron_lwrf_gen2;
using Crestron.SimplSharp.Net.Https;
using System.Collections.Generic;

namespace crestron_lwrf_gen2_test
{
    public class ControlSystem : CrestronControlSystem
    {
        public ControlSystem()
            : base()
        {
            try
            {
                Thread.MaxNumberOfUserThreads = 20;
                 
                //Subscribe to the controller events (System, Program, and Ethernet)
                CrestronEnvironment.SystemEventHandler += new SystemEventHandler(_ControllerSystemEventHandler);
                CrestronEnvironment.ProgramStatusEventHandler += new ProgramStatusEventHandler(_ControllerProgramEventHandler);
                CrestronEnvironment.EthernetEventHandler += new EthernetEventHandler(_ControllerEthernetEventHandler);  
                 
                CrestronConsole.AddNewConsoleCommand((a) => 
                {
                    try   
                    {
                        LwrfGen2.Initialize(@"https://auth.lightwaverf.com/v2/lightwaverf/autouserlogin/lwapps", "example@gmail.com", "password", "xxx.xxx.xxx.xxx", 1600, ()=> 
                        {
                            try
                            {
                                #region Events registration
                                List<string> events = new List<string>();

                                foreach (var item in LwrfGen2.eventList.Keys)
                                {
                                    events.Add(item); 
                                }

                                foreach (var item in events)
                                {
                                    LwrfGen2.eventList[item] += LwrfEventsHandler;
                                }
                                #endregion

                                #region Request Feedback  
                                foreach (var room in LwrfGen2.GetLwrfRooms())
                                {
                                    LwrfGen2.lightwaveRfGetLoadInfo(room.roomName, room.zoneName); 
                                }
                                #endregion 
                            } 
                            catch (Exception e)
                            {
                                ErrorLog.Error("Exception in ControlSystem.LwrfGen2.Initialize: {0}", e.Message);
                            }

                        });
                    } 
                    catch (Exception e) 
                    { 
                        ErrorLog.Error("Exception in start: {0}", e.Message);
                    } 
                }, "start", "", ConsoleAccessLevelEnum.AccessAdministrator); 


                CrestronConsole.AddNewConsoleCommand((a) =>
                {
                    try 
                    {
                        CrestronConsole.PrintLine("Sending power on!");
                        LwrfGen2.lightwaveRfRoomAllOn("Living", "Ground", (res, err) => { });  
                    }
                    catch (Exception e)
                    {
                        ErrorLog.Error("Exception in teston: {0}", e.Message); 
                    }
                }, "teston", "", ConsoleAccessLevelEnum.AccessAdministrator); 


                CrestronConsole.AddNewConsoleCommand((a) => 
                {
                    try
                    {
                        CrestronConsole.PrintLine("Sending power off!");
                        LwrfGen2.lightwaveRfRoomAllOff("Living", "Ground", (res, err) => { });
                    }
                    catch (Exception e)
                    {
                        ErrorLog.Error("Exception in testoff: {0}", e.Message);
                    } 
                }, "testoff", "", ConsoleAccessLevelEnum.AccessAdministrator);


                CrestronConsole.AddNewConsoleCommand((a) =>
                {
                    try
                    {
                        CrestronConsole.PrintLine("Sending power on!");
                        LwrfGen2.lightwaveRfRoomLoadOn("Living", "Ground", 1, (res, err) => { });
                    }
                    catch (Exception e)
                    {
                        ErrorLog.Error("Exception in testloadon: {0}", e.Message);
                    }
                }, "testloadon", "", ConsoleAccessLevelEnum.AccessAdministrator);


                CrestronConsole.AddNewConsoleCommand((a) =>
                {
                    try 
                    {
                        CrestronConsole.PrintLine("Sending power on!"); 
                        LwrfGen2.lightwaveRfRoomLoadOff("Living", "Ground", 1, (res, err) => { }); 
                    } 
                    catch (Exception e)
                    {
                        ErrorLog.Error("Exception in testloadoff: {0}", e.Message);
                    }
                }, "testloadoff", "", ConsoleAccessLevelEnum.AccessAdministrator);

                CrestronConsole.AddNewConsoleCommand((a) =>
                {
                    try
                    {
                        CrestronConsole.PrintLine("Sending level!");
                        LwrfGen2.lightwaveRfRoomLoadLevel("Living", "Ground", 1, int.Parse(a), (res, err) => { });
                    }
                    catch (Exception e) 
                    {
                        ErrorLog.Error("Exception in testloadlevel: {0}", e.Message);
                    } 
                }, "testloadlevel", "", ConsoleAccessLevelEnum.AccessAdministrator);

                CrestronConsole.AddNewConsoleCommand((a) =>
                {
                    try
                    {
                        foreach (var room in LwrfGen2.GetLwrfRooms())
                        {
                            CrestronConsole.PrintLine(room.zoneName + "-" + room.roomName);
                            foreach (var loadName in LwrfGen2.GetLwrfLoadNames(room.roomName, room.zoneName))
                            {
                                CrestronConsole.PrintLine(loadName);
                            } 
                        }
                    }
                    catch (Exception e)
                    {
                        ErrorLog.Error("Exception in testloadnames: {0}", e.Message);
                    }
                }, "testloadnames", "", ConsoleAccessLevelEnum.AccessAdministrator);


                CrestronConsole.AddNewConsoleCommand((a) =>
                {
                    try
                    {
                        foreach (var room in LwrfGen2.GetLwrfRooms())
                        { 
                            CrestronConsole.PrintLine(room.zoneName + "-" + room.roomName);
                            foreach (var loadDimmable in LwrfGen2.GetLwrfLoadDimming(room.roomName, room.zoneName))
                            { 
                                CrestronConsole.PrintLine(loadDimmable.ToString());
                            } 
                        } 
                    }
                    catch (Exception e) 
                    {
                        ErrorLog.Error("Exception in testloaddim: {0}", e.Message);
                    }
                }, "testloaddim", "", ConsoleAccessLevelEnum.AccessAdministrator); 
            }
            catch (Exception e)
            {
                ErrorLog.Error("Error in the constructor: {0}", e.Message);
            } 
        }

        public void LwrfEventsHandler (string room, string zone, int channel, string type, double value) 
        {
            try
            { 
                CrestronConsole.PrintLine($"room={room} zone={zone} channel={channel} type={type} value={value}"); 
            } 
            catch (Exception e) 
            {
                ErrorLog.Error("Exception in LwrfEventsHandler: {0}", e.Message);
            }
        }
        public override void InitializeSystem()
        {
            try
            {

            }
            catch (Exception e)
            {
                ErrorLog.Error("Error in InitializeSystem: {0}", e.Message);
            }
        } 

        void _ControllerEthernetEventHandler(EthernetEventArgs ethernetEventArgs)
        {
            switch (ethernetEventArgs.EthernetEventType)
            {//Determine the event type Link Up or Link Down
                case (eEthernetEventType.LinkDown):
                    //Next need to determine which adapter the event is for. 
                    //LAN is the adapter is the port connected to external networks.
                    if (ethernetEventArgs.EthernetAdapter == EthernetAdapterType.EthernetLANAdapter) 
                    {
                        //
                    }
                    break; 
                case (eEthernetEventType.LinkUp):
                    if (ethernetEventArgs.EthernetAdapter == EthernetAdapterType.EthernetLANAdapter)
                    {
                         
                    }
                    break;
            }
        }

        void _ControllerProgramEventHandler(eProgramStatusEventType programStatusEventType)
        {
            switch (programStatusEventType)
            {
                case (eProgramStatusEventType.Paused):
                    //The program has been paused.  Pause all user threads/timers as needed.
                    break;
                case (eProgramStatusEventType.Resumed):
                    //The program has been resumed. Resume all the user threads/timers as needed.
                    break;
                case (eProgramStatusEventType.Stopping):
                    LwrfGen2.Dispose();
                    //The program has been stopped.
                    //Close all threads. 
                    //Shutdown all Client/Servers in the system.
                    //General cleanup.
                    //Unsubscribe to all System Monitor events
                    break;
            }

        }

        void _ControllerSystemEventHandler(eSystemEventType systemEventType)
        {
            switch (systemEventType)
            {
                case (eSystemEventType.DiskInserted):
                    //Removable media was detected on the system
                    break;
                case (eSystemEventType.DiskRemoved):
                    //Removable media was detached from the system
                    break;
                case (eSystemEventType.Rebooting):
                    //The system is rebooting. 
                    //Very limited time to preform clean up and save any settings to disk.
                    break; 
            }

        }
    }
}