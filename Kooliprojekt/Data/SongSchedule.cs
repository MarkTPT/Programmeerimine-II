using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Data
{
    public class SongSchedule
    {
        [Key]
        public int Id { get; set; }
        public int SongId { get; set; }
        public int ScheduleId { get; set; }
        public Song Song { get; set; }
        public Schedule Schedule { get; set; }
        public DateTime Time { get; set; }
    }
}
