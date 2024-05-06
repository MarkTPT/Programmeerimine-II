using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.FileAccess;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Diagnostics;
using AutoMapper;
using KooliProjekt.Services;
using KooliProjekt.Data.Repositories;

namespace KooliProjekt.Services
{
    public class StorageService : IStorageService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _objectMapper;
        private readonly IStorageRepository _storageRespository;
        public StorageService(IUnitOfWork context, IMapper objectMapper)
        {
            _uow = context;
            _objectMapper = objectMapper;

            _storageRespository = context.Storage;
        }
        public async Task<PagedResult<StorageListModel>> StorageList(int page)
        {
            var storage = await _storageRespository.Paged(page);

            var model = _objectMapper.Map<PagedResult<StorageListModel>>(storage);
            foreach (var item in model.Results)
            {
                item.Song = _objectMapper.Map<SongViewModel>(item.Song);
            }

            return model;
        }
        public async Task<StorageDetailModel> GetForDetail(int id)
        {
            var storage = await _storageRespository.Get(id);

            if (storage == null)
            {
                return null;
            }

            var model = _objectMapper.Map<Storage, StorageDetailModel>(storage);
            model.Song = _objectMapper.Map<Song, SongViewModel>(storage.Song);
            model.Name = _storageRespository.GetSong(model.Song.SongId).Result.Artist.Name;

            return model;
        }
        public async Task<Storage> GetForDelete(int id)
        {
            var storage = await _storageRespository.Get(id);
            if (storage == null)
            {
                return null;
            }

            return storage;
        }
        public async Task Delete(int id)
        {
            await _storageRespository.Delete(id);
            await _uow.CompleteAsync();
        }
    }
}
