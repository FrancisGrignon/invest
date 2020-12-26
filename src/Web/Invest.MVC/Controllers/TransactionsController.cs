using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Invest.MVC;
using static Invest.MVC.ViewModels.TransactionEditViewModel;
using Invest.MVC.ViewModels;

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
            var investContext = _context.Transactions.Include(t => t.Investor).Include(t => t.Operation).Include(t => t.Stock);
            return View(await investContext.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Investor)
                .Include(t => t.Operation)
                .Include(t => t.Stock)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            ViewData["InvestorId"] = new SelectList(_context.Investors, "InvestorId", "Name");
            ViewData["OperationId"] = new SelectList(_context.Operations, "OperationId", "Name");
            ViewData["StockId"] = new SelectList(_context.Stocks, "StockId", "Name");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactionId,StockId,OperationId,InvestorId,Amount,Currency,Quantity,Description,Created")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InvestorId"] = new SelectList(_context.Investors, "InvestorId", "Name", transaction.InvestorId);
            ViewData["OperationId"] = new SelectList(_context.Operations, "OperationId", "Name", transaction.OperationId);
            ViewData["StockId"] = new SelectList(_context.Stocks, "StockId", "Name", transaction.StockId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _context.Transactions.FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new TransactionEditViewModel
            {
                Amount = entity.Amount.ToString("N2"),
                Currency = entity.Currency,
                Description = entity.Description,
                OperationId = entity.OperationId,
                Quantity = entity.Quantity.ToString("N3"),
                StockId = entity.StockId,
                TransactionId = entity.TransactionId
            };

            ViewData["InvestorId"] = new SelectList(_context.Investors, "InvestorId", "Name", entity.InvestorId);
            ViewData["OperationId"] = new SelectList(_context.Operations, "OperationId", "Name", entity.OperationId);
            ViewData["StockId"] = new SelectList(_context.Stocks, "StockId", "Name", entity.StockId);
            
            return View(model);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransactionId,StockId,OperationId,InvestorId,Amount,Currency,Quantity,Description,Created")] TransactionEditViewModel model)
        {
            if (id != model.TransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var entity = await _context.Transactions.FindAsync(id);

                if (entity == null)
                {
                    return NotFound();
                }

                if (decimal.TryParse(model.Amount, out decimal amount))
                {
                    entity.Amount = amount;
                }

                entity.Currency = model.Currency;
                entity.Description = model.Description;
                entity.OperationId = model.OperationId;

                if (decimal.TryParse(model.Quantity, out decimal quantity))
                {
                    entity.Quantity = quantity;
                }

                entity.StockId = model.StockId;
                entity.TransactionId = model.TransactionId;

                try
                {
                    _context.Update(entity);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(model.TransactionId))
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

            ViewData["InvestorId"] = new SelectList(_context.Investors, "InvestorId", "Name", model.InvestorId);
            ViewData["OperationId"] = new SelectList(_context.Operations, "OperationId", "Name", model.OperationId);
            ViewData["StockId"] = new SelectList(_context.Stocks, "StockId", "Name", model.StockId);
            
            return View(model);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Investor)
                .Include(t => t.Operation)
                .Include(t => t.Stock)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.TransactionId == id);
        }
    }
}
