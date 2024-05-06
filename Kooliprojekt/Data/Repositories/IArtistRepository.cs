using KooliProjekt.Controllers.Repositories;
using KooliProjekt.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Data.Repositories
{
    public interface IArtistRepository : IBaseRepository<Artist>
    {
        List<string> GetSongTitles(int id);

        Task<List<SelectListItem>> GetArtists();

        IList<SongDetail> GetSongInfo(int id);

        Task<List<Artist>> List();
        Task<PagedResult<Artist>> GetArtistDto(int page);
        Task<PagedResult<string>> GetArtistNames(int page);

        void ChangeEntityState(ArtistModel entity, EntityState state);
    }
}
