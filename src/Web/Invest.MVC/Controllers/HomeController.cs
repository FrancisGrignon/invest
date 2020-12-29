using Invest.MVC.Infrastructure;
using Invest.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Invest.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly InvestContext _context;
        private readonly UnitOfWork _unitOfWork;

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

        public IActionResult Total()
        {
            var investments = _context.Investments.Include(prop=> prop.Stock);
            var exchangeRate = 1.38M;
            decimal total = 0M;

            foreach (var investment in investments)
            {
                if (Forex.USD == investment.Currency)
                {
                    total += investment.Quantity * investment.Stock.Value * exchangeRate;
                }
            }

            ViewData["Total"] = total.ToString("C0");

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

    }
}