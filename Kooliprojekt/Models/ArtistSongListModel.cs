using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Models
{
    public class ArtistSongListModel
    {
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<string> SongTitles { get; set; }
        public IList<SelectListItem> Artists { get; set; }

        public ArtistSongListModel()
        {
            SongTitles = new List<string>();
            Artists = new List<SelectListItem>();
        }
    }
}
