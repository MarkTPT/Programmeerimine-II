using KooliProjekt.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Models
{
    public class HomeListModel
    {
        public int ScheduleId { get; set; }
        public DateTime Date { get; set; }
        public IList<SongSchedule> Songs { get; set; }
        public HomeListModel()
        {
            Songs = new List<SongSchedule>();
        }
    }
}
