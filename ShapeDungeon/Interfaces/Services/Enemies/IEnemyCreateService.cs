using ShapeDungeon.DTOs.Enemies;

namespace ShapeDungeon.Interfaces.Services.Enemies
{
    public interface IEnemyCreateService
    {
        Task CreateAsync(EnemyDto eDto);
    }
}
