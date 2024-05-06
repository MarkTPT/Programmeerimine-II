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
    public class ArtistRepository : BaseRepository<Artist>, IArtistRepository
    {
        private readonly ApplicationDbContext _context;

        public ArtistRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<List<Artist>> All()
        {
            return await _context.Artists.Include(song => song.Songs).ToListAsync();
        }
        public async Task<List<Artist>> List()
        {
            return await _context.Artists.ToListAsync();
        }

        public override async Task<PagedResult<Artist>> Paged(int page)
        {
            return await _context.Artists.Include(song => song.Songs).GetPagedAsync(page, 5);
        }

        public List<string> GetSongTitles(int id)
        {
            return _context.Songs.Where(s => s.ArtistId == id).Select(song => song.Title).ToList();
        }

        public async Task<List<SelectListItem>> GetArtists()
        {
            return await _context.Artists
                                 .OrderBy(artist => artist.Name)
                                 .Select(artist => new SelectListItem
                                 {
                                     Text = artist.Name,
                                     Value = artist.ArtistId.ToString()
                                 })
                                 .ToListAsync();
        }

        public IList<SongDetail> GetSongInfo(int id)
        {
            return _context.Songs
                           .Where(song => song.ArtistId == id)
                           .Select(song => new SongDetail
                           {
                               Title = song.Title,
                               Tempo = song.Tempo
                           }).ToList();
        }

        public async Task<PagedResult<Artist>> GetArtistDto(int page)
        {
            var artists = await _context.Artists.Include(artist => artist.Songs)
                                                        .ThenInclude(song => song.Storage)
                                                .Include(artist => artist.Songs)
                                                        .ThenInclude(song => song.SongSchedules)
                                                .GetPagedAsync(page, 50);

            return artists;
        }

        public async Task<PagedResult<string>> GetArtistNames(int page)
        {
            return await _context.Artists.Select(artist => artist.Name).GetPagedAsync(page, 100);
        }

        public void ChangeEntityState(ArtistModel entity, EntityState state)
        {
            _context.Entry(entity).State = state;
        }

    }
}
