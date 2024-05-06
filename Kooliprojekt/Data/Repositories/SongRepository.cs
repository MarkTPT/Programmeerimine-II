using KooliProjekt.Controllers.Repositories;
using KooliProjekt.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Data.Repositories
{
    public class SongRepository : BaseRepository<Song>, ISongRepository
    {
        private readonly ApplicationDbContext _context;

        public SongRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<List<Song>> All()
        {
            return await _context.Songs.Include(s => s.Artist)
                .Include(storage => storage.Storage)
                .ToListAsync();
        }

        public override async Task<PagedResult<Song>> Paged(int page)
        {
            return await _context.Songs.Include(s => s.Artist)
                .Include(storage => storage.Storage)
                .GetPagedAsync(page, 5);
        }

        public override async Task<Song> Get(int id)
        {
            return await _context.Songs.Include(s => s.Artist).FirstOrDefaultAsync(m => m.SongId == id);
        }

        public string GetCode(int songId)
        {
            return _context.Storages.Where(storage => storage.SongId == songId).Select(storage => storage.Kood).FirstOrDefault();
        }
        public async Task<PagedResult<Song>> GetSongDto(int page)
        {
            var songs = await _context.Songs.Include(song => song.Artist).GetPagedAsync(page, 10);

            return songs;
        }
    }
}
