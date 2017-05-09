using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using DataLibary;
using GUI.ClientApplication;
using Microsoft.Win32;
using serverNotTheRealOne.PopupBoxes;

namespace serverNotTheRealOne.ClientApplication
{
    public partial class Client : Form
    {
        private const int WmNclbuttondown = 0xA1;
        private const int HtCaption = 0x2;

        private static readonly ASCIIEncoding Asen = new ASCIIEncoding();
        private Image _copyImage;

        private Data _data;
        private int selectedValue;


        #region setup dragBar

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd,
            int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        #endregion


        // Enum for the pages from application
        private enum pages
        {
            HomePage,
            DataPage,
            privateMessagePage,
            CreateClientPage,
            CreateDoctorPage,
            ClickedSessionPage,
            DeleteClientPage,
            BroadcastPage
        }

        // boolean van is graph drawn 
        private bool _isGraphDrawn;
        private readonly BinaryReader _reader;
        private Person _selectedPerson = new Person("", "", "", "");

        private int _selectedPersonNumber = 0;
        private pages _tabSelected = pages.HomePage;
        private readonly BinaryWriter _writer;


        public Client(BinaryReader reader, BinaryWriter writer)
        {
            _reader = reader;
            _writer = writer;
            ReadObjectMessage(reader);

            InitializeComponent();

            var vertScrollWidth = SystemInformation.VerticalScrollBarWidth;

            tableLayoutPanel1.Padding = new Padding(0, 0, vertScrollWidth, 0);

            GenerateClientTable();
        }

        private static void SendTextMessage(BinaryWriter writer, string s)
        {
            try
            {
                var jsonBytes = Asen.GetBytes(s);

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

        private void ReadObjectMessage(BinaryReader reader)
        {
            try
            {
                SettingsData.SendTextMessage(_writer, "getDataObject");
                var packLen = reader.ReadInt32();
                var bytes = reader.ReadBytes(packLen);

                using (Stream stream = new MemoryStream(bytes))
                {
                    var bf = new BinaryFormatter();

                    _data = (Data) bf.Deserialize(stream);
                }
            }
            catch (Exception e)
            
{
                Console.WriteLine(e);
            }
        }


        private void GenerateClientTable(string filter = "")
        {
            var family = new FontFamily("Microsoft Sans Serif");
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.AutoScroll = false;

            var columcount = 0;
            var rowcount = 0;


            var personCount = 0;

            foreach (var i in _data.Persons)

            {
                if (string.Format($"{i.Firstname.ToLower()} {i.LastName.ToLower()}").Contains(filter) ||
                    filter.Equals(""))
                {
                    var wc = new WebClient();

                    var originalData = wc.DownloadData(i.PictureUrl);

                    var stream = new MemoryStream(originalData);
                    var bitmap = new Bitmap(stream);

                    var pw = new Button
                    {
                        Anchor = AnchorStyles.None,
                        BackgroundImage = bitmap,
                        Width = 148,
                        Height = 184,
                        TextImageRelation = TextImageRelation.ImageAboveText,
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Font = new Font(family, 16.0f,
                            FontStyle.Bold),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        FlatAppearance = {BorderColor = Color.DimGray},
                        BackColor = Color.DimGray
                    };


                    pw.MouseEnter += (sender, e) => HoverMouseEnter(sender, e, i);
                    pw.MouseLeave += HoverMouseLeave;
                    pw.Click += (sender, e) => ClickClientAction(sender, e, i, personCount);
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 250));
                    tableLayoutPanel1.Controls.Add(pw, columcount, rowcount);
                    columcount++;
                    if (columcount != 3) continue;
                    columcount = 0;
                    rowcount++;
                }

                personCount++;
            }
            tableLayoutPanel1.AutoScroll = true;
        }


        private string calculateTotalTime()
        {
            DateTime total = new DateTime();
            foreach (var s in _selectedPerson.Sessions)
            {

                //total = total.Add(TimeSpan.Parse($"00:{ s.List[]}"));

            }
            return total.ToString("HH:mm");
        }

