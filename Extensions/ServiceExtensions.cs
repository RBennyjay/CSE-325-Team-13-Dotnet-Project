using SmartBudget.Interfaces;
using SmartBudget.Services;

namespace SmartBudget.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register all business logic services here
            services.AddScoped<IIncomeService, Services.IncomeService>();
            services.AddScoped<IAnalyticsService, Services.AnalyticsService>();
            services.AddScoped<IBudgetService, Services.BudgetService>();
            
            return services;
        }
    }
}
