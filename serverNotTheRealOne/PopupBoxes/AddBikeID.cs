using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace serverNotTheRealOne.PopupBoxes
{
    public partial class AddBikeID : Form
    {

        private string bikeID = "";
        private bool confirmed = false;
        public AddBikeID()
        {
            InitializeComponent();
        }

        public bool Confirmed
        {
            get { return confirmed; }
            set { confirmed = value; }
        }

        public string BikeId
        {
            get { return bikeID; }
            set { bikeID = value; }
        }

        private void add_Click(object sender, EventArgs e)
        {
            confirmed = true;
            bikeID = textBox1.Text;
            Close();
        }
    }
}
