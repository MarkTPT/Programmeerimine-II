using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.Models;

namespace KooliProjekt.Services
{
    public interface ISongService 
    {
        Task<PagedResult<SongListModel>> List(int page);
        Task<SongListModel> GetForDetail(int id);
        Task<SongCreationModel> GetForCreate();
        Task FillCreate(SongCreationModel model);
        Task<OperationResponse> Create(SongCreationModel model);
        Task<SongEditModel> GetForEdit(int id);
        Task Edit(SongEditModel model);
        Task<Song> GetForDelete(int id);
        Task Delete(int id);
        Task<PagedResult<SongDto>> ApiGetList();
    }
}
