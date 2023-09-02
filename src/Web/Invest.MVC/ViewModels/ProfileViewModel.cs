using System.Collections.Generic;
using System.Security.Claims;

namespace Invest.MVC.ViewModels
{
    public class ProfileViewModel
    {
        public string Name { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
    }
}
