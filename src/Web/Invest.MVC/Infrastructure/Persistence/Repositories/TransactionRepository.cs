using Invest.MVC.Infrastructure.Core.Repositories;
using System;
using System.Linq;

namespace Invest.MVC.Infrastructure.Persistence.Repositories
{
    public class TransactionRepository : Repository<Transaction, InvestContext>, ITransactionRepository
    {
        public TransactionRepository(InvestContext context) : base(context)
        {
            // Empty
        }
    }
}