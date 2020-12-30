using Invest.MVC.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    }
}
