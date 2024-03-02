using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Extensions.Hosting;
using Zencareservice.Data;
using Zencareservice.Repository;

namespace Zencareservice
{
    public class Startup
    {

        public IConfiguration ConfigRoot { get; set; }

        public Startup(IConfiguration configuration)
        {
            ConfigRoot = configuration;
        }

        public void ConfigureServices(IServiceCollection Services)
        {
            
            Services.AddDistributedMemoryCache();
            Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(3);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //// Add services to the container.
            Services.AddControllersWithViews();
            Services.AddRazorPages();
            Services.AddDataProtection();
            Services.AddSignalR();
            /*DataAccess page addd */

            var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");

            var connectionString = $"Data Source{dbHost}; Inital Catalog ={dbName}; User ID=sa; Password={dbPassword}";
            Services.AddScoped<DataAccess>();
            Services.AddScoped<SqlDataAccess>();
            Services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.HttpOnly = HttpOnlyPolicy.Always;
                options.Secure = CookieSecurePolicy.Always;
            });

            Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    //options.Cookie.Name = "YourCookieName";
                    //options.DataProtectionProvider = DataProtectionProvider.Create(new DirectoryInfo(@"path-to-keys-directory"));
                    options.LoginPath = "/Account/Login"; // Redirect to login page if not authenticated
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    options.AccessDeniedPath = "/Account/AccessDenied"; // Redirect to access denied page if not authorized
                    options.LogoutPath = "/Account/Logout";

                });
            Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10); // Set the session timeout to 1 minute
            });
        }

        public void Configure(WebApplication zencareserviceapp, IWebHostEnvironment env)
        {
            if(!zencareserviceapp.Environment.IsDevelopment())
            {
                zencareserviceapp.UseExceptionHandler("/Error");
                zencareserviceapp.UseHsts();
            }
            zencareserviceapp.UseSession();
            zencareserviceapp.UseHttpsRedirection();
            zencareserviceapp.UseStaticFiles();
            zencareserviceapp.UseRouting();
            zencareserviceapp.UseAuthentication();
            zencareserviceapp.UseAuthorization();
            zencareserviceapp.MapRazorPages();

            zencareserviceapp.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");


            });
            zencareserviceapp.Run();
        }
  

    }
}
