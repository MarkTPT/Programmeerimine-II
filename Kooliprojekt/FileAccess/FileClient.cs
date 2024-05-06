using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.FileAccess
{
    public class FileClient : IFileClient
    {
        public async Task<IList<string>> List(string container)
        {
            var path = Path.Combine("wwwroot", "files", container);
            List<string> files = Directory.EnumerateFiles(path).ToList();
            List<string> paths = new List<string>();
            foreach (var file in files)
            {
                paths.Add(file.Replace("\\", "/").Substring(7));
            }
            return paths;
            //return Directory.EnumerateFiles(path).ToList();
        }
        public async Task Delete(string container, string fileName)
        {
            var path = Path.Combine("wwwroot", "files", container, fileName);
            File.Delete(path);
        }
        public async Task Save(string container, string fileName, Stream fileStream)
        {          
            var originalName = Path.GetFileName(fileName);
            var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(originalName);
            var path = Path.Combine("wwwroot", "files", container, newFileName);

            using (var savingStream = new FileStream(path, FileMode.CreateNew))
            {
                await fileStream.CopyToAsync(savingStream);
            }
        }
        public async Task Save(string container, string fileName, Stream fileStream, string id)
        {
            var originalName = Path.GetFileName(fileName);
            var newFileName = id + "_" + Guid.NewGuid().ToString() + Path.GetExtension(originalName);
            var path = Path.Combine("wwwroot", "files", container, newFileName);

            using (var savingStream = new FileStream(path, FileMode.CreateNew))
            {
                await fileStream.CopyToAsync(savingStream);
            }

        }
        public async Task<Stream> GetFile(string container, string fileName)
        {
            var path = Path.Combine("wwwroot", "files", container, fileName);
            return File.OpenRead(path);
        }

    }
}