        public void drawGraph(int index)
        {
            if (_isGraphDrawn == false)
            {

                Session selectedSession = _selectedPerson.Sessions[0];
                if (index > 0)
                {
                    selectedSession = _selectedPerson.Sessions[index];
                }



                int ii = 0;
                chart2.Titles.Clear();
                chart2.Titles.Add("speed ");
                foreach (String[] strings in selectedSession.List)
                {

                    int intSpeed = int.Parse(strings[2]);
                    double speed = intSpeed*0.1;

                    chart2.Series["Speed"].Points.AddXY(ii, speed);

                }

                chart3.Titles.Clear();
                chart3.Titles.Add("burned Kcal");

                ii = 0;
                foreach (String[] strings in selectedSession.List)
                {
                    int KJ = int.Parse(strings[5]);
                    double Kcal = KJ*0.239;
                    chart3.Series["burnedKcal"].Points.AddXY(ii, Kcal);

                }

                chart4.Titles.Clear();
                chart4.Titles.Add("RPM");
                ii = 0;
                foreach (String[] strings in selectedSession.List)
                {
                    chart4.Series["RPM"].Points.AddXY(ii, strings[1]);

                }
                chart5.Titles.Clear();
                chart5.Titles.Add("Actual Power in Watt");
                ii = 0;
                foreach (String[] strings in selectedSession.List)
                {
                    chart5.Series["APower"].Points.AddXY(ii, strings[7]);

                }
                chart6.Titles.Clear();
                chart6.Titles.Add("HeartBeats p/m");
                ii = 0;
                foreach (String[] strings in selectedSession.List)
                {
                    chart6.Series["BPM"].Points.AddXY(ii, strings[0]);

                }
                chart7.Titles.Clear();
                chart7.Titles.Add("Distance");
                ii = 0;
                foreach (String[] strings in selectedSession.List)
                {
                    int intDistance = int.Parse(strings[3]);
                    double distance = intDistance*0.1;

                    chart7.Series["distance"].Points.AddXY(ii, distance);
                }

                chart8.Titles.Clear();
                chart8.Titles.Add("Speed");
                List<double> speedList = createSpeedList();
                for (int i = 1; i < _selectedPerson.Sessions.Count + 1; i++)
                {


                    chart8.Series["Max"].Points.AddXY(i, (speedList.ElementAt(0) - speedList.ElementAt(1)));
                    chart8.Series["Avg"].Points.AddXY(i, (speedList.ElementAt(1) - speedList.ElementAt(2)));
                    chart8.Series["Min"].Points.AddXY(i, speedList.ElementAt(2));

                    speedList.RemoveAt(2);
                    speedList.RemoveAt(1);
                    speedList.RemoveAt(0);
                }

                chart9.Titles.Clear();
                chart9.Titles.Add("Kcal");
                List<double> kcalList = createKcalList();
                for (int i = 1; i < _selectedPerson.Sessions.Count + 1; i++)
                {
                    chart9.Series["Max kcal"].Points.AddXY(i, kcalList.ElementAt(0));
                    kcalList.RemoveAt(0);
                }

                chart10.Titles.Clear();
                chart10.Titles.Add("RPM");
                List<double> RPMList = createRPMList();
                for (int i = 1; i < _selectedPerson.Sessions.Count + 1; i++)
                {


                    chart10.Series["Max"].Points.AddXY(i, (RPMList.ElementAt(0) - RPMList.ElementAt(1)));
                    chart10.Series["Avg"].Points.AddXY(i, (RPMList.ElementAt(1) - RPMList.ElementAt(2)));
                    chart10.Series["Min"].Points.AddXY(i, RPMList.ElementAt(2));

                    RPMList.RemoveAt(2);
                    RPMList.RemoveAt(1);
                    RPMList.RemoveAt(0);
                }


                chart11.Titles.Clear();
                chart11.Titles.Add("Distance");
                List<double> DistList = CreateDistanceList();
                for (int i = 1; i < _selectedPerson.Sessions.Count + 1; i++)
                {

                    chart11.Series["Distance"].Points.AddXY(i, DistList.ElementAt(0));
                    DistList.RemoveAt(0);
                }


                chart12.Titles.Clear();
                chart12.Titles.Add("BPM");
                List<double> BPMList = createBPMList();
                for (int i = 1; i < _selectedPerson.Sessions.Count + 1; i++)
                {


                    chart12.Series["Max"].Points.AddXY(i, (BPMList.ElementAt(0) - BPMList.ElementAt(1)));
                    chart12.Series["Avg"].Points.AddXY(i, (BPMList.ElementAt(1) - BPMList.ElementAt(2)));
                    chart12.Series["Min"].Points.AddXY(i, BPMList.ElementAt(2));

                    BPMList.RemoveAt(2);
                    BPMList.RemoveAt(1);
                    BPMList.RemoveAt(0);
                }

                chart13.Titles.Clear();
                chart13.Titles.Add("Total time");
                List<double> TimeList = CreateTimeList();
                for (int i = 1; i < _selectedPerson.Sessions.Count + 1; i++)
                {

                    chart13.Series["Time"].Points.AddXY(i, TimeList.ElementAt(0));
                    TimeList.RemoveAt(0);
                }

            }
        }

