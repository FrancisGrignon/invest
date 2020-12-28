using System;
using System.Threading.Tasks;

namespace Invest.MVC.Infrastructure.Core.Repositories
{
    public interface IStockRepository : IRepository<Stock>
    {
        public Stock GetBySymbol(string symbol);

        public Task<Stock> GetBySymbolAsync(string symbol);

        public void TakeSnapshot(Stock stock, DateTime date);

        public decimal GetValue(Stock stock, DateTime date);
    }
}