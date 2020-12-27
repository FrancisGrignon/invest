using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Invest.MVC.Models;
using Highsoft.Web.Mvc.Charts;

namespace Invest.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly InvestContext _context;

        public HomeController(ILogger<HomeController> logger, InvestContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Graph1()
        {
            var tokyoValues = new List<double> { 7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6 };
            var nyValues = new List<double> { -0.2, 0.8, 5.7, 11.3, 17.0, 22.0, 24.8, 24.1, 20.1, 14.1, 8.6, 2.5 };
            var berlinValues = new List<double> { -0.9, 0.6, 3.5, 8.4, 13.5, 17.0, 18.6, 17.9, 14.3, 9.0, 3.9, 1.0 };

            var tokyoData = new List<LineSeriesData>();
            var nyData = new List<LineSeriesData>();
            var berlinData = new List<LineSeriesData>();

            tokyoValues.ForEach(p => tokyoData.Add(new LineSeriesData { Y = p }));
            nyValues.ForEach(p => nyData.Add(new LineSeriesData { Y = p }));
            berlinValues.ForEach(p => berlinData.Add(new LineSeriesData { Y = p }));

            ViewData["tokyoData"] = tokyoData;
            ViewData["nyData"] = nyData;
            ViewData["berlinData"] = berlinData;

            return View();
        }

        public IActionResult Graph2(int? id)
        {
            var history = _context.StockHistory
                              .Where(m => m.StockId == id)
                              .Select(p => new { p.CreatedUtc, p.Value })
                              .OrderBy(p => p.CreatedUtc);

            if (history == null || false == history.Any())
            {
                return NotFound();
            }

            var costcoValues = history.ToList();

            var costcoData = new List<LineSeriesData>();
            var costcoCategory = new List<String>();

            costcoValues.ForEach(p =>
            {
                costcoData.Add(new LineSeriesData { Y = Convert.ToDouble(p.Value) });
                costcoCategory.Add(p.CreatedUtc.ToString("yyyy-MM-dd"));
            });

            ViewData["costcoData"] = costcoData;
            ViewData["costcoCategory"] = costcoCategory;

            return View();
        }

        public IActionResult Graph3(string id)
        {
            if (true == string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var count = 0;
            var symbols = id.Split(',').ToList();

            var series = new List<Series>();
            var categories = new List<string>();

            foreach (var symbol in symbols)
            {
                var history = _context.StockHistory
                                  .Where(m => m.Symbol == symbol)
                                  .Select(p => new { p.CreatedUtc, p.Value, p.Name })
                                  .OrderBy(p => p.CreatedUtc);

                if (history == null || false == history.Any())
                {
                    // Ignore
                }
                else
                {
                    var values = history.ToList();

                    var data = new List<LineSeriesData>();
                    string name = symbol;

                    values.ForEach(p =>
                    {
                        data.Add(new LineSeriesData { Y = Convert.ToDouble(p.Value) });

                        if (0 == count)
                        {
                            categories.Add(p.CreatedUtc.ToString("yyyy-MM-dd"));
                        }

                        name = p.Name;
                    });

                    var serie = new LineSeries
                    {
                        Name = name,
                        Data = data
                    };

                    series.Add(serie);
                }

                count++;
            }

            var xAxis = new XAxis
            {
                Categories = categories
            };


            ViewData["XAxis"] = new List<XAxis>() { xAxis };
            ViewData["Series"] = series;

            return View();
        }

        public IActionResult Graph(string id)
        {
            if (true == string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var symbols = id.ToUpper().Split(',').ToList();


            var categories = _context.StockHistory
                .Where(m => symbols.Contains(m.Symbol))
                .Select(p => p.CreatedUtc).Distinct()
                .OrderBy(p => p)
                .Select(p => p.ToString("yyyy-MM-dd"))
                .ToList();

            var dict = new Dictionary<string, List<decimal?>>();

            foreach (var symbol in symbols)
            {
                dict[symbol] = new List<decimal?>();
            }

            var histories = _context.StockHistory
                                .Where(m => symbols.Contains(m.Symbol))
                                .Select(p => new { p.CreatedUtc, p.Value, p.Name, p.Symbol })
                                .OrderBy(p => p.CreatedUtc)
                                .ToList();

            if (histories == null || false == histories.Any())
            {
                return NotFound();
            }

            string date = null;
            int index = -1;

            foreach (var history in histories)
            {
                if (date == history.CreatedUtc.ToString("yyyy-MM-dd"))
                {
                    dict[history.Symbol][index] = history.Value;
                }
                else
                {
                    date = history.CreatedUtc.ToString("yyyy-MM-dd");
                    index++;

                    foreach (var symbol in symbols)
                    {
                        dict[symbol].Add(null);
                    }

                    dict[history.Symbol][index] = history.Value;
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
    }
}