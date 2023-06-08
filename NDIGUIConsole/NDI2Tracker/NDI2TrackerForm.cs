using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using NDI.CapiSample.Protocol;
using NDI.CapiSample;

using System.Threading.Tasks;

// 3rd Party
using Zeroconf;
using Force.Crc32;
// NDI
using CAPINDIStreaming;
using System.Numerics;
using System.Linq;
using NDI.CapiSample.Data;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Controls;
using System.Reflection;

namespace CGUINDIStreaming
{

    public partial class NDI2TrackerForm : Form
    {
        const string iniFilePath = "config.ini";
        static string tmpFilePath = "";
        static int sampleRate;
        static int ReferenceHandle;
        static int ActiveHandle;
        static bool stopFlag = false;
        static bool isUsingTmpFile = true;
        static NDI2TrackerForm thisForm;
        Capi cAPI;
        public delegate void ChangePortInformation(string message);
        public static ChangePortInformation myDelegate;
        public static StreamCommand cmd;
        private static List<PortInformation> PortHandles;
        static string host = "P9-01009";

        public NDI2TrackerForm()
        {
            InitializeComponent();
            myDelegate = new ChangePortInformation(ChangePortFunction);


            saveFileDialog1.Filter = "tmp files(*.tmp)|*.tmp|All files(*.*)|*.*";
            openFileDialog1.Filter = "rom files(*.rom)|*.rom|All files(*.*)|*.*";
            BtnStopTracking.Enabled = false;
            PortHandles = new List<PortInformation>();

            thisForm = this;
            CmbReferenceHandle.Items.Add("(None)");

            FileStream fs = File.Open(iniFilePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
            var inireader = new StreamReader(fs);
            while (!inireader.EndOfStream)
            {
                string settingtmp = inireader.ReadLine();
                if (settingtmp.Contains("tmpFilePath"))
                {
                    txtTMPFilePath.Text = settingtmp.Substring(settingtmp.IndexOf("=") + 1);
                    tmpFilePath = txtTMPFilePath.Text;

                }
                else if (settingtmp.Contains("sampleRate"))
                {
                    sampleRate = int.Parse(settingtmp.Substring(settingtmp.IndexOf("=") + 1));

                }
                else if (settingtmp.Contains("rom"))
                {
                    DataGridViewRow row = (DataGridViewRow)TrackingGridView.Rows[0].Clone();
                    int PortHandle = int.Parse(settingtmp.Substring(3, 1));
                    row.Cells[0].Value = PortHandle;
                    PortHandles.Add(new PortInformation());
                    PortHandles[PortHandle - 1].PortHandle = PortHandle;
                    PortHandles[PortHandle - 1].RomPath = settingtmp.Substring(settingtmp.IndexOf("=") + 1);
                    TrackingGridView.Rows.Add(row);
                    CmbPortHandle.Items.Add(PortHandle);
                    CmbReferenceHandle.Items.Add(PortHandle);
                }
                else if (settingtmp.Contains("referenceTool"))
                {
                    ReferenceHandle = int.Parse(settingtmp.Substring(settingtmp.IndexOf("=") + 1));
                    CmbReferenceHandle.SelectedIndex = ReferenceHandle;
                    if (ReferenceHandle != 0)
                        TrackingGridView.Rows[ReferenceHandle - 1].Cells[0].Value = "R" + (ReferenceHandle);
                }
                else if (settingtmp.Contains("cameraSerialNumber"))
                {
                    txtCameraSerial.Text = settingtmp.Substring(settingtmp.IndexOf("=") + 1);
                    host = txtCameraSerial.Text;
                }

            }
            inireader.Close();
            fs.Close();
            SaveIniFile();
        }
        private void ChangePortFunction(string message)
        {
            toolStripStatusLabel1.Text = message;

        }
        private void SaveIniFile()
        {
            string saveIniString = "tmpFilePath=" + (tmpFilePath == "" ? "C:\\Tracker\\MevisNDI_State.tmp" : tmpFilePath) + '\n';
            saveIniString += "sampleRate=" + (sampleRate == 0 ? 60 : sampleRate) + '\n';
            foreach (var port in PortHandles)
            {
                saveIniString += "rom" + port.PortHandle + "=" + port.RomPath + '\n';
            }
            saveIniString += "referenceTool=" + ReferenceHandle + '\n';
            saveIniString += "cameraSerialNumber=" + (host == "" ? "P9-01009" : host) + '\n';
            File.WriteAllText(iniFilePath, saveIniString);
        }
        private void BtnStartTracking_Click(object sender, EventArgs e)
        {
            host = txtCameraSerial.Text;
            Thread trd = new Thread(new ThreadStart(this.TrackingThread));
            trd.IsBackground = true;
            stopFlag = false;
            trd.Start();
            
            BtnUnloadRom.Enabled = false;
            BtnLoadRom.Enabled = false;
            BtnStartTracking.Enabled = false;
            BtnStopTracking.Enabled = true;
        }

        private void BtnStopTracking_Click(object sender, EventArgs e)
        {
            BtnUnloadRom.Enabled = true;
            BtnLoadRom.Enabled = true;
            BtnStartTracking.Enabled = true;
            BtnStopTracking.Enabled = false;
            stopFlag = true;
            Thread trd = new Thread(new ThreadStart(this.stopTrackingThread));
            trd.IsBackground = true;
            trd.Start();
            trd.Join();
        }
        private void stopTrackingThread()
        {
            if (cmd == null)
                return;
            // Stop streaming and wait for final packets to come in
            cAPI.StopStreaming(cmd);
            Task.Delay(1000).Wait();

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
            Port tool;
            foreach (var port in PortHandles)
            {
                tool = cAPI.PortHandleRequest();
                if (tool == null)
                {
                    log("Could not get available port for tool.");
                }
                else if (!tool.LoadSROM(port.RomPath))  //ref
                {
                    log("Could not load SROM file for tool.");
                    return false;
                }
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



                //CmbPortHandle.Items.Add(portinfo.PortHandle);
                //CmbReferenceHandle.Items.Add(portinfo.PortHandle);
                // DataGridViewRow row = (DataGridViewRow)TrackingGridView.Rows[0].Clone();



            }

            // List all enabled ports
            log("Enabled Ports:");
            ports = cAPI.PortHandleSearchRequest(PortHandleSearchType.All);
            foreach (var port in ports)
            {
                PortInformation portinfo = PortHandles[port.PortHandle - 1];

                port.GetInfo(5);
                portinfo.ToolType = (int)port.ToolType;
                portinfo.ManufactureID = port.Manufacturer;
                portinfo.SerialNumber = port.SerialNumber.ToString();
                portinfo.ToolRevision = port.Revision;
                portinfo.PortHandle = port.PortHandle;
                portinfo.PartNumber = port.PartNumber;
                portinfo.isInitalized = true;
                portinfo.isEnabled = true;
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
            log("\tConnecting to device by hostname: "+host+".local");
        }

        private void btnTmpBrowse_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            tmpFilePath = saveFileDialog1.FileName;
            txtTMPFilePath.Text = tmpFilePath;
            SaveIniFile();
        }
        private void TrackingThread()
        {
            log("C# CAPI Sample v" + Capi.GetVersion());

            // Use the host specified if provided, otherwise try to find a network device.


            //}

            if (host == "" || host == "help" || host == "-h" || host == "/?" || host == "--help")
            {
                // host argument was not provided and we couldn't find one
                if (host == "")
                {
                    log("Could not automatically detect an NDI device, please manually specify one.");
                }
                return;
            }

            // Create a new CAPI instance based on the connection type
            cAPI = new CapiTcp(host);
            // Be Verbose
            // cAPI.LogTransit = true;

            // Use the same log output format as this sample application
            cAPI.SetLogger(log);
            log("Connecting...");
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
                Thread trd = new Thread(new ThreadStart(this.stopTrackingThread));
                trd.IsBackground = true;

                trd.Start();
                trd.Join();
                Thread trd1 = new Thread(new ThreadStart(this.TrackingThread));
                trd1.IsBackground = true;

                trd1.Start();
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


            Run(cAPI);

        }
        /// <summary>
        /// Run the CAPI sample regardless of the connection method.
        /// </summary>
        /// <param name="cAPI">The configured CAPI protocol.</param>
        private static void Run(Capi cAPI)
        {
            if (cAPI == null)
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
            cmd = cAPI.StartStreaming("BX2 --6d=tools", "BX2", (int)1000 / sampleRate);

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
                QuatTransformation dtTmpQuatXFrm = new QuatTransformation();
                dtRefQuatXfrm.translation.x = dtRefQuatXfrm.translation.y = dtRefQuatXfrm.translation.z = dtRefQuatXfrm.rotation.q0 = dtRefQuatXfrm.rotation.qx = dtRefQuatXfrm.rotation.qy = dtRefQuatXfrm.rotation.qz = 0;
                dtPortQuatXfrm.translation.x = dtPortQuatXfrm.translation.y = dtPortQuatXfrm.translation.z = dtPortQuatXfrm.rotation.q0 = dtPortQuatXfrm.rotation.qx = dtPortQuatXfrm.rotation.qy = dtPortQuatXfrm.rotation.qz = 0;
                dtNewQuatXfrm.translation.x = dtNewQuatXfrm.translation.y = dtNewQuatXfrm.translation.z = dtNewQuatXfrm.rotation.q0 = dtNewQuatXfrm.rotation.qx = dtNewQuatXfrm.rotation.qy = dtNewQuatXfrm.rotation.qz = 0;
                dtRefQuatXfrmInv.translation.x = dtRefQuatXfrmInv.translation.y = dtRefQuatXfrmInv.translation.z = dtRefQuatXfrmInv.rotation.q0 = dtRefQuatXfrmInv.rotation.qx = dtRefQuatXfrmInv.rotation.qy = dtRefQuatXfrmInv.rotation.qz = 0;
                bool activeselected = false;
                bool referenceBad = false;
                if (ReferenceHandle != 0)
                {
                    var referencetool = tools[ReferenceHandle - 1];
                    if (referencetool.transform.status != 0)
                    {
                        dtRefQuatXfrm.rotation.q0 = 0;
                        dtRefQuatXfrm.rotation.qx = 0;
                        dtRefQuatXfrm.rotation.qy = 0;
                        dtRefQuatXfrm.rotation.qz = 0;
                        dtRefQuatXfrm.translation.x = 0;
                        dtRefQuatXfrm.translation.y = 0;
                        dtRefQuatXfrm.translation.z = 0;
                        DataGridViewRow row = (DataGridViewRow)TrackingGridView.Rows[referencetool.transform.toolHandle - 1];
                        //row.Cells[0].Value = t.transform.toolHandle;
                        row.Cells[1].Value = Enum.GetName(typeof(TransformStatus), referencetool.transform.status);
                        row.Cells[2].Value = "-";
                        row.Cells[3].Value = "-";
                        row.Cells[4].Value = "-";
                        row.Cells[5].Value = "-";
                        row.Cells[6].Value = "-";
                        row.Cells[7].Value = "-";
                        row.Cells[8].Value = "-";
                        row.Cells[9].Value = "-";
                        referenceBad = true;

                    }
                    else
                    {
                        dtRefQuatXfrm.rotation.q0 = referencetool.transform.orientation.q0;
                        dtRefQuatXfrm.rotation.qx = referencetool.transform.orientation.qx;
                        dtRefQuatXfrm.rotation.qy = referencetool.transform.orientation.qy;
                        dtRefQuatXfrm.rotation.qz = referencetool.transform.orientation.qz;
                        dtRefQuatXfrm.translation.x = referencetool.transform.position.x;
                        dtRefQuatXfrm.translation.y = referencetool.transform.position.y;
                        dtRefQuatXfrm.translation.z = referencetool.transform.position.z;
                        DataGridViewRow row = (DataGridViewRow)TrackingGridView.Rows[referencetool.transform.toolHandle - 1];
                        //row.Cells[0].Value = t.transform.toolHandle;
                        row.Cells[1].Value = String.Format("{0:0.000}", referencetool.transform.position.x);
                        row.Cells[2].Value = String.Format("{0:0.000}", referencetool.transform.position.y);
                        row.Cells[3].Value = String.Format("{0:0.000}", referencetool.transform.position.z);
                        row.Cells[4].Value = String.Format("{0:0.0000}", referencetool.transform.orientation.q0);
                        row.Cells[5].Value = String.Format("{0:0.0000}", referencetool.transform.orientation.qx);
                        row.Cells[6].Value = String.Format("{0:0.0000}", referencetool.transform.orientation.qy);
                        row.Cells[7].Value = String.Format("{0:0.0000}", referencetool.transform.orientation.qz);
                        row.Cells[8].Value = String.Format("{0:0.000}", referencetool.transform.error);
                        row.Cells[9].Value = Enum.GetName(typeof(TransformStatus), referencetool.transform.status);
                        referenceBad = false;
                    }
                }

                foreach (var t in tools)
                {
                    if (t.transform.status == 0 && t.transform.toolHandle != ReferenceHandle)
                    {
                        if (activeselected == false)
                        {
                            ActiveHandle = t.transform.toolHandle;
                            activeselected = true;
                        }
                    }
                    if (t.transform.toolHandle == ReferenceHandle)
                    {
                        if (t.transform.status != 0 || t.transform.isMissing == true)
                        {
                            dtRefQuatXfrm.rotation.q0 = 0;
                            dtRefQuatXfrm.rotation.qx = 0;
                            dtRefQuatXfrm.rotation.qy = 0;
                            dtRefQuatXfrm.rotation.qz = 0;
                            dtRefQuatXfrm.translation.x = 0;
                            dtRefQuatXfrm.translation.y = 0;
                            dtRefQuatXfrm.translation.z = 0;
                            DataGridViewRow row = (DataGridViewRow)TrackingGridView.Rows[t.transform.toolHandle - 1];
                            //row.Cells[0].Value = t.transform.toolHandle;
                            row.Cells[1].Value = Enum.GetName(typeof(TransformStatus), t.transform.status);
                            row.Cells[2].Value = "-";
                            row.Cells[3].Value = "-";
                            row.Cells[4].Value = "-";
                            row.Cells[5].Value = "-";
                            row.Cells[6].Value = "-";
                            row.Cells[7].Value = "-";
                            row.Cells[8].Value = "-";
                            row.Cells[9].Value = "-";
                            referenceBad = true;
                            for (int i = 0; i < TrackingGridView.RowCount - 1; i++)
                            {
                                if (i != ReferenceHandle - 1)
                                {
                                    DataGridViewRow brow = (DataGridViewRow)TrackingGridView.Rows[i];
                                    brow.Cells[1].Value = "MISSING";
                                    brow.Cells[2].Value = "-";
                                    brow.Cells[3].Value = "-";
                                    brow.Cells[4].Value = "-";
                                    brow.Cells[5].Value = "-";
                                    brow.Cells[6].Value = "-";
                                    brow.Cells[7].Value = "-";
                                    brow.Cells[8].Value = "-";
                                    brow.Cells[9].Value = "-";
                                }

                            }


                        }
                        else
                        {
                            dtRefQuatXfrm.rotation.q0 = t.transform.orientation.q0;
                            dtRefQuatXfrm.rotation.qx = t.transform.orientation.qx;
                            dtRefQuatXfrm.rotation.qy = t.transform.orientation.qy;
                            dtRefQuatXfrm.rotation.qz = t.transform.orientation.qz;
                            dtRefQuatXfrm.translation.x = t.transform.position.x;
                            dtRefQuatXfrm.translation.y = t.transform.position.y;
                            dtRefQuatXfrm.translation.z = t.transform.position.z;
                            DataGridViewRow row = (DataGridViewRow)TrackingGridView.Rows[t.transform.toolHandle - 1];
                            //row.Cells[0].Value = t.transform.toolHandle;
                            row.Cells[1].Value = String.Format("{0:0.000}", t.transform.position.x);
                            row.Cells[2].Value = String.Format("{0:0.000}", t.transform.position.y);
                            row.Cells[3].Value = String.Format("{0:0.000}", t.transform.position.z);
                            row.Cells[4].Value = String.Format("{0:0.0000}", t.transform.orientation.q0);
                            row.Cells[5].Value = String.Format("{0:0.0000}", t.transform.orientation.qx);
                            row.Cells[6].Value = String.Format("{0:0.0000}", t.transform.orientation.qy);
                            row.Cells[7].Value = String.Format("{0:0.0000}", t.transform.orientation.qz);
                            row.Cells[8].Value = String.Format("{0:0.000}", t.transform.error);
                            row.Cells[9].Value = Enum.GetName(typeof(TransformStatus), t.transform.status);
                            referenceBad = false;
                        }

                    }
                    else
                    {
                        dtPortQuatXfrm.translation.x = t.transform.position.x;
                        dtPortQuatXfrm.translation.y = t.transform.position.y;
                        dtPortQuatXfrm.translation.z = t.transform.position.z;
                        dtPortQuatXfrm.rotation.q0 = t.transform.orientation.q0;
                        dtPortQuatXfrm.rotation.qx = t.transform.orientation.qx;
                        dtPortQuatXfrm.rotation.qy = t.transform.orientation.qy;
                        dtPortQuatXfrm.rotation.qz = t.transform.orientation.qz;
                        dtRefQuatXfrmInv = Conversion.QuatInverseXfrm(dtRefQuatXfrm);
                        dtNewQuatXfrm = Conversion.QuatCombineXfrms(dtPortQuatXfrm, dtRefQuatXfrmInv);
                        if (ReferenceHandle == 0)
                        {
                            dtNewQuatXfrm = dtPortQuatXfrm;
                        }
                        if (t.transform.toolHandle == ActiveHandle && referenceBad == false)
                        {
                            if (ReferenceHandle == 0)
                                dtTmpQuatXFrm = dtPortQuatXfrm;

                            else
                                dtTmpQuatXFrm = dtNewQuatXfrm;

                        }
                        if (t.transform.status == TransformStatus.OK && referenceBad == false)
                        {
                            DataGridViewRow row = (DataGridViewRow)TrackingGridView.Rows[t.transform.toolHandle - 1];
                            //row.Cells[0].Value = t.transform.toolHandle;
                            row.Cells[1].Value = String.Format("{0:0.000}", dtNewQuatXfrm.translation.x);
                            row.Cells[2].Value = String.Format("{0:0.000}", dtNewQuatXfrm.translation.y);
                            row.Cells[3].Value = String.Format("{0:0.000}", dtNewQuatXfrm.translation.z);
                            row.Cells[4].Value = String.Format("{0:0.0000}", dtNewQuatXfrm.rotation.q0);
                            row.Cells[5].Value = String.Format("{0:0.0000}", dtNewQuatXfrm.rotation.qx);
                            row.Cells[6].Value = String.Format("{0:0.0000}", dtNewQuatXfrm.rotation.qy);
                            row.Cells[7].Value = String.Format("{0:0.0000}", dtNewQuatXfrm.rotation.qz);
                            row.Cells[8].Value = String.Format("{0:0.000}", t.transform.error);
                            row.Cells[9].Value = Enum.GetName(typeof(TransformStatus), t.transform.status);
                        }
                        else
                        {
                            DataGridViewRow row = (DataGridViewRow)TrackingGridView.Rows[t.transform.toolHandle - 1];
                            //row.Cells[0].Value = t.transform.toolHandle;
                            if (referenceBad == true)
                                row.Cells[1].Value = "MISSING";
                            else
                                row.Cells[1].Value = Enum.GetName(typeof(TransformStatus), t.transform.status);
                            row.Cells[2].Value = "-";
                            row.Cells[3].Value = "-";
                            row.Cells[4].Value = "-";
                            row.Cells[5].Value = "-";
                            row.Cells[6].Value = "-";
                            row.Cells[7].Value = "-";
                            row.Cells[8].Value = "-";
                            row.Cells[9].Value = "-";
                        }
                    }

                    //log(t.ToString());
                }
                if (referenceBad != true && activeselected == true)
                    WriteToTmpFile(dtTmpQuatXFrm);
            });

