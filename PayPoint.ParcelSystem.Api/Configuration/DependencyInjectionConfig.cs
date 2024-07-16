using Microsoft.EntityFrameworkCore;
using PayPoint.ParcelSystem.Domain.Interfaces;
using PayPoint.ParcelSystem.Domain.Services;
using PayPoint.ParcelSystem.Infrastructure.Data;
using PayPoint.ParcelSystem.Infrastructure.Logging;
using PayPoint.ParcelSystem.Infrastructure.Repositories;
using ILogger = PayPoint.ParcelSystem.Domain.Interfaces.ILogger;

namespace PayPoint.ParcelSystem.Api.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection ResolveDependencies(this IServiceCollection services)
    {
        // Add Application Insights
        

        // Register the logger
        services.AddScoped<ILogger, ApplicationInsightsLogger>();

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();

        services.AddScoped<ICollectionService, CollectionService>();

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
        {
            services.AddDbContext<EFDbContext>(
                options => options.UseSqlServer("name=ConnectionStrings:IdeallyGetThisFrom---Azure---KeyVault---"));

            services.AddScoped<IDbContext, EFDbContext>();
        }
        else
        {
            services.AddScoped<IDbContext, StubDbContext>();
        }

        return services;
    }
}
