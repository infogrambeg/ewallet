using Domain.Repositories;
using Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Threading;

namespace Infrastructure.DataAccess
{
    public class EfCoreUnitOfWork : ICoreUnitOfWork
    {
        private readonly EfCoreDbContext Context;
        private IDbContextTransaction Transaction; 
        
        public IWalletRepository WalletRepository { get; }
        public EfCoreUnitOfWork(EfCoreDbContext context)
        {
            Context = context; 
            WalletRepository = new WalletRepository(context);
        }

        public IExecutionStrategy ExecutionStrategy()
        {
            return Context.Database.CreateExecutionStrategy();
        }
       
        public async Task BeginTransactionAsync(IsolationLevel isolationLevel)
        {
            Transaction = await Context.Database.BeginTransactionAsync(isolationLevel);
        }

        public Task CommitTransactionAsync()
        {
            return Transaction.CommitAsync();
        }
        public Task RollbackTransactionAsync()
        {
            return Transaction.RollbackAsync();
        }
        public async Task SaveChangesAsync()
        {
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException duce)
            {
                //todo: log
                throw duce;
            }
            catch (DbUpdateException due)
            {
                throw due;
            }
            catch (Exception e)
            {
                throw e;
            }
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
                Transaction?.Dispose();
                Context.Dispose();
            }
            disposedValue = true;
        }        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion IDisposable implementation        
        public void ClearTracker()
        {
            Context.ChangeTracker.Clear();
        }
    }
}
