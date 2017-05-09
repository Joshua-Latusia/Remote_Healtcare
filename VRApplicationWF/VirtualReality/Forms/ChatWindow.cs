using BikeApplication;
using DataLibary;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using VRApplicationWF.BikeSniffer;
using VRApplicationWF.VirtualReality.Scripts;

namespace VRApplicationWF.VirtualReality.Forms
{
    public partial class ChatWindow : Form
    {
        private readonly BikeConnection bike;
        private readonly Connection cw;

        private readonly Simulation sim;

        private double hoogte = 6.25;

        private int privateMessageSize;
        private int broadcastMessageSize;

        public Tuple<int, int> pushNotification = new Tuple<int, int>(0, 0);

        private Thread updateThread;



        #region menubbardragsetup

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd,
            int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private const int WmNclbuttondown = 0xA1;
        private const int HtCaption = 0x2;

        #endregion


        public ChatWindow(Connection cw)
        {
            InitializeComponent();
            this.cw = cw;


            try
            {
                var pn = SerialPort.GetPortNames();
                updateThread = new Thread(ActionBike);
                if (pn.Length == 0)
                {
                    MessageBox.Show("USB-Cable isn't plugged in. Please connect this to your pc.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    //uncomment for sim
                    //sim = new Simulation();

                }
                else
                {
                    Console.WriteLine("COM port available, connecting to the bike...");
                    bike = new BikeConnection();
                    if (bike.IsOkay)
                    {
                        updateThread.Start();

                    }

                }


            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("There isn't a stable connection, ERROR (restart sim.bat)");
            }
        }

        private void ActionSim()
        {
            Console.WriteLine("vraagt server om chatdata");
            SettingsData.SendTextMessage(sim.Writer, "getChatData");
            string arrayBroadcast = SettingsData.ReadTextMessage(sim.Reader);
            string arrayPrivate = SettingsData.ReadTextMessage(sim.Reader);
            foreach (string message in arrayBroadcast.Split("\n".ToCharArray()))
            {
                BroadCastTextBox.Items.Add(message);
            }
            foreach (string message2 in arrayPrivate.Split("\n".ToCharArray()))
            {
                PrivateTextBox.Items.Add(message2);
            }
            Console.WriteLine("chatdata ontvangen");

            while (true)

            {


                sim.SendTxtMessageToServer("sendBikeData");
                sim.GenerateRandomSt();
                var simdata = sim.SendCommand();
                var data = simdata.Split("\t".ToCharArray());
                ChangeSpeed(double.Parse(data[2]), data[7], double.Parse(data[3]) * 0.1, double.Parse(data[5]) * 0.238,
                    double.Parse(data[1]));
                sim.SendTxtMessageToServer(simdata);


                Thread.Sleep(1000);
            }
        }

        private void ActionBike()
        {
            Console.WriteLine("vraagt server om chatdata");

            SettingsData.SendTextMessage(bike.Writer, "getChatData");
            string arrayBroadcast = SettingsData.ReadTextMessage(bike.Reader);
            string arrayPrivate = SettingsData.ReadTextMessage(bike.Reader);

            BroadCastTextBox.Items.Clear();
            PrivateTextBox.Items.Clear();
            string[] broadcast = arrayBroadcast.Split("\n".ToCharArray());
            string[] privateMessage = arrayPrivate.Split("\n".ToCharArray());

            broadcastMessageSize = broadcast.Length;
            privateMessageSize = privateMessage.Length;

            timer.Start();
            while (true)
            {
                bike.sendCommand("st");
                var message = bike.Listen();
                var letters = message.Split("\t".ToCharArray());


                ChangeSpeed(double.Parse(letters[2]) * 0.1f, letters[6], double.Parse(letters[3]) * 0.1,
                    double.Parse(letters[5]) * 0.238, double.Parse(letters[0]));



                SettingsData.SendTextMessage(bike.Writer, "sendBikeData");
                SettingsData.SendTextMessage(bike.Writer, message);

                Thread.Sleep(3000);
            }
        }



        private void GetChatData()
        {

            Console.WriteLine("hij komt hier");
            SettingsData.SendTextMessage(bike.Writer, "getChatData");
            string arrayBroadcast = SettingsData.ReadTextMessage(bike.Reader);
            string arrayPrivate = SettingsData.ReadTextMessage(bike.Reader);

            BroadCastTextBox.Items.Clear();
            PrivateTextBox.Items.Clear();
            string[] broadcast = arrayBroadcast.Split("\n".ToCharArray());
            string[] privateMessage = arrayPrivate.Split("\n".ToCharArray());

            foreach (string message in broadcast)
            {
                BroadCastTextBox.Items.Add(message);
            }
            foreach (string message2 in privateMessage)
            {
                PrivateTextBox.Items.Add(message2);
            }

            pushNotification = Tuple.Create(BroadCastTextBox.Items.Count - broadcastMessageSize,
                PrivateTextBox.Items.Count - privateMessageSize);
        }

        //method changes speed on the panel(display) and the moving speed of the biker in scene
        private void ChangeSpeed(double speed, string duration, double distance, double calories, double bpm)
        {

            string s = cw.WriteReadData(JSONMethods.GetNode("Bike"));

            dynamic deserializedValue = JsonConvert.DeserializeObject(s);

            try
            {
                float currentHoogte = (float)deserializedValue["data"]["data"]["data"][0]["components"][0]["position"][1];


                int deltaHoogte = Convert.ToInt32((currentHoogte - hoogte + 5) * 31.25);



                Console.WriteLine(deltaHoogte);

                hoogte = currentHoogte;

                // bike.sendCommand("CM");
                bike.sendCommand("PW" + deltaHoogte);
                Console.WriteLine(bike.Listen());
            }
            catch (Exception)
            {
                Console.WriteLine("niet gelukt");

            }

            cw.WriteReadData(JSONMethods.UpdateNodeBike(speed / 100 * 2));

            cw.WriteReadData(JSONMethods.SetSpeed(speed / 10));



            cw.WriteReadData(JSONMethods.ClearPanel());


            cw.WriteReadData(JSONMethods.AddPanelText("Speed: " + Math.Round(speed, 2) + " km/h", 50.0f, 20.0f));


            cw.WriteReadData(JSONMethods.AddPanelText("Duration: " + duration, 100.0f, 20.0f));

            cw.WriteReadData(JSONMethods.AddPanelText("Distance: " + distance + " km", 150.0f, 20.0f));

            cw.WriteReadData(JSONMethods.AddPanelText("Calories: " + Math.Round(calories, 2) + " kcal", 200.0f, 20.0f));

            cw.WriteReadData(JSONMethods.AddPanelText("heartbeat: " + bpm + " bpm", 250.0f, 20.0f));


            cw.WriteReadData(JSONMethods.AddPanelText("Broadcast", 75.0f, 310f));
            cw.WriteReadData(JSONMethods.AddPanelText(pushNotification.Item1 + "", 125f, 360f));
            cw.WriteReadData(JSONMethods.AddPanelText("Private message", 175f, 280f));
            cw.WriteReadData(JSONMethods.AddPanelText(pushNotification.Item2 + "", 225f, 360f));

            cw.WriteReadData(JSONMethods.AddPanelText("Time: " + DateTime.Now.ToString("HH:mm"), 400.0f, 175f));


            cw.WriteReadData(JSONMethods.swapPanel());
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        private void minimizeButton_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void qTechLogo_Click(object sender, EventArgs e)
        {
            Process.Start("www.4chan.org");
        }

        private void TopPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            ReleaseCapture();
            SendMessage(Handle, WmNclbuttondown, HtCaption, 0);
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            string text = String.Format($"{DateTime.Now}: {ChatBoxClient.Text} (Patient)");
            SettingsData.SendTextMessage(bike.Writer, "sendChatData");
            SettingsData.SendTextMessage(bike.Writer, text);

            Console.WriteLine("private message has been sent");
            PrivateTextBox.Items.Add(text);
            ChatBoxClient.Text = "";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            cw.WriteData(JSONMethods.Play());
            Console.WriteLine("play");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine("pauze");


            cw.WriteData(JSONMethods.Pause());
            Console.WriteLine("pauzes done");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GetChatData();
            Console.WriteLine("chatdata updated");
        }
    }
}
