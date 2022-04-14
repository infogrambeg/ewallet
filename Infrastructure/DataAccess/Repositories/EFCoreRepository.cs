using BuildingBlocks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.Repositories
{
    public class EFCoreRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal EfCoreDbContext _context;
        internal DbSet<TEntity> DbSet; 
        public EFCoreRepository(EfCoreDbContext context)
        {
            _context = context;
            DbSet = context.Set<TEntity>();
        }
        public virtual async Task Insert(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> Update(TEntity entity)
        {
            try
            {
                DbSet.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified; 
                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }
        }
        public virtual async Task<bool> Delete(TEntity entity)
        {
            DbSet.Remove(entity);
            return await Task.FromResult(true);
        }
        public virtual async Task<TEntity> GetById(params object[] id)
        {
            return await DbSet.FindAsync(id);
        }

        #region IDisposable implementation        
        private bool disposedValue = false;
        // To detect redundant calls  
        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }
            if (disposing)
            {
                _context.Dispose();
            }
            disposedValue = true;
        }        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }


        #endregion IDisposable implementation
    }
}