        private void GenerateDataPanel()
        {

            //for (int i = 0; i < dataList.Rows.Count; i++)
            //{
            //    Console.WriteLine(dataList.Rows.Count);
            //    Console.WriteLine(dataList.RowCount);
            //    dataList.Rows.RemoveAt(i);
            //}

            dataList.Rows.Clear();

            //dataList.Columns.Clear();
            //dataList.ResetText();



            foreach (Session s in _data.Persons[_selectedPersonNumber].Sessions)
            {
                int lastData = s.List.Count - 1;
                dataList.Rows.Add(s.List[lastData][8], s.List[lastData][3], s.List[lastData][6]);
            }

            if (_selectedPerson.Sessions.Count != 0)
            {
                drawGraph(0);
                //createSpeedList();
                //createKcalList();
                //createRPMList();
                //createPowerList();

            }
            else
            {
                clearGraph();
            }




        }


        public List<double> createSpeedList()
        {
            double maxSpeed = 0;
            double avgSpeed = 0;
            double totalSpeed = 0;
            double minSpeed = 999;

            List<double> speedValues = new List<double>();
            for (int i = 0; i < _selectedPerson.Sessions.Count; i++)
            {
                Session selectedSession = _selectedPerson.Sessions.ElementAtOrDefault(i);

                foreach (String[] strings in selectedSession.List)
                {
                    double speed = double.Parse(strings[2]);

                    totalSpeed = totalSpeed + speed;

                    if (maxSpeed < speed)
                        maxSpeed = speed;

                    if (minSpeed > speed && speed != 0)
                    {
                        minSpeed = speed;
                    }
                }
                if (minSpeed == 999)
                {
                    minSpeed = 0;
                }

                avgSpeed = totalSpeed/selectedSession.List.Count;
                if (avgSpeed < minSpeed)
                    avgSpeed = minSpeed;
                speedValues.Add(Math.Round(maxSpeed*0.1, 2));
                speedValues.Add(Math.Round(avgSpeed*0.1, 2));
                speedValues.Add(Math.Round(minSpeed*0.1, 2));

                maxSpeed = 0;
                avgSpeed = 0;
                totalSpeed = 0;
                minSpeed = 999;

            }
            return speedValues;
        }


        public List<Double> createKcalList()
        {
            double maxKcal = 0;
            List<double> kcalList = new List<double>();
            for (int i = 0; i < _selectedPerson.Sessions.Count; i++)
            {
                Session selectedSession = _selectedPerson.Sessions.ElementAtOrDefault(i);

                foreach (String[] strings in selectedSession.List)
                {
                    double KJ = double.Parse(strings[5]);
                    double Kcal = KJ*0.239;

                    if (maxKcal < Kcal)
                    {
                        maxKcal = Kcal;
                    }
                }
                kcalList.Add(maxKcal);
                maxKcal = 0;
            }
            return kcalList;
        }


