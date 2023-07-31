using AutoFixture;
using FluentAssertions;
using Moq;
using ShapeDungeon.Data;
using ShapeDungeon.DTOs.Rooms;
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
    public class RoomCreateServiceTests
    {
        private readonly Mock<IRoomRepository> _repoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IRoomCreateService _sut;
        private readonly IFixture _fixture;

        public RoomCreateServiceTests()
        {
            _repoMock = new Mock<IRoomRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _sut = new RoomCreateService(_repoMock.Object, _unitOfWorkMock.Object);
            _fixture = new Fixture();
        }

        [Theory]
        [MemberData(nameof(CreateAsync_ValidItemsData))]
        public async Task CreateAsync_ShouldCreateRoom_WhenProvidedWithDto(
            int initialCoordX, int initialCoordY, int expectedCoordX, int expectedCoordY, bool isStartRoom)
        {
            // Arrange
            var expectedRoomDto = _fixture.Build<RoomDetailsDto>()
                .With(x => x.IsStartRoom, isStartRoom)
                .With(x => x.CoordX, initialCoordX)
                .With(x => x.CoordY, initialCoordY)
                .Create();

            // Act
            var actualRoom = await _sut.CreateAsync(expectedRoomDto);

            // Assert
            actualRoom.IsStartRoom.Should().Be(isStartRoom);
            actualRoom.CoordX.Should().Be(expectedCoordX);
            actualRoom.CoordY.Should().Be(expectedCoordY);
        }

        [Theory]
        [MemberData(nameof(InitializeRoomAsync_ValidItemsData))]
        public async Task InitializeRoomAsync_ShouldInitializeRoom_WhenCoordSpecificationsAreMetAndExistingDirectionIsProvided(
            int initialCoordX,
            int initialCoordY,
            int expectedCoordX,
            int expectedCoordY,
            bool expectedCanGoLeft,
            bool expectedCanGoRight,
            bool expectedCanGoUp,
            bool expectedCanGoDown,
            RoomDirection expectedDirection)
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetCoordXByAsync(It.IsAny<RoomEditSpecification>()))
                .ReturnsAsync(initialCoordX);

            _repoMock
                .Setup(x => x.GetCoordYByAsync(It.IsAny<RoomEditSpecification>()))
                .ReturnsAsync(initialCoordY);

            // Act
            var actualRoom = await _sut.InitializeRoomAsync(expectedDirection);

            var actualCoordX = actualRoom.CoordX;
            var actualCoordY = actualRoom.CoordY;
            var actualCanGoLeft = actualRoom.CanGoLeft;
            var actualCanGoRight = actualRoom.CanGoRight;
            var actualCanGoUp = actualRoom.CanGoUp;
            var actualCanGoDown = actualRoom.CanGoDown;

            // Assert
            actualRoom.Should().BeOfType<RoomDetailsDto>();
            actualCoordX.Should().Be(expectedCoordX);
            actualCoordY.Should().Be(expectedCoordY);
            actualCanGoLeft.Should().Be(expectedCanGoLeft);
            actualCanGoRight.Should().Be(expectedCanGoRight);
            actualCanGoUp.Should().Be(expectedCanGoUp);
            actualCanGoDown.Should().Be(expectedCanGoDown);
        }

        [Fact]
        public async Task InitializeRoomAsync_ShouldThrowException_WhenNoActiveForEditRoomInDb()
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetCoordXByAsync(It.IsAny<RoomEditSpecification>()))
                .Throws<InvalidOperationException>();

            _repoMock
                .Setup(x => x.GetCoordYByAsync(It.IsAny<RoomEditSpecification>()))
                .Throws<InvalidOperationException>();

            // Act
            var action = async () => await _sut.InitializeRoomAsync(It.IsAny<RoomDirection>());

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task InitializeRoomAsync_ShouldThrowException_WhenInvalidRoomDirectionIsProvided()
        {
            // Arrange
            var expectedDirection = (RoomDirection)54;

            _repoMock
                .Setup(x => x.GetCoordXByAsync(It.IsAny<RoomEditSpecification>()))
                .ReturnsAsync(It.IsAny<int>());

            _repoMock
                .Setup(x => x.GetCoordYByAsync(It.IsAny<RoomEditSpecification>()))
                .ReturnsAsync(It.IsAny<int>());

            // Act
            var action = async () => await _sut.InitializeRoomAsync(expectedDirection);

            // Assert
            await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
                .WithMessage("Specified argument was out of the range of valid values. (Parameter 'roomDirection')");
        }

        [Theory]
        [MemberData(nameof(AreCoordsInUse_ValidItemsData))]
        public async Task AreCoordsInUse_ShouldReturnExpectedResult_WhenMethodIsCalled(
            bool expectedResult)
        {
            // Arrange
            _repoMock
                .Setup(x => x.DoCoordsExistByAsync(It.IsAny<RoomCoordsSpecification>()))
                .ReturnsAsync(expectedResult);

            // Act
            var actualResult = await _sut.AreCoordsInUse(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            actualResult.Should().Be(expectedResult);
        } 

        public static IEnumerable<object[]> CreateAsync_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    2000,   // Initial CoordX
                    -200,   // Initial CoordY
                    2000,   // Expected CoordX
                    -200,   // Expected CoordY
                    false,  // Is Start Room
                },
                new object[]
                {
                    2000,   // Initial CoordX
                    -200,   // Initial CoordY
                    0,      // Expected CoordX
                    0,      // Expected CoordY
                    true,   // Is Start Room
                },
            };

        public static IEnumerable<object[]> InitializeRoomAsync_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    0,                      // Initial CoordX
                    0,                      // Initial CoordY
                    -1,                     // Expected CoordX
                    0,                      // Expected CoordY
                    false,                  // Expected Can Go Left
                    true,                   // Expected Can Go Right
                    false,                  // Expected Can Go Up
                    false,                  // Expected Can Go Down
                    RoomDirection.Left,     // Exoected Initialization Direction
                },
                new object[]
                {
                    0,                      // Initial CoordX
                    0,                      // Initial CoordY
                    1,                      // Expected CoordX
                    0,                      // Expected CoordY
                    true,                   // Expected Can Go Left
                    false,                  // Expected Can Go Right
                    false,                  // Expected Can Go Up
                    false,                  // Expected Can Go Down
                    RoomDirection.Right,    // Exoected Initialization Direction
                },
                new object[]
                {
                    0,                      // Initial CoordX
                    0,                      // Initial CoordY
                    0,                      // Expected CoordX
                    1,                      // Expected CoordY
                    false,                  // Expected Can Go Left
                    false,                  // Expected Can Go Right
                    false,                  // Expected Can Go Up
                    true,                   // Expected Can Go Down
                    RoomDirection.Top,      // Exoected Initialization Direction
                },
                new object[]
                {
                    0,                      // Initial CoordX
                    0,                      // Initial CoordY
                    0,                      // Expected CoordX
                    -1,                     // Expected CoordY
                    false,                  // Expected Can Go Left
                    false,                  // Expected Can Go Right
                    true,                   // Expected Can Go Up
                    false,                  // Expected Can Go Down
                    RoomDirection.Bottom,   // Exoected Initialization Direction
                },
            };

        public static IEnumerable<object[]> AreCoordsInUse_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    true,   // Expected Are Coords in Use Value
                },
                new object[]
                {
                    false,  // Expected Are Coords in Use Value
                },
            };
    }
}
