namespace tcc_mypet_back.Extensions
{
    public class ImageExtensions
    {
        public static string ConvertFileToBase64(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            var fileBytes = memoryStream.ToArray();
            return Convert.ToBase64String(fileBytes);
        }
    }
}
