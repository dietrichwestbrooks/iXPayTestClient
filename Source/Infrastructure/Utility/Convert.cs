using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Utility
{
    public class Convert
    {
        public string ToHexString(byte[] bytes)
        {
            string hex = BitConverter.ToString(bytes);
            return hex.Replace("-", "");
        }

        public byte[] ToHexByteArray(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
                bytes[i / 2] = System.Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public byte[] ToImageByteArray(string imagePath)
        {
            string ext = Path.GetExtension(imagePath);

            if (ext == null)
                throw new InvalidOperationException("Invalid image file type");

            ImageFormat format;

            if (ext.ToLower() == ".png")
                format = ImageFormat.Png;
            else if (ext.ToLower() == ".gif")
                format = ImageFormat.Gif;
            else if (ext.ToLower() == ".jpg")
                format = ImageFormat.Jpeg;
            else if (ext.ToLower() == ".bmp")
                format = ImageFormat.Bmp;
            else
                throw new ArgumentException("Image format not supported", nameof(imagePath));

            var image = ToImage(imagePath);

            return ToImageByteArray(image, format);
        }

        public byte[] ToImageByteArray(Image image, ImageFormat format)
        {
            var ms = new MemoryStream();
            image.Save(ms, format);
            return ms.ToArray();
        }

        public Image ToImage(byte[] bytes)
        {
            var ms = new MemoryStream(bytes);
            Image image = Image.FromStream(ms);
            return image;
        }

        public Image ToImage(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                throw new ArgumentNullException(nameof(imagePath));

            string fullPath = Path.GetFullPath(imagePath);

            if (!File.Exists(fullPath))
                throw new ArgumentException("File does not exists", nameof(imagePath));

            var image = new Bitmap(fullPath);

            if (image == null)
                throw new InvalidOperationException("Unable to load image file");

            return image;
        }
    }
}