        public List<double> createRPMList()
        {
            double maxRPM = 0;
            double avgRPM = 0;
            double totalRPM = 0;
            double minRPM = 999;

            List<double> RPMValues = new List<double>();
            for (int i = 0; i < _selectedPerson.Sessions.Count; i++)
            {
                Session selectedSession = _selectedPerson.Sessions.ElementAtOrDefault(i);

                foreach (String[] strings in selectedSession.List)
                {
                    double RPM = double.Parse(strings[1]);

                    totalRPM = totalRPM + RPM;

                    if (maxRPM < RPM)
                        maxRPM = RPM;

                    if (minRPM > RPM && RPM != 0)
                        minRPM = RPM;

                }
                if (minRPM == 999)
                {
                    minRPM = 0;
                }
                avgRPM = totalRPM/selectedSession.List.Count;
                if (avgRPM < minRPM)
                    avgRPM = minRPM;
                RPMValues.Add(Math.Round(maxRPM, 2));
                RPMValues.Add(Math.Round(avgRPM, 2));
                RPMValues.Add(Math.Round(minRPM, 2));

                maxRPM = 0;
                avgRPM = 0;
                totalRPM = 0;
                minRPM = 999;

            }
            return RPMValues;
        }

        public List<double> createPowerList()
        {
            double maxPower = 0;
            double avgPower = 0;
            double totalPower = 0;
            double minPower = 999;

            List<double> PowerValues = new List<double>();
            for (int i = 0; i < _selectedPerson.Sessions.Count; i++)
            {
                Session selectedSession = _selectedPerson.Sessions.ElementAtOrDefault(i);

                foreach (String[] strings in selectedSession.List)
                {
                    double Power = double.Parse(strings[7]);

                    totalPower = totalPower + Power;

                    if (maxPower < Power)
                        maxPower = Power;

                    if (minPower > Power && Power != 0)
                        minPower = Power;

                }
                avgPower = totalPower/selectedSession.List.Count;
                if (avgPower < minPower)
                    avgPower = minPower;
                PowerValues.Add(Math.Round(maxPower, 2));
                PowerValues.Add(Math.Round(avgPower, 2));
                PowerValues.Add(Math.Round(minPower, 2));

                maxPower = 0;
                avgPower = 0;
                totalPower = 0;
                minPower = 999;

            }
            return PowerValues;
        }


        public List<Double> CreateDistanceList()
        {
            double maxDist = 0;
            List<double> DistList = new List<double>();

            for (int i = 0; i < _selectedPerson.Sessions.Count; i++)
            {
                Session selectedSession = _selectedPerson.Sessions.ElementAtOrDefault(i);

                foreach (String[] strings in selectedSession.List)
                {
                    double dist = double.Parse(strings[3]);

                    if (maxDist < dist)
                    {
                        maxDist = dist;
                    }
                }
                DistList.Add(maxDist);
                maxDist = 0;
            }
            return DistList;
        }

        public List<double> createBPMList()
        {
            double maxBPM = 0;
            double avgBPM = 0;
            double totalBPM = 0;
            double minBPM = 999;
            int zeroValues = 0;

            List<double> BPMValues = new List<double>();
            for (int i = 0; i < _selectedPerson.Sessions.Count; i++)
            {
                Session selectedSession = _selectedPerson.Sessions.ElementAtOrDefault(i);

                foreach (String[] strings in selectedSession.List)
                {
                    double BPM = double.Parse(strings[0]);

                    totalBPM = totalBPM + BPM;

                    if (maxBPM < BPM)
                        maxBPM = BPM;

                    if (minBPM > BPM && BPM != 0)
                        minBPM = BPM;

                }

                if (minBPM == 999)
                {
                    minBPM = 0;
                }
                avgBPM = totalBPM/selectedSession.List.Count;

                if (avgBPM < minBPM)
                    avgBPM = minBPM;
                BPMValues.Add(Math.Round(maxBPM, 2));
                BPMValues.Add(Math.Round(avgBPM, 2));
                BPMValues.Add(Math.Round(minBPM, 2));

                maxBPM = 0;
                avgBPM = 0;
                totalBPM = 0;
                minBPM = 999;
                zeroValues = 0;

            }
            return BPMValues;
        }

        public List<Double> CreateTimeList()
        {
            double maxTime = 0;
            List<double> TimeList = new List<double>();

            for (int i = 0; i < _selectedPerson.Sessions.Count; i++)
            {
                Session selectedSession = _selectedPerson.Sessions.ElementAtOrDefault(i);

                foreach (String[] strings in selectedSession.List)
                {
                    double dist = double.Parse(strings[3]);

                    if (maxTime < dist)
                    {
                        maxTime = dist;
                    }
                }
                TimeList.Add(maxTime);
                maxTime = 0;
            }
            return TimeList;
        }

