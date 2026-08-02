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

        public virtual Investment GetByInvestor(Investor investor, Stock stock)
        {
            return Context.Set<Investment>().Where(p => p.Enable && p.StockId == stock.Id && p.InvestorId == investor.Id).SingleOrDefault();
        }

        public virtual Task<Investment> GetByInvestorAsync(Investor investor, Stock stock)
        {
            return Context.Set<Investment>().Where(p => p.Enable && p.StockId == stock.Id && p.InvestorId == investor.Id).SingleOrDefaultAsync();
        }

        public void TakeSnapshot(Investment investment, DateTime date, float stockValue, float exchangeRate)
        {
            var dateUtc = date.ToUniversalTime().Date;

            var history = this.Context
                .InvestmentHistories
                .Where(p => p.InvestmentId == investment.Id && p.DateUtc == dateUtc)
                .SingleOrDefault();

            if (null == history)
            {
                history = InvestmentHistory.CreateFrom(investment, dateUtc, stockValue, exchangeRate);

                investment.InvestmentHistories.Add(history);
            }

            history.StockId = investment.StockId;
            history.Stock = investment.Stock;
            history.InvestorId = investment.InvestorId;
            history.Investor = investment.Investor;

            history.Quantity = investment.Quantity;
            history.Value = stockValue;
            history.Currency = investment.Currency;
            history.ExchangeRate = exchangeRate;

            history.UpdatedUtc = DateTime.UtcNow;
            history.Enable = investment.Enable;
        }
    }
}
