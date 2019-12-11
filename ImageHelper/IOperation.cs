using System.Drawing;

namespace ImageHelper
{
    public interface IOperation
    {
        Bitmap Process(Bitmap source);
    }
}
