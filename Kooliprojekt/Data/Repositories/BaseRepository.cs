using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Controllers.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual async Task<T> Get(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task Save(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public virtual async Task Delete(int id)
        {
            var entity = await Get(id);
            if(entity == null)
            {
                return;
            }

            Delete(entity);
        }
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public virtual async Task<List<T>> All()
        {
            return await _context.Set<T>().ToListAsync(); 

        }

        public virtual async Task<PagedResult<T>> Paged(int page)
        {
            return await _context.Set<T>().GetPagedAsync(page, 10);
        }


    }
}
