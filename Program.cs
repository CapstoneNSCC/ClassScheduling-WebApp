using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using ClassScheduling_WebApp.Data;
using dotenv.net;
DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

// Adds support for environment variables
builder.Configuration.AddEnvironmentVariables();

var connectionString = $"Server={"capstoneteam6db.mysql.database.azure.com"};" +
                    $"Database={"dbclassschedule"};" +
                    $"Uid={"DylanMac"};" +
                    $"Pwd={"Wer45Tgbvf"};" +
                    $"Port={"3306"};" +
                    $"SslMode=Required;" +
                    $"SslCa={"ssl/DigiCertGlobalRootCA.crt.pem"};";

// Adds services to the container.
builder.Services.AddControllersWithViews();

// Configures the EF Core DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Adds service for using in-memory cache
builder.Services.AddDistributedMemoryCache();

// Adds service for session
builder.Services.AddSession(options =>
{
    // Sets a short timeout for easy testing.
    options.IdleTimeout = TimeSpan.FromSeconds(1200);
    // Enables HTTP-only cookie
    options.Cookie.HttpOnly = true;
});

var app = builder.Build();

// Configures the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Maps the default route to the Login controller
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

// Enables the use of session in our app
app.UseSession();

app.Run();
