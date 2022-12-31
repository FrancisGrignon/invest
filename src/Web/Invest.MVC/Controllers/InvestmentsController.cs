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

        private bool ExcludeGenevieve
        {
            get
            {
                return HttpContext.Session.Get<bool>("ExcludeGenevieve");
            }
        }

        public InvestmentsController(InvestContext context)
        {
            _context = context;
            _unitOfWork = new UnitOfWork(_context);
        }

        // GET: Investments
        public async Task<IActionResult> Index(DateTime? from)
        {
            DateTime dateUtc;

            if (from.HasValue)
            {
                dateUtc = from.Value.ToUniversalTime().Date;
            }
            else
            {
                dateUtc = await _context
                    .InvestmentHistories
                    .Where(p => false == ExcludeGenevieve || ExcludeGenevieve && p.Investor.Name != "GENEVIÈVE")
                    .MaxAsync(p => p.DateUtc);
            }

            var history = _context
                .InvestmentHistories
                .Include(p => p.Investor)
                .Include(p => p.Stock)
                .Where(p => false == ExcludeGenevieve || ExcludeGenevieve && p.Investor.Name != "GENEVIÈVE")
                .Where(p => p.DateUtc == dateUtc)
                .OrderBy(p => p.Investor.Name);

            return View(await history.ToListAsync());
        }

        public async Task<IActionResult> Progress()
        {
            var currentDateUtc = await _context
                .InvestmentHistories
                .Where(p => false == ExcludeGenevieve || ExcludeGenevieve && p.Investor.Name != "GENEVIÈVE")
                .MaxAsync(p => p.DateUtc);

            var lastWeekDateUtc = currentDateUtc.AddDays(-7);
            var lastMonthDateUtc = currentDateUtc.AddDays(-7 * 4);
            var sixMonthDateUtc = currentDateUtc.AddDays(-7 * 26);
            var lastYearDateUtc = currentDateUtc.AddDays(-7 * 52);

            var histories = _context
                .InvestmentHistories
                .Include(p => p.Investor)
                .Include(p => p.Stock)
                .Where(p =>
                    (false == ExcludeGenevieve || ExcludeGenevieve && p.Investor.Name != "GENEVIÈVE") &&
                    (
                        p.DateUtc == currentDateUtc ||
                        p.DateUtc == lastWeekDateUtc ||
                        p.DateUtc == lastMonthDateUtc ||
                        p.DateUtc == sixMonthDateUtc ||
                        p.DateUtc == lastYearDateUtc)
                    )
                .OrderByDescending(p => p.Id)
                .OrderByDescending(p => p.DateUtc)
                .ThenBy(p => p.Investor.Name)
                .Select(p => new { 
                    p.DateUtc, 
                    Investor = p.Investor.Name, Stock = p.Stock.Name, p.Stock.Symbol, p.Stock.Market, 
                    Value = (p.Quantity * p.Value * p.ExchangeRate) });

            var dictionnary = new Dictionary<string, ProgressViewModel>();

            var dateUtc = DateTime.MinValue;
            var index = -1;
            float today = 0f, yesterday = 0f;
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
                        Symbol = history.Symbol,
                        Market = history.Market,
                        Values = new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }
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
                Progresses = dictionnary.Values.OrderByDescending(x => x.Values[0]).ToList(),
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
        private const string FORMAT_MONEY = "0.00$;-0.00$";
        private const string FORMAT_PERCENT = "0.00%;-0.00%";

        private string Style(float value)
        {
            if (value < 0f)
            {
                return "fw-price-dn";
            } 
            else if (0f < value)
            {
                return "fw-price-up";
            }
            else
            {
                return "fw-price-eq";
            }            
        }

        public string Investor { get; set; }

        public string Stock { get; set; }

        public string Market { get; set; }
        
        public string Symbol { get; set; }

        public float[] Values { get; set; }

        public string A { get { return Values[0].ToString(FORMAT_MONEY); } }
        public string B1 { get { return Values[1].ToString(FORMAT_MONEY); } }
        public string B2 { get { return Values[5].ToString(FORMAT_PERCENT); } }
        public string B3 { get { return Style(Values[1]); } }
        public string C1 { get { return Values[2].ToString(FORMAT_MONEY); } }
        public string C2 { get { return Values[6].ToString(FORMAT_PERCENT); } }
        public string C3 { get { return Style(Values[2]); } }
        public string D1 { get { return Values[3].ToString(FORMAT_MONEY); } }
        public string D2 { get { return Values[7].ToString(FORMAT_PERCENT); } }
        public string D3 { get { return Style(Values[3]); } }
        public string E1 { get { return Values[4].ToString(FORMAT_MONEY); } }
        public string E2 { get { return Values[8].ToString(FORMAT_PERCENT); } }
        public string E3 { get { return Style(Values[4]); } }
    }
}