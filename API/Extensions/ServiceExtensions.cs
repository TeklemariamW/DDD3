using Azure.Identity;
using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace API.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
        });
    }
    public static void ConfigureIISIntegration(this IServiceCollection services)
    {
        services.Configure<IISOptions>(option =>
        {

        });
    }
    public static void ConfigureRepositoryWrapper(this IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
    {
        string accountEndpoint = configuration["ConnectionString:CosmosDb:AccountKey"];
        string dataBaseName = configuration["ConnectionString:CosmosDb:DbName"];

        if (environment.IsDevelopment())
        {
            services.AddDbContext<RepositoryContext>(delegate (DbContextOptionsBuilder options)
            {
                options.UseCosmos(accountEndpoint, dataBaseName);
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            });
        }
        if (environment.IsProduction())
        {
            services.AddDbContext<RepositoryContext>(delegate (DbContextOptionsBuilder options)
            {
                options.UseCosmos(accountEndpoint, new DefaultAzureCredential(), dataBaseName);
            });
        }
        services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
    }
    public static void EnsureRepositoryContextDatabaseExist(this IHost host)
    {
        using IServiceScope serviceScope = host.Services.CreateScope();
        serviceScope.ServiceProvider.GetRequiredService<RepositoryContext>().Database.EnsureCreated();
    }
}
