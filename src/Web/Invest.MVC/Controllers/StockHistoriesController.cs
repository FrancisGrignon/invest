using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Invest.MVC;

namespace Invest.MVC.Controllers
{
    public class StockHistoriesController : Controller
    {
        private readonly InvestContext _context;

        public StockHistoriesController(InvestContext context)
        {
            _context = context;
        }

        // GET: StockHistories
        public async Task<IActionResult> Index()
        {
            var investContext = _context.StockHistories.Include(s => s.Stock);
            return View(await investContext.ToListAsync());
        }

        // GET: StockHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockHistory = await _context.StockHistories
                .Include(s => s.Stock)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stockHistory == null)
            {
                return NotFound();
            }

            return View(stockHistory);
        }

        // GET: StockHistories/Create
        public IActionResult Create()
        {
            ViewData["StockId"] = new SelectList(_context.Stocks, "StockId", "Currency");
            return View();
        }

        // POST: StockHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StockHistoryId,StockId,Name,Symbol,Value,Currency,CreatedUtc")] StockHistory stockHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stockHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StockId"] = new SelectList(_context.Stocks, "StockId", "Currency", stockHistory.StockId);
            return View(stockHistory);
        }

        // GET: StockHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockHistory = await _context.StockHistories.FindAsync(id);
            if (stockHistory == null)
            {
                return NotFound();
            }
            ViewData["StockId"] = new SelectList(_context.Stocks, "StockId", "Currency", stockHistory.StockId);
            return View(stockHistory);
        }

        // POST: StockHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StockHistoryId,StockId,Name,Symbol,Value,Currency,CreatedUtc")] StockHistory stockHistory)
        {
            if (id != stockHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockHistoryExists(stockHistory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StockId"] = new SelectList(_context.Stocks, "StockId", "Currency", stockHistory.StockId);
            return View(stockHistory);
        }

        // GET: StockHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockHistory = await _context.StockHistories
                .Include(s => s.Stock)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stockHistory == null)
            {
                return NotFound();
            }

            return View(stockHistory);
        }

        // POST: StockHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stockHistory = await _context.StockHistories.FindAsync(id);
            _context.StockHistories.Remove(stockHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockHistoryExists(int id)
        {
            return _context.StockHistories.Any(e => e.Id == id);
        }
    }
}
