using AutoFixture;
using FluentAssertions;
using ShapeDungeon.Data;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Repos;
using ShapeDungeon.Specifications;
using ShapeDungeon.Specifications.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ShapeDungeon.Tests.RepositoryTests
{
    // Not testing Update and AddAsync, since they are direct calls to EFC.
    public class PlayerRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly IPlayerRepository _sut;
        private readonly IFixture _fixture;

        public PlayerRepositoryTests()
        {
            _context = ContextGenerator.Generate();
            _sut = new PlayerRepository(_context);
            _fixture = new Fixture();
        }

        [Theory]
        [MemberData(nameof(GetMultipleByAsync_ValidItemsData))]
        public async Task GetMultipleByAsync_ShouldReturnPlayers_WhenSpecificationMatchesEntitiesInDb(
            int expectedCount, Guid predefinedId, bool[] areActive, string[] names, ISpecification<Player> specification)
        {
            // Arrange
            var players = new List<Player>();
            for (int i = 0; i < areActive.Length; i++)
            {
                var id = Guid.NewGuid();
                if (i == 0) id = predefinedId;

                var player = _fixture.Build<Player>()
                    .With(x => x.Id, id)
                    .With(x => x.IsActive, areActive[i])
                    .With(x => x.Name, names[i])
                    .Create();

                players.Add(player);
            }

            await _context.Players.AddRangeAsync(players);
            await _context.SaveChangesAsync();

            // Act
            var actualPlayers = await _sut.GetMultipleByAsync(specification);

            // Assert
            actualPlayers.Count().Should().Be(expectedCount);
        }

        [Theory]
        [MemberData(nameof(GetMultipleByAsync_InvalidItemsData))]
        public async Task GetMultipleByAsync_ShouldReturnEmptyList_WhenSpecificationDoesNotMatchEntitiesInDb(
            bool[] areActive, string[] names, ISpecification<Player> specification, bool isAllSpecification = false)
        {
            // Arrange
            var players = new List<Player>();

            if (isAllSpecification == false)
            {
                for (int i = 0; i < areActive.Length; i++)
                {
                    var player = _fixture.Build<Player>()
                        .With(x => x.Id, Guid.NewGuid())
                        .With(x => x.IsActive, areActive[i])
                        .With(x => x.Name, names[i])
                        .Create();

                    players.Add(player);
                }
            }

            await _context.Players.AddRangeAsync(players);
            await _context.SaveChangesAsync();

            // Act
            var actualPlayers = await _sut.GetMultipleByAsync(specification);

            // Assert
            actualPlayers.Count().Should().Be(0);
        }

        [Theory]
        [MemberData(nameof(GetFirstAsync_ValidItemsData))]
        public async Task GetFirstAsync_ShouldReturnPlayer_WhenSpecificationMatchesEntityInDb(
            Guid id, bool isActive, string name, ISpecification<Player> specification)
        {
            // Arrange
            var expectedPlayer = _fixture.Build<Player>()
                .With(x => x.Id, id)
                .With(x => x.IsActive, isActive)
                .With(x => x.Name, name)
                .Create();

            await _context.Players.AddAsync(expectedPlayer);
            await _context.SaveChangesAsync();

            // Act
            var actualPlayer = await _sut.GetFirstAsync(specification);

            // Assert
            actualPlayer.Should().As<Combat>();
            actualPlayer.Should().BeEquivalentTo(expectedPlayer);
        }

        [Theory]
        [MemberData(nameof(GetFirstAsync_InvalidItemsData))]
        public async Task GetFirstAsync_ShouldThrowException_WhenSpecificationDoesNotMatchEntityInDb(
            Guid id, bool isActive, string name, ISpecification<Player> specification)
        {
            // Arrange
            var expectedPlayer = _fixture.Build<Player>()
                .With(x => x.Id, id)
                .With(x => x.IsActive, isActive)
                .With(x => x.Name, name)
                .Create();

            await _context.Players.AddAsync(expectedPlayer);
            await _context.SaveChangesAsync();

            // Act
            var action = async () => await _sut.GetFirstAsync(specification);

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No player matches provided specification. (Parameter 'playerToReturn')");
        }

        [Theory]
        [MemberData(nameof(IsValidByAsync_ValidItemsData))]
        public async Task IsValidByAsync_ShouldReturnTrue_WhenSpecificationMatchesEntityInDb(
            Guid id, bool isActive, string name, ISpecification<Player> specification)
        {
            // Arrange
            var player = _fixture.Build<Player>()
                .With(x => x.Id, id)
                .With(x => x.IsActive, isActive)
                .With(x => x.Name, name)
                .Create();

            await _context.Players.AddAsync(player);
            await _context.SaveChangesAsync();

            // Act
            var actualBool = await _sut.IsValidByAsync(specification);

            // Assert
            actualBool.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(IsValidByAsync_InvalidItemsData))]
        public async Task IsValidByAsync_ShouldReturnFalse_WhenSpecificationDoesNotMatchEntityInDb(
            Guid id, bool isActive, string name, ISpecification<Player> specification)
        {
            // Arrange
            var player = _fixture.Build<Player>()
                .With(x => x.Id, id)
                .With(x => x.IsActive, isActive)
                .With(x => x.Name, name)
                .Create();

            await _context.Players.AddAsync(player);
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
                    4,                                                      // Expected Player Count
                    new Guid("68092619-772A-494D-BA83-1EEEDE3A4005"),       // Player Id
                    new bool[] { true, false, true, false },                // Player IsActive
                    new string[] { "Viir", "Liam", "Timor", "Zephyra" },    // Player Name
                    new PlayerAllSpecification(),                           // Repository Specification
                },
                new object[]
                {
                    1,                                                      // Expected Player Count
                    new Guid("68092619-772A-494D-BA83-1EEEDE3A4005"),       // Player Id
                    new bool[] { true, false, true, false },                // Player IsActive
                    new string[] { "Viir", "Liam", "Timor", "Zephyra" },    // Player Name
                    new PlayerIdSpecification(                              // Repository Specification
                        new Guid("68092619-772A-494D-BA83-1EEEDE3A4005")),
                },
                new object[]
                {
                    2,                                                      // Expected Player Count
                    new Guid("68092619-772A-494D-BA83-1EEEDE3A4005"),       // Player Id
                    new bool[] { true, false, true, false },                // Player IsActive
                    new string[] { "Viir", "Liam", "Timor", "Zephyra" },    // Player Name
                    new PlayerIsActiveSpecification(),                      // Repository Specification
                },
                new object[]
                {
                    1,                                                      // Expected Player Count
                    new Guid("68092619-772A-494D-BA83-1EEEDE3A4005"),       // Player Id
                    new bool[] { true, false, true, false },                // Player IsActive
                    new string[] { "Viir", "Liam", "Timor", "Zephyra" },    // Player Name
                    new PlayerNameSpecification("Zephyra"),                 // Repository Specification
                },
            };

        public static IEnumerable<object[]> GetMultipleByAsync_InvalidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    new bool[] { true, false, true, false },                // Player IsActive
                    new string[] { "Viir", "Liam", "Timor", "Zephyra" },    // Player Name
                    new PlayerAllSpecification(),                           // Repository Specification
                    true,                                                   // Is Specification PlayerAllSpecification
                },
                new object[]
                {
                    new bool[] { true, false, true, false },                // Player IsActive
                    new string[] { "Viir", "Liam", "Timor", "Zephyra" },    // Player Name
                    new PlayerIdSpecification(                              // Repository Specification
                        new Guid("68092619-772A-494D-BA83-1EEEDE3A4005")),
                },
                new object[]
                {
                    new bool[] { false, false, false, false },              // Player IsActive
                    new string[] { "Viir", "Liam", "Timor", "Zephyra" },    // Player Name
                    new PlayerIsActiveSpecification(),                      // Repository Specification
                },
                new object[]
                {
                    new bool[] { false, false, true, false },               // Player IsActive
                    new string[] { "Viir", "Liam", "Timor", "Zephyra" },    // Player Name
                    new PlayerNameSpecification("Fred"),                    // Repository Specification
                },
            };

        public static IEnumerable<object[]> GetFirstAsync_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    Guid.NewGuid(),                 // Player Id
                    false,                          // Player IsActive
                    "Liam",                         // Player Name
                    new PlayerAllSpecification(),   // Repository Specification
                },
                new object[]
                {
                    new Guid("68092619-772A-494D-BA83-1EEEDE3A4005"),       // Player Id
                    false,                                                  // Player IsActive
                    "Viir",                                                 // Player Name
                    new PlayerIdSpecification(                              // Repository Specification
                        new Guid("68092619-772A-494D-BA83-1EEEDE3A4005")),                       
                },
                new object[]
                {
                    Guid.NewGuid(),                     // Player Id
                    true,                               // Player IsActive
                    "Bela",                             // Player Name
                    new PlayerIsActiveSpecification(),  // Repository Specification
                },
                new object[]
                {
                    Guid.NewGuid(),                         // Player Id
                    true,                                   // Player IsActive
                    "Terry",                                // Player Name
                    new PlayerNameSpecification("Terry"),   // Repository Specification
                },
            };

        public static IEnumerable<object[]> GetFirstAsync_InvalidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    Guid.NewGuid(),                                         // Player Id
                    false,                                                  // Player IsActive
                    "Viir",                                                 // Player Name
                    new PlayerIdSpecification(                              // Repository Specification
                        new Guid("68092619-772A-494D-BA83-1EEEDE3A4005")),
                },
                new object[]
                {
                    Guid.NewGuid(),                     // Player Id
                    false,                              // Player IsActive
                    "Bela",                             // Player Name
                    new PlayerIsActiveSpecification(),  // Repository Specification
                },
                new object[]
                {
                    Guid.NewGuid(),                         // Player Id
                    true,                                   // Player IsActive
                    "Terry",                                // Player Name
                    new PlayerNameSpecification("Valari"),  // Repository Specification
                },
            };

        public static IEnumerable<object[]> IsValidByAsync_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    Guid.NewGuid(),                 // Player Id
                    false,                          // Player IsActive
                    "Liam",                         // Player Name
                    new PlayerAllSpecification(),   // Repository Specification
                },
                new object[]
                {
                    new Guid("68092619-772A-494D-BA83-1EEEDE3A4005"),       // Player Id
                    false,                                                  // Player IsActive
                    "Viir",                                                 // Player Name
                    new PlayerIdSpecification(                              // Repository Specification
                        new Guid("68092619-772A-494D-BA83-1EEEDE3A4005")),
                },
                new object[]
                {
                    Guid.NewGuid(),                     // Player Id
                    true,                               // Player IsActive
                    "Bela",                             // Player Name
                    new PlayerIsActiveSpecification(),  // Repository Specification
                },
                new object[]
                {
                    Guid.NewGuid(),                         // Player Id
                    true,                                   // Player IsActive
                    "Terry",                                // Player Name
                    new PlayerNameSpecification("Terry"),   // Repository Specification
                },
            };

        public static IEnumerable<object[]> IsValidByAsync_InvalidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    Guid.NewGuid(),                                         // Player Id
                    false,                                                  // Player IsActive
                    "Viir",                                                 // Player Name
                    new PlayerIdSpecification(                              // Repository Specification
                        new Guid("68092619-772A-494D-BA83-1EEEDE3A4005")),
                },
                new object[]
                {
                    Guid.NewGuid(),                     // Player Id
                    false,                              // Player IsActive
                    "Bela",                             // Player Name
                    new PlayerIsActiveSpecification(),  // Repository Specification
                },
                new object[]
                {
                    Guid.NewGuid(),                         // Player Id
                    true,                                   // Player IsActive
                    "Terry",                                // Player Name
                    new PlayerNameSpecification("Valari"),  // Repository Specification
                },
            };
    }
}
