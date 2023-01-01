using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Index(string id)
        {
            if (string.IsNullOrEmpty(id)) 
            {
                var withoutId = _context
                    .Transactions
                    .Include(t => t.Investor)
                    .Include(t => t.Operation)
                    .Include(t => t.Stock)
                    .OrderBy(t => t.DateUtc)
                    .ThenBy(t => t.Id);

                return View(await withoutId.ToListAsync());
            }

            var withId = _context
                 .Transactions
                 .Include(t => t.Investor)
                 .Include(t => t.Operation)
                 .Include(t => t.Stock)
                 .Where(t => t.Investor.Name == id)
                 .OrderBy(t => t.DateUtc)
                 .ThenBy(t => t.Id);

            return View(await withId.ToListAsync());
        }
    }
}
