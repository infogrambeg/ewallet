using System;
using System.Threading.Tasks;
using Api.Contracts.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using WebAppPoc.Helpers;

namespace WebAppPoc.Controllers

{

    [ApiController]
    [Route("[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService _walletService)
        {
            this._walletService = _walletService;
        }

        [HttpPost]
        [Route("CreateWallet")]
        public async Task<IActionResult> CreateWallet(long WalletId, string Name) 
        {
            try
            {
                await _walletService.CreateWallet(WalletId,Name);
                return Ok(new { code = "200", message = "Wallet has been created" });
            }
            catch (Exception e)
            {
                return BadRequest(new { code = "400", message = e.Message });
            }
            
        }
        [HttpGet]
        [Route("GetWalletBalance")]
        public async Task<IActionResult> GetBalanceFromWalletId(long WalletId)
        {
            try
            {
                var balance = await _walletService.GetWalletBalance(WalletId);
                return Ok(new { code = "200", message = balance });
            }
            catch (Exception e)
            {
                return BadRequest(new { code = "400", message = e.Message });
            }
        }
        [HttpPost]
        [Route("DepositToWallet")]
        public async Task<IActionResult> DepositToWallet(long WalletId, decimal Amount)
        {
            try
            {
                await _walletService.DepositToWallet(Amount,WalletId);
                return Ok(new { code = "200", message = "New deposit has been paid to wallet" });
            }
            catch (Exception e)
            {
                return BadRequest(new { code = "400", message = e.Message });
            }
        }
        [HttpPost]
        [Route("WithdrawFromWallet")]
        public async Task<IActionResult> WithdrawFromWallet(long WalletId, decimal Amount)
        {
            try
            {
                await _walletService.WithdrawFromWallet(Amount, WalletId);
                return Ok(new { code = "200", message = $"It was successfully paid out of the wallet" });
            }
            catch (Exception e)
            {
                return BadRequest(new { code = "400", message = e.Message });
            }
        }

        [HttpPost]
        [Route("TransferMoney")]
        public async Task<IActionResult> TransferFromWalletToWalletAsync(long SourceWalletId, long DestinationWalletId, decimal Amount)
        {
            try
            {
                await _walletService.TransferFromWalletToWallet(Amount, SourceWalletId, DestinationWalletId);
                ConsoleMessageHelper.WriteMessageSuccess("Uspesna transakcija");
                return Ok(new { code = "200", message = $"The transfer is completed" });
            }
            catch (Exception e)
            {
                ConsoleMessageHelper.WriteMessageError("Neuspesna transakcija");
                return BadRequest(new { code = "400", message = e.Message });
            }
        }

        [HttpGet]
        [Route("HelloWorld")]
        public async Task<IActionResult> HelloWorld()
        {
            return Ok(new { code = "200", message = await _walletService.HelloWorld() });
        }
    }
}
