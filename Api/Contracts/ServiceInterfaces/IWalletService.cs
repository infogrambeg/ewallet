using System.Threading.Tasks;

namespace Api.Contracts.ServiceInterfaces
{
    public interface IWalletService
    {
        Task CreateWallet(long walletId, string name);
        Task DepositToWallet(decimal amount, long walletId);
        Task WithdrawFromWallet(decimal amount, long walletId);
        Task TransferFromWalletToWallet(decimal amount, long sourceWalletId, long destinationWalletId);
        Task<decimal> GetWalletBalance(long walletId);
        Task<string> HelloWorld();
    }
}
