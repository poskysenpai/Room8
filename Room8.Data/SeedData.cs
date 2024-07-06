using Microsoft.AspNetCore.Identity;
using Room8.Data.Context;
using Room8.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Room8.Data
{
    public class SeedData
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _applicationDbContext;

        public SeedData(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, AppDbContext applicationDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _applicationDbContext = applicationDbContext;
        }

        public async Task SeedAsync()
        {
            await SeedRolesAsync();
            await SeedAdminAsync();
        }

        private async Task SeedRolesAsync()
        {
            string[] roles = { "Admin", "User" };
            foreach (var roleName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private async Task SeedAdminAsync()
        {
            if (_userManager.Users.Any())
            {
                var adminUser = new User
                {
                    UserName = "adminprime@mailinator.com",
                    Email = "adminprime@mailinator.com",
                    FirstName = "Admin",
                    LastName = "Prime",
                    
                };

                await _userManager.CreateAsync(adminUser, "199026_Ll");


                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
