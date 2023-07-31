using AutoFixture;
using FluentAssertions;
using ShapeDungeon.Data;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Repos;
using ShapeDungeon.Specifications;
using ShapeDungeon.Specifications.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ShapeDungeon.Tests.RepositoryTests
{
    // Not testing Update and AddAsync, since they are direct calls to EFC.
    public class RoomRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly IRoomRepository _sut;
        private readonly IFixture _fixture;

        public RoomRepositoryTests()
        {
            _context = ContextGenerator.Generate();
            _sut = new RoomRepository(_context);
            _fixture = new Fixture();
        }

        [Theory]
        [MemberData(nameof(GetMultipleByAsync_ValidItemsData))]
        public async Task GetMultipleByAsync_ShouldReturnRooms_WhenSpecificationMatchesEntitiesInDb(
            int expectedCount, 
            Guid predefinedId, 
            int[] coordsX, 
            int[] coordsY, 
            bool[] areActiveForEdit, 
            bool[] areActiveForMove, 
            bool[] areActiveForScout, 
            ISpecification<Room> specification)
        {
            // Arrange
            var rooms = new List<Room>();
            for (int i = 0; i < coordsX.Length; i++)
            {
                var id = Guid.NewGuid();
                if (i == 0) id = predefinedId;

                var room = _fixture.Build<Room>()
                    .With(x => x.Id, id)
                    .With(x => x.CoordX, coordsX[i])
                    .With(x => x.CoordY, coordsY[i])
                    .With(x => x.IsActiveForEdit, areActiveForEdit[i])
                    .With(x => x.IsActiveForMove, areActiveForMove[i])
                    .With(x => x.IsActiveForScout, areActiveForScout[i])
                    .Create();

                rooms.Add(room);
            }

            await _context.Rooms.AddRangeAsync(rooms);
            await _context.SaveChangesAsync();

            // Act
            var actualCombats = await _sut.GetMultipleByAsync(specification);

            // Assert
            actualCombats.Count().Should().Be(expectedCount);
        }

        [Theory]
        [MemberData(nameof(GetMultipleByAsync_InvalidItemsData))]
        public async Task GetMultipleByAsync_ShouldReturnEmptyList_WhenSpecificationDoesNotMatchEntitiesInDb(
            int[] coordsX,
            int[] coordsY,
            bool[] areActiveForEdit,
            bool[] areActiveForMove,
            bool[] areActiveForScout,
            ISpecification<Room> specification)
        {
            // Arrange
            var rooms = new List<Room>();
            for (int i = 0; i < coordsX.Length; i++)
            {
                var room = _fixture.Build<Room>()
                    .With(x => x.Id, Guid.NewGuid())
                    .With(x => x.CoordX, coordsX[i])
                    .With(x => x.CoordY, coordsY[i])
                    .With(x => x.IsActiveForEdit, areActiveForEdit[i])
                    .With(x => x.IsActiveForMove, areActiveForMove[i])
                    .With(x => x.IsActiveForScout, areActiveForScout[i])
                    .Create();

                rooms.Add(room);
            }

            await _context.Rooms.AddRangeAsync(rooms);
            await _context.SaveChangesAsync();

            // Act
            var actualCombats = await _sut.GetMultipleByAsync(specification);

            // Assert
            actualCombats.Count().Should().Be(0);
        }

        [Theory]
        [MemberData(nameof(GetFirstAsync_ValidItemsData))]
        public async Task GetFirstAsync_ShouldReturnRoom_WhenSpecificationMatchesEntityInDb(
            Guid id, 
            int coordX, 
            int coordY, 
            bool isActiveForEdit, 
            bool isActiveForMove, 
            bool isActiveForScout, 
            ISpecification<Room> specification)
        {
            // Arrange
            var expectedRoom = _fixture.Build<Room>()
                .With(x => x.Id, id)
                .With(x => x.CoordX, coordX)
                .With(x => x.CoordY, coordY)
                .With(x => x.IsActiveForEdit, isActiveForEdit)
                .With(x => x.IsActiveForMove, isActiveForMove)
                .With(x => x.IsActiveForScout, isActiveForScout)
                .Create();

            await _context.Rooms.AddAsync(expectedRoom);
            await _context.SaveChangesAsync();

            // Act
            var actualRoom = await _sut.GetFirstAsync(specification);

            // Assert
            actualRoom.Should().As<Room>();
            actualRoom.Should().BeEquivalentTo(expectedRoom);
        }

        [Theory]
        [MemberData(nameof(GetFirstAsync_InvalidItemsData))]
        public async Task GetFirstAsync_ShouldThrowException_WhenSpecificationDoesNotMatchEntityInDb(
            int coordX, 
            int coordY, 
            bool isActiveForEdit, 
            bool isActiveForMove, 
            bool isActiveForScout, 
            ISpecification<Room> specification)
        {
            // Arrange
            var expectedRoom = _fixture.Build<Room>()
                .With(x => x.Id, Guid.NewGuid())
                .With(x => x.CoordX, coordX)
                .With(x => x.CoordY, coordY)
                .With(x => x.IsActiveForEdit, isActiveForEdit)
                .With(x => x.IsActiveForMove, isActiveForMove)
                .With(x => x.IsActiveForScout, isActiveForScout)
                .Create();

            await _context.Rooms.AddAsync(expectedRoom);
            await _context.SaveChangesAsync();

            // Act
            var action = async () => await _sut.GetFirstAsync(specification);

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No room matches provided specification. (Parameter 'roomToReturn')");
        }

        [Theory]
        [MemberData(nameof(GetCoordByAsync_ValidItemsData))]
        public async Task GetCoordXByAsync_ShouldReturnCoordX_WhenSpecificationMatchesEntityInDb(
            Guid id,
            int coordX,
            int coordY,
            bool isActiveForEdit,
            bool isActiveForMove,
            bool isActiveForScout,
            ISpecification<Room> specification)
        {
            // Arrange
            var expectedRoom = _fixture.Build<Room>()
                .With(x => x.Id, id)
                .With(x => x.CoordX, coordX)
                .With(x => x.CoordY, coordY)
                .With(x => x.IsActiveForEdit, isActiveForEdit)
                .With(x => x.IsActiveForMove, isActiveForMove)
                .With(x => x.IsActiveForScout, isActiveForScout)
                .Create();

            await _context.Rooms.AddAsync(expectedRoom);
            await _context.SaveChangesAsync();

            // Act
            var actualCoordX = await _sut.GetCoordXByAsync(specification);

            // Assert
            actualCoordX.Should().As<int>();
            actualCoordX.Should().Be(coordX);
        }

        [Theory]
        [MemberData(nameof(GetCoordByAsync_InvalidItemsData))]
        public async Task GetCoordXByAsync_ShouldThrowException_WhenSpecificationDoesNotMatchEntityInDb(
            int coordX,
            int coordY,
            bool isActiveForEdit,
            bool isActiveForMove,
            bool isActiveForScout,
            ISpecification<Room> specification)
        {
            // Arrange
            var expectedRoom = _fixture.Build<Room>()
                .With(x => x.Id, Guid.NewGuid())
                .With(x => x.CoordX, coordX)
                .With(x => x.CoordY, coordY)
                .With(x => x.IsActiveForEdit, isActiveForEdit)
                .With(x => x.IsActiveForMove, isActiveForMove)
                .With(x => x.IsActiveForScout, isActiveForScout)
                .Create();

            await _context.Rooms.AddAsync(expectedRoom);
            await _context.SaveChangesAsync();

            // Act
            var action = async() => await _sut.GetCoordXByAsync(specification);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Theory]
        [MemberData(nameof(GetCoordByAsync_ValidItemsData))]
        public async Task GetCoordYByAsync_ShouldReturnCoordY_WhenSpecificationMatchesEntityInDb(
            Guid id,
            int coordX,
            int coordY,
            bool isActiveForEdit,
            bool isActiveForMove,
            bool isActiveForScout,
            ISpecification<Room> specification)
        {
            // Arrange
            var expectedRoom = _fixture.Build<Room>()
                .With(x => x.Id, id)
                .With(x => x.CoordX, coordX)
                .With(x => x.CoordY, coordY)
                .With(x => x.IsActiveForEdit, isActiveForEdit)
                .With(x => x.IsActiveForMove, isActiveForMove)
                .With(x => x.IsActiveForScout, isActiveForScout)
                .Create();

            await _context.Rooms.AddAsync(expectedRoom);
            await _context.SaveChangesAsync();

            // Act
            var actualCoordX = await _sut.GetCoordYByAsync(specification);

            // Assert
            actualCoordX.Should().As<int>();
            actualCoordX.Should().Be(coordY);
        }

        [Theory]
        [MemberData(nameof(GetCoordByAsync_InvalidItemsData))]
        public async Task GetCoordYByAsync_ShouldThrowException_WhenSpecificationDoesNotMatchEntityInDb(
            int coordX,
            int coordY,
            bool isActiveForEdit,
            bool isActiveForMove,
            bool isActiveForScout,
            ISpecification<Room> specification)
        {
            // Arrange
            var expectedRoom = _fixture.Build<Room>()
                .With(x => x.Id, Guid.NewGuid())
                .With(x => x.CoordX, coordX)
                .With(x => x.CoordY, coordY)
                .With(x => x.IsActiveForEdit, isActiveForEdit)
                .With(x => x.IsActiveForMove, isActiveForMove)
                .With(x => x.IsActiveForScout, isActiveForScout)
                .Create();

            await _context.Rooms.AddAsync(expectedRoom);
            await _context.SaveChangesAsync();

            // Act
            var action = async () => await _sut.GetCoordYByAsync(specification);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task DoCoordsExistByAsync_ShouldReturnTrue_WhenSpecificationMatchesEntityInDb()
        {
            // Arrange
            var coordX = 10;
            var coordY = 20;

            var room = _fixture.Build<Room>()
                .With(x => x.Id, Guid.NewGuid())
                .With(x => x.CoordX, coordX)
                .With(x => x.CoordY, coordY)
                .Create();

            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();

            // Act
            var actualBool = await _sut.DoCoordsExistByAsync(
                new RoomCoordsSpecification(coordX, coordY));

            // Assert
            actualBool.Should().BeTrue();
        }

        [Fact]
        public async Task DoCoordsExistByAsync_ShouldReturnFalse_WhenSpecificationDoesNotMatchEntityInDb()
        {
            // Arrange
            var coordX = 10;
            var coordY = 20;

            var room = _fixture.Build<Room>()
                .With(x => x.Id, Guid.NewGuid())
                .With(x => x.CoordX, coordX)
                .With(x => x.CoordY, coordY)
                .Create();

            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();

            // Act
            var actualBool = await _sut.DoCoordsExistByAsync(
                new RoomCoordsSpecification(coordX, coordY - 1));

            // Assert
            actualBool.Should().BeFalse();
        }

        public static IEnumerable<object[]> GetMultipleByAsync_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    1,                                                  // Expected Room Count
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),   // Room Id
                    new int[] { 1, 2, 3, 4, 5 },                        // Room CoordX
                    new int[] { 10, 20, 30, 40, 50 },                   // Room CoordY
                    new bool[] { true, false, false, false, false },    // Room IsActiveForEdit
                    new bool[] { false, true, false, false, false },    // Room IsActiveForMove
                    new bool[] { false, true, false, false, false },    // Room IsActiveForScout
                    new RoomCoordsSpecification(3, 30),                 // Repository Specification
                },
                new object[]
                {
                    1,                                                  // Expected Room Count
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),   // Room Id
                    new int[] { 1, 2, 3, 4, 5 },                        // Room CoordX
                    new int[] { 10, 20, 30, 40, 50 },                   // Room CoordY
                    new bool[] { true, false, false, false, false },    // Room IsActiveForEdit
                    new bool[] { false, true, false, false, false },    // Room IsActiveForMove
                    new bool[] { false, true, false, false, false },    // Room IsActiveForScout
                    new RoomEditSpecification(),                        // Repository Specification
                },
                new object[]
                {
                    1,                                                      // Expected Room Count
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),       // Room Id
                    new int[] { 1, 2, 3, 4, 5 },                            // Room CoordX
                    new int[] { 10, 20, 30, 40, 50 },                       // Room CoordY
                    new bool[] { true, false, false, false, false },        // Room IsActiveForEdit
                    new bool[] { false, true, false, false, false },        // Room IsActiveForMove
                    new bool[] { false, true, false, false, false },        // Room IsActiveForScout
                    new RoomIdSpecification(                                // Repository Specification
                        new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1")),  
                },
                new object[]
                {
                    1,                                                  // Expected Room Count
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),   // Room Id
                    new int[] { 1, 2, 3, 4, 5 },                        // Room CoordX
                    new int[] { 10, 20, 30, 40, 50 },                   // Room CoordY
                    new bool[] { true, false, false, false, false },    // Room IsActiveForEdit
                    new bool[] { false, true, false, false, false },    // Room IsActiveForMove
                    new bool[] { false, true, false, false, false },    // Room IsActiveForScout
                    new RoomMoveSpecification(),                        // Repository Specification
                },
                new object[]
                {
                    1,                                                  // Expected Room Count
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),   // Room Id
                    new int[] { 1, 2, 3, 4, 5 },                        // Room CoordX
                    new int[] { 10, 20, 30, 40, 50 },                   // Room CoordY
                    new bool[] { true, false, false, false, false },    // Room IsActiveForEdit
                    new bool[] { false, true, false, false, false },    // Room IsActiveForMove
                    new bool[] { false, true, false, false, false },    // Room IsActiveForScout
                    new RoomScoutSpecification(),                       // Repository Specification
                },
            };

        public static IEnumerable<object[]> GetMultipleByAsync_InvalidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    new int[] { 1, 2, 3, 4, 5 },                        // Room CoordX
                    new int[] { 10, 20, 30, 40, 50 },                   // Room CoordY
                    new bool[] { true, false, false, false, false },    // Room IsActiveForEdit
                    new bool[] { false, true, false, false, false },    // Room IsActiveForMove
                    new bool[] { false, true, false, false, false },    // Room IsActiveForScout
                    new RoomCoordsSpecification(3, 35),                 // Repository Specification
                },
                new object[]
                {
                    new int[] { 1, 2, 3, 4, 5 },                        // Room CoordX
                    new int[] { 10, 20, 30, 40, 50 },                   // Room CoordY
                    new bool[] { false, false, false, false, false },   // Room IsActiveForEdit
                    new bool[] { false, true, false, false, false },    // Room IsActiveForMove
                    new bool[] { false, true, false, false, false },    // Room IsActiveForScout
                    new RoomEditSpecification(),                        // Repository Specification
                },
                new object[]
                {
                    new int[] { 1, 2, 3, 4, 5 },                            // Room CoordX
                    new int[] { 10, 20, 30, 40, 50 },                       // Room CoordY
                    new bool[] { true, false, false, false, false },        // Room IsActiveForEdit
                    new bool[] { false, true, false, false, false },        // Room IsActiveForMove
                    new bool[] { false, true, false, false, false },        // Room IsActiveForScout
                    new RoomIdSpecification(Guid.NewGuid()),                // Repository Specification
                },
                new object[]
                {
                    new int[] { 1, 2, 3, 4, 5 },                        // Room CoordX
                    new int[] { 10, 20, 30, 40, 50 },                   // Room CoordY
                    new bool[] { true, false, false, false, false },    // Room IsActiveForEdit
                    new bool[] { false, false, false, false, false },   // Room IsActiveForMove
                    new bool[] { false, true, false, false, false },    // Room IsActiveForScout
                    new RoomMoveSpecification(),                        // Repository Specification
                },
                new object[]
                {
                    new int[] { 1, 2, 3, 4, 5 },                        // Room CoordX
                    new int[] { 10, 20, 30, 40, 50 },                   // Room CoordY
                    new bool[] { true, false, false, false, false },    // Room IsActiveForEdit
                    new bool[] { false, true, false, false, false },    // Room IsActiveForMove
                    new bool[] { false, false, false, false, false },   // Room IsActiveForScout
                    new RoomScoutSpecification(),                       // Repository Specification
                },
            };

        public static IEnumerable<object[]> GetFirstAsync_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),   // Room Id
                    10,                                                 // Room CoordX
                    10,                                                 // Room CoordY
                    true,                                               // Room IsActiveForEdit
                    true,                                               // Room IsActiveForMove
                    true,                                               // Room IsActiveForScout
                    new RoomCoordsSpecification(10, 10),                // Repository Specification
                },
                new object[]
                {
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),   // Room Id
                    64,                                                 // Room CoordX
                    10,                                                 // Room CoordY
                    true,                                               // Room IsActiveForEdit
                    false,                                              // Room IsActiveForMove
                    true,                                               // Room IsActiveForScout
                    new RoomEditSpecification(),                        // Repository Specification
                },
                new object[]
                {
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),       // Room Id
                    10,                                                     // Room CoordX
                    10,                                                     // Room CoordY
                    true,                                                   // Room IsActiveForEdit
                    true,                                                   // Room IsActiveForMove
                    true,                                                   // Room IsActiveForScout
                    new RoomIdSpecification(                                // Repository Specification
                        new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"))
                },
                new object[]
                {
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),   // Room Id
                    10,                                                 // Room CoordX
                    10,                                                 // Room CoordY
                    false,                                              // Room IsActiveForEdit
                    true,                                               // Room IsActiveForMove
                    false,                                              // Room IsActiveForScout
                    new RoomMoveSpecification(),                        // Repository Specification
                },
                new object[]
                {
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),   // Room Id
                    10,                                                 // Room CoordX
                    10,                                                 // Room CoordY
                    true,                                               // Room IsActiveForEdit
                    true,                                               // Room IsActiveForMove
                    true,                                               // Room IsActiveForScout
                    new RoomScoutSpecification(),                       // Repository Specification
                },
            };

        public static IEnumerable<object[]> GetFirstAsync_InvalidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    0,                                                  // Room CoordX
                    10,                                                 // Room CoordY
                    true,                                               // Room IsActiveForEdit
                    true,                                               // Room IsActiveForMove
                    true,                                               // Room IsActiveForScout
                    new RoomCoordsSpecification(10, 10),                // Repository Specification
                },
                new object[]
                {
                    64,                                                 // Room CoordX
                    10,                                                 // Room CoordY
                    false,                                              // Room IsActiveForEdit
                    false,                                              // Room IsActiveForMove
                    true,                                               // Room IsActiveForScout
                    new RoomEditSpecification(),                        // Repository Specification
                },
                new object[]
                {
                    10,                                                     // Room CoordX
                    10,                                                     // Room CoordY
                    true,                                                   // Room IsActiveForEdit
                    true,                                                   // Room IsActiveForMove
                    true,                                                   // Room IsActiveForScout
                    new RoomIdSpecification(                                // Repository Specification
                        new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"))
                },
                new object[]
                {
                    10,                                                 // Room CoordX
                    10,                                                 // Room CoordY
                    false,                                              // Room IsActiveForEdit
                    false,                                              // Room IsActiveForMove
                    false,                                              // Room IsActiveForScout
                    new RoomMoveSpecification(),                        // Repository Specification
                },
                new object[]
                {
                    10,                                                 // Room CoordX
                    10,                                                 // Room CoordY
                    false,                                              // Room IsActiveForEdit
                    true,                                               // Room IsActiveForMove
                    false,                                              // Room IsActiveForScout
                    new RoomScoutSpecification(),                       // Repository Specification
                },
            };

        public static IEnumerable<object[]> GetCoordByAsync_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),   // Room Id
                    10,                                                 // Room CoordX
                    10,                                                 // Room CoordY
                    true,                                               // Room IsActiveForEdit
                    true,                                               // Room IsActiveForMove
                    true,                                               // Room IsActiveForScout
                    new RoomCoordsSpecification(10, 10),                // Repository Specification
                },
                new object[]
                {
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),   // Room Id
                    64,                                                 // Room CoordX
                    10,                                                 // Room CoordY
                    true,                                               // Room IsActiveForEdit
                    false,                                              // Room IsActiveForMove
                    true,                                               // Room IsActiveForScout
                    new RoomEditSpecification(),                        // Repository Specification
                },
                new object[]
                {
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),       // Room Id
                    10,                                                     // Room CoordX
                    10,                                                     // Room CoordY
                    true,                                                   // Room IsActiveForEdit
                    true,                                                   // Room IsActiveForMove
                    true,                                                   // Room IsActiveForScout
                    new RoomIdSpecification(                                // Repository Specification
                        new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"))
                },
                new object[]
                {
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),   // Room Id
                    10,                                                 // Room CoordX
                    10,                                                 // Room CoordY
                    false,                                              // Room IsActiveForEdit
                    true,                                               // Room IsActiveForMove
                    false,                                              // Room IsActiveForScout
                    new RoomMoveSpecification(),                        // Repository Specification
                },
                new object[]
                {
                    new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"),   // Room Id
                    10,                                                 // Room CoordX
                    10,                                                 // Room CoordY
                    true,                                               // Room IsActiveForEdit
                    true,                                               // Room IsActiveForMove
                    true,                                               // Room IsActiveForScout
                    new RoomScoutSpecification(),                       // Repository Specification
                },
            };

        public static IEnumerable<object[]> GetCoordByAsync_InvalidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    0,                                                  // Room CoordX
                    10,                                                 // Room CoordY
                    true,                                               // Room IsActiveForEdit
                    true,                                               // Room IsActiveForMove
                    true,                                               // Room IsActiveForScout
                    new RoomCoordsSpecification(10, 10),                // Repository Specification
                },
                new object[]
                {
                    64,                                                 // Room CoordX
                    10,                                                 // Room CoordY
                    false,                                              // Room IsActiveForEdit
                    false,                                              // Room IsActiveForMove
                    true,                                               // Room IsActiveForScout
                    new RoomEditSpecification(),                        // Repository Specification
                },
                new object[]
                {
                    10,                                                     // Room CoordX
                    10,                                                     // Room CoordY
                    true,                                                   // Room IsActiveForEdit
                    true,                                                   // Room IsActiveForMove
                    true,                                                   // Room IsActiveForScout
                    new RoomIdSpecification(                                // Repository Specification
                        new Guid("FB29A8F7-CC9E-4CC9-8732-DA9BF7D2FAE1"))
                },
                new object[]
                {
                    10,                                                 // Room CoordX
                    10,                                                 // Room CoordY
                    false,                                              // Room IsActiveForEdit
                    false,                                              // Room IsActiveForMove
                    false,                                              // Room IsActiveForScout
                    new RoomMoveSpecification(),                        // Repository Specification
                },
                new object[]
                {
                    10,                                                 // Room CoordX
                    10,                                                 // Room CoordY
                    false,                                              // Room IsActiveForEdit
                    true,                                               // Room IsActiveForMove
                    false,                                              // Room IsActiveForScout
                    new RoomScoutSpecification(),                       // Repository Specification
                },
            };
    }
}
