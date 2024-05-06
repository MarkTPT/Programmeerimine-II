using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Data
{
    public class PagedResult<T> : PagedResultBase
    {
        public IList<T> Results { get; set; }

        //Seda siin ei pea olema, mtea kuidas teisiti teha
        public List<SelectListItem> selectList;

        public PagedResult()
        {
            Results = new List<T>();
            selectList = new List<SelectListItem>();
        }
    }
}
