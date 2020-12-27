using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Invest.MVC
{
    public static class MigrateExtension
    {
        public static void MigrateDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<InvestContext>();

                if (context != null && context.Database != null)
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}