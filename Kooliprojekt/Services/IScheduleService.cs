using KooliProjekt.Data;
using KooliProjekt.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public interface IScheduleService
    {
        Task<PagedResult<ScheduleListModel>> List(int page);
        Task<ScheduleDetailModel> GetForDetail(int id);
        Task<OperationResponse> Create(ScheduleCreateModel model);
        ScheduleCreateModel GetForCreate();
        Task<ScheduleEditModel> GetForEdit(int id);
        Task<OperationResponse> Save(ScheduleEditModel model);
        Task<Schedule> GetForDelete(int id);
        Task<OperationResponse> Delete(int? id);
        List<SongSchedule> GetSongSchedules(DateTime modelDate);
        Task<List<HomeListModel>> GetForHome();
        Task<PagedResult<ScheduleDto>> ApiGetList();
    }
}
