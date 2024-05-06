using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KooliProjekt.Data
{
    public class Storage
    {
        public int StorageID { get; set; }

        [StringLength(5)]
        public string Kood { get; set; }
        [ForeignKey("Song")]
        public int SongId { get; set; }
        public virtual Song Song { get; set; }
    }
}
