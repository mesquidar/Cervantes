using Cervantes.CORE;
using Cervantes.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Cervantes.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            try
            {
                //CreateHostBuilder(args).Build().Run();
                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var serviceProvider = scope.ServiceProvider;

                    try
                    {
                        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                        var vulnCategoryManager = serviceProvider.GetRequiredService<Contracts.IVulnCategoryManager>();
                        DataInitializer.SeedData(userManager, roleManager, vulnCategoryManager);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }

                host.Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
