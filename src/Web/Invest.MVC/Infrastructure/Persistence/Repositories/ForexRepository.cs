﻿using Invest.MVC.Infrastructure.Core.Repositories;
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

        public void TakeSnapshot(Forex forex, DateTime date)
        {
            var dateUtc = date.ToUniversalTime().Date;

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


        public float GetExchangeRate(string fromCurrency, string toCurrency, DateTime date)
        {
            if (fromCurrency == toCurrency)
            {
                return 1f;
            }

            var dateUtc = date.ToUniversalTime().Date;

            var exchangeRate = Context.Set<ForexHistory>()
                .Where(p => p.Enable && p.Currency == Forex.USD && p.DateUtc == dateUtc)
                .Select(p => p.ExchangeRate)
                .FirstOrDefault();

            if (fromCurrency == toCurrency)
            {
                exchangeRate = 1f;
            }
            else if (Forex.CAD == fromCurrency && Forex.USD == toCurrency)
            {
                exchangeRate = 1 / exchangeRate;
            }
            else if (Forex.USD == fromCurrency && Forex.CAD == toCurrency)
            {
                // No chang required
            }
            else
            {
                throw new Exception($"Can't found exchange rate from {fromCurrency} to {toCurrency}");
            }

            return exchangeRate;
        }
    }
}