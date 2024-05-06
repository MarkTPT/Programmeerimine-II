using KooliProjekt.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Models
{
    public class ScheduleDetailModel
    {
        public int ScheduleId { get; set; }
        public DateTime Date { get; set; }
        public IList<SongSchedule> Songs { get; set; }
        public ScheduleDetailModel()
        {
            Songs = new List<SongSchedule>();
        }
    }
}
