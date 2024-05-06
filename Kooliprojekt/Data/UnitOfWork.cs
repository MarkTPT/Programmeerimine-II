using KooliProjekt.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IArtistRepository Artists { get; private set; }
        public ISongRepository Songs { get; private set; }
        public IStorageRepository Storage { get; private set; }
        public IScheduleRepository Schedule { get; private set; }
        public ISongScheduleRepository SongSchedule { get; private set; }

        public UnitOfWork(ApplicationDbContext context, IArtistRepository artistRepository, 
            ISongRepository songRepository, IStorageRepository storageRepository, IScheduleRepository scheduleRepository, ISongScheduleRepository songScheduleRepository)
        {
            _context = context;
            Artists = artistRepository;
            Songs = songRepository;
            Storage = storageRepository;
            Schedule = scheduleRepository;
            SongSchedule = songScheduleRepository;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
