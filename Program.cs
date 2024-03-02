using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.SignalR;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using Zencareservice;
using Zencareservice.Data;
using Zencareservice.Repository;

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var zencareserviceapp = builder.Build();
startup.Configure(zencareserviceapp, builder.Environment);


