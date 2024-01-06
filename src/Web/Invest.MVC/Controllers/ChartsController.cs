using Highsoft.Web.Mvc.Charts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Invest.MVC.Controllers
{
    public class ChartsController : Controller
    {
        private readonly ILogger<ChartsController> _logger;
        private readonly InvestContext _context;

        private bool ExcludeGenevieve
        {
            get
            {
                return HttpContext.Session.Get<bool>("ExcludeGenevieve");
            }
        }

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
                symbols = _context.Stocks.Select(p => p.Symbol).ToList();
            }
            else
            {
                symbols = id.Split(',').ToList();
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

            var dict = new Dictionary<string, List<double?>>();

            foreach (var symbol in symbols)
            {
                dict[symbol] = new List<double?>();
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
                    data.Add(new LineSeriesData { Y = (p.HasValue ? p.Value : null) });
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

            AddRange();

            return View();
        }



        public IActionResult Investors(string id, DateTime? from, DateTime? to)
        {
            List<string> names;

            if (true == string.IsNullOrEmpty(id))
            {
                names = _context.Investors.Select(p => p.Name).ToList();
            }
            else
            {
                names = id.Split(',').ToList();
            }

            names = names.Where(p => false == ExcludeGenevieve || ExcludeGenevieve && p != "Geneviève").ToList();

            var query = _context.InvestmentHistories
                .Where(m => names.Contains(m.Investor.Name));

            DateTime dateUtc;

            if (from.HasValue)
            {
                dateUtc = from.Value.ToUniversalTime().Date;
                query = _context.InvestmentHistories.Where(p => dateUtc <= p.DateUtc);
            }

            if (to.HasValue)
            {
                dateUtc = from.Value.ToUniversalTime().Date;
                query = _context.InvestmentHistories.Where(p => p.DateUtc <= dateUtc);
            }

            var categories = query
                .Select(p => p.DateUtc).Distinct()
                .OrderBy(p => p)
                .Select(p => p.ToString("yyyy-MM-dd"))
                .ToList();

            var dict = new Dictionary<string, List<double?>>();

            foreach (var name in names)
            {
                dict[name] = new List<double?>();
            }

            query = _context.InvestmentHistories
               .Where(m => names.Contains(m.Investor.Name));

            if (from.HasValue)
            {
                dateUtc = from.Value.ToUniversalTime().Date;
                query = _context.InvestmentHistories.Where(p => dateUtc <= p.DateUtc);
            }

            if (to.HasValue)
            {
                dateUtc = from.Value.ToUniversalTime().Date;
                query = _context.InvestmentHistories.Where(p => p.DateUtc <= dateUtc);
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
                key = history.Name;

                if (date == history.DateUtc.ToString("yyyy-MM-dd"))
                {
                    dict[key][index] = Math.Round(Convert.ToDouble(history.Value), 2);
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
                    data.Add(new LineSeriesData { Y = (p.HasValue ? p.Value : null) });
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

            AddRange();

            return View();
        }

        public IActionResult Average(string id, DateTime? from, DateTime? to)
        {
            List<string> names;

            if (true == string.IsNullOrEmpty(id))
            {
                names = _context.Investors.Select(p => p.Name).ToList();
            }
            else
            {
                names = id.Split(',').ToList();
            }

            names = names.Where(p => false == ExcludeGenevieve || ExcludeGenevieve && p != "Geneviève").ToList();

            DateTime dateUtc;

            var query = _context.InvestmentHistories
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
                .GroupBy(p => p.DateUtc)
                .Select(p => new
                {
                    DateUtc = p.Key,
                    Count = p.Count(),
                    Value = p.Sum(y => y.Quantity * y.Value * y.ExchangeRate)
                })
                .OrderBy(p => p.DateUtc)
                .ToList();

            if (histories == null || false == histories.Any())
            {
                return NotFound();
            }

            var values = new List<double>();
            var categories = new List<string>();

            foreach (var history in histories)
            {
                categories.Add(history.DateUtc.ToString("yyyy-MM-dd"));
                values.Add(Math.Round(Convert.ToDouble(history.Value / history.Count), 2));
            }

            var xAxis = new XAxis
            {
                Categories = categories
            };

            var data = new List<LineSeriesData>();

            values.ForEach(p =>
            {
                data.Add(new LineSeriesData { Y = p });
            });

            var serie = new LineSeries
            {
                Name = "Moyenne",
                Data = data
            };

            ViewData["XAxis"] = new List<XAxis>() { xAxis };
            ViewData["Series"] = new List<Series> { serie };

            AddRange();

            return View();
        }

        public IActionResult Sum(string id, DateTime? from, DateTime? to)
        {
            List<string> names;

            if (true == string.IsNullOrEmpty(id))
            {
                names = _context.Investors.Select(p => p.Name).ToList();
            }
            else
            {
                names = id.Split(',').ToList();
            }

            names = names.Where(p => false == ExcludeGenevieve || ExcludeGenevieve && p != "GENEVIÈVE").ToList();

            DateTime dateUtc;

            var query = _context.InvestmentHistories
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
                .GroupBy(p => p.DateUtc)
                .Select(p => new
                {
                    DateUtc = p.Key,
                    Value = p.Sum(y => y.Quantity * y.Value * y.ExchangeRate)
                })
                .OrderBy(p => p.DateUtc)
                .ToList();

            var deposits = _context.Transactions
                .Where(p => (p.OperationId == Operation.Deposit || p.OperationId == Operation.Withdraw) && p.Currency == Forex.CAD)
                .GroupBy(p => p.DateUtc.Date)
                .Select(p => new
                {
                    DateUtc = p.Key,
                    Value = p.Sum(y => (y.OperationId == Operation.Deposit ? y.Amount : -y.Amount) * y.ExchangeRate)
                })
                .OrderBy(p => p.DateUtc)
                .ToList();

            if (deposits == null || false == deposits.Any())
            {
                return NotFound();
            }

            var xValue = new List<double>();
            var xDepot = new List<double>();

            //   var values = new List<double>();
            var categories = new List<string>();
            double sum = 0;
            float amount;

            foreach (var history in histories)
            {
                categories.Add(history.DateUtc.ToString("yyyy-MM-dd"));
                xValue.Add(Math.Round(Convert.ToDouble(history.Value), 2));

                sum = deposits.Where(p => p.DateUtc <= history.DateUtc).Sum(p => p.Value);

                xDepot.Add(Math.Round(Convert.ToDouble(sum), 2));
            }

            var xAxis = new XAxis
            {
                Categories = categories
            };

            var sumData = new List<LineSeriesData>();

            xValue.ForEach(p =>
            {
                sumData.Add(new LineSeriesData { Y = p });
            });

            var depotData = new List<LineSeriesData>();

            xDepot.ForEach(p =>
            {
                depotData.Add(new LineSeriesData { Y = p });
            });

            var series = new List<Series>()
            {
                new LineSeries
                {
                    Name = "Valeur",
                    Data = sumData
                },
                new LineSeries
                {
                    Name = "Dépots",
                    Data = depotData
                }
            };

            //var serie = new LineSeries
            //{
            //    Name = "Somme",
            //    Data = data
            //};

            ViewData["XAxis"] = new List<XAxis>() { xAxis };
            // ViewData["Series"] = new List<Series> { serie };
            ViewData["Series"] = series;

            AddRange();

            return View();
        }

        private void AddRange()
        {
            var currentDateUtc = _context
                .InvestmentHistories
                .Where(p => false == ExcludeGenevieve || ExcludeGenevieve && p.Investor.Name != "GENEVIÈVE")
                .Max(p => p.DateUtc);

            ViewData["1w"] = currentDateUtc.AddDays(-7).ToString("yyyy-MM-dd");
            ViewData["1m"] = currentDateUtc.AddDays(-7 * 4).ToString("yyyy-MM-dd");
            ViewData["6m"] = currentDateUtc.AddDays(-7 * 26).ToString("yyyy-MM-dd");
            ViewData["1y"] = currentDateUtc.AddYears(-1).ToString("yyyy-MM-dd");
            ViewData["all"] = new DateTime(2019, 06, 07).ToUniversalTime().ToString("yyyy-MM-dd");
            ViewData["cousin"] = new DateTime(2020, 12, 25).ToUniversalTime().ToString("yyyy-MM-dd");
        }
    }
}
