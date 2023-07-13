using ShapeDungeon.Data;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services;
using ShapeDungeon.Interfaces.Services.Enemies;
using ShapeDungeon.Interfaces.Services.EnemiesRooms;
using ShapeDungeon.Interfaces.Services.Players;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Middlewares;
using ShapeDungeon.Repos;
using ShapeDungeon.Services;
using ShapeDungeon.Services.Enemies;
using ShapeDungeon.Services.EnemiesRooms;
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
            services.AddScoped<IPlayerCreateService, PlayerCreateService>();
            services.AddScoped<IPlayerCombatService, PlayerCombatService>();
            services.AddScoped<IPlayerGetService, PlayerGetService>();
            services.AddScoped<IPlayerScoutService, PlayerScoutService>();
            services.AddScoped<IPlayerSelectService, PlayerSelectService>();
            services.AddScoped<IPlayerUpdateService, PlayerUpdateService>();

            // Enemy
            services.AddScoped<IEnemyCreateService, EnemyCreateService>();
            services.AddScoped<IEnemyGetService, EnemyGetService>();
            services.AddScoped<IEnemyUpdateService, EnemyUpdateService>();

            // Room
            services.AddScoped<IGetRoomService, GetRoomService>();
            services.AddScoped<IRoomEnemyService, RoomEnemyService>();
            services.AddScoped<IRoomCreateService, RoomCreateService>();
            services.AddScoped<IRoomTravelService, RoomTravelService>();
            services.AddScoped<IRoomActiveForEditService, RoomActiveForEditService>();
            services.AddScoped<ICheckRoomNeighborsService, CheckRoomNeighborsService>();
            services.AddScoped<IRoomConditionService, RoomConditionService>();
            services.AddScoped<IRoomValidateService, RoomValidateService>();

            // Enemy Room mapping entity
            services.AddScoped<IEnemiesRoomsService, EnemiesRoomsService>();

            // Repos
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRoomRepository, RoomRepositoryOld>();
            services.AddScoped<IEnemyRepository, EnemyRepository>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IEnemiesRoomsRepository, EnemiesRoomsRepository>();
            services.AddScoped<ICombatRepository, CombatRepository>();

            // New Room Repos
            services.AddScoped<IRepositoryGet<Room>, RoomRepository>();
            services.AddScoped<IRepositoryUpdate<Room>, RoomRepository>();
            services.AddScoped<IRepositoryCoordsGet<Room>, RoomRepository>();

            // Combat
            services.AddScoped<ICombatService, CombatService>();

            // Cookies
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Custom Middleware
            services.AddTransient<GlobalExceptionHandlingMiddleware>();

            return services;
        }
    }
}