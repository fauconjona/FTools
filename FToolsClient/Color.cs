using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FToolsClient
{
    class Color
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public int A { get; set; }

        public Color(int r, int g, int b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = 255;
        }

        public Color(int r, int g, int b, int a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        public double RPercent()
        {
            return this.R / 255;
        }

        public double GPercent()
        {
            return this.G / 255;
        }

        public double BPercent()
        {
            return this.B / 255;
        }
    }
}
