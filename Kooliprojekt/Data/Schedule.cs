using KooliProjekt.Custom_Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using KooliProjekt.Models;

namespace KooliProjekt.Data
{
    public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }
        [CustomScheduleDate(ErrorMessage = "Schedule on this date has already been planned")]
        public DateTime Date { get; set; }
        public IList<SongSchedule> Songs { get; set; }

        public Schedule()
        {
            Songs = new List<SongSchedule>();
        }
    }
}
