using AutoFixture;
using FluentAssertions;
using Moq;
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
    public class RoomValidateServiceTests
    {
        private readonly Mock<IRoomRepository> _repoMock;
        private readonly IRoomValidateService _sut;
        private readonly IFixture _fixture;

        public RoomValidateServiceTests()
        {
            _repoMock = new Mock<IRoomRepository>();
            _sut = new RoomValidateService(_repoMock.Object);
            _fixture = new Fixture();
        }

        [Theory]
        [MemberData(nameof(CanEnterRoomFromDirection_ValidItemsData))]
        public async Task CanEnterRoomFromDirection_ShouldReturnExpectedValues_WhenDirectionIsValidAndRoomIsInDb(
            bool expectedResult,
            bool expectedDoCoordsExist,
            bool expectedCanGoLeft,
            bool expectedCanGoRight,
            bool expectedCanGoUp,
            bool expectedCanGoDown,
            RoomDirection expectedDirection)
        {
            // Arrange
            var expectedRoom = _fixture.Build<Room>()
                .With(x => x.CanGoLeft, expectedCanGoLeft)
                .With(x => x.CanGoRight, expectedCanGoRight)
                .With(x => x.CanGoUp, expectedCanGoUp)
                .With(x => x.CanGoDown, expectedCanGoDown)
                .Create();

            _repoMock
                .Setup(x => x.DoCoordsExistByAsync(It.IsAny<RoomCoordsSpecification>()))
                .ReturnsAsync(expectedDoCoordsExist);

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomCoordsSpecification>()))
                .ReturnsAsync(expectedRoom);

            // Act
            var actualResult = await _sut.CanEnterRoomFromDirection(
                It.IsAny<int>(), It.IsAny<int>(), expectedDirection);

            // Assert
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public async Task CanEnterRoomFromDirection_ShouldThrowException_WhenNoRoomWithProvidedCoordsInDb()
        {
            // Arrange
            _repoMock
                .Setup(x => x.DoCoordsExistByAsync(It.IsAny<RoomCoordsSpecification>()))
                .ReturnsAsync(true);

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomCoordsSpecification>()))
                .ThrowsAsync(new ArgumentNullException("roomToReturn", "No room matches provided specification."));

            // Act
            var action = async () => await _sut.CanEnterRoomFromDirection(
                It.IsAny<int>(), It.IsAny<int>(), RoomDirection.Left);

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No room matches provided specification. (Parameter 'roomToReturn')");
        }

        [Fact]
        public async Task CanEnterRoomFromDirection_ShouldThrowException_WhenInvalidDirectionIsPassed()
        {
            // Arrange
            var expectedDirection = (RoomDirection)459;

            var expectedRoom = _fixture.Build<Room>()
                .Create();

            _repoMock
                .Setup(x => x.DoCoordsExistByAsync(It.IsAny<RoomCoordsSpecification>()))
                .ReturnsAsync(true);

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomCoordsSpecification>()))
                .ReturnsAsync(expectedRoom);

            // Act
            var action = async () => await _sut.CanEnterRoomFromDirection(
                It.IsAny<int>(), It.IsAny<int>(), expectedDirection);

            // Assert
            await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
                .WithMessage("Specified argument was out of the range of valid values. (Parameter 'direction')");
        }

        public static IEnumerable<object[]> CanEnterRoomFromDirection_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    true,               // Expected Result
                    true,               // Do Coords Exist
                    false,              // Expected Can Go Left
                    true,               // Expected Can Go Right
                    true,               // Expected Can Go Up
                    true,               // Expected Can Go Down
                    RoomDirection.Left  // Direction From The Room Is Being Exited
                },
                new object[]
                {
                    true,               // Expected Result
                    true,               // Do Coords Exist
                    true,               // Expected Can Go Left
                    true,               // Expected Can Go Right
                    true,               // Expected Can Go Up
                    true,               // Expected Can Go Down
                    RoomDirection.Right // Direction From The Room Is Being Exited
                },
                new object[]
                {
                    true,               // Expected Result
                    true,               // Do Coords Exist
                    false,              // Expected Can Go Left
                    false,              // Expected Can Go Right
                    false,              // Expected Can Go Up
                    true,               // Expected Can Go Down
                    RoomDirection.Top   // Direction From The Room Is Being Exited
                },
                new object[]
                {
                    true,                   // Expected Result
                    true,                   // Do Coords Exist
                    false,                  // Expected Can Go Left
                    true,                   // Expected Can Go Right
                    true,                   // Expected Can Go Up
                    true,                   // Expected Can Go Down
                    RoomDirection.Bottom    // Direction From The Room Is Being Exited
                },
                new object[]
                {
                    false,              // Expected Result
                    true,               // Do Coords Exist
                    false,              // Expected Can Go Left
                    false,              // Expected Can Go Right
                    true,               // Expected Can Go Up
                    true,               // Expected Can Go Down
                    RoomDirection.Left  // Direction From The Room Is Being Exited
                },
                new object[]
                {
                    false,              // Expected Result
                    true,               // Do Coords Exist
                    false,              // Expected Can Go Left
                    true,               // Expected Can Go Right
                    true,               // Expected Can Go Up
                    true,               // Expected Can Go Down
                    RoomDirection.Right // Direction From The Room Is Being Exited
                },
                new object[]
                {
                    false,              // Expected Result
                    true,               // Do Coords Exist
                    false,              // Expected Can Go Left
                    false,              // Expected Can Go Right
                    false,              // Expected Can Go Up
                    false,              // Expected Can Go Down
                    RoomDirection.Top   // Direction From The Room Is Being Exited
                },
                new object[]
                {
                    false,                  // Expected Result
                    true,                   // Do Coords Exist
                    true,                   // Expected Can Go Left
                    true,                   // Expected Can Go Right
                    false,                  // Expected Can Go Up
                    true,                   // Expected Can Go Down
                    RoomDirection.Bottom    // Direction From The Room Is Being Exited
                },
                new object[]
                {
                    false,                  // Expected Result
                    false,                  // Do Coords Exist
                    false,                  // Expected Can Go Left
                    true,                   // Expected Can Go Right
                    true,                   // Expected Can Go Up
                    true,                   // Expected Can Go Down
                    RoomDirection.Bottom    // Direction From The Room Is Being Exited
                },
            };
    }
}
