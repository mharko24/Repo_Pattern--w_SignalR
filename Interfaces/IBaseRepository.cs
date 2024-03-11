using ContractMonitoringSystem.Common;
using System.Linq.Expressions;

namespace ContractMonitoringSystem.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task Create(T t);
        Task<T> GetOne(object id);
        T GetId(object id);
        Task<IEnumerable<T>> GetAll();
        Task Update(object id, object t);
        Task Delete(object id);
        Task<PaginatedResult<T>> GetPaginated<TKey>(int page, int pageSize, Expression<Func<T, TKey>> orderBy,Expression<Func<T,bool>> condition);
        IEnumerable<T> Where(Expression<Func<T, bool>> predicate);

    }
}
