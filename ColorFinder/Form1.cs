using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Global.Functions;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using Point = System.Drawing.Point;

namespace ColorFinder
{
    public partial class Form1 : Form
    {
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);

        string dd = "Drag/Drop";
        public Color GetColorAt(Point location)
        {
            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }

            return screenPixel.GetPixel(0, 0);
        }

        public Form1()
        {
            InitializeComponent();
            button3.Text = dd;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            timer1.Enabled = true;
            button1.Enabled = false;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            timer1.Enabled = false;
            button2.Enabled = false;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Point cPos = myMouse.GetCursorPosition();
            Color mColor = GetColorAt(cPos);
            //lblColor.BackColor = Color.FromArgb(int.Parse(txtR.Text.ToString()),int.Parse(txtG.Text.ToString()),int.Parse(txtB.Text.ToString()));
            lblColor.BackColor = mColor;
            txtR.Text = mColor.R.ToString();
            txtG.Text = mColor.G.ToString();
            txtB.Text = mColor.B.ToString();
            txtHex.Text = "# " + mColor.R.ToString("X2") + mColor.G.ToString("X2") + mColor.B.ToString("X2");
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            //
        }

        private void Button3_MouseDown(object sender, MouseEventArgs e)
        {
            button3.Text = "";
            timer1.Enabled = true;
        }

        private void Button3_MouseUp(object sender, MouseEventArgs e)
        {
            button3.Text = dd;
            timer1.Enabled = false;
        }
    }
}
