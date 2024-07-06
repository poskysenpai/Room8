using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Room8.Data.Context;
using Room8.Domain.Entities;

namespace Room8.API.Extensions;


public static class DbRegistration
{
    public static void AddDbServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                optionsBuilder =>
                {
                    optionsBuilder.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name);
                }));

        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
    }
}
