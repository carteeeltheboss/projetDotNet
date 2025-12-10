using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;

using SecureChat.Web.Data;
using SecureChat.Web.Hubs;
using SecureChat.Web.Infrastructure;
using SecureChat.Web.Models;

var builder = WebApplication.CreateBuilder(args);

// --- DB (keeps your existing SQLite connection from appsettings.json) ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// --- Identity: register ONCE, using ApplicationUser ---
builder.Services
    .AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false; // dev mode
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

// --- MVC/Pages/SignalR ---
builder.Services.AddRazorPages();
builder.Services.AddControllers();                // for /api/users/search
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, NameIdUserIdProvider>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat");

// (dev helper) show current logged-in userId
app.MapGet("/whoami", (HttpContext ctx) =>
{
    var id = ctx.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "anonymous";
    return Results.Text(id);
}).RequireAuthorization();

app.Run();
