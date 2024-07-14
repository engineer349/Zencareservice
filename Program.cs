using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using System.Data.SqlClient;
using System.Security.Claims;
using Zencareservice;
using Zencareservice.Data;
using Zencareservice.Models;
using Zencareservice.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;


//var builder = WebApplication.CreateBuilder(args);
//var startup = new Startup(builder.Configuration);
//startup.ConfigureServices(builder.Services);

//var app = builder.Build();
//var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
//var dbName = Environment.GetEnvironmentVariable("DB_NAME");
//var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
//var connectionString = $"Data Source{dbHost}; Inital Catalog ={dbName}; User ID=sa; Password={dbPassword}";

//startup.Configure(app, builder.Environment);

var builder = WebApplication.CreateBuilder(args);
var ConnectionStrings = builder.Configuration.GetConnectionString("ZencareserviceConnection");

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(3);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddControllersWithViews();
//builder.Services.AddControllersWithViews();
//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(ConnectionStrings));
//builder.Services.AddIdentity<AppUser, IdentityRole>(
//    options =>
//    {
//        options.Password.RequiredUniqueChars = 0;
//        options.Password.RequireUppercase = true;
//        options.Password.RequiredLength = 8;
//        options.Password.RequireNonAlphanumeric = true;
//        options.Password.RequireLowercase = true;
//    }
//    )
//    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddRazorPages();
builder.Services.AddDataProtection();
builder.Services.AddSignalR();
builder.Services.AddSingleton<DataAccess>();
builder.Services.AddSingleton<SqlDataAccess>();
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.HttpOnly = HttpOnlyPolicy.Always;
    options.Secure = CookieSecurePolicy.Always;
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        
        options.LoginPath = "/Account/PatientLogin"; 
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.LogoutPath = "/Account/Logout";

    })
    .AddGoogle(options =>
    {
        options.ClientId = "YOUR_GOOGLE_CLIENT_ID";
        options.ClientSecret = "YOUR_GOOGLE_CLIENT_SECRET";
        options.Scope.Add("email");
        options.Scope.Add("profile");
        options.SaveTokens = true;
        options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
    });
    builder.Services.AddAuthorization(
		options =>
		{
			options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
			options.AddPolicy("PatientPolicy", policy => policy.RequireRole("Patient"));
			options.AddPolicy("DoctorPolicy", policy => policy.RequireRole("Doctor"));
		}
		);
    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(10); 
    });
builder.Services.AddHttpClient();
builder.Services.AddSingleton<EmailVerifier>(provider =>
{
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    var apiKey = "a23bb3c665924806bbf7b19854e5a4ec";
    return new EmailVerifier(httpClientFactory, apiKey);
});
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
var connectionString = $"Data Source{dbHost}; Inital Catalog ={dbName}; User ID=sa; Password={dbPassword}";
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
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

