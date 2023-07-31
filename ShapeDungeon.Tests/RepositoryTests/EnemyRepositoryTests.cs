using AutoFixture;
using FluentAssertions;
using ShapeDungeon.Data;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Repos;
using ShapeDungeon.Specifications;
using ShapeDungeon.Specifications.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ShapeDungeon.Tests.RepositoryTests
{
    // Not testing Update and AddAsync, since they are direct calls to EFC.
    public class EnemyRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly IEnemyRepository _sut;
        private readonly IFixture _fixture;

        public EnemyRepositoryTests()
        {
            _context = ContextGenerator.Generate();
            _sut = new EnemyRepository(_context);
            _fixture = new Fixture();
        }

        [Theory]
        [MemberData(nameof(GetMultipleByAsync_ValidItemsData))]
        public async Task GetMultipleByAsync_ShouldReturnEnemies_WhenSpecificationMatchesEntitiesInDb(
            int expectedCount, Guid predefinedId, int[] levels, bool[] areActiveForCombat, ISpecification<Enemy> specification)
        {
            // Arrange
            var enemies = new List<Enemy>();
            for (int i = 0; i < levels.Length; i++)
            {
                var id = Guid.NewGuid();
                if (i == 0) id = predefinedId;
                
                var enemy = _fixture.Build<Enemy>()
                    .With(x => x.Id, id)
                    .With(x => x.Level, levels[i])
                    .With(x => x.IsActiveForCombat, areActiveForCombat[i])
                    .Create();

                enemies.Add(enemy);
            }

            await _context.Enemies.AddRangeAsync(enemies);
            await _context.SaveChangesAsync();

            // Act
            var actualEnemies = await _sut.GetMultipleByAsync(specification);

            // Assert
            actualEnemies.Count().Should().Be(expectedCount);
        }

        [Theory]
        [MemberData(nameof(GetMultipleByAsync_InvalidItemsData))]
        public async Task GetMultipleByAsync_ShouldReturnEmptyList_WhenSpecificationDoesNotMatchEntitiesInDb(
            int[] levels, bool[] areActiveForCombat, ISpecification<Enemy> specification)
        {
            // Arrange
            var enemies = new List<Enemy>();
            for (int i = 0; i < levels.Length; i++)
            {
                var id = Guid.NewGuid();

                var enemy = _fixture.Build<Enemy>()
                    .With(x => x.Id, id)
                    .With(x => x.Level, levels[i])
                    .With(x => x.IsActiveForCombat, areActiveForCombat[i])
                    .Create();

                enemies.Add(enemy);
            }

            await _context.Enemies.AddRangeAsync(enemies);
            await _context.SaveChangesAsync();

            // Act
            var actualEnemies = await _sut.GetMultipleByAsync(specification);

            // Assert
            actualEnemies.Count().Should().Be(0);
        }

        [Theory]
        [MemberData(nameof(GetFirstAsync_ValidItemsData))]
        public async Task GetFirstAsync_ShouldReturnEnemy_WhenSpecificationMatchesEntityInDb(
            Guid id, int level, bool isActiveForCombat, ISpecification<Enemy> specification)
        {
            // Arrange
            var expectedEnemy = _fixture.Build<Enemy>()
                .With(x => x.Id, id)
                .With(x => x.Level, level)
                .With(x => x.IsActiveForCombat, isActiveForCombat)
                .Create();

            await _context.Enemies.AddAsync(expectedEnemy);
            await _context.SaveChangesAsync();

            // Act
            var actualEnemy = await _sut.GetFirstAsync(specification);

            // Assert
            actualEnemy.Should().As<Enemy>();
            actualEnemy.Should().BeEquivalentTo(expectedEnemy);
        }

        [Theory]
        [MemberData(nameof(GetFirstAsync_InvalidItemsData))]
        public async Task GetFirstAsync_ShouldThrowException_WhenSpecificationDoesNotMatchEntityInDb(
            Guid id, int level, bool isActiveForCombat, ISpecification<Enemy> specification)
        {
            // Arrange
            var expectedEnemy = _fixture.Build<Enemy>()
                .With(x => x.Id, id)
                .With(x => x.Level, level)
                .With(x => x.IsActiveForCombat, isActiveForCombat)
                .Create();

            await _context.Enemies.AddAsync(expectedEnemy);
            await _context.SaveChangesAsync();

            // Act
            var action = async () => await _sut.GetFirstAsync(specification);

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No enemy matches provided specification. (Parameter 'enemyToReturn')");
        }

        [Theory]
        [MemberData(nameof(IsValidByAsync_ValidItemsData))]
        public async Task IsValidByAsync_ShouldReturnTrue_WhenSpecificationMatchesEntityInDb(
            Guid id, int level, bool isActiveForCombat, ISpecification<Enemy> specification)
        {
            // Arrange
            var enemy = _fixture.Build<Enemy>()
                .With(x => x.Id, id)
                .With(x => x.Level, level)
                .With(x => x.IsActiveForCombat, isActiveForCombat)
                .Create();

            await _context.Enemies.AddAsync(enemy);
            await _context.SaveChangesAsync();

            // Act
            var actualBool = await _sut.IsValidByAsync(specification);

            // Assert
            actualBool.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(IsValidByAsync_InvalidItemsData))]
        public async Task IsValidByAsync_ShouldReturnFalse_WhenSpecificationDoesNotMatchEntityInDb(
            Guid id, int level, bool isActiveForCombat, ISpecification<Enemy> specification)
        {
            // Arrange
            var enemy = _fixture.Build<Enemy>()
                .With(x => x.Id, id)
                .With(x => x.Level, level)
                .With(x => x.IsActiveForCombat, isActiveForCombat)
                .Create();

            await _context.Enemies.AddAsync(enemy);
            await _context.SaveChangesAsync();

            // Act
            var actualBool = await _sut.IsValidByAsync(specification);

            // Assert
            actualBool.Should().BeFalse();
        }

        public static IEnumerable<object[]> GetMultipleByAsync_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    2,                                          // Expected Returned Enemies Count
                    Guid.NewGuid(),                             // Enemy Id
                    new int[] { 10, 20, 30 },                   // Enemy Level
                    new bool[] { true, true, false },           // Enemy IsActiveForCombat
                    new EnemyActiveForCombatSpecification(),    // Repository Specification
                },
                new object[]
                {
                    3,                                          // Expected Returned Enemies Count
                    Guid.NewGuid(),                             // Enemy Id
                    new int[] { 10, 20, 30, 40 },               // Enemy Level
                    new bool[] { true, true, false, false },    // Enemy IsActiveForCombat
                    new EnemyLevelRangeSpecification(5, 30),    // Repository Specification
                },
                new object[]
                {
                    1,                                                      // Expected Returned Enemies Count
                    new Guid("5C60F693-BEF5-E011-A485-80EE7300C695"),       // Enemy Id
                    new int[] { 10, 20, 30 },                               // Enemy Level
                    new bool[] { true, true, false },                       // Enemy IsActiveForCombat
                    new EnemyIdSpecification(                               // Repository Specification    
                        new Guid("5C60F693-BEF5-E011-A485-80EE7300C695")),
                },
            };

        public static IEnumerable<object[]> GetMultipleByAsync_InvalidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    new int[] { 10, 20, 30 },                   // Enemy Level
                    new bool[] { false, false, false },         // Enemy IsActiveForCombat
                    new EnemyActiveForCombatSpecification(),    // Repository Specification
                },
                new object[]
                {
                    new int[] { 4, 31, 35, 40 },                // Enemy Level
                    new bool[] { true, true, false, false },    // Enemy IsActiveForCombat
                    new EnemyLevelRangeSpecification(5, 30),    // Repository Specification
                },
                new object[]
                {
                    new int[] { 10, 20, 30 },                               // Enemy Level
                    new bool[] { true, true, false },                       // Enemy IsActiveForCombat
                    new EnemyIdSpecification(                               // Repository Specification    
                        new Guid("5C60F693-BEF5-E011-A485-80EE7300C695")),
                },
            };

        public static IEnumerable<object[]> GetFirstAsync_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    Guid.NewGuid(),                             // Enemy Id
                    10,                                         // Enemy Level
                    true,                                       // Enemy IsActiveForCombat
                    new EnemyActiveForCombatSpecification(),    // Repository Specification
                },
                new object[]
                {
                    Guid.NewGuid(),                             // Enemy Id
                    10,                                         // Enemy Level
                    true,                                       // Enemy IsActiveForCombat
                    new EnemyLevelRangeSpecification(5, 10),    // Repository Specification
                },
                new object[]
                {
                    new Guid("5C60F693-BEF5-E011-A485-80EE7300C695"),       // Enemy Id
                    10,                                                     // Enemy Level
                    true,                                                   // Enemy IsActiveForCombat
                    new EnemyIdSpecification(                               // Repository Specification    
                        new Guid("5C60F693-BEF5-E011-A485-80EE7300C695")), 
                },
            };

        public static IEnumerable<object[]> GetFirstAsync_InvalidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    Guid.NewGuid(),                             // Enemy Id
                    10,                                         // Enemy Level
                    false,                                      // Enemy IsActiveForCombat
                    new EnemyActiveForCombatSpecification(),    // Repository Specification
                },
                new object[]
                {
                    Guid.NewGuid(),                             // Enemy Id
                    11,                                         // Enemy Level
                    true,                                       // Enemy IsActiveForCombat
                    new EnemyLevelRangeSpecification(5, 10),    // Repository Specification
                },
                new object[]
                {
                    new Guid("5C60F693-BEF5-E011-A485-80EE7300C695"),   // Enemy Id
                    10,                                                 // Enemy Level
                    true,                                               // Enemy IsActiveForCombat
                    new EnemyIdSpecification(new Guid()),               // Repository Specification
                },
            };

        public static IEnumerable<object[]> IsValidByAsync_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    Guid.NewGuid(),                             // Enemy Id
                    10,                                         // Enemy Level
                    true,                                       // Enemy IsActiveForCombat
                    new EnemyActiveForCombatSpecification(),    // Repository Specification
                },
                new object[]
                {
                    Guid.NewGuid(),                             // Enemy Id
                    10,                                         // Enemy Level
                    true,                                       // Enemy IsActiveForCombat
                    new EnemyLevelRangeSpecification(5, 10),    // Repository Specification
                },
                new object[]
                {
                    new Guid("5C60F693-BEF5-E011-A485-80EE7300C695"),       // Enemy Id
                    10,                                                     // Enemy Level
                    true,                                                   // Enemy IsActiveForCombat
                    new EnemyIdSpecification(                               // Repository Specification    
                        new Guid("5C60F693-BEF5-E011-A485-80EE7300C695")),
                },
            };

        public static IEnumerable<object[]> IsValidByAsync_InvalidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    Guid.NewGuid(),                             // Enemy Id
                    10,                                         // Enemy Level
                    false,                                      // Enemy IsActiveForCombat
                    new EnemyActiveForCombatSpecification(),    // Repository Specification
                },
                new object[]
                {
                    Guid.NewGuid(),                             // Enemy Id
                    11,                                         // Enemy Level
                    true,                                       // Enemy IsActiveForCombat
                    new EnemyLevelRangeSpecification(5, 10),    // Repository Specification
                },
                new object[]
                {
                    new Guid("5C60F693-BEF5-E011-A485-80EE7300C695"),   // Enemy Id
                    10,                                                 // Enemy Level
                    true,                                               // Enemy IsActiveForCombat
                    new EnemyIdSpecification(new Guid()),               // Repository Specification
                },
            };
    }
}
