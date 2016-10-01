using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iXPayTestClient.Modules.Script.Services
{
    public class Utility
    {
        public string ByteArrayToHexString(byte[] bytes)
        {
            string hex = BitConverter.ToString(bytes);
            return hex.Replace("-", "");
        }

        public byte[] HexStringToByteArray(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public byte[] ImageToByteArray(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            string fullPath = Path.GetFullPath(path);

            if (!File.Exists(fullPath))
                throw new ArgumentException("File does not exists", nameof(path));

            string ext = Path.GetExtension(fullPath);

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
                throw new ArgumentException("Unknown format", nameof(path));

            var image = new Bitmap(fullPath);

            if (image == null)
                throw new InvalidOperationException("Unable to load image file");

            return ImageToByteArray(image, format);
        }

        public byte[] ImageToByteArray(Image image, ImageFormat format)
        {
            var ms = new MemoryStream();
            image.Save(ms, format);
            return ms.ToArray();
        }

        public Image ByteArrayToImage(byte[] bytes)
        {
            var ms = new MemoryStream(bytes);
            Image image = Image.FromStream(ms);
            return image;
        }
    }
}
