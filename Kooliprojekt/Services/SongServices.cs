using System.Threading.Tasks;
using AutoMapper;
using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.FileAccess;
using KooliProjekt.Models;

namespace KooliProjekt.Services
{
    public class SongService : ISongService
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileClient _fileClient;
        private readonly IMapper _objectMapper;
        private readonly ISongRepository _songRespository;
        private readonly IArtistRepository _artistRepository;

        public SongService(IUnitOfWork context, IMapper objectMapper)
        {
            _uow = context;
            _objectMapper = objectMapper;

            _songRespository = context.Songs;
            _artistRepository = context.Artists;
        }

        public async Task<PagedResult<SongListModel>> List(int page)
        {
            var songs = await _songRespository.Paged(page);

            var model = _objectMapper.Map<PagedResult<SongListModel>>(songs);
            foreach (var item in model.Results)
            {
                item.Artist = _artistRepository.Get(item.ArtistId).Result.Name;
                item.Code = _songRespository.GetCode(item.SongId);
            }

            return model;
        }
        public async Task<SongListModel> GetForDetail(int id)
        {
            var song = await _songRespository.Get(id);

            if(song == null)
            {
                return null;
            }

            var model = _objectMapper.Map<SongListModel>(song);
            model.Artist = _artistRepository.Get(model.ArtistId).Result.Name;
            model.Code = _songRespository.GetCode(model.SongId);

            return model;
        }
        public async Task<SongCreationModel> GetForCreate()
        {
            var model = new SongCreationModel();
            model.Artists = await _artistRepository.GetArtists();

            return model;
        }
        public async Task FillCreate(SongCreationModel model)
        {
            model.Artists = await _artistRepository.GetArtists();

        }

        public async Task<OperationResponse> Create(SongCreationModel model)
        {

            var response = new OperationResponse();

            if (model == null)
            {
                return response.AddError("", "Model was null");
            }


            var song = new Song();
            var storage = new Storage();

            _objectMapper.Map(model.Storage, song.Storage);
            _objectMapper.Map(model, song);

            if (!response.Success)
            {
                return response;
            }

            await _songRespository.Save(song);
            await _uow.CompleteAsync();

            return response;
        }

        public async Task<SongEditModel> GetForEdit(int id)
        {
            var song = await _songRespository.Get(id);

            if(song == null)
            {
                return null;
            }

            var model = _objectMapper.Map<SongEditModel>(song);
            model.Artists = await _artistRepository.GetArtists();

            return model;
        }
        public async Task Edit(SongEditModel model)
        {
            var song = new Song();

            song = await _songRespository.Get(model.SongId);
            _objectMapper.Map(model, song);

            await _uow.CompleteAsync();
        }
        public async Task<Song> GetForDelete(int id)
        {
            return await _songRespository.Get(id);
        }
        public async Task Delete(int id)
        {
            await _songRespository.Delete(id);
            await _uow.CompleteAsync();
        }
        public async Task<PagedResult<SongDto>> ApiGetList()
        {
            var pagedResult = await _songRespository.GetSongDto(1);

            var model = _objectMapper.Map<PagedResult<SongDto>>(pagedResult);

            return model;
        }
    }
}
