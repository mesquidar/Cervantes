using Cervantes.Application;
using Cervantes.Contracts;
using Cervantes.CORE;
using Cervantes.DAL;
using Cervantes.Web.LocalizationResources;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Globalization;

namespace Cervantes.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // Add the localization services to the services container
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter());
            });

            services.AddRazorPages();



            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //.AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IRoleManager, RoleManager>();
            services.AddScoped<IClientManager, ClientManager>();
            services.AddScoped<IProjectManager, ProjectManager>();
            services.AddScoped<IOrganizationManager, OrganizationManager>();
            services.AddScoped<IProjectUserManager, ProjectUserManager>();
            services.AddScoped<IProjectNoteManager, ProjectNoteManager>();
            services.AddScoped<IProjectAttachmentManager, ProjectAttachmentManager>();
            services.AddScoped<ITargetManager, TargetManager>();
            services.AddScoped<ITaskManager, TaskManager>();
            services.AddScoped<IVulnManager, VulnManager>();
            services.AddScoped<IVulnCategoryManager, VulnCategoryManager>();
            services.AddScoped<IDocumentManager, DocumentManager>();



            var cultures = new[]
             {
                new CultureInfo("en"),
                new CultureInfo("es"),
            };

            services.AddControllersWithViews()
                .AddExpressLocalization<ExpressLocalizationResource, ViewLocalizationResource>(ops =>
                {
                    // When using all the culture providers, the localization process will
                    // check all available culture providers in order to detect the request culture.
                    // If the request culture is found it will stop checking and do localization accordingly.
                    // If the request culture is not found it will check the next provider by order.
                    // If no culture is detected the default culture will be used.

                    // Checking order for request culture:
                    // 1) RouteSegmentCultureProvider
                    //      e.g. http://localhost:1234/tr
                    // 2) QueryStringCultureProvider
                    //      e.g. http://localhost:1234/?culture=tr
                    // 3) CookieCultureProvider
                    //      Determines the culture information for a request via the value of a cookie.
                    // 4) AcceptedLanguageHeaderRequestCultureProvider
                    //      Determines the culture information for a request via the value of the Accept-Language header.
                    //      See the browsers language settings

                    // Uncomment and set to true to use only route culture provider
                    ops.UseAllCultureProviders = false;
                    ops.ResourcesPath = "LocalizationResources";
                    ops.RequestLocalizationOptions = o =>
                    {
                        o.SupportedCultures = cultures;
                        o.SupportedUICultures = cultures;
                        o.DefaultRequestCulture = new RequestCulture("en");
                    };
                }); ;


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRequestLocalization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{culture=en}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();

                endpoints.MapAreaControllerRoute(
                 name: "Workspace",
                 areaName: "Workspace",
                 pattern: "{culture=en}/Workspace/{project}/{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}