        public void printValues(List<double> list)
        {
            foreach (double value in list)
            {
                Console.WriteLine(value);
            }
        }

        // fills in the sessionDatalist grid vieuw with the needed values
        private void FillSessionDataGrid(int index)
        {

            sessionDataList.Rows.Clear();
            var selectedSession = _data.Persons[_selectedPersonNumber].Sessions[index];

            foreach (var strings in selectedSession.List)
                sessionDataList.Rows.Add(strings[8], strings[0], strings[1], strings[2], strings[3], strings[5],
                    strings[6], strings[7]);
        }

        private void clearSessionDataGrid()
        {
            sessionDataList.SelectAll();
            sessionDataList.ClearSelection();
        }

        // parameter is the selected type to calculate the total for
        // 23 = distance  5 = energy
        private string CalculateTotal(int selectedIndex)
        {
            if ((selectedIndex == 3) || (selectedIndex == 5))
            {
                var total = 0;
                foreach (var s in _data.Persons[_selectedPersonNumber].Sessions)
                {
                    foreach (var s2 in s.List)
                    {
                        total += int.Parse(s2[selectedIndex]) ;
                    }

                }
                return total.ToString();
            }
            return null;
        }

        // calculates the total time from all sessions and converts this to hours
        // it only takes the whole minutes form the sessions
        private string CalculateTotalTime()
        {
            double total = 0;

            foreach (var s in _selectedPerson.Sessions)
            {

                var ss = s.List[s.List.Count - 1];
                var time = Convert.ToDouble(ss[6].Substring(0, 2));
                total += time;
            }
            total = Math.Round(total/60, 2);

            return total + "";
        }

        // Switching to the different pages from the application
        private void SwitchPage(pages page)
        {

            ReadObjectMessage(_reader);


            try
            {
                if (_data.Persons[_selectedPersonNumber].Sessions.Count != 0 && _data.Persons.Count != 0)
                {
                    List<Session> sessions = _data.Persons[_selectedPersonNumber].Sessions;            
                    List<string[]> data = sessions[sessions.Count - 1].List;
                    string lastPostedTime = data[data.Count - 1][8];
                    Console.WriteLine(lastPostedTime);
                    string sub = lastPostedTime.Substring(0, 10);
                    string date = String.Format($"{DateTime.Now.Month}/{DateTime.Now.Day}/{DateTime.Now.Year}");


                    int hour = (int.Parse(lastPostedTime.Substring(11, 2)));
                    int minute = (int.Parse(lastPostedTime.Substring(14, 2)));
                    if (date == sub && (DateTime.Now.Minute - minute) < 2 &&
                        DateTime.Now.Hour == hour)
                    {

                        livePicture.Visible = true;
                    }
                    else
                    {
                        livePicture.Visible = false;
                    }
                }
            }
            catch (Exception)
            {

                livePicture.Visible = false;
            }
               


            if (_data.Persons.Count != 0)
            {
                _selectedPerson = _data.Persons[_selectedPersonNumber];
            }
            if(_tabSelected != page) {

                switch (_tabSelected)
                {
                    case pages.HomePage:
                        tableLayoutPanel1.Visible = false;
                        homeButton.BackColor = Color.Transparent;
                        break;
                    case pages.DataPage:
                        dataCenterButton.BackColor = Color.Transparent;
                        dataPanel.Visible = false;
                        break;
                    case pages.privateMessagePage:
                        privateMailButton.BackColor = Color.Transparent;
                        privateMailPanel.Visible = false;
                        RefreshGraphs();
                        break;
                    case pages.CreateClientPage:
                        setupButton.BackColor = Color.Transparent;
                        break;
                    case pages.CreateDoctorPage:
                        addDoctorButton.BackColor = Color.Transparent;
                        doctorPanel.Visible = false;
                        break;
                    case pages.ClickedSessionPage:
                        rawDataPanel.Visible = false;
                        break;
                    case pages.DeleteClientPage:
                        deleteClientButton.BackColor = Color.Transparent;
                        break;
                    case pages.BroadcastPage:
                        broadcastButton.BackColor = Color.Transparent;
                        broadcastPanel.Visible = false;
                        break;
                }
                _tabSelected = page;
            }
            

            SetPanelsInvisible();
            switch (page)
            {
                case pages.HomePage:
                    homeButton.BackColor = Color.FromArgb(0, 108, 1);
                    
                    clientPanel.Visible = false;
                    tableLayoutPanel1.Visible = true;
                    GenerateClientTable();
                    break;
                case pages.DataPage:

                    dataCenterButton.BackColor = Color.FromArgb(0, 108, 1);
                    dataPanel.Visible = true;
                    refresh();
                    break;
                case pages.privateMessagePage:                    
                    privateMailButton.BackColor = Color.FromArgb(0, 108, 1);
                    privateMailText.Items.Clear();
                    foreach (string message in _data.Persons[_selectedPersonNumber].Messages)
                    {
                        privateMailText.Items.Add(message);
                    }
                    privateMailPanel.Visible = true;
                    break;
                case pages.CreateClientPage:                    
                    addClientPanel.Visible = true;
                    bikeIDBox.Items.Clear();
                    foreach (string bikeid in _data.BikeIDs)
                    {
                        bikeIDBox.Items.Add(bikeid);
                    }
                    setupButton.BackColor = Color.FromArgb(0, 108, 1);
                    break;
                case pages.CreateDoctorPage:                   
                    addDoctorButton.BackColor = Color.FromArgb(0, 108, 1);
                    doctorPanel.Visible = true;
                    break;
                case pages.ClickedSessionPage:
                    FillSessionDataGrid(selectedValue);
                    rawDataPanel.Visible = true;
                    checkBox1_CheckedChanged(new object(), new EventArgs());
                    break;
                case pages.DeleteClientPage:
                    deleteClientButton.BackColor = Color.FromArgb(0, 108, 1);
                    break;
                case pages.BroadcastPage:
                    broadcastButton.BackColor = Color.FromArgb(0, 108, 1);
                    GenerateBroadCastPanel();
                    broadcastPanel.Visible = true;
                    break;
            }
        }

