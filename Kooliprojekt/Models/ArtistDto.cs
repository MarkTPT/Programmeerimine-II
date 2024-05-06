using KooliProjekt.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Models
{
    public class ArtistDto
    {
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<SongDtoForArtist> Songs { get; set; }

        public ArtistDto()
        {
            Songs = new List<SongDtoForArtist>();
        }

    }
    public class SongDtoForArtist
    {
        public int SongId { get; set; }
        public string Title { get; set; }
        public int Tempo { get; set; }
    }

}

