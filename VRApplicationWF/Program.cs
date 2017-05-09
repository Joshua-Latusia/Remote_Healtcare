using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using VRApplicationWF.VirtualReality.Forms;

namespace BikeApplication
{
    internal static class Program
    {
        private const string ipAdressServer = "145.102.71.175";
        private const int portNumber = 1234;

        /// <summary>
        ///     BikeVRApplication
        ///     1. Generates a VR scene for the cliënt who will be on the bike
        ///     2. Sniffs data out of the bike and sends it to the server to store data for the dokter
        ///     3. Sniffs data out of the bike and sends it to the VR scene to make the VR interact live with the users progress on
        ///     the bike
        /// </summary>

        [STAThread]
        private static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new VRSelect());

        }

     

      
    }
}