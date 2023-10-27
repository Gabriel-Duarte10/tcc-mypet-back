using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.AspNetCore.Http;
using SkiaSharp;

namespace tcc_mypet_back.Extensions
{
    public class ImageExtensions
    {
        public static string ConvertFileToBase64(IFormFile file, long quality = 75L, SKFilterQuality quality2 = SKFilterQuality.Medium)
        {
            using var stream = file.OpenReadStream();
            using var ms = new MemoryStream();
            using var correctedImage = CorrectOrientation(stream);
            using var bitmap = SKBitmap.Decode(correctedImage);
            using var resizedBitmap = bitmap.Resize(new SKImageInfo(bitmap.Width / 10, bitmap.Height / 10), quality2);
            using var image = SKImage.FromBitmap(resizedBitmap);
            using var data = image.Encode(SKEncodedImageFormat.Jpeg, (int)quality);
            data.SaveTo(ms);
            var base64String = Convert.ToBase64String(ms.ToArray());
            Console.WriteLine($"Base64 string length: {base64String.Length}");
            return base64String;
        }

        private static MemoryStream CorrectOrientation(Stream stream)
        {
            var image = Image.FromStream(stream);
            var orientationProperty = Array.Find(image.PropertyItems, pi => pi.Id == 0x112);
            if (orientationProperty != null)
            {
                var orientation = orientationProperty.Value[0];
                switch (orientation)
                {
                    case 1: // No rotation required.
                        break;
                    case 2: // Flip on horizontal axis.
                        image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        break;
                    case 3: // Rotate 180°
                        image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case 4: // Flip on vertical axis
                        image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        break;
                    case 5: // Rotate 90° and flip
                        image.RotateFlip(RotateFlipType.Rotate90FlipX);
                        break;
                    case 6: // Rotate 90°
                        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 7: // Rotate 270° and flip
                        image.RotateFlip(RotateFlipType.Rotate270FlipX);
                        break;
                    case 8: // Rotate 270°
                        image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                }
            }

            var correctedStream = new MemoryStream();
            image.Save(correctedStream, ImageFormat.Jpeg);
            correctedStream.Seek(0, SeekOrigin.Begin);
            return correctedStream;
        }
    }
}
