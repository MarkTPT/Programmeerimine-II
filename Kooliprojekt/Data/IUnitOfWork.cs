using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KooliProjekt.Data.Repositories;

namespace KooliProjekt.Data
{
    public interface IUnitOfWork
    {
        public IArtistRepository Artists { get; }
        public ISongRepository Songs { get; }
        public IStorageRepository Storage { get; }
        public IScheduleRepository Schedule { get; }
        public ISongScheduleRepository SongSchedule { get; }

        Task CompleteAsync();
    }
}
