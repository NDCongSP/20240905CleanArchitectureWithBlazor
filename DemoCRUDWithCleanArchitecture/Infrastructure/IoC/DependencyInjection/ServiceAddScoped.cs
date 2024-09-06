using Application.Services;
using Application.Services.Contracts;
using Infrastructure.Repos;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure.IoC.DependencyInjection
{
    public static class ServiceAddScoped
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IAccount, AccountRepository>();
            services.AddScoped<IProduct, ProductRepository>();

            services.AddScoped<Repository>();
        }
    }
}
