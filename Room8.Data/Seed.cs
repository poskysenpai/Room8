using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Room8.Data.Context;
using Room8.Domain.Entities;

namespace Room8.Data;

public static class Seed
{
    public static void EnsurePopulated(IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            if (dbContext.Database.GetPendingMigrations().Any())
            {
                Console.WriteLine("Applying Migrations.....");
               // dbContext.Database.Migrate();
                Console.WriteLine("Migrations Applied.");
            }
        }
    }
}
