
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;

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
        Wall[] currentWalls;
        Gem[] currentGems;
        Point minPoint;
        Point maxPoint;
        int width;
        int height;
        public float ratio = 133.333f;

        private void Form1_Load(object sender, EventArgs e)
        {
            ip = new ImageProcessor();
            ip.CaptureChanged += myEventHandler;
            ip.startCapture(ImageProcessor.LOAD_FROM_CAMERA);
            currentWalls = new Wall[]
            {
                new Wall { StartPosition = new Point() { X = 100, Y = 100 },   EndPosition = new Point() { X = 600, Y = 700 } },
                new Wall { StartPosition = new Point() { X = 100, Y = 100 }  , EndPosition = new Point() { X = 600, Y = 700 } },
                new Wall { StartPosition = new Point() { X = 100, Y = 100 }  , EndPosition = new Point() { X = 600, Y = 700 } },
                new Wall { StartPosition = new Point() { X = 100, Y = 100 }  , EndPosition = new Point() { X = 600, Y = 700 } },
                new Wall { StartPosition = new Point() { X = 100, Y = 100 }  , EndPosition = new Point() { X = 600, Y = 700 } },
            };
            currentGems = new Gem[]
            {
                new Gem { Position = new Point { X = 100, Y = 100 } },
                new Gem { Position = new Point { X = 100, Y = 100 } },
            };
        }

      
        public void  myEventHandler(OpenCvSharp.Mat frame, OpenCvSharp.Mat thresholded, List<Wall> walls, List<Gem> gems)
        {
            System.Drawing.Bitmap image1  = BitmapConverter.ToBitmap(frame);
            
            pictureBox1.Image = image1;
            System.Drawing.Bitmap image2 = BitmapConverter.ToBitmap(thresholded);
            
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
            currentGems = new Gem[gems.Count];
            currentWalls = new Wall[walls.Count];
            gems.CopyTo(currentGems);
            walls.CopyTo(currentWalls);
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

        private void SaveToTxt(string fileName )
        {
            calculateRatio();
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter($"{fileName}.txt"))
            {
                
                foreach (var wall in currentWalls)
                {
                    wall.StartPosition -= minPoint ;
                    wall.EndPosition -= minPoint ;
                    string line = $"wall {(wall.StartPosition.X - width / 2.0f) / ratio} {(wall.StartPosition.Y - height / 2.0f) / ratio } {(wall.EndPosition.X - width / 2.0f) / ratio } {(wall.EndPosition.Y - height / 2.0f) / ratio } 0 0 0";
                    file.WriteLine(line);
                }
                foreach (var gem in currentGems)
                {
                    gem.Position -= minPoint ;
                    string line = $"gem {(gem.Position.Y - width / 2.0f) / ratio } {(gem.Position.Y - height / 2.0f) / ratio } 0 255 0";
                    file.WriteLine(line);
                }
            }
        }

        private void calculateRatio()
        {
            minPoint = new Point() { X = 0, Y = 0 };
            maxPoint = new Point() { X = 1920, Y = 1080 };
            System.Drawing.Point[] minPoints = new System.Drawing.Point[] { new System.Drawing.Point(), new System.Drawing.Point(), new System.Drawing.Point(), };
            minPoints[0].X  = currentGems.Min(g => g.Position.X);
            minPoints[0].Y = currentGems.Min(g => g.Position.Y);
            minPoints[1].X = currentWalls.Min(g => g.StartPosition.X);
            minPoints[1].Y = currentWalls.Min(g => g.StartPosition.Y);
            minPoints[2].X = currentWalls.Min(g => g.EndPosition.X);
            minPoints[2].Y = currentWalls.Min(g => g.EndPosition.Y);
            minPoint.X = minPoints.Min(p => p.X);
            minPoint.Y = minPoints.Min(p => p.Y);

            System.Drawing.Point[] maxPoints = new System.Drawing.Point[] { new System.Drawing.Point(), new System.Drawing.Point(), new System.Drawing.Point(), };
            maxPoints[0].X = currentGems.Max(g => g.Position.X);
            maxPoints[0].Y = currentGems.Max(g => g.Position.Y);
            maxPoints[1].X = currentWalls.Max(g => g.StartPosition.X);
            maxPoints[1].Y = currentWalls.Max(g => g.StartPosition.Y);
            maxPoints[2].X = currentWalls.Max(g => g.EndPosition.X);
            maxPoints[2].Y = currentWalls.Max(g => g.EndPosition.Y);
            maxPoint.X = maxPoints.Max(p => p.X);
            maxPoint.Y = maxPoints.Max(p => p.Y);

            width = maxPoint.X - minPoint.X;
            height = maxPoint.Y - minPoint.Y;

            float horizontalRatio = width / 14.4f;
            float verticalRatio = height / 10.0f;

            ratio =  horizontalRatio > verticalRatio ? horizontalRatio : verticalRatio;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
                textBox1.Text = "Level1";
                
            SaveToTxt( textBox1.Text );
        }
    }
}
