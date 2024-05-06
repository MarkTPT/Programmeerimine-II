using KooliProjekt.Controllers.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Data.Repositories
{
    public class SongScheduleRepository : BaseRepository<SongSchedule>, ISongScheduleRepository
    {
        private readonly ApplicationDbContext _context;

        public SongScheduleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
