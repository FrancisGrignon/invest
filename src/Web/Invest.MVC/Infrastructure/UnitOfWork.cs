using Invest.MVC.Infrastructure.Core.Repositories;
using Invest.MVC.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invest.MVC.Infrastructure
{
    public class UnitOfWork
    {
        private InvestContext _context;
        private IForexRepository _forexRepository;
        private IInvestmentRepository _investmentRepository;
        private IInvestorRepository _investorRepository;
        private IStockRepository _stockRepository;
        private ITransactionRepository _transactionRepository;

        public IForexRepository ForexRepository
        { 
            get
            {
                if (null == _forexRepository)
                {
                    _forexRepository = new ForexRepository(_context);
                }

                return _forexRepository;
            }
        }

        public IInvestmentRepository InvestmentRepository
        {
            get
            {
                if (null == _investmentRepository)
                {
                    _investmentRepository = new InvestmentRepository(_context);
                }

                return _investmentRepository;
            }
        }

        public IInvestorRepository InvestorRepository
        {
            get
            {
                if (null == _investorRepository)
                {
                    _investorRepository = new InvestorRepository(_context);
                }

                return _investorRepository;
            }
        }

        public IStockRepository StockRepository
        {
            get
            {
                if (null == _stockRepository)
                {
                    _stockRepository = new StockRepository(_context);
                }

                return _stockRepository;
            }
        }

        public ITransactionRepository TransactionRepository
        {
            get
            {
                if (null == _transactionRepository)
                {
                    _transactionRepository = new TransactionRepository(_context);
                }

                return _transactionRepository;
            }
        }

        public UnitOfWork(InvestContext context)
        {
            _context = context;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
