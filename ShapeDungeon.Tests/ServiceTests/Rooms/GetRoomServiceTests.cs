using AutoFixture;
using FluentAssertions;
using Moq;
using ShapeDungeon.DTOs.Rooms;
using ShapeDungeon.Entities;
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
    public class GetRoomServiceTests
    {
        private readonly Mock<IRoomRepository> _repoMock;
        private readonly IGetRoomService _sut;
        private readonly IFixture _fixture;

        public GetRoomServiceTests()
        {
            _repoMock = new Mock<IRoomRepository>();
            _sut = new GetRoomService(_repoMock.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetActiveForMoveAsync_ShouldReturnRoomDto_WhenActiveForMoveRoomisInDb()
        {
            // Arrange
            var expectedRoom = _fixture.Build<Room>()
                .With(x => x.IsActiveForMove, true)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomMoveSpecification>()))
                .ReturnsAsync(expectedRoom);

            // Act
            var actualRoomDto = await _sut.GetActiveForMoveDtoAsync();

            // Assert
            actualRoomDto.Should().BeOfType<RoomDto>();
            actualRoomDto.IsActiveForMove.Should().BeTrue();
            actualRoomDto.CoordX.Should().Be(expectedRoom.CoordX);
            actualRoomDto.CoordY.Should().Be(expectedRoom.CoordY);
        }

        [Fact]
        public async Task GetActiveForMoveAsync_ShouldThrowException_WhenNoActiveForMoveRoomInDb()
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomMoveSpecification>()))
                .ThrowsAsync(new ArgumentNullException("roomToReturn", "No room matches provided specification."));

            // Act
            var action = async () => await _sut.GetActiveForMoveDtoAsync();

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No room matches provided specification. (Parameter 'roomToReturn')");
        }

        [Fact]
        public async Task GetActiveForScoutAsync_ShouldReturnRoomDto_WhenActiveForScoutRoomInDb()
        {
            // Arrange
            var expectedRoom = _fixture.Build<Room>()
                .With(x => x.IsActiveForScout, true)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomScoutSpecification>()))
                .ReturnsAsync(expectedRoom);

            // Act
            var actualRoomDto = await _sut.GetActiveForScoutAsync();

            // Assert
            actualRoomDto.Should().BeOfType<RoomDto>();
            actualRoomDto.IsActiveForScout.Should().BeTrue();
            actualRoomDto.CoordX.Should().Be(expectedRoom.CoordX);
            actualRoomDto.CoordY.Should().Be(expectedRoom.CoordY);
        }

        [Fact]
        public async Task GetActiveForScoutAsync_ShouldThrowException_WhenNoActiveForScoutRoomInDb()
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomScoutSpecification>()))
                .ThrowsAsync(new ArgumentNullException("roomToReturn", "No room matches provided specification."));

            // Act
            var action = async () => await _sut.GetActiveForScoutAsync();

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No room matches provided specification. (Parameter 'roomToReturn')");
        }

        [Fact]
        public async Task GetActiveForEditAsync_ShouldReturnRoomDto_WhenActiveForEditRoomInDb()
        {
            // Arrange
            var expectedRoom = _fixture.Build<Room>()
                .With(x => x.IsActiveForEdit, true)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomEditSpecification>()))
                .ReturnsAsync(expectedRoom);

            // Act
            var actualRoomDto = await _sut.GetActiveForEditAsync();

            // Assert
            actualRoomDto.Should().BeOfType<RoomDetailsDto>();
            actualRoomDto.IsActiveForEdit.Should().BeTrue();
            actualRoomDto.CoordX.Should().Be(expectedRoom.CoordX);
            actualRoomDto.CoordY.Should().Be(expectedRoom.CoordY);
        }

        [Fact]
        public async Task GetActiveForEditAsync_ShouldThrowException_WhenNoActiveForEditRoomInDb()
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomEditSpecification>()))
                .ThrowsAsync(new ArgumentNullException("roomToReturn", "No room matches provided specification."));

            // Act
            var action = async () => await _sut.GetActiveForEditAsync();

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No room matches provided specification. (Parameter 'roomToReturn')");
        }

        [Fact]
        public async Task GetActiveForMoveId_ShouldThrowException_WhenNoActiveForMoveRoomInDb()
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomMoveSpecification>()))
                .ThrowsAsync(new ArgumentNullException("roomToReturn", "No room matches provided specification."));

            // Act
            var action = async () => await _sut.GetActiveForMoveId();

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No room matches provided specification. (Parameter 'roomToReturn')");
        }

        [Fact]
        public async Task GetActiveForScoutId_ShouldThrowException_WhenNoActiveForScoutRoomInDb()
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomScoutSpecification>()))
                .ThrowsAsync(new ArgumentNullException("roomToReturn", "No room matches provided specification."));

            // Act
            var action = async () => await _sut.GetActiveForScoutId();

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No room matches provided specification. (Parameter 'roomToReturn')");
        }

        [Fact]
        public async Task GetActiveForEditId_ShouldThrowException_WhenNoActiveForEditRoomInDb()
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomEditSpecification>()))
                .ThrowsAsync(new ArgumentNullException("roomToReturn", "No room matches provided specification."));

            // Act
            var action = async () => await _sut.GetActiveForEditId();

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No room matches provided specification. (Parameter 'roomToReturn')");
        }
    }
}