        private void GenerateBroadCastPanel()
        {
            broadcastMessageBox.Items.Clear();
            foreach (string message in _data.BroadcastMsg)
            {
                broadcastMessageBox.Items.Add(message);
            }
           
        }

        public void refresh()
        {
            if (_tabSelected == pages.DataPage || _tabSelected == pages.ClickedSessionPage)
            {
                Console.WriteLine("komt hierroo");
                clearGraph();
                RefreshGraphs();
                dataCenterButton.BackColor = Color.FromArgb(0, 108, 1);
                distanceTotal.Text = CalculateTotal(3) + " km";
                cyclingTimeTotal.Text = CalculateTotalTime() + " hours";
                caloriesTotal.Text = CalculateTotal(5) + " Kj";
                GenerateDataPanel();
            }

            if (_tabSelected == pages.privateMessagePage)
            {
                SwitchPage(pages.privateMessagePage);
            }

            if (_tabSelected == pages.privateMessagePage)
            {
                SwitchPage(pages.ClickedSessionPage);
                clearGraph();
                _isGraphDrawn = false;
                drawGraph(selectedValue);
                clearSessionDataGrid();
                Console.WriteLine(selectedValue);
                //FillSessionDataGrid(selectedValue);
            }
        }


        private void ClickClientAction(object sender, EventArgs e, Person p, int personCount)
        {
            _selectedPersonNumber = personCount;
            _selectedPerson = p;
            currentClientLable.Text = p.Firstname[0].ToString().ToUpper() + ". " + p.LastName;
            clientPanel.Visible = true;
            SwitchPage(pages.DataPage);
            RefreshGraphs();
        }

        private void HoverMouseLeave(object sender, EventArgs e)
        {
            var b = (Button) sender;
            b.FlatAppearance.BorderColor = Color.DimGray;
            b.BackgroundImage = _copyImage;
            b.Text = "";
        }

