using Invest.MVC.Infrastructure.Core.Repositories;
using System;
using System.Linq;

namespace Invest.MVC.Infrastructure.Persistence.Repositories
{
    public class StockRepository : Repository<Stock, InvestContext>, IStockRepository
    {
        public StockRepository(InvestContext context) : base(context)
        {
            // Empty
        }
    }
}