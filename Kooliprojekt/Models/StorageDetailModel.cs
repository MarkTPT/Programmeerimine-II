using KooliProjekt.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Models
{
    public class StorageDetailModel
    {
        public int StorageID { get; set; }
        public string Kood { get; set; }
        public string Name { get; set; }
        public int ArtistId { get; set; }
        public SongViewModel Song { get; set; }


    }
}
