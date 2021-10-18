using System;
using Microsoft.AspNetCore.Identity;
using Cervantes.CORE;
using Microsoft.EntityFrameworkCore;

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
            if (userManager.FindByEmailAsync("admin@cervantes.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "admin@cervantes.com";
                user.Email = "admin@cervantes.com";

                IdentityResult result = userManager.CreateAsync(user, "Admin123.").Result;

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


            if (!roleManager.RoleExistsAsync("SuperUser").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "SuperUser";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync("User").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "User";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync("Client").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Client";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }


    }
}

