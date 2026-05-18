using AutoMapper;
using Connectify.Data;
using Connectify.Hubs;
using Connectify.Interfaces;
using Connectify.Mappings;
using Connectify.Models;
using Connectify.Helpers;
using Connectify.Repositories;
using Connectify.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// MVC
builder.Services.AddControllersWithViews();


// SignalR
builder.Services.AddSignalR();

// Sigletons
builder.Services.AddSingleton<UserConnectionManager>();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));


// Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


// Cookie Config
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";

    options.AccessDeniedPath = "/Account/Login";
});


// AutoMapper
var mapperConfig = new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

IMapper mapper = mapperConfig.CreateMapper();

builder.Services.AddSingleton(mapper);


// Repository + Services
builder.Services.AddScoped<IChatRepository, ChatRepository>();

builder.Services.AddScoped<IChatService, ChatService>();


var app = builder.Build();


// Auto Migration
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    db.Database.Migrate();
}


// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();


// SignalR Hub
app.MapHub<ChatHub>("/chatHub");


// Routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();