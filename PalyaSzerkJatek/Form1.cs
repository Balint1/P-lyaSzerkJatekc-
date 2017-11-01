using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalyaSzerkJatek
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ImageProcessor ip;
        private void Form1_Load(object sender, EventArgs e)
        {
           // pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            ip = new ImageProcessor();
            ip.CaptureChanged += myEventHandler;
            ip.startCapture(ImageProcessor.LOAD_FROM_CAMERA);
        }

      
        public void  myEventHandler(Mat frame,Mat thresholded)
        {
            Bitmap image1  = BitmapConverter.ToBitmap(frame);
            
            pictureBox1.Image = image1;
            Bitmap image2 = BitmapConverter.ToBitmap(thresholded);

            pictureBox2.Image = image2;

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            ip.stopCapture();
            
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            ip.thresholdNum = trackBar1.Value;
        }

     
    }
}
