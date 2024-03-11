using ContractMonitoringSystem.Common;
using ContractMonitoringSystem.Interfaces;
using ContractMonitoringSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;


namespace ContractMonitoringSystem.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        private readonly DbSet<T> _table;
        public BaseRepository(ApplicationDbContext db)
        {
            _db = db;
            _table = _db.Set<T>();
        }
        public async Task Create(T t)
        {
            await _db.AddAsync(t);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(object id)
        {
            var result = await GetOne(id);
            if (result != null)
            {
                _table.Remove(result);
                await _db.SaveChangesAsync();
                   
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _table.ToListAsync();
        }

        public T GetId(object id)
        {
            return _table.Find(id);
        }

        public async Task<T> GetOne(object id)
        {
            return await _table.FindAsync(id);
        }

        public async  Task<PaginatedResult<T>> GetPaginated<TKey>(int page, int pageSize,Expression<Func<T, TKey>> orderBy,Expression<Func<T,bool>>condition)
        {
            
            var count = await _table.Where(condition).CountAsync();
            var records = await _table.Where(condition).OrderByDescending(orderBy)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new PaginatedResult<T>
            {
                data = records,
                pageCurrent = page,
                numSize = (int)Math.Ceiling(count / (double)pageSize)

            };
        }



        public async Task Update(object id, object t)
        {
            var result = await GetOne(id);
            if (result != null)
            {
                _db.Entry(result).CurrentValues.SetValues(t);
                await _db.SaveChangesAsync();
            }
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _table.Where(predicate).AsEnumerable();
        }
    }
}
