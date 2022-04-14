using Domain;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DataAccess.Repositories
{
    public class WalletRepository : EFCoreRepository<Wallet>,IWalletRepository
    {
        public WalletRepository(EfCoreDbContext context) : base(context)
        {
        }
    }
}
