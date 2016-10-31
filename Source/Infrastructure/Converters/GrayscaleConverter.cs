using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Extensions;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Converters
{
    [ValueConversion(typeof(BitmapImage), typeof(BitmapImage))]
    public class GrayscaleConverter : ConverterBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var source = value as BitmapImage;

            if (source == null)
                return null;

            Bitmap original = source.ToBitmap();

            //create a blank bitmap the same size as original
            Bitmap bitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            using (var g = Graphics.FromImage(bitmap))
            {
                //create the grayscale ColorMatrix
                ColorMatrix colorMatrix = new ColorMatrix(
                    new float[][]
                        {
                            new float[] {.3f, .3f, .3f, 0, 0},
                            new float[] {.59f, .59f, .59f, 0, 0},
                            new float[] {.11f, .11f, .11f, 0, 0},
                            new float[] {0f, 0, 0, 1, 0},
                            new float[] {0f, 0, 0, 0, 1}
                        });

                //create some image attributes
                ImageAttributes attributes = new ImageAttributes();

                //set the color matrix attribute
                attributes.SetColorMatrix(colorMatrix);

                //draw the original image on the new image
                //using the grayscale color matrix
                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                    0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            }

            return bitmap.ToImage();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
