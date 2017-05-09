using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DataLibary;

namespace BikeApplication
{
    internal class BikeConnection
    {
        private readonly SerialPort sp;

        private bool isOkay = false;

        public BikeConnection()
        {

            Console.WriteLine("trying to connect...");
            connection();

            SettingsData.SendTextMessage(Writer, "patient");

            string[] pn = SerialPort.GetPortNames();


            sp = new SerialPort(pn[0], 9600);
            sp.Open();
            Console.WriteLine("connected!");
            WriteLine("ID");
            Thread.Sleep(200);
            string bikeID = Listen();
            Console.WriteLine(bikeID);
            SettingsData.SendTextMessage(Writer, bikeID);
            string isCorrect = SettingsData.ReadTextMessage(Reader);
            if (isCorrect == "incorrect")
            {
                Console.WriteLine("bikeID is not a legal ID in the servers database");
                Application.Exit();
                Environment.Exit(1);
            }
            else
            {
                Console.WriteLine("correct BikeID, LETS BIKE!");
                WriteLine("rs");
                Console.WriteLine(sp.ReadLine());
                Thread.Sleep(500);
                WriteLine("CM");

                Console.WriteLine(sp.ReadLine());

                Console.WriteLine("heeft hem in command mode gezet");
                isOkay = true;
            }
        }

        public bool IsOkay
        {
            get { return isOkay; }
            set { isOkay = value; }
        }

        public BinaryWriter Writer { get; private set; }

        public BinaryReader Reader { get; set; }

        public void WriteLine(string s)
        {
            try
            {
                sp.WriteLine(s);
            }
            catch (Exception)
            {
                DialogResult dialogResult =
                    MessageBox.Show(
                        "Usb cable isn't plugged in or there is something wrong with the pc to bike simulation connection. Please reconnect.",
                        "QTech - Error", MessageBoxButtons.OK);

                Application.Exit();

            }
        }


        public void sendCommand(string message)
        {
            sp.WriteLine(message);
        }

        public string Listen()
        {
            string s = "";
            try
            {
                s = sp.ReadLine();
            }
            catch (Exception)
            {
                DialogResult dialogResult = MessageBox.Show("Usb cable isn't plugged in or there is something wrong with the pc to bike simulation connection. Please reconnect.", "QTech - Error", MessageBoxButtons.OK);

                Application.Exit();

            }
            return s;

        }

        public void connection()
        {
            try
            {
                TcpClient client = new TcpClient(SettingsData.IPADRESSSERVER, SettingsData.PORTNUMBERSERVER);

                NetworkStream netStream = client.GetStream();
                SslStream ssl = new SslStream(netStream, false, new RemoteCertificateValidationCallback(ValidateCert));
                ssl.AuthenticateAsClient("InstantMessengerServer");
                Reader = new BinaryReader(ssl);
                Writer = new BinaryWriter(ssl);
            }
            catch (Exception)
            {
                DialogResult dialogResult = MessageBox.Show("Server is ofline, services aren't available. Please contact the developers.", "QTech - Error", MessageBoxButtons.RetryCancel);
                if (dialogResult == DialogResult.Retry)
                {
                    connection();
                }
                if (dialogResult == DialogResult.Cancel)
                {
                    Application.Exit();
                }

            }
        }

        private static bool ValidateCert(object sender, X509Certificate certificate,
           X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true; // Allow untrusted certificates.
        }
    }
}