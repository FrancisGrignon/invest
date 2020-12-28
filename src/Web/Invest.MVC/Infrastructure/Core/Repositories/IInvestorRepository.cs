using System.Threading.Tasks;

namespace Invest.MVC.Infrastructure.Core.Repositories
{
    public interface IInvestorRepository : IRepository<Investor>
    {
        public Investor GetByName(string name);

        public Task<Investor> GetByNameAsync(string name);
    }
}