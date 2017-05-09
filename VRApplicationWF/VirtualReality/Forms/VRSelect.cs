using System;
using System.Data;
using System.Windows.Forms;
using Newtonsoft.Json;
using VRApplicationWF.VirtualReality.Scripts;

namespace VRApplicationWF.VirtualReality.Forms
{
    public partial class VRSelect : Form
    {
        private readonly Connection cw;
        private readonly SessionList sList;

        public VRSelect()
        {
            InitializeComponent();


            cw = new Connection();

            var test = cw.WriteReadData(JSONMethods.SessionList());


            sList = JsonConvert.DeserializeObject<SessionList>(test);


            GenerateTable();
        }

        private void GenerateTable()
        {
            var table = new DataTable();

            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("Host", typeof(string));

            foreach (var d in sList.data)
                table.Rows.Add(d.id, d.clientinfo.host);

            dataGridView1.DataSource = table;
        }


        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var pwframe = new Password {StartPosition = FormStartPosition.CenterParent};
                pwframe.ShowDialog();

                Console.WriteLine("chosen row: " + e.RowIndex);
                var tunnel = cw.WriteReadData(JSONMethods.CreateTunnel(sList.data[e.RowIndex].id, pwframe.textBox));

                var id = tunnel.Substring(50, 36);
                Console.WriteLine(id);


                this.Hide();
                Loading gui = new Loading(cw, id);
                gui.Closed += (s, args) => this.Close();
                gui.Show();
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("There isn't a stable connection, ERROR (restart sim.bat)");
            }
        }
    }
}