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
            using (ImageProcessor processor = ImageProcessor.FromFile(@"D:\Pictures\Profilbilder\IMG_9084.JPG"))
            {
                processor
                    .Add(Spinner.OrientToExif())
                    .Add(Grinder.MakeRound(1, 20))
                    .Add(ColorAdaption.Decolorize())
                    .Add(ColorAdaption.Lighten(-30))
                    //.Add(ColorAdaption.Invert());
                    ;
                    
                processor.SaveToFile(@"C:\Users\flo\Pictures\rounded.png");
            }
        }
    }
}
