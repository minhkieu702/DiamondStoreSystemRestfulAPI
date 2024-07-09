using DiamondStoreSystem.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DiamondStoreSystem.DAO.IDAO
{
    public interface IBaseDAO<TEntity> where TEntity : class
    {
        public DiamondStoreSystemDBContext GetDBContext();
        public TEntity Find(Func<TEntity, bool> predicate);
        public IQueryable<TEntity> FindAll(Func<TEntity, bool> predicate);
        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
        public DbSet<TEntity> GetAll();
        public void DeleteRange(IQueryable<TEntity> entities);
        public Task<TEntity> GetById(string id);
        public Task<TEntity> GetByIdGuid(Guid id);
        public Task<IEnumerable<TEntity>> GetWhere(Expression<Func<TEntity, bool>> predicate);
        public Task HardDelete(string key);
        public void Delete(TEntity entity);
        public Task HardDeleteGuid(Guid key);
        public Task InsertAsync(TEntity entity);
        public void Insert(TEntity entity);
        public void InsertRangeAsync(IQueryable<TEntity> entities);
        public void Update(TEntity entity);
        public Task UpdateById(TEntity entity, string id);
        public Task UpdateGuid(TEntity entity, Guid id);
        public void UpdateRange(IQueryable<TEntity> entities);
        public IQueryable<TEntity> GetAll(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string? includeProperties = null);
        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>>? filter = null,
            string? includeProperties = null);
        public bool Any(Func<TEntity, bool> predicate);
        public int Count(Func<TEntity, bool> predicate);
        public int Count();
        public TEntity FistOrDefault(Func<TEntity, bool> predicate);
        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        public int SaveChanges();
        public Task<int> SaveChangesAsync();
        public bool IsMin(Func<TEntity, bool> predicate);
        public bool IsMax(Func<TEntity, bool> predicate);
        public TEntity GetMin();
        public TEntity GetMax();
        public Task<bool> IsMaxAsync(Expression<Func<TEntity, bool>> predicate);
        public Task<bool> IsMinAsync(Expression<Func<TEntity, bool>> predicate);
        public TEntity GetMin(Func<TEntity, bool> predicate);
        public TEntity GetMax(Func<TEntity, bool> predicate);
    }

}
