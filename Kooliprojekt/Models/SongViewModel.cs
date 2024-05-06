using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Models
{
    public class SongViewModel
    {
        public int SongId { get; set; }
        public int ScheduleId { get; set; }
        public string Title { get; set; }
        public int Tempo { get; set; }
        public int ArtistId { get; set; }
    }
}
