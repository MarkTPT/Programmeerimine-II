using KooliProjekt.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Models
{
    public class ScheduleDto
    {
        public int ScheduleId { get; set; }
        public DateTime Date { get; set; }
        //public IList<SongSchedule> Songs { get; set; }
        public IList<SongScheduleModel> Songs { get; set; }
        public ScheduleDto()
        {
            Songs = new List<SongScheduleModel>();
        }
    }
}
