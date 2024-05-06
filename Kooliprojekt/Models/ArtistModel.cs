using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Models
{
    public class ArtistModel
    {
        public int ArtistId { get; set; }
        [Required]
        [StringLength(25)]
        public string Name { get; set; }
        public string Description { get; set; }


    }
}
