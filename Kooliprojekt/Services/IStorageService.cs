using KooliProjekt.Data;
using KooliProjekt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public interface IStorageService
    {
        Task<PagedResult<StorageListModel>> StorageList(int page);
        Task<StorageDetailModel> GetForDetail(int id);
        Task<Storage> GetForDelete(int id);
        Task Delete(int id);
    }
}
