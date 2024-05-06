using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Data
{
    public class Song
    {
        [Key]
        public int SongId { get; set; }
        [Required]
        public string Title { get; set; }
        public int Tempo { get; set; }
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }
        public Storage Storage { get; set; }
        public IList<SongSchedule> SongSchedules { get; set; }

        public Song()
        {
            SongSchedules = new List<SongSchedule>();
        }
    }
}
