using ShapeDungeon.Data;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Players;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Repos;
using ShapeDungeon.Services.Players;
using ShapeDungeon.Services.Rooms;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AppServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Db
            services.AddScoped<IDbContext, AppDbContext>();

            // Player
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IPlayerScoutService, PlayerScoutService>();

            // Room
            services.AddScoped<IGetRoomService, GetRoomService>();
            services.AddScoped<IRoomCreateService, RoomCreateService>();
            services.AddScoped<IRoomTravelService, RoomTravelService>();
            services.AddScoped<IRoomActiveForEditService, RoomActiveForEditService>();
            services.AddScoped<ICheckRoomNeighborsService, CheckRoomNeighborsService>();

            // Repos
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRoomRepository, RoomRepository>();

            return services;
        }
    }
}