using Invest.MVC.Infrastructure.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Invest.MVC.Infrastructure.Persistence.Repositories
{
    public class ForexRepository : Repository<Forex, InvestContext>, IForexRepository
    {
        public ForexRepository(InvestContext context) : base(context)
        {
            // Empty
        }

        public Forex GetByCurrency(string currency)
        {
            return Context.Set<Forex>().Where(p => p.Enable && currency == p.Currency).SingleOrDefault();
        }

        public Task<Forex> GetByCurrencyAsync(string currency)
        {
            return Context.Set<Forex>().Where(p => p.Enable && currency == p.Currency).SingleOrDefaultAsync();
        }

        public void TakeSnapshot(Forex forex, DateTime dateUtc)
        {
            dateUtc = dateUtc.Date;

            var history = this.Context
                .ForexHistories
                .Where(p => p.ForexId == forex.Id && p.DateUtc == dateUtc)
                .SingleOrDefault();

            if (null == history)
            {
                history = new ForexHistory();
                history.ForexId = forex.Id;
                history.Forex = forex;

                history.DateUtc = dateUtc;

                history.CreatedUtc = DateTime.UtcNow;

                forex.ForexHistories.Add(history);
            }

            history.Currency = forex.Currency;
            history.ExchangeRate = forex.ExchangeRate;

            history.UpdatedUtc = DateTime.UtcNow;
            history.Enable = forex.Enable;
        }
    }
}