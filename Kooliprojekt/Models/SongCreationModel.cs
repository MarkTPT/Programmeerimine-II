using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Models
{
    public class SongCreationModel
    {
        public int SongId { get; set; }
        [Required]
        public string Title { get; set; }
        public int Tempo { get; set; }
        public int ArtistId { get; set; }
        public StorageViewModel Storage { get; set; }
        public IList<SelectListItem> Tempos { get; set; }
        public IList<SelectListItem> Artists { get; set; }

        public SongCreationModel()
        {
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
            Artists = new List<SelectListItem>();
        }

    }
}
