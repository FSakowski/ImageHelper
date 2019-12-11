using System;
using System.Collections.Generic;
using System.Text;

namespace ImageHelper
{
    public class ColorAdaption : Shader
    {
        private bool invert = false;

        private bool decolorize = false;

        private int lightenRate = 0;

        public static ColorAdaption Invert()
        {
            return new ColorAdaption() { invert = true };
        }

        public static ColorAdaption Decolorize()
        {
            return new ColorAdaption() { decolorize = true };
        }

        public static ColorAdaption Lighten(int percentage)
        {
            return new ColorAdaption() { lightenRate = percentage };
        }

        protected override void ReplacePixel(int x, int y, byte r, byte g, byte b, byte a, out byte nr, out byte ng, out byte nb, out byte na)
        {
            nr = r;
            ng = g;
            nb = b;

            if (invert)
            {
                nr = (byte)(255 - r);
                ng = (byte)(255 - g);
                nb = (byte)(255 - b);
            }

            if (decolorize)
            {
                ng = nr;
                nb = nr;
            }

            if (lightenRate != 0)
            {
                double rate = 1 + lightenRate / 100d;

                nr = (byte)Math.Min(255, Math.Max(0, nr * rate));
                ng = (byte)Math.Min(255, Math.Max(0, ng * rate));
                nb = (byte)Math.Min(255, Math.Max(0, nb * rate));
            }

            na = a;
        }
    }
}
