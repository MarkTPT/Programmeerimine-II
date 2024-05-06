using KooliProjekt.Custom_Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Models
{
    public class ScheduleEditModel
    {
        public int ScheduleId { get; set; }
        [ScheduleEditDate]
        public DateTime Date { get; set; }

    }
}
