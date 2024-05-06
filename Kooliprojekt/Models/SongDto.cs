using KooliProjekt.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Models
{
    public class SongDto
    {
        public int SongId { get; set; }
        public string Title { get; set; }
        public int Tempo { get; set; }
        public ArtistModel Artist { get; set; }
    }
}
