using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Models
{
    public class StorageListModel
    {
        public int StorageID { get; set; }
        public string Kood { get; set; }
        public SongViewModel Song { get; set; }
    }
}
