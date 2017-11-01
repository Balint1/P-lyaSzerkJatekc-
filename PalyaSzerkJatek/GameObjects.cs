using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalyaSzerkJatek
{
    struct GameObjects
    {
        public Mat frame;
        public Point[][] walls;
        public Point[][] fires;
        public Point[][] gems;
    }
}
