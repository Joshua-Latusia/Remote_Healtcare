using System;
using System.Windows.Forms;

namespace VRApplicationWF
{
    public partial class Password : Form
    {
        public Password()
        {
            InitializeComponent();
        }

        public string textBox { get; private set; }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox = textBox1.Text;
            Close();
        }
    }
}