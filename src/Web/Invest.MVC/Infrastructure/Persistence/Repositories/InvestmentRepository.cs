using Invest.MVC.Infrastructure.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Invest.MVC.Infrastructure.Persistence.Repositories
{
    public class InvestmentRepository : Repository<Investment, InvestContext>, IInvestmentRepository
    {
        public InvestmentRepository(InvestContext context) : base(context)
        {
            // Empty
        }

        public virtual Investment GetByStock(Stock stock)
        {
            return Context.Set<Investment>().Where(p => p.Enable && p.StockId == stock.Id).SingleOrDefault();
        }

        public virtual Task<Investment> GetByStockAsync(Stock stock)
        {
            return Context.Set<Investment>().Where(p => p.Enable && p.StockId == stock.Id).SingleOrDefaultAsync();
        }

        public void TakeSnapshot(Investment entity, DateTime date, decimal stockValue, decimal exchangeRate)
        {
            var dateUtc = date.ToUniversalTime().Date;

            var history = this.Context
                .InvestmentHistories
                .Where(p => p.InvestmentId == entity.Id && p.DateUtc == dateUtc)
                .SingleOrDefault();

            if (null == history)
            {
                history = new InvestmentHistory();
                history.InvestmentId = entity.Id;
                history.Investment = entity;
                
                history.DateUtc = dateUtc;

                history.CreatedUtc = DateTime.UtcNow;

                entity.InvestmentHistories.Add(history);
            }

            history.StockId = entity.StockId;
            history.Stock = entity.Stock;
            history.InvestorId = entity.InvestorId;
            history.Investor = entity.Investor;

            history.Quantity = entity.Quantity;
            history.Value = stockValue;
            history.Currency = entity.Currency;
            history.ExchangeRate = exchangeRate;

            history.UpdatedUtc = DateTime.UtcNow;
            history.Enable = entity.Enable;
        }
    }
}
