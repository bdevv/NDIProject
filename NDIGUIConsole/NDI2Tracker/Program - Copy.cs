﻿//----------------------------------------------------------------------------
//
// Copyright 2020 by Northern Digital Inc.
// All Rights Reserved
//
//----------------------------------------------------------------------------
// By using this Sample Code the licensee agrees to be bound by the
// provisions of the agreement found in the LICENSE.txt file.

// System
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

// 3rd Party
using Zeroconf;
using Force.Crc32;
// NDI
using NDI.CapiSample;
using NDI.CapiSample.Protocol;
using System.Text;
using System.IO;
using CAPINDIStreaming;

namespace NDI.CapiSampleStreaming
{
    /// <summary>
    /// This sample program showcases how to use the STREAM command supported by Vega Position Sensors since G.003.
    /// 
    /// The stream command can be used to continuously receive new frames of data instead of manually using the request-response method for commands.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            log("C# CAPI Sample v" + Capi.GetVersion());

            // Use the host specified if provided, otherwise try to find a network device.
            Capi cAPI;
            string host = "P9-01009";
            //if (host.Length > 0)
            //{
               // host = host; // args[0];
            //}
            //else
            //{
            //    log("Finding an NDI device on the network.");
            //    var task = GetIP();
            //    task.Wait(5000);

            //    if (task.IsCompleted && task.Result != null)
            //    {
            //        host = task.Result;
            //    }
            //    else
            //    {
            //        log("Could not find an NDI device on the network.");
            //   }
            //}

            if (host == "" || host == "help" || host == "-h" || host == "/?" || host == "--help")
            {
                // host argument was not provided and we couldn't find one
                if (host == "")
                {
                    log("Could not automatically detect an NDI device, please manually specify one.");
                }

                PrintHelp();
                return;
            }

            // Create a new CAPI instance based on the connection type
            cAPI = new CapiTcp(host);

