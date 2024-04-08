using Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Models;

namespace Infra.Data
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddInfraestructureDependencies(this IServiceCollection services)
        {
            services.AddScoped<IEmployeesContract, EmployeesRepository>();

            return services;
        }
    }
}
