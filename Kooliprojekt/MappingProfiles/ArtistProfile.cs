using AutoMapper;
using KooliProjekt.Data;
using KooliProjekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.MappingProfiles
{
    public class ArtistProfile : Profile
    {
        public ArtistProfile()
        {
            //Artists
            CreateMap<Artist, ArtistSongListModel>();
            CreateMap<PagedResult<Artist>, PagedResult<ArtistSongListModel>>();

            CreateMap<Artist, ArtistDetailModel>();

            CreateMap<Artist, ArtistModel>();
            CreateMap<ArtistModel, Artist>()
                .ForMember(artist => artist.ArtistId, member => member.Ignore());

            CreateMap<PagedResult<Artist>, PagedResult<ArtistDto>>();
            CreateMap<Artist, ArtistDto>();
        }
    }
}
