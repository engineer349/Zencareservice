using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Zencareservice.Models
{
    public class CaptchaGenerator
    {

        public static Tuple<string, byte[]> GenerateCaptchaImage(int width = 200, int height = 50)
        {
            var text = GenerateRandomText();
            var image = GenerateImage(text, width, height);

            using (var stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Png);
                return Tuple.Create(text, stream.ToArray());
            }
        }

        private static string GenerateRandomText()
        {
            // Generate random text for the CAPTCHA
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static Bitmap GenerateImage(string text, int width, int height)
        {
            // Create an image with the generated text
            var image = new Bitmap(width, height);
            var graphics = Graphics.FromImage(image);

            // Add noise or background pattern if needed

            // Draw the text on the image
            graphics.DrawString(text, new Font("Arial", 16), Brushes.Black, new PointF(10, 10));

            return image;
        }

    }
}
