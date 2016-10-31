using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Extensions
{
    public static class BitmapExtenstions
    {
        public static Bitmap ToBitmap(this BitmapImage source)
        {
            if (source == null)
                return null;

            Bitmap bitmap;

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(source));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }

            return bitmap;
        }

        public static BitmapImage ToImage(this Bitmap source)
        {
            if (source == null)
                return null;

            MemoryStream ms = new MemoryStream();

            source.Save(ms, ImageFormat.Bmp);
            ms.Position = 0;

            BitmapImage image = new BitmapImage();

            image.BeginInit();
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }
    }
}
