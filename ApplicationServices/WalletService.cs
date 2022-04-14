using Api.Contracts.ServiceInterfaces;
using Domain;
using Domain.Exceptions;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Threading.Tasks;

namespace ApplicationServices
{
    public class WalletService : IWalletService
    {
        private const IsolationLevel _ISOLATION_LEVEL = IsolationLevel.ReadCommitted;

        private readonly ICoreUnitOfWork _coreUnitOfWork;

        public WalletService(ICoreUnitOfWork coreUnitOfWork)
        {
            _coreUnitOfWork = coreUnitOfWork;
        }
        public async Task CreateWallet(long walletId, string name)
        {

            var NewWallet = new Wallet(walletId, name);
            if (await _coreUnitOfWork.WalletRepository.GetById(NewWallet.Id) != null)
            {
                throw new WalletException($"Wallet ID {walletId} does exist");

            }
            await _coreUnitOfWork.WalletRepository.Insert(NewWallet);
            await _coreUnitOfWork.SaveChangesAsync();


        }

        public async Task DepositToWallet(decimal amount, long walletId)
        {
            try
            {
                await _coreUnitOfWork.BeginTransactionAsync(_ISOLATION_LEVEL);
                var GetWallet = await _coreUnitOfWork.WalletRepository.GetById(walletId);
                if (GetWallet == null)
                {
                throw new WalletException($"Wallet ID {walletId} does not exist");

                }
                GetWallet.PayIn(amount);
                await _coreUnitOfWork.WalletRepository.Update(GetWallet);
                await _coreUnitOfWork.SaveChangesAsync();
                await _coreUnitOfWork.CommitTransactionAsync();
            }
            catch (DbUpdateException e)
            {
                await _coreUnitOfWork.RollbackTransactionAsync();
                throw new WalletException(e.Message);
            }
            

        }

        public async Task WithdrawFromWallet(decimal amount, long walletId)
        {
            var GetWallet = await _coreUnitOfWork.WalletRepository.GetById(walletId);
            if (GetWallet == null)
            {
                throw new WalletException($"Wallet ID {walletId} does not exist");

            }

            GetWallet.PayOut(amount);
            await _coreUnitOfWork.WalletRepository.Update(GetWallet);
            await _coreUnitOfWork.SaveChangesAsync();
        }

        public async Task TransferFromWalletToWallet(decimal amount, long sourceWalletId, long destinationWalletId)
        {
            await _coreUnitOfWork.BeginTransactionAsync(_ISOLATION_LEVEL);
            try
            {
                var SourceWallet = await _coreUnitOfWork.WalletRepository.GetById(sourceWalletId);
                var DestinationWallet = await _coreUnitOfWork.WalletRepository.GetById(destinationWalletId);
                if (SourceWallet == null)
                {
                    await _coreUnitOfWork.RollbackTransactionAsync();
                    throw new WalletException($"Source Wallet ID {sourceWalletId} does not exist");

                }
                if (DestinationWallet == null)
                {
                    await _coreUnitOfWork.RollbackTransactionAsync();
                    throw new WalletException($"Destination Wallet ID {destinationWalletId} does not exist");

                }
                SourceWallet.PayOut(amount);
                DestinationWallet.PayIn(amount);
                await _coreUnitOfWork.WalletRepository.Update(SourceWallet);
                await _coreUnitOfWork.WalletRepository.Update(DestinationWallet);
                await _coreUnitOfWork.SaveChangesAsync();
                await _coreUnitOfWork.CommitTransactionAsync();
            }
            catch(Exception e)
            {
                await _coreUnitOfWork.RollbackTransactionAsync();
                throw new WalletException(e.Message);
            }
                
           
        }

        public async Task<decimal> GetWalletBalance(long walletId)
        {
            var getWallet = await _coreUnitOfWork.WalletRepository.GetById(walletId);
            if (getWallet == null)
            {
                throw new WalletException($"Wallet ID {walletId} does not exist");

            }
            return getWallet.Balance;
        }

        public async Task<string> HelloWorld()
        {
            return await Task.FromResult("Helo World");
        }
    }
}
