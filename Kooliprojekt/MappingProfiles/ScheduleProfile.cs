using AutoMapper;
using KooliProjekt.Data;
using KooliProjekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.MappingProfiles
{
    public class ScheduleProfile : Profile
    {
        public ScheduleProfile()
        {
            CreateMap<Schedule, ScheduleListModel>();
            CreateMap<PagedResult<Schedule>, PagedResult<ScheduleListModel>>();

            CreateMap<Schedule, ScheduleDetailModel>();

            CreateMap<ICollection<Song>, List<ScheduleDetailModel>>();

            CreateMap<Schedule, ScheduleCreateModel>();
            CreateMap<ScheduleCreateModel, Schedule>()
                 .ForMember(schedule => schedule.Songs, m => m.MapFrom(schedule => schedule.SongSchedules));
            CreateMap<IList<SongSchedule>, List<SongSchedule>>();

            CreateMap<Schedule, ScheduleEditModel>();
            CreateMap<ScheduleEditModel, Schedule>()
                .ForMember(schedule => schedule.ScheduleId, member => member.Ignore())
                .ForMember(schedule => schedule.Songs, member => member.Ignore());

            CreateMap<Schedule, HomeListModel>()
                .ForMember(schedule => schedule.Songs, m => m.MapFrom(schedule => schedule.Songs));

            CreateMap<PagedResult<Schedule>, PagedResult<ScheduleDto>>();
            CreateMap<Schedule, ScheduleDto>();
            CreateMap<Song, SongScheduleModel>();
            CreateMap<SongSchedule, SongScheduleModel>()
                .ForMember(sc => sc.ArtistName, m => m.MapFrom(s => s.Song.Artist.Name))
                .ForMember(sc => sc.ArtistId, m => m.MapFrom(s => s.Song.Artist.ArtistId))
                .ForMember(sc => sc.Title, m => m.MapFrom(s => s.Song.Title));
        }
    }
}
