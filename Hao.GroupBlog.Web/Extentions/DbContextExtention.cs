using Hao.GroupBlog.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Hao.GroupBlog.Web.Extentions
{
    public static class DbContextExtention
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, ConfigurationManager configuration)
        {
            string connectStr = configuration.GetConnectionString("Default");
            services.AddDbContext<GbDbContext>(
                config => config.UseSqlite(connectStr,
                optionBuilder => optionBuilder.MigrationsAssembly(typeof(GbDbContext).Assembly.GetName().Name)),
                ServiceLifetime.Scoped
                );
            return services;
        }
    }
}
