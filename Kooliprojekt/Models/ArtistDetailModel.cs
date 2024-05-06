using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Models
{
    public class ArtistDetailModel
    {
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<SongDetail> Songs { get; set; }

        public ArtistDetailModel()
        {
            Songs = new List<SongDetail>();
        }
    }
    public class SongDetail
    {
        public string Title { get; set; }
        public int Tempo { get; set; }
    }
}
