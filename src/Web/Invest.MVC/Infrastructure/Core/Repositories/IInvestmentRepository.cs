using System;
using System.Threading.Tasks;

namespace Invest.MVC.Infrastructure.Core.Repositories
{
    public interface IInvestmentRepository : IRepository<Investment>
    {
        Investment GetByStock(Stock stock);

        Task<Investment> GetByStockAsync(Stock stock);

        void TakeSnapshot(Investment entity, DateTime date, decimal stockValue, decimal exchangeRate);
    }
}