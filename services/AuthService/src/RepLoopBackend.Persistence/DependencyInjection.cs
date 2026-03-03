using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepLoopBackend.Application.Common.Interfaces;
using RepLoopBackend.Persistence.Entities;
using RepLoopBackend.Persistence.Services;

namespace RepLoopBackend.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(opts => opts.UseNpgsql(connectionString));

        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(opts =>
        {
            opts.Password.RequireDigit = true;
            opts.Password.RequiredLength = 8;
            opts.Password.RequireNonAlphanumeric = false;
            opts.Password.RequireUppercase = false;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}
