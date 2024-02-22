var builder = WebApplication.CreateBuilder(args);

// Adds support for environment variables
builder.Configuration.AddEnvironmentVariables();

// Configures the database connection string fully from environment variables
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                      $"Server={Environment.GetEnvironmentVariable("DB_SERVER") ?? "localhost"};" +
                      $"Database={Environment.GetEnvironmentVariable("MYSQL_DATABASE")};Uid={Environment.GetEnvironmentVariable("MYSQL_USER")};" +
                      $"Pwd={Environment.GetEnvironmentVariable("MYSQL_PASSWORD")};Port={Environment.GetEnvironmentVariable("DB_PORT") ?? "3306"};" +
                      $"SslMode=Required;";

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
