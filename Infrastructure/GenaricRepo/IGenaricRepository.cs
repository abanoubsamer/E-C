using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.GenaricRepo
{
    public interface IGenaricRepository <T> where T : class
    {

        public Task AddAsync(T item);
        public Task AddRangeAsync(List<T> items);
        public Task UpdateAsync(T item);
        public Task DeleteAsync(T item);
        public Task DeleteRangeAsync(IEnumerable<T> entities);
        public Task<T>  FindOneAsync(Expression<Func<T,bool>> march);
        public Task<T>  FindOneWithNoTrackingAsync(Expression<Func<T,bool>> march);
        public Task<List<T>> FindMoreAsNoTrackingAsync(Expression<Func<T, bool>> Match);
        public Task<List<T>>  FindMoreAsync(Expression<Func<T,bool>> march);
        public Task<bool> IsExistAsync(Expression<Func<T, bool>> Match);
        public IQueryable<T> GetQueryable();
        public Task<List<T>> GetAllAsyncWithNoTracking();

    }
}
