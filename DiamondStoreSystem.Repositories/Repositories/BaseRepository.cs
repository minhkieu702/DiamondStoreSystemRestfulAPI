using DiamondStoreSystem.DAO.DAO;
using DiamondStoreSystem.DAO.IDAO;
using DiamondStoreSystem.DataLayer.Models;
using DiamondStoreSystem.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DiamondStoreSystem.Repositories.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        public bool Any(Func<TEntity, bool> predicate)
        {
            bool result = false;

            try
            {
                result = BaseDAO<TEntity>.Instance.Any(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public int Count(Func<TEntity, bool> predicate)
        {
            int total = 0;
            try
            {
                total = BaseDAO<TEntity>.Instance.Count(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return total;
        }

        public int Count()
        {
            int total = 0;
            try
            {
                total = BaseDAO<TEntity>.Instance.Count();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return total;
        }

        public void DeleteRange(IQueryable<TEntity> entities)
        {
            try
            {
                BaseDAO<TEntity>.Instance.DeleteRange(entities);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public TEntity Find(Func<TEntity, bool> predicate)
        {
            TEntity result;
            try
            {
                result = BaseDAO<TEntity>.Instance.Find(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public IQueryable<TEntity> FindAll(Func<TEntity, bool> predicate)
        {
            IQueryable<TEntity> result;
            try
            {
                result = BaseDAO<TEntity>.Instance.FindAll(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            Task<TEntity> result;
            try
            {
                result = BaseDAO<TEntity>.Instance.FindAsync(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            Task<TEntity> result;
            try
            {
                result = BaseDAO<TEntity>.Instance.FirstOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public TEntity FistOrDefault(Func<TEntity, bool> predicate)
        {
            TEntity result;
            try
            {
                result = BaseDAO<TEntity>.Instance.FistOrDefault(predicate);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public DbSet<TEntity> GetAll()
        {
            DbSet<TEntity> result;
            try
            {
                result = BaseDAO<TEntity>.Instance.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public IQueryable<TEntity> GetAll(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string? includeProperties = null)
        {
            IQueryable<TEntity> result;
            try
            {
                result = BaseDAO<TEntity>.Instance.GetAll(filter, orderBy, includeProperties);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public Task<TEntity> GetById(string id)
        {
            Task<TEntity> result;
            try
            {
                result = BaseDAO<TEntity>.Instance.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public Task<TEntity> GetByIdGuid(Guid id)
        {
            Task<TEntity> result;
            try
            {
                result = BaseDAO<TEntity>.Instance.GetByIdGuid(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>>? filter = null, string? includeProperties = null)
        {
            TEntity result;
            try
            {
                result = BaseDAO<TEntity>.Instance.GetFirstOrDefault(filter, includeProperties);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public TEntity GetMax()
        {
            TEntity result;
            try
            {
                result = BaseDAO<TEntity>.Instance.GetMax();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public TEntity GetMax(Func<TEntity, bool> predicate)
        {
            TEntity result;
            try
            {
                result = BaseDAO<TEntity>.Instance.GetMax(predicate);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public TEntity GetMin()
        {
            TEntity result;
            try
            {
                result = BaseDAO<TEntity>.Instance.GetMin();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public TEntity GetMin(Func<TEntity, bool> predicate)
        {
            TEntity result;
            try
            {
                result = BaseDAO<TEntity>.Instance.GetMin(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public Task<IEnumerable<TEntity>> GetWhere(Expression<Func<TEntity, bool>> predicate)
        {
            Task<IEnumerable<TEntity>> result;
            try
            {
                result = BaseDAO<TEntity>.Instance.GetWhere(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public async Task HardDelete(string key)
        {
            try
            {
                await BaseDAO<TEntity>.Instance.HardDelete(key);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task HardDeleteGuid(Guid key)
        {
            try
            {
                await BaseDAO<TEntity>.Instance.HardDeleteGuid(key);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Insert(TEntity entity)
        {
            try
            {
                BaseDAO<TEntity>.Instance.Insert(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Update(TEntity entity)
        {
            try
            {
                BaseDAO<TEntity>.Instance.Update(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task InsertAsync(TEntity entity)
        {
            try
            {
                await BaseDAO<TEntity>.Instance.InsertAsync(entity);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public void InsertRangeAsync(IQueryable<TEntity> entities)
        {
            try
            {
                BaseDAO<TEntity>.Instance.InsertRangeAsync(entities);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool IsMax(Func<TEntity, bool> predicate)
        {
            bool result = false;
            try
            {
                result = BaseDAO<TEntity>.Instance.IsMax(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public Task<bool> IsMaxAsync(Expression<Func<TEntity, bool>> predicate)
        {
            Task<bool> result;
            try
            {
                result = BaseDAO<TEntity>.Instance.IsMaxAsync(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public bool IsMin(Func<TEntity, bool> predicate)
        {
            bool result = false;
            try
            {
                result = BaseDAO<TEntity>.Instance.IsMin(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public Task<bool> IsMinAsync(Expression<Func<TEntity, bool>> predicate)
        {
            Task<bool> result;
            try
            {
                result = BaseDAO<TEntity>.Instance.IsMinAsync(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public int SaveChanges()
        {
            try
            {
                return BaseDAO<TEntity>.Instance.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await BaseDAO<TEntity>.Instance.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateById(TEntity entity, string id)
        {
            try
            {
                await BaseDAO<TEntity>.Instance.UpdateById(entity, id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateGuid(TEntity entity, Guid id)
        {
            try
            {
                await BaseDAO<TEntity>.Instance.UpdateGuid(entity, id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateRange(IQueryable<TEntity> entities)
        {
            try
            {
                BaseDAO<TEntity>.Instance.UpdateRange(entities);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Delete(TEntity entity)
        {
            try
            {
                BaseDAO<TEntity>.Instance.Delete(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DiamondStoreSystemDBContext GetDBContext()
        {
            return BaseDAO<TEntity>.Instance.GetDBContext();
        }
    }

}
