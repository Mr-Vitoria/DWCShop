using AnimeShop.Models;
using System.IO;
using System;

namespace AnimeShop.Helpers
{
    public static class FileUploaderHelper
    {
        public static async Task<string> Upload(IFormFile formFile)
        {
            if (formFile != null)
            {
                var filename = $"{Guid.NewGuid()}{Path.GetExtension(formFile.FileName)}";
                using var fs = new FileStream(@$"wwwroot/uploads/{filename}", FileMode.Create);

                await formFile.CopyToAsync(fs);
                return @$"/uploads/{filename}";
            }
            throw new Exception("File was not uploaded");
        }
        public static Task DeleteImg(string Url)
        {
            if(Url==null)
            {
                return Task.Run(() => { });
            }
            string str = Environment.CurrentDirectory;
            if (File.Exists("wwwroot"+Url))
            {

                return Task.Run(() => { File.Delete("wwwroot"+Url); });
            }
            return Task.Run(() => { });

        }


        public static async Task<string> UploadCategory(IFormFile formFile)
        {
            if (formFile != null)
            {
                var filename = $"{Guid.NewGuid()}{Path.GetExtension(formFile.FileName)}";
                using var fs = new FileStream(@$"wwwroot/images/categories/{filename}", FileMode.Create);

                await formFile.CopyToAsync(fs);
                return @$"/images/categories/{filename}";
            }
            throw new Exception("File was not uploaded");
        }
    }
}