            // Stream data for 5 seconds then get some parameters and wait 5 more seconds.
            Task.Delay(5000).Wait();
            log(cAPI.GetUserParameter("Param.Tracking.Frame Frequency"));
            log(cAPI.GetUserParameter("Param.Tracking.Track Frequency"));
            while (!stopFlag)
            {
                if(cAPI.IsConnected == false && cAPI.IsTracking == false)
                {
                    Thread trd = new Thread(new ThreadStart(thisForm.stopTrackingThread));
                    trd.IsBackground = true;

                    trd.Start();
                    trd.Join();
                    Thread trd1 = new Thread(new ThreadStart(thisForm.TrackingThread));
                    trd1.IsBackground = true;
                    trd1.Start();
                }
                Thread.Sleep(1000);
            }
                


        }

        private void CmbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CmbMode.SelectedText == "Using Tmp File")
                isUsingTmpFile = true;
            else
                isUsingTmpFile = false;
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
            string tmpString = "V  Q  " + String.Format("{0:0.000}", Tx) + "  " + String.Format("{0:0.000}", Ty) + "  " + String.Format("{0:0.000}", Tz) + "  " + String.Format("{0:0.0000}", Q0) + "  " + String.Format("{0:0.0000}", Qx) + "  " + String.Format("{0:0.0000}", Qy) + "  " + String.Format("{0:0.0000}", Qz) + " " + timestamp.ToString() + "l";
            log(tmpString + " CurrentPort:" + ActiveHandle);
            // string checkString = "V  Q  -22.539063  -184.778229  118.444290  -0.014033  -0.093944  0.031202  0.994772 1006210395l";
            // Convert a C# string to a byte array  
            byte[] bytes = Encoding.ASCII.GetBytes(tmpString + '\n');

            long crc = Crc32Algorithm.Compute(bytes, 0, bytes.Length);
            string tmptag = tmpString + '\n' + ";" + crc.ToString("x8") + '\0' + '\0' + '\n';
            try
            {
                File.WriteAllText(tmpFilePath, tmptag);
                string[] partnumbertxt = new string[1];
                partnumbertxt[0] = PortHandles[ActiveHandle - 1].PartNumber;
                File.AppendAllLines(tmpFilePath, partnumbertxt);
            }
            catch (Exception e)
            {
                return;
            }


            //TrackingGridView.data
        }

        /// <summary>
        /// Timestamped log output function.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        public static void log(string message)
        {
            myDelegate.Invoke(message);

            string time = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

            Console.WriteLine(time + " [CAPISample] " + message);
        }

        private void CmbPortHandle_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = CmbPortHandle.SelectedIndex;

            TxtToolType.Text = Enum.GetName(typeof(PortToolType), PortHandles[index].ToolType);
            TxtToolRevision.Text = PortHandles[index].ToolRevision;
            TxtSerialNumber.Text = PortHandles[index].SerialNumber;
            TxtPartNumber.Text = PortHandles[index].PartNumber;
            TxtManufactureID.Text = PortHandles[index].ManufactureID;
        }

        private void CmbReferenceHandle_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var port in PortHandles)
            {
                TrackingGridView.Rows[port.PortHandle - 1].Cells[0].Value = port.PortHandle;
            }
            if (CmbReferenceHandle.SelectedIndex != 0)
            {
                TrackingGridView.Rows[CmbReferenceHandle.SelectedIndex - 1].Cells[0].Value = "R" + CmbReferenceHandle.SelectedIndex;
            }
            ReferenceHandle = CmbReferenceHandle.SelectedIndex;

            SaveIniFile();
        }

        private void BtnLoadRom_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            PortInformation portInformation = new PortInformation();
            portInformation.RomPath = openFileDialog1.FileName;
            portInformation.PortHandle = PortHandles.Count() + 1;
            DataGridViewRow row = (DataGridViewRow)TrackingGridView.Rows[0].Clone();
            row.Cells[0].Value = portInformation.PortHandle;
            CmbPortHandle.Items.Add(portInformation.PortHandle);
            CmbReferenceHandle.Items.Add(portInformation.PortHandle);
            TrackingGridView.Rows.Add(row);
            PortHandles.Add(portInformation);
            SaveIniFile();
        }

        private void BtnUnloadRom_Click(object sender, EventArgs e)
        {

            int index = TrackingGridView.CurrentCell.RowIndex;
            TrackingGridView.Rows.RemoveAt(index);
            CmbPortHandle.Items.RemoveAt(index);
            CmbReferenceHandle.Items.RemoveAt(index + 1);
            PortHandles.RemoveAt(index);
            SaveIniFile();
        }

        private void TrackingGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (TrackingGridView.CurrentCell.RowIndex != TrackingGridView.RowCount - 1)
            {
                PortInformation portInformation = PortHandles[TrackingGridView.CurrentCell.RowIndex];
                CmbPortHandle.SelectedIndex = TrackingGridView.CurrentCell.RowIndex;
                TxtRomPath.Text = PortHandles[TrackingGridView.CurrentCell.RowIndex].RomPath;
                TxtManufactureID.Text = portInformation.ManufactureID;
                TxtPartNumber.Text = portInformation.PartNumber;
                TxtSerialNumber.Text = portInformation.SerialNumber;
                TxtToolRevision.Text = portInformation.ToolRevision;
                TxtToolType.Text = Enum.GetName(typeof(PortToolType), portInformation.ToolType);
                ChkHandleInitialized.Checked = portInformation.isInitalized;
                ChkHandleEnabled.Checked = portInformation.isEnabled;
            }
        }

        private void NDI2TrackerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (stopFlag == false)
                BtnStopTracking_Click(sender, e);
            System.Windows.Forms.Application.ExitThread();
        }
    }
    class PortInformation
    {
        public int PortHandle;
        public int ToolType;
        public string ManufactureID;
        public string PartNumber;
        public string SerialNumber;
        public string ToolRevision;
        public bool isInitalized;
        public bool isEnabled;
        public string RomPath;
    }
}
