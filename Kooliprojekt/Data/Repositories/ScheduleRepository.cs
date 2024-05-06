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
    public class ScheduleRepository : BaseRepository<Schedule>, IScheduleRepository
    {
        private readonly ApplicationDbContext _context;
        public ScheduleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        //.Include(s => s.Song);
        public override async Task<List<Schedule>> All()
        {
            return await _context.Schedules.Include(s => s.Songs).ToListAsync();
        }

        public override async Task<PagedResult<Schedule>> Paged(int page)
        {
            return await _context.Schedules.Include(s => s.Songs).GetPagedAsync(page, 10);
        }
        public async Task<Schedule> GetSchedule(int id)
        {
            return await _context.Schedules.Where(s => s.ScheduleId == id).Include(s => s.Songs).ThenInclude(s => s.Song).FirstOrDefaultAsync();
        }
        public string GetSongTitle(int id)
        {
            return _context.Songs.FindAsync(id).Result.Title;
        }
        public async Task<IList<SelectListItem>> GetTitles(int id)
        {
            return await _context.Songs
                     .OrderBy(song => song.Title)
                     .Select(song => new SelectListItem
                     {
                         Text = song.Title,
                         Value = song.SongId.ToString(),
                         Selected = song.SongId == id
                     })
                     .ToListAsync();
        }
        public string GetArtist(int id)
        {
            var artist = _context.Songs.FindAsync(id).Result.ArtistId;
            return _context.Artists.FindAsync(artist).Result.Name;
        }
        public DateTime[] GetScheduleDates()
        {
            return _context.Schedules.Select(s => s.Date.Date).ToArray();
        }
        public IEnumerable<IGrouping<int, Song>> SongsByTempo()
        {
            return _context.Songs.AsEnumerable().GroupBy(x => x.Tempo);
        }

        public async Task<List<Schedule>> GetLatestSchedules()
        {
            return await _context.Schedules.Where(s => s.Date >= DateTime.Now.AddDays(-5) && s.Date <= DateTime.Now.AddDays(5)).Include(s => s.Songs).ThenInclude(s => s.Song).ToListAsync();
        }

        public async Task<PagedResult<Schedule>> GetForApiPaged(int page)
        {
            var artists = await _context.Schedules.Include(s => s.Songs).ThenInclude(s => s.Song).ThenInclude(s=>s.Artist).GetPagedAsync(1, 50);
            return artists;
        }
    }
}
