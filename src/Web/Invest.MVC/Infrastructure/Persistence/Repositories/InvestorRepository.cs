using Invest.MVC.Infrastructure.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Invest.MVC.Infrastructure.Persistence.Repositories
{
    public class InvestorRepository : Repository<Investor, InvestContext>, IInvestorRepository
    {
        public InvestorRepository(InvestContext context) : base(context)
        {
            // Empty
        }

        public Investor GetByName(string name)
        {
            return Context.Set<Investor>().Where(p => p.Enable && name == p.Name).SingleOrDefault();
        }

        public Task<Investor> GetByNameAsync(string name)
        {
            return Context.Set<Investor>().Where(p => p.Enable && name == p.Name).SingleOrDefaultAsync();
        }
    }
}