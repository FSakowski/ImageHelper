using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageHelper
{
    public sealed class ImageProcessor : IDisposable
    {
        private readonly List<IOperation> imageOperations;

        private bool disposed = false;

        public Bitmap Source { get; private set; }

        public Bitmap Output { get; private set; }

        public ImageProcessor()
        {
            imageOperations = new List<IOperation>();
        }

        public ImageProcessor(Bitmap source) : this()
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public ImageProcessor Add(IOperation imgOp)
        {
            if (imgOp == null)
                throw new ArgumentNullException(nameof(imgOp));

            imageOperations.Add(imgOp);
            return this;
        }

        public void Clear()
        {
            imageOperations.Clear();
        }

        public static ImageProcessor FromFile(string fileName)
        {
            return new ImageProcessor().LoadImage(fileName);
        }

        public ImageProcessor LoadImage(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            Source = new Bitmap(fileName);
            EnsurePixelFormat();
            return this;
        }

        public ImageProcessor LoadImage(Image img)
        {
            if (img == null)
                throw new ArgumentNullException(nameof(img));

            Source = new Bitmap(img);
            EnsurePixelFormat();
            return this;
        }

        private void EnsurePixelFormat()
        {
            if (Source == null)
                return;

            if (Source.PixelFormat != PixelFormat.Format32bppArgb)
            {
                Bitmap converted = Source.Clone(new Rectangle(0, 0, Source.Width, Source.Height), PixelFormat.Format32bppArgb);
                Source.Dispose();
                Source = converted;
            }
        }

        public void Execute()
        {
            if (Source == null)
                return;

            Bitmap currImg = Source;
            foreach (IOperation imgOp in imageOperations)
            {
                Bitmap lastImg = Output;
                Output = imgOp.Process(currImg);
                currImg = Output;

                if (lastImg != null)
                    lastImg.Dispose();
            }
        }

        public void SaveToFile(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            if (Output == null)
                Execute();

            if (Output != null)
                Output.Save(fileName);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (Source != null)
                    Source.Dispose();

                if (Output != null)
                    Output.Dispose();
            }

            disposed = true;
        }
    }
}
