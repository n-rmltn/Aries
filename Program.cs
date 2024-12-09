using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Aries.Data;
using Aries.Models;
using Aries.Filters;
using Aries.Middleware;
using Aries.Services.Implementations;
using Aries.Services.Interfaces;
using Aries.Repositories.Implementations;
using Aries.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection(nameof(DatabaseSettings)));

builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    var databaseSettings = builder.Configuration
        .GetSection(nameof(DatabaseSettings))
        .Get<DatabaseSettings>();

    options.UseMySql(
        databaseSettings!.ConnectionString,
        ServerVersion.AutoDetect(databaseSettings.ConnectionString),
        mysqlOptions =>
        {
            mysqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
        });
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllers(options => 
{
    options.Filters.Add<GlobalExceptionFilter>();
});

var app = builder.Build();

app.UseMiddleware<GlobalErrorHandlingMiddleware>();
app.UseAuthentication();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
