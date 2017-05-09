using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DataLibary;

namespace VRApplicationWF.VirtualReality.Scripts
{
    public class Connection
    {
        private readonly ASCIIEncoding asen = new ASCIIEncoding();
        private readonly BinaryReader reader;
        private readonly BinaryWriter writer;



        public Connection()
        {
            try
            {
                var tcpclnt = new TcpClient(SettingsData.IPADRESSJOHAN, SettingsData.PORTNUMBERJOHAN);
                Console.WriteLine("Connecting.....");


                Console.WriteLine("Connected");

                reader = new BinaryReader(tcpclnt.GetStream());
                writer = new BinaryWriter(tcpclnt.GetStream());
            }
            catch (Exception e)
            {
                Console.WriteLine("error" + e.StackTrace);
            }
        }

        public BinaryReader Reader
        {
            get { return reader; }
        }

        public BinaryWriter Writer
        {
            get { return writer; }
        }

        public string WriteReadData(string s)
        {
            var jsonBytes = asen.GetBytes(s);


            writer.Write(BitConverter.GetBytes(jsonBytes.Length));
            writer.Write(jsonBytes);

            var packLen = reader.ReadInt32();
            var bytes = reader.ReadBytes(packLen);
            return Encoding.ASCII.GetString(bytes);
        }


        //public Task<string> WriteReadDataAsync(string s)
        //{
        //    var jsonBytes = asen.GetBytes(s);


        //    writer.Write(BitConverter.GetBytes(jsonBytes.Length));
        //    writer.Write(jsonBytes);

        //    var packLen = reader.ReadInt32();
        //    var bytes = reader.ReadBytes(packLen);

           
        //    return Encoding.ASCII.GetString(bytes);
        //    return new Task<string>();
        //}

        public string ReadData()
        {
            var packLen = reader.ReadInt32();
            var bytes = reader.ReadBytes(packLen);
            return Encoding.ASCII.GetString(bytes);
        }

        public void WriteData(string s)
        {
            var jsonBytes = asen.GetBytes(s);


            writer.Write(BitConverter.GetBytes(jsonBytes.Length));
            writer.Write(jsonBytes);
        }
    }
}