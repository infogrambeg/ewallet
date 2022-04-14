using BuildingBlocks;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DataAccess
{
    public interface ICoreUnitOfWork : IUnitOfWork
    {
        public IWalletRepository WalletRepository { get; }

        public IExecutionStrategy ExecutionStrategy();
    }
}
