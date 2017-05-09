using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using DataLibary;

namespace VRApplicationWF.BikeSniffer
{
    internal class Simulation
    {
        private static readonly Random rnd = new Random();
        private static readonly ASCIIEncoding asen = new ASCIIEncoding();


        public List<string[]> data = new List<string[]>();

        private string st;

        private BinaryWriter writer;

        private BinaryReader reader;




        public Simulation()
        {
       
            connection();

            SettingsData.SendTextMessage(writer, "patient");

            SettingsData.SendTextMessage(writer, "SG1B7300");
            string isCorrect = SettingsData.ReadTextMessage(reader);
            if (isCorrect == "incorrect")
            {
                Console.WriteLine("bikeID is not a legal ID in the servers database");
                Application.Exit();
                Environment.Exit(1);
            }
            else
            {
                Console.WriteLine("correct BikeID, LETS BIKE!");
            }

            GenerateRandomSt();
            
        }

        public BinaryWriter Writer
        {
            get { return writer; }
            set { writer = value; }
        }

        public BinaryReader Reader
        {
            get { return reader; }
            set { reader = value; }
        }

        private void connection()
        {
            try
            {
                TcpClient client = new TcpClient(SettingsData.IPADRESSSERVER, SettingsData.PORTNUMBERSERVER);

                NetworkStream netStream = client.GetStream();
                SslStream ssl = new SslStream(netStream, false, new RemoteCertificateValidationCallback(ValidateCert));
                ssl.AuthenticateAsClient("InstantMessengerServer");
                reader = new BinaryReader(ssl);
                writer = new BinaryWriter(ssl);
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


        public string SendCommand()
        {
            GenerateRandomSt();
            Console.WriteLine(st);
            return st;
        }

        public void GenerateRandomSt()
        {
            var pulse = RndInt(0, 120);
            var RPM = RndInt(0, 110);
            var speed = RndDouble(0, 60);
            var distance = RndDouble(0, 999);
            var watt = RndInt(0, 200);
            var burnedEnergy = RndInt(0, 999);

            var minutes = RndInt(0, 59);
            var seconds = RndInt(0, 59);
            var datetime = string.Format($"{minutes}:{seconds}");
            var currenttime = DateTime.Now.ToString("MM/dd/yyyy HH:mm");

            var bikeID = "1";

            var actualwatt = RndInt(0, 200);

            st =
                string.Format(
                    $"{pulse}\t{RPM}\t{speed}\t{distance}\t{watt}\t{burnedEnergy}\t{datetime}\t{actualwatt}\t{currenttime}\t{bikeID}");
        }


        //{pulse in HZ}{rpm}{speed in 0.1km/h}{distance in 0.1 km}
        //{requested power}{energy in kJ}{time in minutes:seconds}{actual power}.


        private static double RndDouble(double min, double max)
        {
            var range = max - min;
            var difference = 0 + min;
            var newDouble = rnd.NextDouble()*range + difference;
            return newDouble;
        }

        private static int RndInt(int min, int max)
        {
            var newInt = rnd.Next(min, max);
            return newInt;
        }

        public void SendTxtMessageToServer(string s)
        {
            var jsonBytes = asen.GetBytes(s);

            writer.Write(BitConverter.GetBytes(jsonBytes.Length));
            writer.Write(jsonBytes);
        }

        public string ReadNormalTextMessage()
        {
            var packLen = reader.ReadInt32();
            var bytes = reader.ReadBytes(packLen);
            var message = Encoding.ASCII.GetString(bytes);
            return message;
        }
    }
}