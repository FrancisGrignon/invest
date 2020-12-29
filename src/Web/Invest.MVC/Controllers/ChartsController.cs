using Highsoft.Web.Mvc.Charts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invest.MVC.Controllers
{
    public class ChartsController : Controller
    {
        private readonly ILogger<ChartsController> _logger;
        private readonly InvestContext _context;

        public ChartsController(ILogger<ChartsController> logger, InvestContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Stocks(string id, DateTime? from, DateTime? to)
        {
            List<string> symbols;

            if (true == string.IsNullOrEmpty(id))
            {
                symbols = _context.Stocks.Select(p => p.Symbol.ToUpper()).ToList();
            }
            else
            {
                symbols = id.ToUpper().Split(',').ToList();
            }

            var query = _context.StockHistories
                .Where(m => symbols.Contains(m.Symbol));

            DateTime dateUtc;

            if (from.HasValue)
            {
                dateUtc = from.Value.ToUniversalTime().Date;
                query = query.Where(p => dateUtc <= p.DateUtc);
            }

            if (to.HasValue)
            {
                dateUtc = from.Value.ToUniversalTime().Date;
                query = query.Where(p => p.DateUtc <= dateUtc);
            }

            var categories = query
                .Select(p => p.DateUtc).Distinct()
                .OrderBy(p => p)
                .Select(p => p.ToString("yyyy-MM-dd"))
                .ToList();

            var dict = new Dictionary<string, List<decimal?>>();

            foreach (var symbol in symbols)
            {
                dict[symbol] = new List<decimal?>();
            }

            query = _context.StockHistories
                .Where(m => symbols.Contains(m.Symbol));

            if (from.HasValue)
            {
                dateUtc = from.Value.ToUniversalTime().Date;
                query = query.Where(p => dateUtc <= p.DateUtc);
            }

            if (to.HasValue)
            {
                dateUtc = from.Value.ToUniversalTime().Date;
                query = query.Where(p => p.DateUtc <= dateUtc);
            }

            var histories = query
                .Select(p => new { p.DateUtc, p.Value, p.Name, p.Symbol })
                .OrderBy(p => p.DateUtc)
                .ToList();

            if (histories == null || false == histories.Any())
            {
                return NotFound();
            }

            string date = null;
            int index = -1;

            foreach (var history in histories)
            {
                if (date == history.DateUtc.ToString("yyyy-MM-dd"))
                {
                    dict[history.Symbol][index] = Math.Round(history.Value, 2);
                }
                else
                {
                    date = history.DateUtc.ToString("yyyy-MM-dd");
                    index++;

                    foreach (var symbol in symbols)
                    {
                        dict[symbol].Add(null);
                    }

                    dict[history.Symbol][index] = Math.Round(history.Value, 2);
                }
            }

            var series = new List<Series>();

            foreach (var symbol in symbols)
            {
                var values = dict[symbol];
                var data = new List<LineSeriesData>();

                values.ForEach(p =>
                {
                    data.Add(new LineSeriesData { Y = (p.HasValue ? Convert.ToDouble(p.Value) : null) });
                });

                var serie = new LineSeries
                {
                    Name = symbol,
                    Data = data
                };

                series.Add(serie);
            }

            var xAxis = new XAxis
            {
                Categories = categories
            };

            ViewData["XAxis"] = new List<XAxis>() { xAxis };
            ViewData["Series"] = series;

            return View();
        }

        public IActionResult Investors(string id, DateTime? from, DateTime? to)
        {
            List<string> names;

            if (true == string.IsNullOrEmpty(id))
            {
                names = _context.Investors.Select(p => p.Name.ToUpper()).ToList();
            }
            else
            {
                names = id.ToUpper().Split(',').ToList();
            }

            names = names.Where(p => p != "GENEVIÈVE").ToList();

            var query = _context.InvestmentHistories
                .Where(m => names.Contains(m.Investor.Name));

            DateTime dateUtc;

            if (from.HasValue)
            {
                dateUtc = from.Value.ToUniversalTime().Date;
                query = query.Where(p => dateUtc <= p.DateUtc);
            }

            if (to.HasValue)
            {
                dateUtc = from.Value.ToUniversalTime().Date;
                query = query.Where(p => p.DateUtc <= dateUtc);
            }

            var categories = query
                .Select(p => p.DateUtc).Distinct()
                .OrderBy(p => p)
                .Select(p => p.ToString("yyyy-MM-dd"))
                .ToList();

            var dict = new Dictionary<string, List<decimal?>>();

            foreach (var name in names)
            {
                dict[name] = new List<decimal?>();
            }

            query = _context.InvestmentHistories
                .Where(m => names.Contains(m.Investor.Name));

            if (from.HasValue)
            {
                dateUtc = from.Value.ToUniversalTime().Date;
                query = query.Where(p => dateUtc <= p.DateUtc);
            }

            if (to.HasValue)
            {
                dateUtc = from.Value.ToUniversalTime().Date;
                query = query.Where(p => p.DateUtc <= dateUtc);
            }

            var histories = query
                .Select(p => new { Value = (p.Quantity * p.Value * p.ExchangeRate), p.DateUtc, p.Investor.Name, p.Stock.Symbol })
                .OrderBy(p => p.DateUtc)
                .ToList();

            if (histories == null || false == histories.Any())
            {
                return NotFound();
            }

            string date = null;
            int index = -1;
            string key;

            foreach (var history in histories)
            {
                key = history.Name.ToUpper();

                if (date == history.DateUtc.ToString("yyyy-MM-dd"))
                {
                    dict[key][index] = Math.Round(history.Value, 2);
                }
                else
                {
                    date = history.DateUtc.ToString("yyyy-MM-dd");
                    index++;

                    foreach (var name in names)
                    {
                        dict[name].Add(null);
                    }

                    dict[key][index] = Math.Round(history.Value, 2);
                }
            }

            var series = new List<Series>();

            foreach (var name in names)
            {
                var values = dict[name];
                var data = new List<LineSeriesData>();

                values.ForEach(p =>
                {
                    data.Add(new LineSeriesData { Y = (p.HasValue ? Convert.ToDouble(p.Value) : null) });
                });

                var serie = new LineSeries
                {
                    Name = name,
                    Data = data
                };

                series.Add(serie);
            }

            var xAxis = new XAxis
            {
                Categories = categories
            };

            ViewData["XAxis"] = new List<XAxis>() { xAxis };
            ViewData["Series"] = series;

            return View();
        }
    }
}
