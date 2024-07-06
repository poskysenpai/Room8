using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Room8.Infrastructure.Abstractions
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        IQueryable<T> FindByConditionAsNoTracking(Expression<Func<T, bool>> expression);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        int Count();
        Task<IDbContextTransaction> GetTransactionObject();
    }

}
