using System;
using System.Drawing;
using System.Windows.Forms;

namespace ScrapbookQA
{
    public partial class ImgForm : Form
    {
        public ImgForm()
        {
            InitializeComponent();
            this.Text = "";
        }

        private void MsgBox_Load(object sender, EventArgs e)
        {
        }

        public void SetPath(string imgURL)
        {
            LoadImage(imgURL);
            CenterWindow();
        }

        private void LoadImage(string imgURL)
        {
            pictureBox1.Load(string.Format(imgURL));
        }

        private void CenterWindow()
        {
            this.Location = new Point(this.Location.X - (pictureBox1.Size.Width / 2), this.Location.Y - (pictureBox1.Size.Height / 2));
        }
    }
}
