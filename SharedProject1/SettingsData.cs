using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataLibary
{
    public static class SettingsData
    {
        public static readonly string IPADRESSSERVER = "145.102.70.171";
        public const int PORTNUMBERSERVER = 6666;

        private static readonly ASCIIEncoding asen = new ASCIIEncoding();



        public const string IPADRESSJOHAN = "145.48.6.10";
        public const int PORTNUMBERJOHAN = 6666;



        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        public static void SendArrayMessage(BinaryWriter writer, List<string> list)
        {
            string message = "";
            foreach (string s in list)
            {
                message += s + "\n";
            }
            SendTextMessage(writer, message);
        }

        public static void SendTextMessage(BinaryWriter writer, string s)
        {
            try
            {
                var jsonBytes = asen.GetBytes(s);

                writer.Write(BitConverter.GetBytes(jsonBytes.Length));
                writer.Write(jsonBytes);
            }
            catch (Exception)
            {
                MessageBox.Show("Server is offline. Please ask the administrator for further information.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        public static string ReadTextMessage(BinaryReader reader)
        {
            try
            {
                var packLen = reader.ReadInt32();
                var bytes = reader.ReadBytes(packLen);
                var message = Encoding.ASCII.GetString(bytes);
                return message;
            }
            catch (Exception)
            {
                return "";

            }
         
        }

        public static string[] MessageToArray(string message)
        {
            
            message += "\t" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            var messagearray = message.Split('\t');

            return messagearray;

        }

        public static void SendObjectMessage(BinaryWriter writer, object obj)
        {
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                var message = ms.ToArray();
                writer.Write(BitConverter.GetBytes(message.Length));
                writer.Write(message);
            }
        }


    }



}
