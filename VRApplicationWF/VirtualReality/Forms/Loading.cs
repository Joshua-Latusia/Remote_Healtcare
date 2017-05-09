using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using VRApplicationWF.VirtualReality.Scripts;

namespace VRApplicationWF.VirtualReality.Forms
{
    public partial class Loading : Form
    {
        private readonly Connection cw;
        private readonly JSONMethods jsonmethods;



        public Loading(object connection, string id)
        {
            InitializeComponent();


            cw = (Connection)connection;
            jsonmethods = new JSONMethods(id);

            // Console.WriteLine("start generating map");
            backgroundWorker1.RunWorkerAsync();
        }


        private void DoStuff()
        {
            //hoewerktdit
            //Task<string> t = Task.Factory.StartNew(() => cw.WriteReadData(jsonmethods.GetNode("GroundPlane")));
            //Console.WriteLine(t.Result);

            cw.WriteReadData(jsonmethods.ResetScene());

            backgroundWorker1.ReportProgress(4);


            cw.WriteReadData(jsonmethods.AddNode("data/NetworkEngine/models/bike/bike_anim.fbx", "Bike",
                "Armature|Fietsen", true, 0.02f));

            backgroundWorker1.ReportProgress(8);


            cw.WriteReadData(JSONMethods.GetNode("Camera"));

            backgroundWorker1.ReportProgress(14);


            var uuidGP =
                JsonConvert.DeserializeObject<RootObject>(cw.WriteReadData(JSONMethods.GetNode("GroundPlane")))
                    .data.data.data[0].uuid;

            backgroundWorker1.ReportProgress(18);
            var uuidCamera =
                JsonConvert.DeserializeObject<RootObject>(cw.WriteReadData(JSONMethods.GetNode("Camera")))
                    .data.data.data[0].uuid;
            cw.WriteReadData(JSONMethods.GetNode("Bike"));

            backgroundWorker1.ReportProgress(22);
            var uuidBike =
                JsonConvert.DeserializeObject<RootObject>(cw.WriteReadData(JSONMethods.GetNode("Bike"))).data.data.data[
                    0].uuid;


            backgroundWorker1.ReportProgress(26);
            cw.WriteReadData(jsonmethods.AddNodePanel("Panel", uuidCamera));

            backgroundWorker1.ReportProgress(31);
            JSONMethods.PanelUuid =
                JsonConvert.DeserializeObject<RootObject>(cw.WriteReadData(JSONMethods.GetNode("Panel"))).data.data.data
                    [0].uuid;

            backgroundWorker1.ReportProgress(35);

            cw.WriteReadData(jsonmethods.UpdateNode(uuidCamera));

            backgroundWorker1.ReportProgress(39);

            cw.WriteReadData(jsonmethods.UpdateNodeParent(uuidCamera, uuidBike));

            backgroundWorker1.ReportProgress(43);

            cw.WriteReadData(jsonmethods.RemoveObject(uuidGP));

            backgroundWorker1.ReportProgress(47);


            cw.WriteReadData(jsonmethods.SetTime());

            backgroundWorker1.ReportProgress(51);

            // not using this method because we prefer the normal skybox
            //   Console.WriteLine(cw.writeReadData(jsonmethods.addSkyBox()));

            cw.WriteReadData(jsonmethods.AddNodeWater("Water"));

            backgroundWorker1.ReportProgress(55);

            cw.WriteReadData(jsonmethods.AddFlatTerrain());


            backgroundWorker1.ReportProgress(72);

            var tnuuid = cw.WriteReadData(jsonmethods.AddNodeTerrain("test")).Substring(124, 36);

            backgroundWorker1.ReportProgress(77);

            var addlayer = jsonmethods.AddLayer(tnuuid, "data/NetworkEngine/textures/tarmac_diffuse.png",
                "data/NetworkEngine/textures/terrain/grass_green_d.jpg", 0, 5);
            var addlayer2 = jsonmethods.AddLayer(tnuuid, "data/NetworkEngine/textures/tarmac_diffuse.png",
               "data/NetworkEngine/textures/terrain/mntn_green_d.jpg", 0, 200);


            backgroundWorker1.ReportProgress(82);
            cw.WriteReadData(addlayer);
            cw.WriteReadData(addlayer2);





            backgroundWorker1.ReportProgress(88);

            var uuid = cw.WriteReadData(jsonmethods.AddRoute());

            backgroundWorker1.ReportProgress(92);

            uuid = uuid.Substring(110, 36);


            cw.WriteReadData(jsonmethods.AddRoad(uuid));

            backgroundWorker1.ReportProgress(96);

            cw.WriteReadData(jsonmethods.FollowRoute(uuidBike));
            JSONMethods.BikeUuid = uuidBike;


            //doesn't work?
            cw.WriteData(jsonmethods.ShowRoute(false));

            cw.WriteReadData(jsonmethods.SetPanelClearColor());

          

            backgroundWorker1.ReportProgress(100);
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DoStuff();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            loadingCircle.Invoke(
                (MethodInvoker)delegate { loadingCircle.UpdateProgress(Convert.ToInt32(e.ProgressPercentage)); });
        }


        private void backgroundWorker1_RunWorkerCompleted(object sender,
            RunWorkerCompletedEventArgs e)
        {
            this.Hide();
            ChatWindow form2 = new ChatWindow(cw);
            form2.Closed += (s, args) => this.Close();
            form2.Show();
        }


        private void GUI_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}