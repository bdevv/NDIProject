using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CGUINDIStreaming;
using DeviceId;
using System.Security.Cryptography;

namespace NDI2Tracker
{
    public partial class Login : Form
    {
        const string keyFilePath = "licence.key";
        string deviceId;
        public Login()
        {
            InitializeComponent();
            deviceId = new DeviceIdBuilder().AddMachineName().AddMacAddress().ToString().Substring(0, 16);
            string displayId = deviceId.Insert(4, "-");
            displayId = displayId.Insert(9, "-");
            displayId = displayId.Insert(14, "-");

            TxtMachineId.Text = displayId;
            FileStream fs = File.Open(keyFilePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
            var keyreader = new StreamReader(fs);
            string licensekey = keyreader.ReadLine();
            if (licensekey == null)
                licensekey = "";
            TxtLicense.Text = licensekey;

        }
        private void Login_Shown(object sender, EventArgs e)
        {
            string key = GenerateLicenseKey(deviceId.ToUpper());
            if (GenerateLicenseKey(deviceId.ToUpper()) == TxtLicense.Text)
            {
                this.Hide();
                NDI2TrackerForm ndi2 = new NDI2TrackerForm();
                ndi2.Show();
            }
            else
            {
                this.Show();
            }
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.ExitThread();
        }
        static string GenerateLicenseKey(string productIdentifier)
        {
            return FormatLicenseKey(GetMd5Sum(productIdentifier));
        }

        static string GetMd5Sum(string productIdentifier)
        {
            System.Text.Encoder enc = System.Text.Encoding.Unicode.GetEncoder();
            byte[] unicodeText = new byte[productIdentifier.Length * 2];
            enc.GetBytes(productIdentifier.ToCharArray(), 0, productIdentifier.Length, unicodeText, 0, true);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(unicodeText);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }
            return sb.ToString();
        }

        static string FormatLicenseKey(string productIdentifier)
        {
            productIdentifier = productIdentifier.Substring(0, 16).ToUpper();
            char[] serialArray = productIdentifier.ToCharArray();
            StringBuilder licenseKey = new StringBuilder();

            int j = 0;
            for (int i = 0; i < 16; i++)
            {
                for (j = i; j < 4 + i; j++)
                {
                    licenseKey.Append(serialArray[j]);
                }
                if (j == 16)
                {
                    break;
                }
                else
                {
                    i = (j) - 1;
                    licenseKey.Append("-");
                }
            }
            return licenseKey.ToString();
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            if (TxtLicense.Text != GenerateLicenseKey(deviceId))
            {
                MessageBox.Show("Invalid License Key", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FileStream fs = File.Open(keyFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            var keywriter = new StreamWriter(fs);
            keywriter.WriteLine(TxtLicense.Text);
            keywriter.Close();
            fs.Close();
            NDI2TrackerForm ndi2 = new NDI2TrackerForm();
            ndi2.Show();
            this.Hide();
        }
    }
}
