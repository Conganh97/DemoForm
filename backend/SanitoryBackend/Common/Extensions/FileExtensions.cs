using Microsoft.AspNetCore.Http;

namespace Common.Extensions
{
    public static class FileExtensions
    {
        public static async Task<string>
        UploadFile(this IFormFile file, string folderDestination)
        {
            var destinationFolder =
                Directory.GetCurrentDirectory() + folderDestination;
            if (
                !System.IO.Directory.Exists(destinationFolder) //create path
            )
            {
                Directory.CreateDirectory(destinationFolder);
            }
            string indentityFileName =
                Path.GetFileNameWithoutExtension(file.FileName) +
                "_" +
                DateTime.Now.ToString("HHmmssfffffff") +
                Path.GetExtension(file.FileName);
            var path =
                Path
                    .Combine(destinationFolder,
                    Path.GetFileName(indentityFileName)); //the path to upload
            await file.CopyToAsync(new FileStream(path, FileMode.Create));
            var result = $"{folderDestination}/{indentityFileName}";
            if (result.Contains("/wwwroot"))
            {
                result = result.Replace("/wwwroot", "");
            }
            return result;
        }
    }
}
