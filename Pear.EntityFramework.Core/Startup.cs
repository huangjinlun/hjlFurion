using Furion;
using Microsoft.Extensions.DependencyInjection;

namespace Pear.EntityFramework.Core
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabaseAccessor(options =>
            {
                options.AddDbPool<PearDbContext>();
            }, "Pear.Database.Migrations");
        }
    }
}