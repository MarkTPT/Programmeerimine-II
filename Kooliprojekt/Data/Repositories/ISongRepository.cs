using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KooliProjekt.Controllers.Repositories;
using KooliProjekt.Models;

namespace KooliProjekt.Data.Repositories
{
    public interface ISongRepository : IBaseRepository<Song>
    {
        string GetCode(int songId);
        Task<PagedResult<Song>> GetSongDto(int page);
    }
}
