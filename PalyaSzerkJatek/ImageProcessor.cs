using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PalyaSzerkJatek
{
    class ImageProcessor
    {
        public static readonly string LOAD_IMG = "img";
        public static readonly string LOAD_FROM_CAMERA = "cam";
        private Point[][] contours ;
        private Thread camera;
        VideoCapture cap;
        public delegate void CaptureEventHandler(Mat frame, Mat Thresholded);
        public event CaptureEventHandler CaptureChanged;
        public int thresholdNum = 120;
        private Mat thresholded = new Mat();

        public void capture(string mode)
        {
            Mat frame = new Mat();
                switch (mode)
            {
                case "img":
                    
                frame = Cv2.ImRead("C:\\Users\\Balint\\Pictures\\palya.jpg");
                    if (frame.Empty())
                        throw new NullReferenceException("Nem sikerült a képet betölteni");
                    break;
                case "cam":
                   
                    cap = new VideoCapture();
                    cap.Open(0);
                    break;
            }
            if(cap != null)
            while (true)
            {
            cap.Read(frame);
            thresholded = process(frame);
            frame = findPoly(frame);
            CaptureChanged(frame,thresholded);
            
            }
            CaptureChanged(frame, process(frame));


        }

        private Mat findPoly(Mat frame)
        {
            foreach (Point[] contour in contours)
            {
                Point[] walls = Cv2.ApproxPolyDP(contour, 5.2, false);
                if ((Cv2.ContourArea(walls, false) > 1000 && walls.Length < 9) || walls.Length < 4)
                    drawShape(frame, walls, Scalar.Green);
                Point[] shapes = Cv2.ApproxPolyDP(contour, 5.2, true);
                if (Cv2.ContourArea(shapes, false) < 1000 && shapes.Length < 5 && shapes.Length > 2)
                    drawShape(frame, shapes, Scalar.Blue);
            }
            return frame;
        }

        public void startCapture(string mode)
        {
            camera = new Thread(() => capture(mode));
            camera.Start();
        }
        public void stopCapture()
        {
            camera.Abort();
        }

        private Mat process(Mat frame)
        {
            Mat grayScaled = new Mat();
           // Mat thresholded = new Mat();
            
            Cv2.CvtColor(frame, grayScaled, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(grayScaled, thresholded, thresholdNum, 255, ThresholdTypes.Binary);
            findContours(thresholded,frame);
            return thresholded; // findContours(thresholded,frame); ;

        }

        public Mat findContours(Mat thresholded,Mat frame)
        {
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(thresholded,out contours,out hierarchy, RetrievalModes.List, ContourApproximationModes.ApproxNone);
           
            Mat ret = new Mat();
           // frame.CopyTo(ret);
            //foreach (Point[] contour in contours)
           // {
            //   Point[] appr = Cv2.ApproxPolyDP(contour,10, true);
          //      if(Cv2.ContourArea(appr,false) >1 && appr.Length < 10)
           //    drawShape(ret, appr, Scalar.Red);
          //  }
            return ret;
        }

        private void drawShape(Mat img, Point[] approx, Scalar color)
        {
            int count = approx.Length;
            for (int i = 0; i < count; i++)
            {
                 if (i != count - 1)
                    Cv2.Line(img, approx[i].X, approx[i].Y, approx[i + 1].X, approx[i + 1].Y, color, 3);
                  else Cv2.Line(img, approx[i].X, approx[i].Y, approx[0].X, approx[0].Y, color, 3);
                Cv2.Circle(img, approx[i], 4, Scalar.Red,3);


            }

        }

    }
}
