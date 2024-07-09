using DiamondStoreSystem.DAO.IDAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondStoreSystem.Repositories.IRepositories
{
    public interface IBaseRepository<TEntity> : IBaseDAO<TEntity> where TEntity : class
    {
    }
}
