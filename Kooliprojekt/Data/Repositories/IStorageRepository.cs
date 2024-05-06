using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KooliProjekt.Controllers.Repositories;

namespace KooliProjekt.Data.Repositories
{
    public interface IStorageRepository : IBaseRepository<Storage>
    {
        Task<Song> GetSong(int id);
    }
}