        private void HoverMouseEnter(object sender, EventArgs e, Person p)
        {
            var b = (Button) sender;
            b.FlatAppearance.BorderColor = Color.FromArgb(0, 108, 1);
            _copyImage = b.BackgroundImage;
            b.BackgroundImage = null;
            b.Text = p.Firstname + " " + p.LastName;
        }


        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WmNclbuttondown, HtCaption, 0);
            }
        }


        private void sessionButton_Click(object sender, EventArgs e)
        {
            SwitchPage(pages.privateMessagePage);
        }

        private void setupButton_Click(object sender, EventArgs e)
        {
            SwitchPage(pages.CreateClientPage);
        }

        private void addDoctorButton_Click(object sender, EventArgs e)
        {
            SwitchPage(pages.CreateDoctorPage);
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void minimizeButton_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            SwitchPage(_tabSelected);
            refresh();
        }

        private void dktApplicationLabel_MouseDown(object sender, MouseEventArgs e)
        {
            panel1_MouseDown(sender, e);
        }

        private void qTechLogo_Click(object sender, EventArgs e)
        {
            Process.Start("www.4chan.org");
        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            SwitchPage(0);
        }

        private void dataButton_Click(object sender, EventArgs e)
        {
            tableLayoutPanel1.Visible = false;
            SwitchPage(pages.DataPage);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            SwitchPage(pages.HomePage);
            GenerateClientTable(filterTextbox.Text.ToLower());
        }

        private void dataList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //_selectedSessionIndex = e.RowIndex;
            SwitchPage(pages.ClickedSessionPage);
            clearGraph();
            _isGraphDrawn = false;
            drawGraph(e.RowIndex);
            clearSessionDataGrid();
            Console.WriteLine(e.RowIndex);
            FillSessionDataGrid(e.RowIndex);
            selectedValue = e.RowIndex;

        }

        private void createAccountButton_Click(object sender, EventArgs e)
        {
            if ((usernameBox.TextLength >= 4) && (passwordBox.TextLength >= 4) &&
                (passwordBox.Text == passwordConfirmBox.Text))
            {
                SendTextMessage(_writer, "addUser");
                SendTextMessage(_writer, usernameBox.Text);
                SendTextMessage(_writer, Password.GenerateSHA256Hash(passwordBox.Text, Password.Salt));

                ClearDoctorAddBoxes();
                MessageBox.Show("Account created");
            }

            else
            {
                ClearDoctorAddBoxes();
                MessageBox.Show("Account was not created due to incorrect fields.");
            }
        }

        private void ClearDoctorAddBoxes()
        {
            usernameBox.Text = "";
            passwordBox.Text = "";
            passwordConfirmBox.Text = "";
        }

        //Makes all the GUI panels invisible
        private void SetPanelsInvisible()
        {
            dataPanel.Visible = false;
            rawDataPanel.Visible = false;
            doctorPanel.Visible = false;
            addClientPanel.Visible = false;
            privateMailPanel.Visible = false;
            broadcastPanel.Visible = false;
        }

        // graph refresher 
        private void RefreshGraphs()
        {
            chart2.Refresh();
            chart3.Refresh();
            chart4.Refresh();
            chart5.Refresh();
            chart6.Refresh();
            chart7.Refresh();
            chart8.Refresh();
            chart9.Refresh();
            chart10.Refresh();
            chart11.Refresh();
            chart12.Refresh();
            chart13.Refresh();
        }

        private void uploadPreviewPictureButton_Click(object sender, EventArgs e)
        {
            var url = clientPictureUrlTextBox.Text;

            clientPicPreview.SizeMode = PictureBoxSizeMode.StretchImage;
            clientPicPreview.ImageLocation = url;
        }

        private void createClient_Click(object sender, EventArgs e)
        {
            // if all fields are corrected filled in
            if ((clientPicPreview.Image != null) && (clientFirstNameTextBox.Text.Length > 0) &&
                (clientLastNameTextBox.Text.Length > 0) && ( clientPictureUrlTextBox != null))
            {
                // voeg persoont toe
                SendTextMessage(_writer, "addClient");
                SendTextMessage(_writer, clientPictureUrlTextBox.Text);
                SendTextMessage(_writer, clientFirstNameTextBox.Text);
                SendTextMessage(_writer, clientLastNameTextBox.Text);
                SendTextMessage(_writer, bikeIDBox.Text);

                ClearClientFields();
                MessageBox.Show("Client was succesfull created.");
                clientPicPreview.Image = null;
                SwitchPage(pages.HomePage);
            }
            else
            {
                MessageBox.Show("Client was not created due to incorrect fields.");
            }
        }

        private void ClearClientFields()
        {
            clientPictureUrlTextBox.Text = "";
            clientFirstNameTextBox.Text = "";
            clientLastNameTextBox.Text = "";
        }

        private void deleteClientButton_Click(object sender, EventArgs e)
        {
        
            var dialogResult =
                MessageBox.Show(
                    $"Are yous sure you want to delete : {_selectedPerson.Firstname} {_selectedPerson.LastName} from the database?",
                    "Confirm", MessageBoxButtons.YesNo);
            switch (dialogResult)
            {
                case DialogResult.Yes:
                    SendTextMessage(_writer, "deleteClient");
                    SendTextMessage(_writer, _selectedPersonNumber + "");
                    MessageBox.Show("Client was succesfull deleted.");

                    SwitchPage(pages.HomePage);
                    break;
                case DialogResult.No:
                    SwitchPage(pages.DataPage);
                    break;
            }

 
        }


     

        private void clearGraph()
        {
            foreach (var series in chart2.Series)
                series.Points.Clear();
            foreach (var series in chart3.Series)
                series.Points.Clear();
            foreach (var series in chart4.Series)
                series.Points.Clear();
            foreach (var series in chart5.Series)
                series.Points.Clear();
            foreach (var series in chart6.Series)
                series.Points.Clear();
            foreach (var series in chart7.Series)
                series.Points.Clear();
        }

        private void dataList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Console.WriteLine(e.RowIndex);
            //selectedSessionGraph = e.RowIndex;

            clearGraph();
            _isGraphDrawn = false;
            drawGraph(e.RowIndex);


        }

        private void broadcastButton_Click(object sender, EventArgs e)
        {
            SwitchPage(pages.BroadcastPage);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void broadcastSendButton_Click(object sender, EventArgs e)
        {
            SettingsData.SendTextMessage(_writer, "addBroadcast");
            SettingsData.SendTextMessage(_writer, String.Format($"{DateTime.Now}: {broadcastTextBox.Text} (Doctor broadcast)"));
            SwitchPage(pages.BroadcastPage);
            broadcastTextBox.Text = "";
        }

        private void privateMailSendButton_Click(object sender, EventArgs e)
        {
            SettingsData.SendTextMessage(_writer, "addPrivatemessage");
            SettingsData.SendTextMessage(_writer, _selectedPersonNumber.ToString());
            SettingsData.SendTextMessage(_writer, String.Format($"{DateTime.Now}: {privateMailMessageBox.Text} (Doctor)"));
            privateMailMessageBox.Text = "";
            SwitchPage(pages.privateMessagePage);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddBikeID bikeidpopup = new AddBikeID();
            bikeidpopup.ShowDialog();
            if (bikeidpopup.Confirmed)
            {
                SettingsData.SendTextMessage(_writer, "addBikeID");
                SettingsData.SendTextMessage(_writer, bikeidpopup.BikeId);
            }
           
            SwitchPage(pages.CreateClientPage);
        }

        private void broadcastTextBox_Enter(object sender, EventArgs e)
        {
            broadcastTextBox.Text = "";
            broadcastTextBox.ForeColor = Color.Black;
        }

        private void privateMailMessageBox_Enter(object sender, EventArgs e)
        {
            privateMailMessageBox.Text = "";
            privateMailMessageBox.ForeColor = Color.Black;
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            //if(_data.Persons[_selectedPersonNumber].Sessions[_data.Persons[_selectedPersonNumber].Sessions.Count-1].List.Count != 0)
            //Console.WriteLine(_tabSelected);
            //SwitchPage(_tabSelected);
            //refresh();
            if (_tabSelected == pages.ClickedSessionPage)
            {
                dataList_CellDoubleClick(new object(), new DataGridViewCellEventArgs(0, selectedValue));
                checkBox1_CheckedChanged(new object(), new EventArgs());

            }
            else
            {
                checkBox1_CheckedChanged(new object(), new EventArgs());

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (refreshTimer.Enabled)
            {
                checkBox1.Checked = false;
                refreshTimer.Stop();
            }
            else
            {
                checkBox1.Checked = true;
                refreshTimer.Start();
            }
        }
    }
}