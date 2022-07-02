using Invest.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Invest.MVC.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly InvestContext _context;

        public TransactionsController(InvestContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var investContext = _context
                .Transactions
                .Include(t => t.Investor)
                .Include(t => t.Operation)
                .Include(t => t.Stock)
                .OrderBy(t => t.DateUtc)
                .ThenBy(t => t.Id);

            return View(await investContext.ToListAsync());
        }
    }
}
