using System;

namespace Domain
{
    public class WalletTransaction
    {

        public long Id { get; private set; }
        public DateTime TimeofTransaction { get; private set; }
        public decimal Amount { get; private set; }
        public TransactionType Type { get; private set; }

        public WalletTransaction(decimal amount, TransactionType type)
        {
            Amount = amount;
            Type = type;
            TimeofTransaction = DateTime.Now;

        }

    }
}
