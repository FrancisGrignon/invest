using System;
using System.Threading.Tasks;

namespace Invest.MVC.Infrastructure.Core.Repositories
{
    public interface IForexRepository : IRepository<Forex>
    {
        Forex GetByCurrency(string currency);

        Task<Forex> GetByCurrencyAsync(string currency);

        void TakeSnapshot(Forex forex, DateTime date);
    }
}
