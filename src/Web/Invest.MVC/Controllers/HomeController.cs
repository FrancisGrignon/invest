using Invest.MVC.Infrastructure;
using Invest.MVC.Models;
using Invest.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;

namespace Invest.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly InvestContext _context;
        private readonly UnitOfWork _unitOfWork;

        private bool ExcludeGenevieve
        {
            get
            {
                return HttpContext.Session.Get<bool>("ExcludeGenevieve");
            }
            set
            {
                HttpContext.Session.Set<bool>("ExcludeGenevieve", value);
            }
        }

        public HomeController(ILogger<HomeController> logger, InvestContext context)
        {
            _logger = logger;
            _context = context;
            _unitOfWork = new UnitOfWork(_context);
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/import")]
        public IActionResult Import()
        {
            if (false == System.IO.File.Exists("invest.db"))
            {
                _context.Database.Migrate();
                _context.Import();
            }

            return View();
        }

        public IActionResult Genevieve()
        {
            ExcludeGenevieve = !ExcludeGenevieve;

            return View();
        }

        public IActionResult Total()
        {
            var investments = _context.Investments.Include(prop => prop.Stock)
              .Where(p => false == ExcludeGenevieve || ExcludeGenevieve && p.Investor.Name != "GENEVIÈVE");

            var exchangeRate = 1.27f;
            float total = 0f;

            foreach (var investment in investments)
            {
                if (Forex.USD == investment.Currency)
                {
                    total += investment.Quantity * investment.Stock.Value * exchangeRate;
                }
                else
                {
                    total += investment.Quantity * investment.Stock.Value;
                }
            }

            ViewData["Total"] = total.ToString("0.00");
            ViewData["From"] = 700.ToString("0.00");
            ViewData["Increase"] = (total - 700).ToString("0.00");

            return View();
        }

        public IActionResult Help()
        {
            return View();
        }

        public IActionResult Bug()
        {
            return View();
        }

        public IActionResult Progress()
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


        [Route("profile")]
        public IActionResult Profile()
        {
            var model = new ProfileViewModel
            {
                Name = User.Identity.Name,
                Claims = User.Claims
            };

            return View(model);
        }
    }
}