            Run(cAPI);
        }

        /// <summary>
        /// Print usage information
        /// </summary>
        public static void PrintHelp()
        {
            log("");
            log("usage: CAPISampleStreaming.exe [<hostname>]");
            log("where:");
            log("\t<hostname>\t(optional) The measurement device's hostname, IP address.");
            log("example hostnames:");
            log("\tConnecting to device by IP address: 169.254.8.50");
            log("\tConnecting to device by hostname: P9-B0103.local");
        }

        /// <summary>
        /// Timestamped log output function.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        public static void log(string message)
        {
            string time = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            Console.WriteLine(time + " [CAPISample] " + message);
        }

        /// <summary>
        /// Find the first device on the network that is broadcasting the _ndi._tcp.local. service.
        /// </summary>
        /// <returns>Task with an IP string result.</returns>
        private static async Task<string> GetIP()
        {
            IReadOnlyList<IZeroconfHost> results = await ZeroconfResolver.ResolveAsync("_ndi._tcp.local.");
            foreach (var result in results)
            {
                return result.IPAddress;
            }

            return null;
        }

        private static bool InitializePorts(Capi cAPI)
        {
            // Polaris Section
            // ---
            // Request a new tool port handle so that we can load an SROM
            Port tool = cAPI.PortHandleRequest();
            if (tool == null)
            {
                log("Could not get available port for tool.");
            }
            else if (!tool.LoadSROM("sroms/973-3309PurpleRef.rom"))  //ref
            {
                log("Could not load SROM file for tool.");
                return false;
            }
            tool = cAPI.PortHandleRequest();
            if (tool == null)
            {
                log("Could not get available port for tool.");
            }
            else if (!tool.LoadSROM("sroms/960-556.rom"))   //Probe
            {
                log("Could not load SROM file for tool.");
                return false;
            }
            // ---

            // Initialize all ports not currently initialized
            var ports = cAPI.PortHandleSearchRequest(PortHandleSearchType.NotInit);
            foreach (var port in ports)
            {
                if (!port.Initialize())
                {
                    log("Could not initialize port " + port.PortHandle + ".");
                    return false;
                }

                if (!port.Enable())
                {
                    log("Could not enable port " + port.PortHandle + ".");
                    //return false;
                }
            }

            // List all enabled ports
            log("Enabled Ports:");
            ports = cAPI.PortHandleSearchRequest(PortHandleSearchType.All);
            foreach (var port in ports)
            {
                port.GetInfo();
                log(port.ToString());
            }

            return true;
        }

        /// <summary>
        /// Check for BX2 command support.
        /// </summary>
        /// <param name="apiRevision">API revision string returned by CAPI.GetAPIRevision()</param>
        /// <returns>True if supported.</returns>
        private static bool IsSupported(string apiRevision)
        {
            // Refer to the API guide for how to interpret the APIREV response
            char deviceFamily = apiRevision[0];
            int majorVersion = int.Parse(apiRevision.Substring(2, 3));

            // As of early 2020, the only NDI device supporting BX2/STREAM is the Vega
            // Vega is a Polaris device with API major version 003
            if (deviceFamily == 'G' && majorVersion >= 3)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Run the CAPI sample regardless of the connection method.
        /// </summary>
        /// <param name="cAPI">The configured CAPI protocol.</param>
        private static void Run(Capi cAPI)
        {
            // Be Verbose
            // cAPI.LogTransit = true;

            // Use the same log output format as this sample application
            cAPI.SetLogger(log);

            if (!cAPI.Connect())
            {
                log("Could not connect to " + cAPI.GetConnectionInfo());
                PrintHelp();
                return;
            }
            log("Connected");

            // Get the API Revision this will tell us if BX2 is supported.
            string revision = cAPI.GetAPIRevision();
            log("Revision:" + revision);

            // Check streaming compatibility
            if (!IsSupported(revision))
            {
                log("Your position sensor does not support streaming.");
                log("Streaming requires CAPI version G.003 or newer.");
                return;
            }

            if (!cAPI.Initialize())
            {
                log("Could not initialize.");
                return;
            }
            log("Initialized");

            // The Frame Frequency may not be possible to set on all devices, so an error response is okay.
            cAPI.SetUserParameter("Param.Tracking.Frame Frequency", "60");
            cAPI.SetUserParameter("Param.Tracking.Track Frequency", "2");

            // Read the final values
            log(cAPI.GetUserParameter("Param.Tracking.Frame Frequency"));
            log(cAPI.GetUserParameter("Param.Tracking.Track Frequency"));

            // Initialize tool ports
            if (!InitializePorts(cAPI))
            {
                return;
            }

            if (!cAPI.TrackingStart())
            {
                log("Could not start tracking.");
                return;
            }
            log("TrackingStarted");

            // Start streaming
            StreamCommand cmd = cAPI.StartStreaming("BX2 --6d=tools", "BX2");

            // Add a listener for new streamed packets
            cmd.Listeners.Add((Packet p) =>
            {
                // Received an incorrect packet type
                if (!(p is BinaryPacket))
                {
                    log($"Got {p.GetType()}. Expected BinaryPacket.");
                    return;
                }

                // Packet could not be read correctly
                if (!p.IsValid)
                {
                    log($"Invalid {p.GetType()} received.");
                    return;
                }

                // We received the correct packet type with our stream id, so parse the BX2 packet and output all tools in the frame of data.
                var tools = Bx2Reply.Parse((BinaryPacket)p);
                QuatTransformation dtRefQuatXfrm = new QuatTransformation();
                QuatTransformation dtRefQuatXfrmInv = new QuatTransformation();
                QuatTransformation dtPortQuatXfrm = new QuatTransformation();
                QuatTransformation dtNewQuatXfrm = new QuatTransformation();
                foreach (var t in tools)
                {
                    if(t.transform.toolHandle == 1)
                    {
                        dtRefQuatXfrm.rotation.q0 = t.transform.orientation.q0;
                        dtRefQuatXfrm.rotation.qx = t.transform.orientation.qx;
                        dtRefQuatXfrm.rotation.qy = t.transform.orientation.qy;
                        dtRefQuatXfrm.rotation.qz = t.transform.orientation.qz;
                        dtRefQuatXfrm.translation.x = t.transform.position.x;
                        dtRefQuatXfrm.translation.y = t.transform.position.y;
                        dtRefQuatXfrm.translation.z = t.transform.position.z;
                    }
                    else if(t.transform.toolHandle == 2)
                    {
                        dtPortQuatXfrm.rotation.q0 = t.transform.orientation.q0;
                        dtPortQuatXfrm.rotation.qx = t.transform.orientation.qx;
                        dtPortQuatXfrm.rotation.qy = t.transform.orientation.qy;
                        dtPortQuatXfrm.rotation.qz = t.transform.orientation.qz;
                        dtPortQuatXfrm.translation.x = t.transform.position.x;
                        dtPortQuatXfrm.translation.y = t.transform.position.y;
                        dtPortQuatXfrm.translation.z = t.transform.position.z;
                    }
                    log(t.ToString());
                }
                dtRefQuatXfrmInv = Conversion.QuatInverseXfrm(dtRefQuatXfrm);
                dtNewQuatXfrm = Conversion.QuatCombineXfrms(dtPortQuatXfrm, dtRefQuatXfrmInv);
                WriteToTmpFile(dtNewQuatXfrm);
            });

            // Stream data for 5 seconds then get some parameters and wait 5 more seconds.
            Task.Delay(100000).Wait();
            log(cAPI.GetUserParameter("Param.Tracking.Frame Frequency"));
            log(cAPI.GetUserParameter("Param.Tracking.Track Frequency"));
            Task.Delay(100000).Wait();

            // Stop streaming and wait for final packets to come in
            cAPI.StopStreaming(cmd);
            Task.Delay(10000).Wait();

            if (!cAPI.TrackingStop())
            {
                log("Could not stop tracking.");
                return;
            }
            log("TrackingStopped");

            if (!cAPI.Disconnect())
            {
                log("Could not disconnect.");
                return;
            }
            log("Disconnected");
        }
        private static void WriteToTmpFile(QuatTransformation dtNewQuatXfrmt)
        {
           
            
            double Tx = dtNewQuatXfrmt.translation.x;
            double Ty = dtNewQuatXfrmt.translation.y;
            double Tz = dtNewQuatXfrmt.translation.z;
            double Q0 = dtNewQuatXfrmt.rotation.q0;
            double Qx = dtNewQuatXfrmt.rotation.qx;
            double Qy = dtNewQuatXfrmt.rotation.qy;
            double Qz = dtNewQuatXfrmt.rotation.qz;
            long prevTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            long timestamp = prevTime % 10000000000;
            string tmpString = "V  Q  " + Tx + "  " + Ty + "  " + Tz + "  " + Q0 + "  " + Qx + "  " + Qy + "  " + Qz + " " + timestamp.ToString() + "l";
            Console.WriteLine(tmpString);
            // string checkString = "V  Q  -22.539063  -184.778229  118.444290  -0.014033  -0.093944  0.031202  0.994772 1006210395l";
            // Convert a C# string to a byte array  
            byte[] bytes = Encoding.ASCII.GetBytes(tmpString + '\n');

            long crc = Crc32Algorithm.Compute(bytes, 0, bytes.Length);
            string tmptag = tmpString + '\n' + ";" + crc.ToString("x8") + '\0' + '\0';
            File.WriteAllText("C:\\Tracker\\MevisNDI_State.tmp", tmptag);
           
            //TrackingGridView.data
        }
    }
}
