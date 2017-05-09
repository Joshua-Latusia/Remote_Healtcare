using BikeApplication;
using DataLibary;
using System;
using System.IO.Ports;
using System.Threading;
using VRApplicationWF.BikeSniffer;
using VRApplicationWF.VirtualReality.Forms;

namespace VRApplicationWF.VirtualReality.Scripts
{
    internal class DataHandling
    {
        private readonly BikeConnection bike;
        private readonly Connection cw;

        private readonly Simulation sim;
        //  private JSONMethods jw = new JSONMethods();

        public DataHandling(Connection cw)
        {

            this.cw = cw;
            try
            {
                var pn = SerialPort.GetPortNames();
                Thread thread;
                if (pn.Length == 0)
                {
                    Console.WriteLine("there is no COM port available, starting the simulation");
                    sim = new Simulation();
                    thread = new Thread(ActionSim);
                }
                else
                {
                    Console.WriteLine("COM port available, connecting to the bike...");
                    bike = new BikeConnection();
                    thread = new Thread(ActionBike);
                }

                thread.Start();
                
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("There isn't a stable connection, ERROR (restart sim.bat)");
            }
        }

        private void ActionSim()
        {
            sim.SendTxtMessageToServer("testid");
            Console.WriteLine(sim.ReadNormalTextMessage());
            while (true)
            {
                sim.GenerateRandomSt();
                var simdata = sim.SendCommand();
                var data = simdata.Split("\t".ToCharArray());
                ChangeSpeed(double.Parse(data[2]), data[7], double.Parse(data[3]) * 0.1, double.Parse(data[5]) * 0.238);
                sim.SendTxtMessageToServer(simdata);


                Thread.Sleep(1000);
            }
        }

        private void ActionBike()
        {
            while (true)
            {
                bike.sendCommand("st");
                var message = bike.Listen();
                var letters = message.Split("\t".ToCharArray());


                ChangeSpeed(double.Parse(letters[2]) * 0.1f, letters[6], double.Parse(letters[3]) * 0.1,
                    double.Parse(letters[5]) * 0.238);

                SettingsData.SendTextMessage(bike.Writer, message);

                Thread.Sleep(100);
            }
        }

        //method changes speed on the panel(display) and the moving speed of the biker in scene
        private void ChangeSpeed(double speed, string duration, double distance, double calories)
        {
            cw.WriteReadData(JSONMethods.UpdateNodeBike(speed / 100 * 2));

            cw.WriteReadData(JSONMethods.SetSpeed(speed / 10));

            cw.WriteData(JSONMethods.ClearPanel());

            cw.WriteReadData(JSONMethods.AddPanelText("Speed: " + speed + " km/h", 50.0f, 20.0f));


            cw.WriteReadData(JSONMethods.AddPanelText("Duration: " + duration, 100.0f, 20.0f));

            cw.WriteReadData(JSONMethods.AddPanelText("Distance: " + distance + " km", 150.0f, 20.0f));

            cw.WriteReadData(JSONMethods.AddPanelText("Calories: " + calories + " kcal", 200.0f, 20.0f));

            cw.WriteReadData(JSONMethods.AddPanelText("Time: " + DateTime.Now.ToString("HH:mm"), 390.0f, 320.0f));

            cw.WriteData(JSONMethods.swapPanel());
        }
    }
}