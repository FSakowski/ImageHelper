using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace ImageHelper
{
    public abstract class Shader : IOperation
    {
        protected const int pixelSize = 4;

        protected Bitmap Source { get; private set; }

        public Bitmap Process(Bitmap source)
        {
            Source = source;

            Bitmap target = CreateTarget(source);

            ProcessInternal(source, target);

            return target;
        }

        private unsafe void ProcessInternal(Bitmap source, Bitmap target)
        {
            BitmapData sourceData = null;
            BitmapData targetData = null;

            try
            {
                sourceData = source.LockBits(
                    new Rectangle(0, 0, source.Width, source.Height),
                    ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                targetData = target.LockBits(
                        new Rectangle(0, 0, target.Width, target.Height),
                        ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                for (int y = 0; y < source.Height; y++)
                {
                    byte* rowSrc = (byte*)sourceData.Scan0 + (y * sourceData.Stride);
                    byte* rowTgt = (byte*)targetData.Scan0 + (y * targetData.Stride);

                    for (int x = 0; x < source.Width; x++)
                    {
                        byte r = rowSrc[x * pixelSize + 0];
                        byte g = rowSrc[x * pixelSize + 1];
                        byte b = rowSrc[x * pixelSize + 2];
                        byte a = rowSrc[x * pixelSize + 3];

                        ReplacePixel(x, y, r, g, b, a, out byte nr, out byte ng, out byte nb, out byte na);

                        rowTgt[x * pixelSize + 0] = nr;
                        rowTgt[x * pixelSize + 1] = ng;
                        rowTgt[x * pixelSize + 2] = nb;
                        rowTgt[x * pixelSize + 3] = na;
                    }
                }
            } finally
            {
                if (sourceData != null)
                    source.UnlockBits(sourceData);

                if (targetData != null)
                    target.UnlockBits(targetData);
            }
        }

        protected virtual Bitmap CreateTarget(Bitmap source)
        {
            return new Bitmap(source.Width, source.Height, PixelFormat.Format32bppArgb);
        }

        protected abstract void ReplacePixel(int x, int y, byte r, byte g, byte b, byte a, out byte nr, out byte ng, out byte nb, out byte na);
    }
}
