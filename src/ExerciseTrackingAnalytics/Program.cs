using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;
using ExerciseTrackingAnalytics.BusinessLogic;
using ExerciseTrackingAnalytics.Data;
using ExerciseTrackingAnalytics.Data.Repositories;
using ExerciseTrackingAnalytics.Extensions;
using ExerciseTrackingAnalytics.Models.Identity;
using ExerciseTrackingAnalytics.Models.Strava;
using ExerciseTrackingAnalytics.Security.Authentication;
using ExerciseTrackingAnalytics.Security.Authentication.Strava;
using ExerciseTrackingAnalytics.Security.Authorization;
using ExerciseTrackingAnalytics.Security.DataProtection;
using ExerciseTrackingAnalytics.Services.Strava.API;
using ExerciseTrackingAnalytics.Services.Strava.ActivitySync;
using ExerciseTrackingAnalytics.Services.Time;
using StravaOAuth = ExerciseTrackingAnalytics.Security.Authentication.Strava.Constants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Logging
builder.Services.AddLogging();

Log.Logger = new LoggerConfiguration()
   .MinimumLevel.Debug()
   .WriteTo.Console()
   .CreateLogger();

builder.Host.UseSerilog();

// Database
var connectionStringBuilder = new NpgsqlConnectionStringBuilder(builder.Configuration.GetConnectionString("DefaultConnection"));
connectionStringBuilder.Password = builder.Configuration["ExerciseStatsDbPassword"];
var connectionString = connectionStringBuilder.ConnectionString;

if (string.IsNullOrEmpty(connectionString))
{
    Log.Logger.Error("Cannot start application server without connection string.");
    throw new InvalidOperationException("No connection string.");
}

Log.Logger.Debug("Using connection string: '{0}'", connectionString.MaskPassword());

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Data Protection (to encrypt user tokens)
builder.Services.AddDataProtection();
builder.Services.AddSingleton<KeyRing>();
builder.Services.AddSingleton<LookupProtector>();
builder.Services.Configure<DataProtectionKeyRingOptions>(options =>
{
    options.MasterKey = builder.Configuration["ApplicationDataProtectionMasterKey"];
});

// Identity
builder.Services
    .AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.Stores.ProtectPersonalData = true;
    })
    .AddRoles<ApplicationRole>()
    .AddRoleManager<ApplicationRoleManager>()
    .AddUserManager<ApplicationUserManager>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddPersonalDataProtection<LookupProtector, KeyRing>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationClaimsPrincipalFactory>();

// Strava OAuth
builder.Services
    .AddAuthentication()
    .AddOAuth<StravaOAuthOptions, StravaOAuthHandler>(
        StravaOAuth.AuthenticationScheme,
        StravaOAuth.DisplayName,
        (options) =>
        {
            options.ClientId = builder.Configuration["StravaClientId"];
            options.ClientSecret = builder.Configuration["StravaClientSecret"];
        });

// Strava API Service and its dependencies
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IStravaApiService, StravaApiService>();
builder.Services.AddScoped<IStravaActivitySyncService, StravaActivitySyncService>();

// Strava Webhook
builder.Services.Configure<StravaWebhookOptions>(options =>
{
    options.VerificationToken = builder.Configuration["StravaWebhookVerificationToken"];
});

// Database Repositories
builder.Services.AddScoped<IUserActivityRepository, UserActivityRepository>();

// Time Provider
builder.Services.AddSingleton<ITimeProvider, TimeProvider>();

// Business Logic
builder.Services.AddScoped<IActivityAggregateStatisticsBusinessLogic, ActivityAggregateStatisticsBusinessLogic>();

// ASP.NET MVC Stuff
builder.Services
    .AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
