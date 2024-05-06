using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Data
{
    public class Artist
    {
        [Key]
        public int ArtistId { get; set;}
        [Required]
        [StringLength(25)]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public IList<Song> Songs { get; set; }
    }
}
