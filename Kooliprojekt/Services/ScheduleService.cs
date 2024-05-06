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
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _objectMapper;
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleService(IUnitOfWork context, IMapper objectMapper)
        {
            _uow = context;
            _objectMapper = objectMapper;

            _scheduleRepository = context.Schedule;
        }
        public async Task<PagedResult<ScheduleListModel>> List(int page)
        {
            var schedule = await _scheduleRepository.Paged(page);

            var model = _objectMapper.Map<PagedResult<ScheduleListModel>>(schedule);

            return model;
        }
        public async Task<ScheduleDetailModel> GetForDetail(int id)
        {
            var schedule = await _scheduleRepository.GetSchedule(id);

            if (schedule == null)
            {
                return null;
            }

            var model = _objectMapper.Map<ScheduleDetailModel>(schedule);


            return model;
        }
        public ScheduleCreateModel GetForCreate()
        {
            var model = new ScheduleCreateModel();
            //var schedule = new Schedule();
            //var model = _objectMapper.Map<Schedule, ScheduleCreateModel>(schedule);

            return model;
        }

        [ExcludeFromCodeCoverage]
        public List<SongSchedule> GetSongSchedules(DateTime modelDate)
        {
            //if format exception is thrown change between the current one and commented date (second one is on line 114)
            var songs = _scheduleRepository.SongsByTempo();
            if(songs == null)
            {
                return null;
            }
            var songSchedule = new List<SongSchedule>();
            
            var date = modelDate.ToString("dd-MM-yyyy");
            //var date = modelDate.ToString("MM-dd-yyyy");

            int songLength = 125;

            DateTime currentTime;
            DateTime startTime;
            DateTime endTime;

            foreach (var songList in songs)
            {
                if (songList.Key == 1)
                {
                    startTime = DateTime.Parse(date + " " + TempoTimes.tempo1_morning_start);
                    endTime = DateTime.Parse(date + " " + TempoTimes.tempo1_morning_end);
                }
                else if (songList.Key == 2)
                {
                    startTime = DateTime.Parse(date + " " + TempoTimes.tempo2_start);
                    endTime = DateTime.Parse(date + " " + TempoTimes.tempo2_end);
                }
                else
                {
                    startTime = DateTime.Parse(date + " " + TempoTimes.tempo3_start);
                    endTime = DateTime.Parse(date + " " + TempoTimes.tempo3_end);
                }


                   currentTime = startTime;
                foreach (var song in songList)
                {
                    //songLength = (int)(song.DurationInMinutes == null ? 30 : song.DurationInMinutes);
                    //checks in case the last song will exceed the tempo time
                    if (currentTime.CompareTo(endTime) >= 0 || currentTime.AddMinutes(songLength).CompareTo(endTime) > 0) 
                    {
                        if (songList.Key == 1)
                        {
                            startTime = DateTime.Parse(date + " " + TempoTimes.tempo1_evening_start);
                            currentTime = startTime;

                            date = modelDate.AddDays(1).ToString("dd-MM-yyyy");
                            //date = modelDate.AddDays(1).ToString("MM-dd-yyyy");

                            endTime = DateTime.Parse(date + " " + TempoTimes.tempo1_evening_end);
                            continue;
                        }
                        break; 
                    }
                    songSchedule.Add(new SongSchedule { Song = song, Time = currentTime });
                    currentTime = currentTime.AddMinutes(songLength);
                }
            }

            return songSchedule;
        }
        public async Task<ScheduleEditModel> GetForEdit(int id)
        {
            var schedule = await _scheduleRepository.Get(id);
            if(schedule == null)
            {
                return null;
            }

            var model = _objectMapper.Map<ScheduleEditModel>(schedule);

            return model;
        }

        public async Task<OperationResponse> Create(ScheduleCreateModel model)
        {
            var response = new OperationResponse();

            if (model == null)
            {
                return response.AddError("", "Model was null");
            }


            var schedule = new Schedule();

            model.SongSchedules = GetSongSchedules(model.Date);

            if(model.SongSchedules == null)
            {
                return response.AddError("", "Cannot find any songs");
            }

            _objectMapper.Map(model, schedule);

            if (!response.Success)
            {
                return response;
            }

            await _scheduleRepository.Save(schedule);
            await _uow.CompleteAsync();

            return response;
        }

        public async Task<OperationResponse> Save(ScheduleEditModel model)
        {
            var response = new OperationResponse();

            if (model == null)
            {
                return response.AddError("", "Model was null");
            }


            var schedule = new Schedule();

            schedule = await _scheduleRepository.GetSchedule(model.ScheduleId);

            if(schedule == null)
            {
                return response.AddError("", "Cannot find artist with id " + model.ScheduleId);
            }

            _objectMapper.Map(model, schedule);

            if (!response.Success)
            {
                return response;
            }

            await _uow.CompleteAsync();

            return response;
        }

        public async Task<Schedule> GetForDelete(int id)
        {
            return await _scheduleRepository.GetSchedule(id);
        }

        public async Task<OperationResponse> Delete(int? id)
        {
            var response = new OperationResponse();
            if (id == null)
            {
                return response.AddError("", "Id was null");
            }

            var schedule = await _scheduleRepository.Get(id.Value);
            if (schedule == null)
            {
                return response.AddError("", "Cannot find schedule with id " + id.Value);
            }

            await _scheduleRepository.Delete(id.Value);
            await _uow.CompleteAsync();

            return response;
        }

        public async Task<List<HomeListModel>> GetForHome()
        {
            var schedules =  await _scheduleRepository.GetLatestSchedules();
            var model = _objectMapper.Map<List<HomeListModel>>(schedules);

            return model;
        }

        public async Task<PagedResult<ScheduleDto>> ApiGetList()
        {
            var pagedResult = await _scheduleRepository.GetForApiPaged(1);

            var model = _objectMapper.Map<PagedResult<ScheduleDto>>(pagedResult);

            
            return model;
        }
    }
}
