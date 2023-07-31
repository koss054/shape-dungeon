using AutoFixture;
using FluentAssertions;
using Moq;
using ShapeDungeon.DTOs.Rooms;
using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Services.Rooms;
using ShapeDungeon.Specifications.Rooms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ShapeDungeon.Tests.ServiceTests.Rooms
{
    public class CheckRoomNeighborsServiceTests
    {
        private readonly Mock<IRoomRepository> _repoMock;
        private readonly Mock<IRoomValidateService> _validateServiceMock;
        private readonly ICheckRoomNeighborsService _sut;
        private readonly IFixture _fixture;

        public CheckRoomNeighborsServiceTests()
        {
            _repoMock = new Mock<IRoomRepository>();
            _validateServiceMock = new Mock<IRoomValidateService>();
            _sut = new CheckRoomNeighborsService(_repoMock.Object, _validateServiceMock.Object);
            _fixture = new Fixture();
        }

        [Theory]
        [MemberData(nameof(SetDtoNeighborsAsync_ValidItemsData))]
        public async Task SetDtoNeighborsAsync_ShouldSetRoomDtoNeighbors_WhenRoomWithProvidedCoordsIsInDb(
            int coordX, 
            int coordY, 
            bool canGoLeft,
            bool canGoRight,
            bool canGoUp,
            bool canGoDown,
            bool roomHasLeftNeighbor,
            bool roomCanEnterFromLeft,
            bool roomHasRightNeigbor,
            bool roomCanEnterFromRight,
            bool roomHasUpNeighbor,
            bool roomCanEnterFromUp,
            bool roomHasDownNeighbor,
            bool roomCanEnterFromDown,
            RoomNavDto expectedRoomDto)
        {
            // Arrange
            var expectedRoom = _fixture.Build<Room>()
                .With(x => x.CoordX, coordX)
                .With(x => x.CoordY, coordY)
                .With(x => x.CanGoLeft, canGoLeft)
                .With(x => x.CanGoRight, canGoRight)
                .With(x => x.CanGoUp, canGoUp)
                .With(x => x.CanGoDown, canGoDown)
                .Create();

            var baseSpecification = new RoomCoordsSpecification(coordX, coordY);

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomCoordsSpecification>()))
                .ReturnsAsync(expectedRoom);

            _repoMock
                .Setup(x => x.DoCoordsExistByAsync(new RoomCoordsSpecification(coordX - 1, coordY)))
                .ReturnsAsync(roomHasLeftNeighbor);

            _validateServiceMock
                .Setup(x => x.CanEnterRoomFromDirection(coordX - 1, coordY, RoomDirection.Left))
                .ReturnsAsync(roomCanEnterFromLeft);

            _repoMock
                .Setup(x => x.DoCoordsExistByAsync(new RoomCoordsSpecification(coordX + 1, coordY)))
                .ReturnsAsync(roomHasRightNeigbor);

            _validateServiceMock
                .Setup(x => x.CanEnterRoomFromDirection(coordX + 1, coordY, RoomDirection.Right))
                .ReturnsAsync(roomCanEnterFromRight);

            _repoMock
                .Setup(x => x.DoCoordsExistByAsync(new RoomCoordsSpecification(coordX, coordY + 1)))
                .ReturnsAsync(roomHasUpNeighbor);

            _validateServiceMock
                .Setup(x => x.CanEnterRoomFromDirection(coordX, coordY + 1, RoomDirection.Top))
                .ReturnsAsync(roomCanEnterFromUp);

            _repoMock
                .Setup(x => x.DoCoordsExistByAsync(new RoomCoordsSpecification(coordX, coordY - 1)))
                .ReturnsAsync(roomHasDownNeighbor);

            _validateServiceMock
                .Setup(x => x.CanEnterRoomFromDirection(coordX, coordY - 1, RoomDirection.Bottom))
                .ReturnsAsync(roomCanEnterFromDown);

            // Act
            RoomNavDto actualRoomDto = await _sut.SetDtoNeighborsAsync(coordX, coordY);

            // Assert
            actualRoomDto.HasLeftNeighbor.Should().Be(expectedRoomDto.HasLeftNeighbor);
            actualRoomDto.IsLeftDeadEnd.Should().Be(expectedRoomDto.IsLeftDeadEnd);
            actualRoomDto.HasRightNeighbor.Should().Be(expectedRoomDto.HasRightNeighbor);
            actualRoomDto.IsRightDeadEnd.Should().Be(expectedRoomDto.IsRightDeadEnd);
            actualRoomDto.HasUpNeighbor.Should().Be(expectedRoomDto.HasUpNeighbor);
            actualRoomDto.IsUpDeadEnd.Should().Be(expectedRoomDto.IsUpDeadEnd);
            actualRoomDto.HasDownNeighbor.Should().Be(expectedRoomDto.HasDownNeighbor);
            actualRoomDto.IsDownDeadEnd.Should().Be(expectedRoomDto.IsDownDeadEnd);
        }

        [Fact]
        public async Task SetDtoNeighborsAsync_ShouldThrowException_WhenNoRoomWithProvidedCoordsIsInDb()
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomCoordsSpecification>()))
                .ThrowsAsync(new ArgumentNullException("roomToReturn", "No room matches provided specification."));

            // Act
            var action = async () => await _sut.SetDtoNeighborsAsync(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No room matches provided specification. (Parameter 'roomToReturn')");
        }

        [Fact]
        public void SetHasNeighborsProperties_ShouldReturnMatchingRoomDto_WhenMethodIsCalled()
        {
            // Arrange
            var expectedRoomDto = _fixture.Build<RoomDto>()
                .Create();

            var expectedRoomNavDto = _fixture.Build<RoomNavDto>()
                .Create();

            // Act
            var actualRoomDto = _sut.SetHasNeighborsProperties(expectedRoomDto, expectedRoomNavDto);

            // Assert
            actualRoomDto.Should().BeOfType<RoomDto>();
            actualRoomDto.IsActiveForMove.Should().Be(expectedRoomDto.IsActiveForMove);
            actualRoomDto.IsActiveForScout.Should().Be(expectedRoomDto.IsActiveForScout);
            actualRoomDto.HasLeftNeighbor.Should().Be(expectedRoomNavDto.HasLeftNeighbor);
            actualRoomDto.HasRightNeighbor.Should().Be(expectedRoomNavDto.HasRightNeighbor);
            actualRoomDto.HasUpNeighbor.Should().Be(expectedRoomNavDto.HasUpNeighbor);
            actualRoomDto.HasDownNeighbor.Should().Be(expectedRoomNavDto.HasDownNeighbor);
            actualRoomDto.IsLeftDeadEnd.Should().Be(expectedRoomNavDto.IsLeftDeadEnd);
            actualRoomDto.IsRightDeadEnd.Should().Be(expectedRoomNavDto.IsRightDeadEnd);
            actualRoomDto.IsUpDeadEnd.Should().Be(expectedRoomNavDto.IsUpDeadEnd);
            actualRoomDto.IsDownDeadEnd.Should().Be(expectedRoomNavDto.IsDownDeadEnd);
        }

        public static IEnumerable<object[]> SetDtoNeighborsAsync_ValidItemsData
            => new List<object[]>
            {
                // Can go in any direction.
                new object[]
                {
                    0,              // Room Coord X
                    0,              // Room Coord Y
                    true,           // Can Go Left
                    true,           // Can Go Right
                    true,           // Can Go Up
                    true,           // Can Go Down
                    true,           // Room Has Left Neighbor
                    true,           // Room Can Enter From Left
                    true,           // Room Has Right Neighbor
                    true,           // Room Can Enter From Right
                    true,           // Room Has Up Neighbor
                    true,           // Room Can Enter From Up
                    true,           // Room Has Down Neighbor
                    true,           // Room Can Enter From Down
                    new RoomNavDto  // Expected Room Nav Dto
                    {
                        CanGoLeft = true,
                        IsLeftDeadEnd = false,
                        CanGoRight = true,
                        IsRightDeadEnd = false,
                        CanGoUp = true,
                        IsUpDeadEnd = false,
                        CanGoDown = true,
                        IsDownDeadEnd = false,
                    }
                },
                // Left is dead end. Everything else is available.
                new object[]
                {
                    20,             // Room Coord X
                    -30,            // Room Coord Y
                    true,           // Can Go Left
                    true,           // Can Go Right
                    true,           // Can Go Up
                    true,           // Can Go Down
                    true,           // Room Has Left Neighbor
                    false,          // Room Can Enter From Left
                    true,           // Room Has Right Neighbor
                    true,           // Room Can Enter From Right
                    true,           // Room Has Up Neighbor
                    true,           // Room Can Enter From Up
                    true,           // Room Has Down Neighbor
                    true,           // Room Can Enter From Down
                    new RoomNavDto  // Expected Room Nav Dto
                    {
                        CanGoLeft = true,
                        IsLeftDeadEnd = true,
                        CanGoRight = true,
                        IsRightDeadEnd = false,
                        CanGoUp = true,
                        IsUpDeadEnd = false,
                        CanGoDown = true,
                        IsDownDeadEnd = false,
                    }
                },
                // Left is dead end. Only up is available.
                new object[]
                {
                    20,             // Room Coord X
                    30,             // Room Coord Y
                    true,           // Can Go Left
                    false,          // Can Go Right
                    true,           // Can Go Up
                    false,          // Can Go Down
                    true,           // Room Has Left Neighbor
                    false,          // Room Can Enter From Left
                    true,           // Room Has Right Neighbor
                    false,          // Room Can Enter From Right
                    true,           // Room Has Up Neighbor
                    true,           // Room Can Enter From Up
                    false,          // Room Has Down Neighbor
                    false,          // Room Can Enter From Down
                    new RoomNavDto  // Expected Room Nav Dto
                    {
                        CanGoLeft = true,
                        IsLeftDeadEnd = true,
                        CanGoRight = false,
                        IsRightDeadEnd = false,
                        CanGoUp = true,
                        IsUpDeadEnd = false,
                        CanGoDown = false,
                        IsDownDeadEnd = false,
                    }
                },
                // Everything except down is dead end.
                new object[]
                {
                    120,            // Room Coord X
                    30,             // Room Coord Y
                    true,           // Can Go Left
                    true,           // Can Go Right
                    true,           // Can Go Up
                    true,           // Can Go Down
                    true,           // Room Has Left Neighbor
                    false,          // Room Can Enter From Left
                    true,           // Room Has Right Neighbor
                    false,          // Room Can Enter From Right
                    true,           // Room Has Up Neighbor
                    false,          // Room Can Enter From Up
                    true,           // Room Has Down Neighbor
                    true,           // Room Can Enter From Down
                    new RoomNavDto  // Expected Room Nav Dto
                    {
                        CanGoLeft = true,
                        IsLeftDeadEnd = true,
                        CanGoRight = true,
                        IsRightDeadEnd = true,
                        CanGoUp = true,
                        IsUpDeadEnd = true,
                        CanGoDown = true,
                        IsDownDeadEnd = false,
                    }
                },
                // Can go lett and right. No rooms up or down.
                new object[]
                {
                    25,             // Room Coord X
                    30,             // Room Coord Y
                    true,           // Can Go Left
                    true,           // Can Go Right
                    false,          // Can Go Up
                    false,          // Can Go Down
                    true,           // Room Has Left Neighbor
                    true,           // Room Can Enter From Left
                    true,           // Room Has Right Neighbor
                    true,           // Room Can Enter From Right
                    false,          // Room Has Up Neighbor
                    false,          // Room Can Enter From Up
                    false,          // Room Has Down Neighbor
                    false,          // Room Can Enter From Down
                    new RoomNavDto  // Expected Room Nav Dto
                    {
                        CanGoLeft = true,
                        IsLeftDeadEnd = false,
                        CanGoRight = true,
                        IsRightDeadEnd = false,
                        CanGoUp = false,
                        IsUpDeadEnd = false,
                        CanGoDown = false,
                        IsDownDeadEnd = false,
                    }
                },
                // Can go up and down. No rooms left or right.
                new object[]
                {
                    25,             // Room Coord X
                    42,             // Room Coord Y
                    false,          // Can Go Left
                    false,          // Can Go Right
                    true,           // Can Go Up
                    true,           // Can Go Down
                    false,          // Room Has Left Neighbor
                    false,          // Room Can Enter From Left
                    false,          // Room Has Right Neighbor
                    false,          // Room Can Enter From Right
                    true,           // Room Has Up Neighbor
                    true,           // Room Can Enter From Up
                    true,           // Room Has Down Neighbor
                    true,           // Room Can Enter From Down
                    new RoomNavDto  // Expected Room Nav Dto
                    {
                        CanGoLeft = false,
                        IsLeftDeadEnd = false,
                        CanGoRight = false,
                        IsRightDeadEnd = false,
                        CanGoUp = true,
                        IsUpDeadEnd = false,
                        CanGoDown = true,
                        IsDownDeadEnd = false,
                    }
                },
                // IMPOSSIBLE CASE - Walls on every side.
                new object[]
                {
                    64,             // Room Coord X
                    64,             // Room Coord Y
                    false,          // Can Go Left
                    false,          // Can Go Right
                    false,          // Can Go Up
                    false,          // Can Go Down
                    false,          // Room Has Left Neighbor
                    false,          // Room Can Enter From Left
                    false,          // Room Has Right Neighbor
                    false,          // Room Can Enter From Right
                    false,          // Room Has Up Neighbor
                    false,          // Room Can Enter From Up
                    true,           // Room Has Down Neighbor
                    true,           // Room Can Enter From Down
                    new RoomNavDto  // Expected Room Nav Dto
                    {
                        CanGoLeft = false,
                        IsLeftDeadEnd = false,
                        CanGoRight = false,
                        IsRightDeadEnd = false,
                        CanGoUp = false,
                        IsUpDeadEnd = false,
                        CanGoDown = false,
                        IsDownDeadEnd = false,
                    }
                },
                // IMPOSSIBLE CASE - Dead ends on every side.
                new object[]
                {
                    64,             // Room Coord X
                    64,             // Room Coord Y
                    true,           // Can Go Left
                    true,           // Can Go Right
                    true,           // Can Go Up
                    true,           // Can Go Down
                    true,           // Room Has Left Neighbor
                    false,          // Room Can Enter From Left
                    true,           // Room Has Right Neighbor
                    false,          // Room Can Enter From Right
                    true,           // Room Has Up Neighbor
                    false,          // Room Can Enter From Up
                    true,           // Room Has Down Neighbor
                    false,          // Room Can Enter From Down
                    new RoomNavDto  // Expected Room Nav Dto
                    {
                        CanGoLeft = true,
                        IsLeftDeadEnd = true,
                        CanGoRight = true,
                        IsRightDeadEnd = true,
                        CanGoUp = true,
                        IsUpDeadEnd = true,
                        CanGoDown = true,
                        IsDownDeadEnd = true,
                    }
                },
            };
    }
}
