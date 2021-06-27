using System;
using System.Threading.Tasks;

namespace Invest.MVC.Infrastructure.Core.Repositories
{
    public interface IInvestmentRepository : IRepository<Investment>
    {
        Investment GetByInvestor(Investor investor, Stock stock);
        Task<Investment> GetByInvestorAsync(Investor investor, Stock stock);
        Investment GetByStock(Stock stock);

        Task<Investment> GetByStockAsync(Stock stock);

        void TakeSnapshot(Investment entity, DateTime date, decimal stockValue, decimal exchangeRate);
    }
}