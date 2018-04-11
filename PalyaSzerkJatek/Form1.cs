using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
            //pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;

            ip = new ImageProcessor();
            ip.CaptureChanged += myEventHandler;
            ip.startCapture(ImageProcessor.LOAD_FROM_CAMERA);
        }

      
        public void  myEventHandler(Mat frame,Mat thresholded, List<Wall> walls, List<Gem> gems)
        {
            Bitmap image1  = BitmapConverter.ToBitmap(frame);
            
            pictureBox1.Image = image1;
            Bitmap image2 = BitmapConverter.ToBitmap(thresholded);
            
            pictureBox2.Image = image2;
            Debug.WriteLine("Capture:");
            Debug.WriteLine($"Found gems : {gems.Count}");

            foreach (var gem in gems)
            {
                Debug.Write($"X : {gem.Position.X}   ");
                Debug.WriteLine($"Y : {gem.Position.Y}");
            }

            Debug.WriteLine($"");
            Debug.WriteLine($"Found walls : {walls.Count}");

            foreach (var wall in walls)
            {
                Debug.Write($"X : {wall.StartPosition.X}   ");
                Debug.WriteLine($"Y : {wall.StartPosition.Y}");
            }

            Debug.WriteLine("");
            Debug.WriteLine("");

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            ip.stopCapture();
            
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            ip.ThresholdNum = trackBar1.Value;
        }

     
    }
}
