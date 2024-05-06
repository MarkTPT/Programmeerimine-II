using KooliProjekt.Controllers.Repositories;
using KooliProjekt.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Data.Repositories
{
    public interface IScheduleRepository : IBaseRepository<Schedule>
    {
        string GetSongTitle(int id);
        Task<IList<SelectListItem>> GetTitles(int id);
        Task<Schedule> GetSchedule(int id);
        string GetArtist(int id);
        DateTime[] GetScheduleDates();
        IEnumerable<IGrouping<int, Song>> SongsByTempo();
        Task<List<Schedule>> GetLatestSchedules();
        Task<PagedResult<Schedule>> GetForApiPaged(int page);
    }
}
