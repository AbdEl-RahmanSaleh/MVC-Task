namespace Task.Helper
{
    public static class DocumentSettings
    {
        public static async Task<string> UploadFile(IFormFile file, string folderName)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", folderName);

            var fileName = $"{Guid.NewGuid()}-{Path.GetFileName(file.FileName)}";

            var filePath = Path.Combine(folderPath, fileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(fileStream);

            return fileName;
        }

        public static void DeleteFile(string folderName, string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", $"{folderName}/", fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
