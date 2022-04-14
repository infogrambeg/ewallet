using Domain;
using Infrastructure.DataAccess.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.DataAccess.Repositories
{
    public class EfCoreDbContext:DbContext
    {
        public EfCoreDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Wallet> Wallets { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WalletConfiguration());
            base.OnModelCreating(modelBuilder);
        }


    }
}