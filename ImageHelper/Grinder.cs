using System;
using System.Collections.Generic;
using System.Text;

namespace ImageHelper
{
    public class Grinder : Shader
    {
        public double Radius { get; private set; }

        public int Border { get; private set; }

        private Grinder()
        {

        }

        public static Grinder MakeRound(double radius, int border = 0)
        {
            return new Grinder() { Radius = radius, Border = border };
        }

        protected override void ReplacePixel(int x, int y, byte r, byte g, byte b, byte a, out byte nr, out byte ng, out byte nb, out byte na)
        {
            double rad = (Math.Min(Source.Width, Source.Height) / 2) * Radius;

            double xc = Source.Width / 2;
            double yc = Source.Height / 2;

            double h = Math.Sqrt((x - xc) * (x - xc) + (y - yc) * (y - yc));

            nr = r;
            ng = g;
            nb = b;

            if (h <= rad) {
                double d = rad - h;
                double faded = a;
                if (Border >= 1)
                    faded = Math.Min(255, Math.Max(0, a * (d / Border)));

                na = (byte)faded;
            } else
            {
                na = 0;
            }
        }
    }
}
