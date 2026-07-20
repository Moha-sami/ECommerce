using ECommerce.Application.Contacts;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.DataSeeding;
using ECommerce.Domain.Entities.Identity;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Data.Identity;
using ECommerce.Infrastructure.DataSeeding;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Stripe;
using InfraTokenService = ECommerce.Infrastructure.Services.TokenService;

namespace ECommerce.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Main DB
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Identity DB
        services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));

        // Identity
        services.AddIdentityCore<AppUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();
        services.AddScoped<SignInManager<AppUser>>();

        // Redis
        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")
                ?? throw new InvalidOperationException("Redis connection string is missing.")));

        // Stripe
        StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"];

        // Application services
        services.AddScoped<ITokenService, InfraTokenService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped<IResponseCacheService, ResponseCacheService>();
        services.AddKeyedScoped<IDataSeeder, CatalogDataSeeder>(nameof(CatalogDataSeeder));
        services.AddKeyedScoped<IDataSeeder, IdentityDataSeeder>(nameof(IdentityDataSeeder));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
