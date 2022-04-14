using System;

using System.Threading.Tasks;

namespace BuildingBlocks
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        Task Insert(TEntity entity);
        Task<bool> Update(TEntity entity);
        Task<bool> Delete(TEntity entity);
        Task<TEntity> GetById(params object[] ids);

    }

}
