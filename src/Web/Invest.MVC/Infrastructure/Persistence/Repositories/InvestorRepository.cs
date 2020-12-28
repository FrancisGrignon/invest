using Invest.MVC.Infrastructure.Core.Repositories;
using System;
using System.Linq;

namespace Invest.MVC.Infrastructure.Persistence.Repositories
{
    public class InvestorRepository : Repository<Investor, InvestContext>, IInvestorRepository
    {
        public InvestorRepository(InvestContext context) : base(context)
        {
            // Empty
        }
    }
}