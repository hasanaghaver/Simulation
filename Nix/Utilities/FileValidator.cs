using Nix.Utilities.Enums;
using System.Threading.Tasks;

namespace Nix.Utilities
{
    public static class FileValidator
    {
        public static bool CheckType(this IFormFile file, string type)
        {
            if (file.ContentType.Contains(type))
            {
                return true;
            }
            return false;
        }
        public static bool CheckSize(this IFormFile file, FileSize fileSize, decimal size)
        {
            switch (fileSize)
            {
                case FileSize.Kb:
                    return file.Length <= size * 1024;
                case FileSize.Mb:
                    return file.Length <= size * 1024 * 1024;
                case FileSize.Gb:
                    return file.Length <= size * 1024 *1024 *1024;
            }
            return false;
        }

        public static async Task<string> CreateFile(this IFormFile file, params string[] roots)
        {
            string fileName = String.Concat(Guid.NewGuid().ToString(),file.FileName);

            string path = String.Empty;

            foreach (string root in roots)
            {
                path = Path.Combine(path, root);
            }
            path = Path.Combine(path, fileName);

            using (FileStream fileStream = new FileStream(path,FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return fileName;
        }

        public static void DeleteFile(this string fileName, params string[] roots)
        {
            string path = String.Empty;

            foreach (string root in roots)
            {
                path = Path.Combine(path, root);
            }
            path = Path.Combine(path, fileName);

            File.Delete(path);
        }
    }
}
