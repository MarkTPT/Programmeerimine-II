using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.FileAccess;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using KooliProjekt.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileClient _fileClient;
        private readonly IMapper _objectMapper;
        private readonly IArtistRepository _artistRepository;

        public ArtistService(IUnitOfWork context, IFileClient fileClient, IMapper objectMapper)
        {
            _uow = context;
            _fileClient = fileClient;
            _objectMapper = objectMapper;

            _artistRepository = context.Artists;
        }
        public async Task<ArtistModel> GetForEdit(int id)
        {

            var artist = await _artistRepository.Get(id);
            if (artist == null)
            {
                return null;
            }

            var model = _objectMapper.Map<ArtistModel>(artist);
            return model;
        }
        public async Task<PagedResult<ArtistSongListModel>> List(int page)
        {
            var artists = await _artistRepository.Paged(page);

            var model = _objectMapper.Map<PagedResult<ArtistSongListModel>>(artists);
            foreach (var item in model.Results)
            {
                item.SongTitles = _artistRepository.GetSongTitles(item.ArtistId);
            }
            model.selectList = await _artistRepository.GetArtists();

            return model;
        }
        public async Task<PagedResult<ArtistSongListModel>> SaveFile(List<IFormFile> formFiles, string artistId, int page)
        {
            var artists = await _artistRepository.Paged(page);

            //
            if (artistId == null)
            {
                foreach (var file in formFiles)
                {
                    using (var fileStream = file.OpenReadStream())
                    {
                        await _fileClient.Save(ContainerNames.Artists, file.FileName, fileStream);
                    }
                }
            }
            else
            {
                foreach (var file in formFiles)
                {
                    using (var fileStream = file.OpenReadStream())
                    {
                        await _fileClient.Save(ContainerNames.Artists, file.FileName, fileStream, artistId);
                    }
                }
            }

            //this will throw NullReferenceException error on service unit testing
            //var model = _objectMapper.Map<PagedResult<ArtistSongListModel>>(artists);

            var model = new PagedResult<ArtistSongListModel>();
            _objectMapper.Map(artists, model);

            foreach (var item in model.Results)
            {
                item.SongTitles = _artistRepository.GetSongTitles(item.ArtistId);
            }
            model.selectList = await _artistRepository.GetArtists();

            return model;
        }

        public async Task<ArtistDetailModel> GetForDetail(int id)
        {
            var artist = await _artistRepository.Get(id);
            //var files = await _fileClient.List(ContainerNames.Artists);

            if(artist == null)
            {
                return null;
            }

            var model = _objectMapper.Map<ArtistDetailModel>(artist);

            model.Songs = _artistRepository.GetSongInfo(id);

            return model;
        }

        //public async Task Create(ArtistModel model)
        //{
        //    var artist = new Artist();

        //    _objectMapper.Map(model, artist);

        //    await _artistRepository.Save(artist);
        //    await _uow.CompleteAsync();

        //}

        public async Task<OperationResponse> Save(ArtistModel model)
        {
            var response = new OperationResponse();

            if (model == null)
            {
                return response.AddError("", "Model was null");
            }

            var artist = new Artist();

            if (model.ArtistId != 0)
            {
                artist = await _artistRepository.Get(model.ArtistId);
                if (artist == null)
                {
                    return response.AddError("", "Cannot find artist with id " + model.ArtistId);
                }
            }

            _objectMapper.Map(model, artist);

            if (!response.Success)
            {
                return response;
            }

            await _artistRepository.Save(artist);
            await _uow.CompleteAsync();

            return response;
        }

        public async Task<Artist> GetForDelete(int id)
        {
            return await _artistRepository.Get(id);
        }

        public async Task<OperationResponse> Delete(int? id)
        {
            var response = new OperationResponse();
            if (id == null)
            {
                return response.AddError("", "Model was null");
            }

            var artist = await _artistRepository.Get(id.Value);
            if (artist == null)
            {
                return response.AddError("", "Cannot find artist with id " + id.Value);
            }

            await _artistRepository.Delete(id.Value);
            await _uow.CompleteAsync();

            return response;
        }
        public async Task<ArtistAboutModel> GetForAbout()
        {
            var model = new ArtistAboutModel()
            {
                Artists = await _artistRepository.All(),
                Files = await _fileClient.List(ContainerNames.Artists)
            };

            return model;
        }
        public async Task<ArtistAboutModel> DeleteFile(string container, string fileName)
        {
            await _fileClient.Delete(container, fileName);
            var model = new ArtistAboutModel()
            {
                Artists = await _artistRepository.All(),
                Files = await _fileClient.List(ContainerNames.Artists)
            };

            return model;
        }

        public async Task<PagedResult<ArtistDto>> ApiGetList()
        {
            var pagedResult = await _artistRepository.GetArtistDto(1);

            var model = _objectMapper.Map<PagedResult<ArtistDto>>(pagedResult);

            return model;
        }
        public async Task<PagedResult<string>> GetArtistNames(int page)
        {
            var pagedResult = await _artistRepository.GetArtistNames(page);
            return null;
        }
        public void ChangeEntityStateTo(ArtistModel artist, EntityState state)
        {
            _artistRepository.ChangeEntityState(artist, state);
        }
    }
}
