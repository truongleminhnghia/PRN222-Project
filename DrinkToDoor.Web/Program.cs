using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Business.Services;
using DrinkToDoor.Business.Services.BackgroundServices;
using DrinkToDoor.Data.Context;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;
using DrinkToDoor.Data.Interfaces;
using DrinkToDoor.Data.Repositories;
using DrinkToDoor.Web.Configurations;
using DrinkToDoor.Web.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Net.payOS;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddRazorPages()
       .AddRazorRuntimeCompilation();

var dbSection = builder.Configuration.GetSection("Database");
var server = dbSection["Server"];
var port = dbSection["Port"];
var database = dbSection["DataName"];
var user = dbSection["UserId"];
var password = dbSection["Password"];

var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? server;
var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? port;
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? database;
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? user;
var dbPass = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? password;

var connectionString =
    $"Server={dbHost};Port={dbPort};Database={dbName};User Id={dbUser};Password={dbPass};SslMode=Required;";

builder.Services.AddDbContext<DrinkToDoorDbContext>(options =>
{
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 31)),
        mySqlOptions =>
            mySqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(60),
                errorNumbersToAdd: null
            )
    );
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Login";
});

builder.Services.AddHttpContextAccessor();


builder.Services.AddProjectDependencies();
builder.Services.AddAutoMapperConfiguration();
builder.Services.AddHostedService<OrderStatusUpdater>();


// Add services to the container.
builder.Services.AddRazorPages().AddNToastNotifyNoty(new NotyOptions
{
    ProgressBar = true,
    Timeout = 5000,
    Theme = "mint",
    Layout = "topRight"
});

builder.Services.AddNotyf(options =>
{
    options.DurationInSeconds = 5;
    options.Position = NotyfPosition.TopRight;
});

var PayOS = builder.Configuration.GetSection("PAYOS");
var ClientId = PayOS["CLIENT_ID"];
var APILEY = PayOS["API_KEY"];
var CHECKSUMKEY = PayOS["CHECKSUM_KEY"];

PayOS payOS = new PayOS(ClientId,
                    APILEY,
                    CHECKSUMKEY);

builder.Services.AddSingleton(payOS);

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DrinkToDoorDbContext>();
    db.Database.Migrate();

    bool adminExists = db.Users.Any(u => u.RoleName == EnumRoleName.ROLE_ADMIN);

    if (!adminExists)
    {
        var adminUser = new User
        {
            Email = "admin@example.com",
            Password = BCrypt.Net.BCrypt.HashPassword("123456789"),
            RoleName = EnumRoleName.ROLE_ADMIN,
            EnumAccountStatus = EnumAccountStatus.ACTIVE
        };

        db.Users.Add(adminUser);
        db.SaveChanges();
    }
}

app.MapHub<IngredientHub>("/ingredientHub");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseNotyf();

app.UseNToastNotify();

app.MapRazorPages();

app.Run();
