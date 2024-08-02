using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Zencareservice.Data;
using Zencareservice.Models;
using Zencareservice.Repository;

var builder = WebApplication.CreateBuilder(args);

var connectionStrings = builder.Configuration.GetConnectionString("ZencareserviceConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionStrings));

builder.Services.AddTransient<OtpService>(provider => new OtpService(connectionStrings));
builder.Services.AddHttpClient();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDataProtection()
	.PersistKeysToFileSystem(new DirectoryInfo(@"C:\keys")) // Ensure this directory exists
	.SetApplicationName("Zencareservice");
builder.Services.AddSignalR();
builder.Services.AddSingleton<DataAccess>();
builder.Services.AddSingleton<SqlDataAccess>();
builder.Services.Configure<CookiePolicyOptions>(options =>
{
	options.MinimumSameSitePolicy = SameSiteMode.None;
	options.HttpOnly = HttpOnlyPolicy.Always;
	options.Secure = CookieSecurePolicy.Always;
});

builder.Configuration.AddUserSecrets<Program>();

var clientId = builder.Configuration["GoogleAuth:ClientId"];
var clientSecret = builder.Configuration["GoogleAuth:ClientSecret"];

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
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
	options.ClientId = clientId;
	options.ClientSecret = clientSecret;
	options.CallbackPath = new PathString("/Account/GoogleSignInCallback");
	options.SaveTokens = true;
	options.Events = new OAuthEvents
	{
		OnRedirectToAuthorizationEndpoint = context =>
		{
			context.Response.Redirect(context.RedirectUri);
			return Task.CompletedTask;
		}
	};
});

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("RequireAdminPatientDoctorRole", policy => policy.RequireRole("Admin", "Patient", "Doctor"));
});

builder.Services.AddSingleton<EmailVerifier>(provider =>
{
	var apiKey = builder.Configuration["EmailVerifier:ApiKey"];
	return new EmailVerifier(apiKey);
});

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILogger<Program>>();
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}
else
{
	app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
