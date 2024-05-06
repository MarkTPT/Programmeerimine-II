using KooliProjekt.Custom_Validation;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Models
{
    public class ScheduleCreateModel
    {
        public int ScheduleId { get; set; }
        [CustomScheduleDate(ErrorMessage = "Please choose a date that hasn't beeb planned or later than today.")]
        public DateTime Date { get; set; }
        public IList<SongSchedule> SongSchedules { get; set; }
        public ScheduleCreateModel()
        {
            Date = DateTime.Now;

            SongSchedules = new List<SongSchedule>();
        }

    }
}
