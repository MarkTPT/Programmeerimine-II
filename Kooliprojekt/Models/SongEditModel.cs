using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Models
{
    public class SongEditModel
    {
        public int SongId { get; set; }
        [Required]
        public string Title { get; set; }
        public int ArtistId { get; set; }
        public int Tempo { get; set; }
        public IList<SelectListItem> Artists { get; set; }
        public IList<SelectListItem> Tempos { get; set; }

        public SongEditModel()
        {
            Artists = new List<SelectListItem>();
            Tempos = new List<SelectListItem>()
            {
                new SelectListItem {
                    Text = "1", Value = "1"
                },
                new SelectListItem {
                    Text = "2", Value = "2"
                },
                new SelectListItem {
                    Text = "3", Value = "3"
                }
            };
        }
    }
}
