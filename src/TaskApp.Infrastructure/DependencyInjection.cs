using Microsoft.Extensions.DependencyInjection;
using TaskApp.Domain.Interfaces.Geniric;
using TaskApp.Domain.Interfaces.Repositories;
using TaskApp.Infrastructure.Persistence;
using TaskApp.Infrastructure.Repositories;

namespace TaskApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITaskCategoryRepository, TaskCategoryRepository>();
            services.AddScoped<ITaskItemRepository, TaskRepository>();

            return services;
        }
    }
}
