using ImageHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    class Program
    {
        static void Main(string[] args)
        {
            ImageProcessor processor = ImageProcessor.FromFile(@"D:\Pictures\Sync\2019\09\IMG_1149.JPG");
            processor.Add(Grinder.MakeRound(1, 50));
            processor.Add(ColorAdaption.Decolorize());
            processor.Add(ColorAdaption.Lighten(-30));
            processor.SaveToFile(@"C:\Users\flo\Pictures\rounded.png");

        }
    }
}
