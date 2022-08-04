using Hao.GroupBlog.Domain.Interfaces;
using Hao.GroupBlog.Manager.Implements;
using Hao.GroupBlog.Manager.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Hao.GroupBlog.Manager
{

    public static class DomainInjection
    {
        public static IServiceCollection DomainConfigureServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DomainInjection).Assembly);
            services.AddMemoryCache();
            services.AddSingleton<ICacheProvider, MemoryCacheProvider>();

            services.AddTransient<IResourceManager, ResourceManager>();
            services.AddTransient<IMemberManager, MemberManager>();
            services.AddTransient<INoteManager, NoteManager>();
            services.AddTransient<ISeriesManager, SeriesManager>();
            services.AddScoped<PrivilegeManager>();

            return services;
        }

        //public static IApplicationBuilder DomainConfigure(this IApplicationBuilder app)
        //{
        //    return app;
        //}
    }
}
