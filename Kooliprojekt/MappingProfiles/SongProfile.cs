using AutoMapper;
using KooliProjekt.Data;
using KooliProjekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.MappingProfiles
{
    public class SongProfile : Profile
    {
        public SongProfile()
        {
            CreateMap<Song, SongListModel>();
            CreateMap<PagedResult<Song>, PagedResult<SongListModel>>();

            CreateMap<Song, SongCreationModel>();
            CreateMap<Storage, StorageViewModel>();
            CreateMap<StorageViewModel, Storage>();
            CreateMap<SongCreationModel, Song>()
                .ForMember(song => song.SongId, member => member.Ignore())
                .ForMember(song => song.Artist, member => member.Ignore());

            CreateMap<Song, SongEditModel>();
            CreateMap<SongEditModel, Song>()
                .ForMember(song => song.SongId, member => member.Ignore());

            CreateMap<PagedResult<Song>, PagedResult<SongDto>>();
            CreateMap<Song, SongDtoForArtist>();
            CreateMap<Song, SongDto>();

        }
    }
}
