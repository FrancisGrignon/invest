using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Invest.MVC.Models;
using Highsoft.Web.Mvc.Charts;
using Invest.MVC.Infrastructure.Services;
using Invest.MVC.Infrastructure;

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


        public IActionResult Graph(string id)
        {
            if (true == string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var symbols = id.ToUpper().Split(',').ToList();


            var categories = _context.StockHistories
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

            var histories = _context.StockHistories
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