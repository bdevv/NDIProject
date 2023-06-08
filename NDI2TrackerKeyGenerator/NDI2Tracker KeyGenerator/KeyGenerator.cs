using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DeviceId;
using System.Security.Cryptography;

namespace NDI2Tracker
{
    public partial class KeyGenerator : Form
    {
        public KeyGenerator()
        {
            InitializeComponent();


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
            string deviceId = TxtMachineId.Text.Replace("-",string.Empty);
            if (deviceId.Length != 16)
            {
                MessageBox.Show("Invalid Machine ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            deviceId = deviceId.Replace("-", string.Empty);
            TxtLicense.Text = GenerateLicenseKey(deviceId.ToUpper());
        }
    }
}
