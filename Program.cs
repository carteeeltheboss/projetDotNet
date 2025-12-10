using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using AspNetCore.Identity.MongoDbCore.Models;

using SecureChat.Web.Hubs;
using SecureChat.Web.Infrastructure;
using SecureChat.Web.Models;

var builder = WebApplication.CreateBuilder(args);

// --- MongoDB Identity configuration ---
var mongoSettings = builder.Configuration.GetSection("Mongo")
    .Get<MongoDbSettings>() ?? throw new InvalidOperationException("Mongo settings missing");

var mongoIdentityConfig = new MongoDbIdentityConfiguration
{
    MongoDbSettings = mongoSettings,
    IdentityOptionsAction = options =>
    {
        options.SignIn.RequireConfirmedAccount = false; // dev mode
    }
};

builder.Services.ConfigureMongoDbIdentity<ApplicationUser, ApplicationRole, Guid>(mongoIdentityConfig)
    .AddDefaultTokenProviders()
    .AddDefaultUI();

// --- MVC/Pages/SignalR ---
builder.Services.AddRazorPages();
builder.Services.AddControllers();                // for /api/users/search
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, NameIdUserIdProvider>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
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
