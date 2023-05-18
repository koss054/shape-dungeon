using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Data;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Services;
using ShapeDungeon.Services.Rooms;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IDbContext, AppDbContext>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IGetRoomService, GetRoomService>();
builder.Services.AddScoped<IRoomCreateService, RoomCreateService>();
builder.Services.AddScoped<ICheckRoomNeighborsService, CheckRoomNeighborsService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

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
