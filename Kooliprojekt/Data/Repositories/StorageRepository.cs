using KooliProjekt.Controllers.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Data.Repositories
{
    public class StorageRepository : BaseRepository<Storage>, IStorageRepository
    {
        private readonly ApplicationDbContext _context;

        public StorageRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<List<Storage>> All()
        {
            return await _context.Storages
                .Include(s => s.Song).ToListAsync();
        }

        public override async Task<PagedResult<Storage>> Paged(int page)
        {
            return await _context.Storages.Include(s => s.Song).GetPagedAsync(page, 10);
        }
        public override async Task<Storage> Get(int id)
        {
            return await _context.Storages
                .Include(s => s.Song)
                .ThenInclude(a => a.Artist)
                .FirstOrDefaultAsync(m => m.StorageID == id);
        }

        public async Task<Song> GetSong(int id)
        {
            return await _context.Songs.Where(song => song.SongId == id).FirstOrDefaultAsync();
        }

    }
}
