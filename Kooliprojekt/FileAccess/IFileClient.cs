using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.FileAccess
{
    public interface IFileClient
    {
        public Task Save(string container, string fileName, Stream fileStream);
        public Task Save(string container, string fileName, Stream fileStream, string id);
        public Task Delete(string container, string fileName);
        public Task<IList<string>> List(string container);
        public Task<Stream> GetFile(string container, string fileName);
    }
}
