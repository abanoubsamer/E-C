using Infrastructure.Data.AppDbContext;
using Infrastructure.GenaricRepo;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {


        #region Fildes
        private readonly AppDbContext _db;
        private readonly Dictionary<Type, object> _Repository;
        private IDbContextTransaction _dbTransaction;
        private bool _disposed = false;
        #endregion


        #region Constractor
        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            _Repository = new Dictionary<Type, object>();
        }
        #endregion


        #region Implemntation
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_dbTransaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }
            _dbTransaction = await _db.Database.BeginTransactionAsync();
            return _dbTransaction;
        }

        public async Task CommentAsync()
        {
           await _dbTransaction.CommitAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    _db.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public IGenaricRepository<T> Repository<T>() where T : class
        {
            if (_Repository.ContainsKey(typeof(T)))
            {
                var repo = _Repository[typeof(T)] as GenaricRepository<T>;
                if (repo != null)
                {
                    return repo;
                }
            }
            var _repo = new GenaricRepository<T>(_db);
            _Repository.Add(typeof(T),_repo);
            return _repo;
        }

        public async Task RollBackAsync()
        {
            await _dbTransaction.RollbackAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }
        #endregion


    }
}
