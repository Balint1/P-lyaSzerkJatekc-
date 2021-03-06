﻿using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalyaSzerkJatek
{
    class Converter
    {
        private static readonly Color BlackColor = new Color(0, 0, 0);
        private static readonly Color GreenColor = new Color(0, 255, 0);

        public static List<Wall> ContourToWall(OpenCvSharp.Point[] contours)
        {
            List<Wall> walls = new List<Wall>();

            if (contours.Count() == 0)
                return walls;

            if (contours == null)
                throw new NullReferenceException("Contours nem lehet null");

            for (int i = 0; i < contours.Length - 1; i++)
            {
                var start = new Point { X = contours[i].X, Y = contours[i].Y };
                var end = new Point { X = contours[i + 1].X, Y = contours[i + 1].Y };
                walls.Add(new Wall { Color = BlackColor, StartPosition = start, EndPosition = end });
            }
            walls.Add(new Wall { StartPosition = contours[0], EndPosition = contours.Last(), Color = BlackColor });


            return walls;
        }

        public static Gem ContourToGem(OpenCvSharp.Point[] contours)
        {
            Gem gem;

            if (contours == null)
                throw new NullReferenceException("Contours nem lehet null");

            gem = new Gem { Color = GreenColor, Position = CenterPoint( contours ) };

            return gem;
        }

        public static Point CenterPoint(Point[] points)
        {
            Point avgPoint = new Point { X = 0, Y = 0 };

            foreach (var point in points)
            {
                avgPoint.X += point.X;
                avgPoint.Y += point.Y;
            }
            avgPoint.X /= points.Length;
            avgPoint.Y /= points.Length;
            return avgPoint;
        }
        
    }
}
