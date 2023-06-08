//----------------------------------------------------------------------------
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
using System.Windows.Forms;
using CGUINDIStreaming;
using NDI2Tracker;

namespace NDI.CapiSampleStreaming
{
    /// <summary>
    /// This sample program showcases how to use the STREAM command supported by Vega Position Sensors since G.003.
    /// 
    /// The stream command can be used to continuously receive new frames of data instead of manually using the request-response method for commands.
    /// </summary>
    class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
        }
        
    }
}
