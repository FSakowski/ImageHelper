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

        public ImageProcessor(Bitmap source)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            imageOperations = new List<IOperation>();
        }

        public void Add(IOperation imgOp)
        {
            if (imgOp == null)
                throw new ArgumentNullException(nameof(imgOp));

            imageOperations.Add(imgOp);
        }

        public void Clear()
        {
            imageOperations.Clear();
        }

        public static ImageProcessor FromFile(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            Bitmap source = new Bitmap(fileName);

            if (source.PixelFormat == PixelFormat.Format32bppArgb)
                return new ImageProcessor(source);

            Bitmap convert = source.Clone(new Rectangle(0, 0, source.Width, source.Height), PixelFormat.Format32bppArgb);
            source.Dispose();
            return new ImageProcessor(convert);
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
