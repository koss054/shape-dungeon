using AutoFixture;
using FluentAssertions;
using ShapeDungeon.Data;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Repos;
using ShapeDungeon.Specifications;
using ShapeDungeon.Specifications.EnemiesRooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ShapeDungeon.Tests.RepositoryTests
{
    // Not testing Update and AddAsync, since they are direct calls to EFC.
    public class EnemyRoomRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly IEnemyRoomRepository _sut;
        private readonly IFixture _fixture;

        public EnemyRoomRepositoryTests()
        {
            _context = ContextGenerator.Generate();
            _sut = new EnemyRoomRepository(_context);
            _fixture = new Fixture();
        }

        [Theory]
        [MemberData(nameof(GetMultipleByAsync_ValidItemsData))]
        public async Task GetMultipleByAsync_ShouldReturnEnemiesRooms_WhenSpecificationMatchesEntitiesInDb(
            int expectedCount, Guid predefinedEnemyId, Guid predefinedRoomId, bool[] areEnemiesDefeated, ISpecification<EnemyRoom> specification)
        {
            // Arrange
            var enemiesRooms = new List<EnemyRoom>();
            for (int i = 0; i < areEnemiesDefeated.Length; i++)
            {
                var enemyId = Guid.NewGuid();
                var roomId = Guid.NewGuid();

                if (i == 0) 
                {
                    enemyId = predefinedEnemyId;
                    roomId = predefinedRoomId; 
                }

                var enemy = _fixture.Build<Enemy>()
                .With(x => x.Id, enemyId)
                .Create();

                var room = _fixture.Build<Room>()
                    .With(x => x.Id, roomId)
                    .Create();

                var enemyRoom = _fixture.Build<EnemyRoom>()
                    .With(x => x.EnemyId, enemyId)
                    .With(x => x.Enemy, enemy)
                    .With(x => x.RoomId, roomId)
                    .With(x => x.Room, room)
                    .With(x => x.IsEnemyDefeated, areEnemiesDefeated[i])
                    .Create();

                enemiesRooms.Add(enemyRoom);
            }

            await _context.EnemiesRooms.AddRangeAsync(enemiesRooms);
            await _context.SaveChangesAsync();

            // Act
            var actualCombats = await _sut.GetMultipleByAsync(specification);

            // Assert
            actualCombats.Count().Should().Be(expectedCount);
        }

        [Theory]
        [MemberData(nameof(GetMultipleByAsync_InvalidItemsData))]
        public async Task GetMultipleByAsync_ShouldReturnEmptyList_WhenSpecificationDoesNotMatchEntitiesInDb(
            bool[] areEnemiesDefeated, ISpecification<EnemyRoom> specification)
        {
            // Arrange
            var enemiesRooms = new List<EnemyRoom>();
            for (int i = 0; i < areEnemiesDefeated.Length; i++)
            {
                var enemyId = Guid.NewGuid();
                var roomId = Guid.NewGuid();

                var enemy = _fixture.Build<Enemy>()
                .With(x => x.Id, enemyId)
                .Create();

                var room = _fixture.Build<Room>()
                    .With(x => x.Id, roomId)
                    .Create();

                var enemyRoom = _fixture.Build<EnemyRoom>()
                    .With(x => x.EnemyId, enemyId)
                    .With(x => x.Enemy, enemy)
                    .With(x => x.RoomId, roomId)
                    .With(x => x.Room, room)
                    .With(x => x.IsEnemyDefeated, areEnemiesDefeated[i])
                    .Create();

                enemiesRooms.Add(enemyRoom);
            }

            await _context.EnemiesRooms.AddRangeAsync(enemiesRooms);
            await _context.SaveChangesAsync();

            // Act
            var actualEnemies = await _sut.GetMultipleByAsync(specification);

            // Assert
            actualEnemies.Count().Should().Be(0);
        }

        [Theory]
        [MemberData(nameof(GetFirstAsync_ValidItemsData))]
        public async Task GetFirstAsync_ShouldReturnEnemyRoom_WhenSpecificationMatchesEntityInDb(
            Guid enemyId, Guid roomId, bool isEnemyDefeated, ISpecification<EnemyRoom> specification)
        {
            // Arrange
            var enemy = _fixture.Build<Enemy>()
                .With(x => x.Id, enemyId)
                .Create();

            var room = _fixture.Build<Room>()
                .With(x => x.Id, roomId)
                .Create();

            var expectedEnemyRoom = _fixture.Build<EnemyRoom>()
                .With(x => x.EnemyId, enemyId)
                .With(x => x.Enemy, enemy)
                .With(x => x.RoomId, roomId)
                .With(x => x.Room, room)
                .With(x => x.IsEnemyDefeated, isEnemyDefeated)
                .Create();

            await _context.EnemiesRooms.AddAsync(expectedEnemyRoom);
            await _context.SaveChangesAsync();

            // Act
            var actualEnemyRoom = await _sut.GetFirstAsync(specification);

            // Assert
            Assert.IsType<EnemyRoom>(actualEnemyRoom);
            Assert.Equal(expectedEnemyRoom, actualEnemyRoom);
        }

        [Theory]
        [MemberData(nameof(GetFirstAsync_InvalidItemsData))]
        public async Task GetFirstAsync_ShouldThrowException_WhenSpecificationDoesNotMatchEntityInDb(
            Guid enemyId, Guid roomId, bool isEnemyDefeated, ISpecification<EnemyRoom> specification)
        {
            // Arrange
            var enemy = _fixture.Build<Enemy>()
                .With(x => x.Id, enemyId)
                .Create();

            var room = _fixture.Build<Room>()
                .With(x => x.Id, roomId)
                .Create();

            var expectedEnemyRoom = _fixture.Build<EnemyRoom>()
                .With(x => x.EnemyId, enemyId)
                .With(x => x.Enemy, enemy)
                .With(x => x.RoomId, roomId)
                .With(x => x.Room, room)
                .With(x => x.IsEnemyDefeated, isEnemyDefeated)
                .Create();

            await _context.EnemiesRooms.AddAsync(expectedEnemyRoom);
            await _context.SaveChangesAsync();

            // Act
            var action = async () => await _sut.GetFirstAsync(specification);

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No enemy room matches provided specification. (Parameter 'enemyRoomToReturn')");
        }

        [Theory]
        [MemberData(nameof(IsValidByAsync_ValidItemsData))]
        public async Task IsValidByAsync_ShouldReturnTrue_WhenSpecificationMatchesEntityInDb(
            Guid enemyId, Guid roomId, bool isEnemyDefeated, ISpecification<EnemyRoom> specification)
        {
            // Arrange
            var enemy = _fixture.Build<Enemy>()
                .With(x => x.Id, enemyId)
                .Create();

            var room = _fixture.Build<Room>()
                .With(x => x.Id, roomId)
                .Create();

            var expectedEnemyRoom = _fixture.Build<EnemyRoom>()
                .With(x => x.EnemyId, enemyId)
                .With(x => x.Enemy, enemy)
                .With(x => x.RoomId, roomId)
                .With(x => x.Room, room)
                .With(x => x.IsEnemyDefeated, isEnemyDefeated)
                .Create();

            await _context.EnemiesRooms.AddAsync(expectedEnemyRoom);
            await _context.SaveChangesAsync();

            // Act
            var actualBool = await _sut.IsValidByAsync(specification);

            // Assert
            actualBool.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(IsValidByAsync_InvalidItemsData))]
        public async Task IsValidByAsync_ShouldReturnFalse_WhenSpecificationDoesNotMatchEntityInDb(
            Guid enemyId, Guid roomId, bool isEnemyDefeated, ISpecification<EnemyRoom> specification)
        {
            // Arrange
            var enemy = _fixture.Build<Enemy>()
                .With(x => x.Id, enemyId)
                .Create();

            var room = _fixture.Build<Room>()
                .With(x => x.Id, roomId)
                .Create();

            var expectedEnemyRoom = _fixture.Build<EnemyRoom>()
                .With(x => x.EnemyId, enemyId)
                .With(x => x.Enemy, enemy)
                .With(x => x.RoomId, roomId)
                .With(x => x.Room, room)
                .With(x => x.IsEnemyDefeated, isEnemyDefeated)
                .Create();

            await _context.EnemiesRooms.AddAsync(expectedEnemyRoom);
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
                    1,                                                      // Excpected Count
                    new Guid("68092619-772A-494D-BA83-1EEEDE3A4005"),       // Enemy Id
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),       // Room Id
                    new bool[]{ true, false, false },                       // Enemy Room IsEnemyDefeated
                    new EnemyRoomDefeatedSpecification(                     // Repository Specification
                        new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1")),
                },
                new object[]
                {
                    1,                                                      // Excpected Count
                    new Guid("68092619-772A-494D-BA83-1EEEDE3A4005"),       // Enemy Id
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),       // Room Id
                    new bool[]{ true, false, false },                       // Enemy Room IsEnemyDefeated
                    new EnemyRoomIdSpecification(                           // Repository Specification
                        new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1")),
                },
                new object[]
                {
                    1,                                                      // Excpected Count
                    new Guid("68092619-772A-494D-BA83-1EEEDE3A4005"),       // Enemy Id
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),       // Room Id
                    new bool[]{ true, false, false },                       // Enemy Room IsEnemyDefeated
                    new RoomEnemyIdSpecification(                           // Repository Specification
                        new Guid("68092619-772A-494D-BA83-1EEEDE3A4005")),
                },
            };

        public static IEnumerable<object[]> GetMultipleByAsync_InvalidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    new bool[]{ false, false, false },                      // Enemy Room IsEnemyDefeated
                    new EnemyRoomDefeatedSpecification(                     // Repository Specification
                        new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1")),
                },
                new object[]
                {
                    new bool[]{ true, false, false },                       // Enemy Room IsEnemyDefeated
                    new EnemyRoomIdSpecification(                           // Repository Specification
                        new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1")),
                },
                new object[]
                {
                    new bool[]{ true, false, false },                       // Enemy Room IsEnemyDefeated
                    new RoomEnemyIdSpecification(                           // Repository Specification
                        new Guid("68092619-772A-494D-BA83-1EEEDE3A4005")),
                },
            };

        public static IEnumerable<object[]> GetFirstAsync_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    new Guid("68092619-772A-494D-BA83-1EEEDE3A4005"),       // Enemy Id
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),       // Room Id
                    true,                                                   // Enemy Room IsEnemyDefeated
                    new EnemyRoomDefeatedSpecification(                     // Repository Specification
                        new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1")),  
                },
                new object[]
                {
                    new Guid("68092619-772A-494D-BA83-1EEEDE3A4005"),       // Enemy Id
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),       // Room Id
                    false,                                                  // Enemy Room IsEnemyDefeated
                    new EnemyRoomIdSpecification(                           // Repository Specification
                        new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1")),  
                },
                new object[]
                {
                    new Guid("68092619-772A-494D-BA83-1EEEDE3A4005"),       // Enemy Id
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),       // Room Id
                    true,                                                   // Enemy Room IsEnemyDefeated
                    new RoomEnemyIdSpecification(                           // Repository Specification
                        new Guid("68092619-772A-494D-BA83-1EEEDE3A4005")),  
                },
            };

        public static IEnumerable<object[]> GetFirstAsync_InvalidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    new Guid("68092619-772A-494D-BA83-1EEEDE3A4005"),       // Enemy Id
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),       // Room Id
                    false,                                                   // Enemy Room IsEnemyDefeated
                    new EnemyRoomDefeatedSpecification(                     // Repository Specification
                        new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1")),  
                },
                new object[]
                {
                    new Guid("68092619-772A-494D-BA83-1EEEDE3A4005"),       // Enemy Id
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),       // Room Id
                    false,                                                  // Enemy Room IsEnemyDefeated
                    new EnemyRoomIdSpecification(Guid.NewGuid()),           // Repository Specification
                },
                new object[]
                {
                    new Guid("68092619-772A-494D-BA83-1EEEDE3A4005"),       // Enemy Id
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),       // Room Id
                    true,                                                   // Enemy Room IsEnemyDefeated
                    new EnemyRoomIdSpecification(Guid.NewGuid()),           // Repository Specification
                },
            };

        public static IEnumerable<object[]> IsValidByAsync_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    new Guid("68092619-772A-494D-BA83-1EEEDE3A4005"),       // Enemy Id
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),       // Room Id
                    true,                                                   // Enemy Room IsEnemyDefeated
                    new EnemyRoomDefeatedSpecification(                     // Repository Specification
                        new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1")),
                },
                new object[]
                {
                    new Guid("68092619-772A-494D-BA83-1EEEDE3A4005"),       // Enemy Id
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),       // Room Id
                    false,                                                  // Enemy Room IsEnemyDefeated
                    new EnemyRoomIdSpecification(                           // Repository Specification
                        new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1")),
                },
                new object[]
                {
                    new Guid("68092619-772A-494D-BA83-1EEEDE3A4005"),       // Enemy Id
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),       // Room Id
                    true,                                                   // Enemy Room IsEnemyDefeated
                    new RoomEnemyIdSpecification(                           // Repository Specification
                        new Guid("68092619-772A-494D-BA83-1EEEDE3A4005")),
                },
            };

        public static IEnumerable<object[]> IsValidByAsync_InvalidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    new Guid("68092619-772A-494D-BA83-1EEEDE3A4005"),       // Enemy Id
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),       // Room Id
                    false,                                                   // Enemy Room IsEnemyDefeated
                    new EnemyRoomDefeatedSpecification(                     // Repository Specification
                        new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1")),
                },
                new object[]
                {
                    new Guid("68092619-772A-494D-BA83-1EEEDE3A4005"),       // Enemy Id
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),       // Room Id
                    false,                                                  // Enemy Room IsEnemyDefeated
                    new EnemyRoomIdSpecification(Guid.NewGuid()),           // Repository Specification
                },
                new object[]
                {
                    new Guid("68092619-772A-494D-BA83-1EEEDE3A4005"),       // Enemy Id
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),       // Room Id
                    true,                                                   // Enemy Room IsEnemyDefeated
                    new EnemyRoomIdSpecification(Guid.NewGuid()),           // Repository Specification
                },
            };
    }
}
