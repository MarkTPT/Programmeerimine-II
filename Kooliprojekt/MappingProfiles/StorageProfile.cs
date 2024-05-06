using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.Models;

namespace KooliProjekt.MappingProfiles
{
    public class StorageProfile : Profile
    {
        public StorageProfile()
        {
            CreateMap<Storage, StorageListModel>();
            CreateMap<PagedResult<Storage>, PagedResult<StorageListModel>>();
            CreateMap<PagedResult<StorageListModel>, PagedResult<Storage>>();

            CreateMap<Storage, StorageDetailModel>();

            CreateMap<Song, SongViewModel>();

        }
    }
}
