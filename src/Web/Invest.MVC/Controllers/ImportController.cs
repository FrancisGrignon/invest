using Invest.MVC.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Invest.MVC.Controllers
{
    [Authorize]
    public class ImportController : Controller
    {
        private readonly ImportService _importService;

        public ImportController(InvestContext context)
        {
            _importService = new ImportService(context);
        }

        public IActionResult Index()
        {
            _importService.Execute();

            return View("Index");
        }
    }
}
