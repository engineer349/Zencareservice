using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Extensions.Hosting;
using Zencareservice.Data;
using Zencareservice.Repository;
using Microsoft.AspNetCore.Authentication.Google;

namespace Zencareservice
{
    public class Startup
    {

        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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


            Services.AddScoped<DataAccess>();
            Services.AddScoped<SqlDataAccess>();
            Services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.HttpOnly = HttpOnlyPolicy.Always;
                options.Secure = CookieSecurePolicy.Always;
            });

			//Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			//    .AddCookie(options =>
			//    {
			//        options.LoginPath = "/Account/Login"; 
			//        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
			//        options.AccessDeniedPath = "/Account/AccessDenied"; // Redirect to access denied page if not authorized
			//        options.LogoutPath = "/Account/Logout";


			//    });
			        Services.AddAuthentication(options =>
			        {
				        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;  
				        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				    
			        })
	        .AddCookie(options =>
	        {
		        options.LoginPath = "/Account/Login";
		        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
		        options.AccessDeniedPath = "/Account/AccessDenied";
		        options.LogoutPath = "/Account/Logout";
	        })
	        .AddGoogle(options =>
	        {
		        options.ClientId = "your-client-id";
		        options.ClientSecret = "your-client-secret";
		        options.CallbackPath = "/signin-google";
	        });
			        Services.AddSession(options =>
                    {
                        options.IdleTimeout = TimeSpan.FromMinutes(10); // Set the session timeout to 1 minute
                    });
                }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            //if(!app.Environment.IsDevelopment())
            //{
            //    app.UseExceptionHandler("/Error");
            //    app.UseHsts();
            //}
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Handle non-development environment errors
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // Middleware to handle 401 Unauthorized errors
            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 401)
                {
                    // Redirect to the custom 401 error page
                    context.Response.Redirect("/Home/401.html");
                }
              else  if (context.Response.StatusCode == 404)
                {
                    // Redirect to the custom 404 error page
                    context.Response.Redirect("/Home/404.html");
                }
                else if  (context.Response.StatusCode == 500)
                {
                    // Redirect to the custom 401 error page
                    context.Response.Redirect("/Home/500.html");
                }
            });
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapRazorPages();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");


            });
            app.Run();
        }
  

    }
}
