using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VRApplicationWF.VirtualReality.Forms
{
    public partial class LoadingMap : UserControl
    {
        private int progress;

        public LoadingMap()
        {
            progress = 0;
            InitializeComponent();
        }

        private void LoadingMap_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(Width/2, Height/2);
            e.Graphics.RotateTransform(-90);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var pen = new Pen(Color.FromArgb(0, 108, 1));
            var rect = new Rectangle(0 - Width/2 + 20, 0 - Height/2 + 20, Width - 40, Height - 40);
            e.Graphics.DrawPie(pen, rect, 0, (int) (progress*3.6));
            e.Graphics.FillPie(new SolidBrush(Color.FromArgb(0, 108, 1)), rect, 0, (int) (progress*3.6));


            pen = new Pen(Color.FromArgb(35, 33, 35));
            rect = new Rectangle(0 - Width/2 + 30, 0 - Height/2 + 30, Width - 60, Height - 60);
            e.Graphics.DrawPie(pen, rect, 0, 360);
            e.Graphics.FillPie(new SolidBrush(Color.FromArgb(35, 33, 35)), rect, 0, 360);

            e.Graphics.RotateTransform(90);

            var ft = new StringFormat();
            ft.Alignment = StringAlignment.Center;
            ft.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawString(progress + "%", new Font("Arial", 30), new SolidBrush(Color.White), rect, ft);
        }

        public void UpdateProgress(int progress)
        {
            this.progress = progress;
            Invalidate();
        }
    }
}