using Infrastructure.Data.AppDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.GenaricRepo
{
    public class GenaricRepository<T> : IGenaricRepository<T> where T : class
    {
        #region Fildes

        private readonly DbSet<T> _dbSet;
        private readonly AppDbContext _db;

        #endregion Fildes

        #region Constractor

        public GenaricRepository(AppDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }

        #endregion Constractor

        #region Implemntation

        public async Task AddAsync(T item)
        {
            await _dbSet.AddAsync(item);
            await _db.SaveChangesAsync();
        }

        public async Task AddRangeAsync(List<T> items)
        {
            await _dbSet.AddRangeAsync(items);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> Match)
        {
            return await _dbSet.AnyAsync(Match);
        }

        public async Task DeleteAsync(T item)
        {
            _dbSet.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            await _db.SaveChangesAsync();
        }

        public async Task<List<T>> FindMoreAsNoTrackingAsync(Expression<Func<T, bool>> Match)
        {
            return await _dbSet.AsNoTracking().Where(Match).ToListAsync();
        }

        public async Task<List<T>> FindMoreAsync(Expression<Func<T, bool>> march)
        {
            return await _dbSet.Where(march).ToListAsync();
        }

        public async Task<T> FindOneAsync(Expression<Func<T, bool>> march)
        {
            return
                await _dbSet.FirstOrDefaultAsync(march);
        }

        public async Task<T> FindOneWithNoTrackingAsync(Expression<Func<T, bool>> march)
        {
            return
                await _dbSet.AsNoTracking().FirstOrDefaultAsync(march);
        }

        public async Task<List<T>> GetAllAsyncWithNoTracking()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public IQueryable<T> GetQueryable()
        {
            return _dbSet.AsNoTracking().AsQueryable();
        }

        public async Task<bool> IsExistAsync(Expression<Func<T, bool>> Match)
        {
            return await _dbSet.AnyAsync(Match);
        }

        public Task UpateRangeAsync(List<T> items)
        {
            _dbSet.UpdateRange(items);
            return _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(T item)
        {
            _dbSet.Update(item);
            await _db.SaveChangesAsync();
        }

        #endregion Implemntation
    }
}