using KooliProjekt.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Controllers.Repositories
{
    public interface IBaseRepository<T>
    {
        Task<T> Get(int id);
        Task Save(T entity);
        Task Delete(int id);
        void Delete(T entity);
        Task<List<T>> All();
        Task<PagedResult<T>> Paged(int page);
    }
}
