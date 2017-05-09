using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using DataLibary;
using serverNotTheRealOne.ClientApplication;

namespace GUI.ClientApplication
{
    public partial class Password : Form
    {
        private const int WmNclbuttondown = 0xA1;
        private const int HtCaption = 0x2;

        private static readonly ASCIIEncoding Asen = new ASCIIEncoding();
        private readonly BinaryReader _reader;
        private readonly BinaryWriter _writer;
        private static string salt = "D1-03-53-95-4C-62-EA-6A-F4-07-FC-0D-AC-31-F0-2E-A3-A3-FF-81-20-95-C8-E4-84-78-28-BB-85-DB-7C-B5";

        public Password()
        {
            try
            {
                InitializeComponent();

                var client = new TcpClient(SettingsData.IPADRESSSERVER, SettingsData.PORTNUMBERSERVER);
                NetworkStream netStream = client.GetStream();
                SslStream ssl = new SslStream(netStream, false, new RemoteCertificateValidationCallback(ValidateCert));
                ssl.AuthenticateAsClient("InstantMessengerServer");
                _reader = new BinaryReader(ssl);
                _writer = new BinaryWriter(ssl);

                SendTxtMessageToServer("");

              
                passwordField.PasswordChar = '*';
            }

            catch (SocketException e)
            {
                MessageBox.Show("Setting up a connection with the database failed");
                Console.WriteLine(e);
                Environment.Exit(1);
            }
        }

        public static bool ValidateCert(object sender, X509Certificate certificate, 
              X509Chain chain, SslPolicyErrors sslPolicyErrors)
{
    return true; // Allow untrusted certificates.
}

        public static string Salt
        {
            get { return salt; }
        }

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd,
            int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private void button3_Click(object sender, EventArgs e)
        {
            string hashedpassword = GenerateSHA256Hash(passwordField.Text, salt);
       
          
            SendTxtMessageToServer(nameField.Text);
            SendTxtMessageToServer(passwordField.Text);
            if (ReadNormalTextMessage(_reader) == "correct")
            {
                Console.WriteLine("goed binen gekomen, start control panel");

                this. Hide();
                Client client = new Client(_reader, _writer);
                client.Closed += (s, args) => Close();
                client.Show();
            }
            else
            {
                passwordField.Text = "";
                nameField.Text = "";
                errorMessage.Text = "Username or password is/are inccorect, please try again";
            }
        }

        private static string CreateSalt(int size)
        {
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public static string GenerateSHA256Hash(string input, string salt)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input + salt);
            System.Security.Cryptography.SHA256Managed sha256Hashstring = new System.Security.Cryptography.SHA256Managed();
            byte[] hash = sha256Hashstring.ComputeHash(bytes);
            return BitConverter.ToString(hash);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WmNclbuttondown, HtCaption, 0);
            }
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            panel1_MouseDown(sender, e);
        }

        public void SendTxtMessageToServer(string s)
        {
            var jsonBytes = Asen.GetBytes(s);

            _writer.Write(BitConverter.GetBytes(jsonBytes.Length));
            _writer.Write(jsonBytes);
            Console.WriteLine("text message send to server");
        }

        private string ReadNormalTextMessage(BinaryReader reader)
        {
            var packLen = reader.ReadInt32();
            var bytes = reader.ReadBytes(packLen);
            var message = Encoding.ASCII.GetString(bytes);
            return message;
        }
    }
}