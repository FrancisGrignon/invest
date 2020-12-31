using Invest.MVC.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invest.MVC.Controllers
{
    public class InvestmentsController : Controller
    {
        private readonly InvestContext _context;
        private readonly UnitOfWork _unitOfWork;

        public InvestmentsController(InvestContext context)
        {
            _context = context;
            _unitOfWork = new UnitOfWork(_context);
        }

        // GET: Investments
        public async Task<IActionResult> Index()
        {
            var dateUtc = await _context
                .InvestmentHistories
                .OrderByDescending(p => p.Id)
                .Select(p => p.DateUtc)
                .FirstOrDefaultAsync();

            var history = _context
                .InvestmentHistories
                .Include(p => p.Investor)
                .Include(p => p.Stock)
                .Where(p => p.DateUtc == dateUtc)
                .OrderBy(p => p.Investor.Name);

            return View(await history.ToListAsync());
        }

        public async Task<IActionResult> Progress()
        {
            var currentDateUtc = await _context
                .InvestmentHistories
                .OrderByDescending(p => p.Id)
                .Select(p => p.DateUtc)
                .FirstOrDefaultAsync();

            var lastWeekDateUtc = currentDateUtc.AddDays(-7);
            var lastMonthDateUtc = currentDateUtc.AddDays(-7 * 4);
            var sixMonthDateUtc = currentDateUtc.AddDays(-7 * 26);
            var lastYearDateUtc = currentDateUtc.AddDays(-7 * 52);

            var histories = _context
                .InvestmentHistories
                .Include(p => p.Investor)
                .Include(p => p.Stock)
                .Where(p =>
                     p.DateUtc == currentDateUtc ||
                     p.DateUtc == lastWeekDateUtc ||
                     p.DateUtc == lastMonthDateUtc ||
                     p.DateUtc == sixMonthDateUtc ||
                     p.DateUtc == lastYearDateUtc)
                .OrderByDescending(p => p.Id)
                .OrderByDescending(p => p.DateUtc)
                .ThenBy(p => p.Investor.Name)
                .Select(p => new { p.DateUtc, Investor = p.Investor.Name, Stock = $"{p.Stock.Name} ({p.Stock.Symbol})", Value = (p.Quantity * p.Value * p.ExchangeRate) });

            var dictionnary = new Dictionary<string, ProgressViewModel>();

            var dateUtc = DateTime.MinValue;
            var index = -1;
            decimal today = 0M, yesterday = 0M;
            ProgressViewModel model;

            foreach (var history in await histories.ToListAsync())
            {
                if (dateUtc == history.DateUtc)
                {
                    // Ignore
                }
                else
                {
                    index = index + 1;

                    dateUtc = history.DateUtc;
                }

                if (false == dictionnary.TryGetValue(history.Investor, out model))
                {
                    model = new ProgressViewModel
                    {
                        Investor = history.Investor,
                        Stock = history.Stock,
                        Values = new decimal[] { 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M }
                    };

                    dictionnary[history.Investor] = model;
                }

                if (0 == index)
                {
                    model.Values[index] = history.Value;
                }
                else
                {
                    today = model.Values[0];
                    yesterday = history.Value;

                    model.Values[index] = today - yesterday;

                    if (0 < today)
                    {
                        model.Values[index + 4] = ((today - yesterday) / yesterday);
                    }
                }
            }

            var viewModel = new ProgressListViewModel
            {
                Progresses = dictionnary.Values.ToList(),
                Date = currentDateUtc.ToString("yyyy-MM-dd")
            };

            return View(viewModel);
        }
    }
    public class ProgressListViewModel
    {
        public List<ProgressViewModel> Progresses { get; set; }

        public string Date { get; set; }
    }

    public class ProgressViewModel
    {
        public string Investor { get; set; }

        public string Stock { get; set; }

        public decimal[] Values { get; set; }

        public string A { get { return Values[0].ToString("C2"); } }
        public string B1 { get { return Values[1].ToString("C2"); } }
        public string B2 { get { return Values[5].ToString("P2"); } }
        public string C1 { get { return Values[2].ToString("C2"); } }
        public string C2 { get { return Values[6].ToString("P2"); } }
        public string D1 { get { return Values[3].ToString("C2"); } }
        public string D2 { get { return Values[7].ToString("P2"); } }
        public string E1 { get { return Values[4].ToString("C2"); } }
        public string E2 { get { return Values[8].ToString("P2"); } }

    }
}