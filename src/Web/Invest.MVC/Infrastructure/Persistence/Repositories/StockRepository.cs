using Invest.MVC.Infrastructure.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Invest.MVC.Infrastructure.Persistence.Repositories
{
    public class StockRepository : Repository<Stock, InvestContext>, IStockRepository
    {
        public StockRepository(InvestContext context) : base(context)
        {
            // Empty
        }

        public Stock GetBySymbol(string symbol)
        {
            return Context.Set<Stock>().Where(p => p.Enable && symbol == p.Symbol).SingleOrDefault();
        }

        public Task<Stock> GetBySymbolAsync(string symbol)
        {
            return Context.Set<Stock>().Where(p => p.Enable && symbol == p.Symbol).SingleOrDefaultAsync();
        }

        public void TakeSnapshot(Stock stock, DateTime date)
        {
            var dateUtc = date.ToUniversalTime().Date;

            var history = this.Context
                .StockHistories
                .Where(p => p.StockId == stock.Id && p.DateUtc == dateUtc)
                .SingleOrDefault();

            if (null == history)
            {
                history = new StockHistory();
                history.StockId = stock.Id;
                history.Stock = stock;

                history.DateUtc = dateUtc;

                history.CreatedUtc = DateTime.UtcNow;

                stock.StockHistories.Add(history);
            }

            history.Name = stock.Name;
            history.Symbol = stock.Symbol;
            history.Value = stock.Value;
            history.Currency = stock.Currency;

            history.UpdatedUtc = DateTime.UtcNow;
            history.Enable = stock.Enable;
        }

        public decimal GetValue(Stock stock, DateTime date)
        {
            var dateUtc = date.ToUniversalTime().Date;

            return Context.Set<StockHistory>().Where(p => p.Enable && stock.Id == p.StockId && dateUtc == p.DateUtc).Select(p => p.Value).SingleOrDefault();
        }
    }
}