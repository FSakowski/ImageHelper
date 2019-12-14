using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace ImageHelper
{
    public class Spinner : IOperation
    {
        const int ExifOrientationId = 0x112;

        protected Bitmap Source { get; private set; }

        private bool useEXIF;

        private RotateFlipType rotate = RotateFlipType.RotateNoneFlipNone;

        public Bitmap Process(Bitmap source)
        {
            Source = source;
            Bitmap target = source.Clone(new Rectangle(0, 0, Source.Width, Source.Height), Source.PixelFormat);

            if (useEXIF)
            {
                rotate = GetOrientationFromImage(Source);
                ClearExifOrientation(target);
            }

            target.RotateFlip(rotate);
            return target;
        }

        public static Spinner RotateFlip(RotateFlipType type)
        {
            return new Spinner() { rotate = type };
        }

        public static Spinner OrientToExif()
        {
            return new Spinner() { useEXIF = true };
        }

        private RotateFlipType GetOrientationFromImage(Image source)
        {
            if (!source.PropertyIdList.Contains(ExifOrientationId))
                return RotateFlipType.RotateNoneFlipNone;
            
            var prop = source.GetPropertyItem(ExifOrientationId);
            var orient = BitConverter.ToInt16(prop.Value, 0);
                
            switch(orient)
            {
                case 1: return RotateFlipType.RotateNoneFlipNone;
                case 2: return RotateFlipType.RotateNoneFlipX;
                case 3: return RotateFlipType.Rotate180FlipNone;
                case 4: return RotateFlipType.Rotate180FlipX;
                case 5: return RotateFlipType.Rotate90FlipX;
                case 6: return RotateFlipType.Rotate90FlipNone;
                case 7: return RotateFlipType.Rotate270FlipX;
                case 8: return RotateFlipType.Rotate270FlipNone;
                default: return RotateFlipType.RotateNoneFlipNone;
            }
        }

        private void ClearExifOrientation(Image img)
        {
            if (img.PropertyIdList.Contains(ExifOrientationId))
            {
                var prop = img.GetPropertyItem(ExifOrientationId);
                prop.Value = BitConverter.GetBytes((short)1);
                img.SetPropertyItem(prop);
            }
        }
    }
}
