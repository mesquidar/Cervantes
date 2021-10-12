using System;
using Microsoft.AspNetCore.Identity;
using Cervantes.CORE;

namespace Cervantes.DAL
{
    public class DataInitializer
    {

        public static void SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByEmailAsync("admin").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "admin";
                user.Email = "admin";

                IdentityResult result = userManager.CreateAsync(user, "admin123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }

        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync("Registered").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Registered";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync("Professional").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Professional";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync("Business").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Business";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }
    }
}

