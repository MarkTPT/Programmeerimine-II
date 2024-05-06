using KooliProjekt.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Models
{
    public class ArtistAboutModel
    {
        public List<Artist> Artists { get; set; }
        public IList<string> Files { get; set; }
    }
}
