using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Wallet
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        [ConcurrencyCheck]
        public decimal Balance { get; private set; }

        [Timestamp]
        public byte[] RowVersion { get; private set; }
        public List<WalletTransaction> Transactions { get; private set; }

        public Wallet(long Id, string Name)
        {
            Transactions = new List<WalletTransaction>();
            this.Id = Id;
            this.Name = Name;
            Balance = 0;
        }

        public void PayIn(decimal Amount)
        {
            Balance += Amount;
            var newTransaction = new WalletTransaction(Amount, TransactionType.PAYIN);
            Transactions.Add(newTransaction);
        }

        public void PayOut(decimal Amount)
        {
            if(Amount > Balance)
            {
                throw new WalletException("There are not enough funds in this account");
            }
            Balance -= Amount;
            var newTransaction = new WalletTransaction(Amount, TransactionType.PAYOUT);
            Transactions.Add(newTransaction);
        }
    }
}
