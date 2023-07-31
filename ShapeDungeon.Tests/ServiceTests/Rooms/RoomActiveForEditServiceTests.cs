using AutoFixture;
using FluentAssertions;
using Moq;
using ShapeDungeon.Data;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Services.Rooms;
using ShapeDungeon.Specifications.Rooms;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ShapeDungeon.Tests.ServiceTests.Rooms
{
    public class RoomActiveForEditServiceTests
    {
        private readonly Mock<IRoomRepository> _repoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IRoomActiveForEditService _sut;
        private readonly IFixture _fixture;

        public RoomActiveForEditServiceTests()
        {
            _repoMock = new Mock<IRoomRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _sut = new RoomActiveForEditService(_repoMock.Object, _unitOfWorkMock.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task ApplyActiveForEditAsync_ShouldToggleRoomsActiveForEditProperty_WhenThereIsActiveAndMatchingIdRoomsInDb()
        {
            // Arrange
            var expectedNewRoomId = Guid.NewGuid();

            var expectedOldRoom = _fixture.Build<Room>()
                .With(x => x.IsActiveForEdit, true)
                .Create();

            var expectedNewRoom = _fixture.Build<Room>()
                .With(x => x.Id, expectedNewRoomId)
                .With(x => x.IsActiveForEdit, false)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomEditSpecification>()))
                .ReturnsAsync(expectedOldRoom);

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomIdSpecification>()))
                .ReturnsAsync(expectedNewRoom);

            _repoMock.Object.Update(expectedOldRoom);
            _repoMock.Object.Update(expectedNewRoom);

            // Act
            await _sut.ApplyActiveForEditAsync(expectedNewRoomId);

            var actualOldRoomStatus = expectedOldRoom.IsActiveForEdit;
            var actualNewRoomStatus = expectedNewRoom.IsActiveForEdit;
            var actualNewRoomId = expectedNewRoom.Id;

            // Assert
            _repoMock.Verify(x => x.Update(expectedOldRoom), Times.Once);
            _repoMock.Verify(x => x.Update(expectedNewRoom), Times.Once);
            actualOldRoomStatus.Should().BeFalse();
            actualNewRoomStatus.Should().BeTrue();
            actualNewRoomId.Should().Be(expectedNewRoomId);
        }

        [Fact]
        public async Task ApplyActiveForEditAsync_ShouldThrowException_WhenNoActiveForEditRoomInDb()
        {
            // Arrange
            var expectedNewRoomId = Guid.NewGuid();
            var expectedNewRoom = _fixture.Build<Room>()
                .With(x => x.Id, expectedNewRoomId)
                .With(x => x.IsActiveForEdit, false)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomEditSpecification>()))
                .ThrowsAsync(new ArgumentNullException("roomToReturn", "No room matches provided specification."));

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomIdSpecification>()))
                .ReturnsAsync(expectedNewRoom);

            // Act
            var action = async () => await _sut.ApplyActiveForEditAsync(expectedNewRoomId);

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No room matches provided specification. (Parameter 'roomToReturn')");
            _repoMock.Verify(x => x.Update(expectedNewRoom), Times.Never);
        }

        [Fact]
        public async Task ApplyActiveForEditAsync_ShouldThrowException_WhenNoRoomWithProvidedIdInDb()
        {
            // Arrange
            var expectedOldRoom = _fixture.Build<Room>()
                .With(x => x.IsActiveForEdit, true)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomEditSpecification>()))
                .ReturnsAsync(expectedOldRoom);

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomIdSpecification>()))
                .ThrowsAsync(new ArgumentNullException("roomToReturn", "No room matches provided specification."));
            // Act
            var action = async () => await _sut.ApplyActiveForEditAsync(It.IsAny<Guid>());

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No room matches provided specification. (Parameter 'roomToReturn')");
            _repoMock.Verify(x => x.Update(expectedOldRoom), Times.Never);
        }

        [Fact]
        public async Task MoveActiveForEditAsync_ShouldToggleRoomsActiveForEditProperty_WhenThereIsActiveAndMatchingCoordsRoomsInDb()
        {
            // Arrange
            var expectedCoordX = 54;
            var expectedCoordY = 64;

            var expectedOldRoom = _fixture.Build<Room>()
                .With(x => x.IsActiveForEdit, true)
                .Create();

            var expectedNewRoom = _fixture.Build<Room>()
                .With(x => x.CoordX, expectedCoordX)
                .With(x => x.CoordY, expectedCoordY)
                .With(x => x.IsActiveForEdit, false)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomEditSpecification>()))
                .ReturnsAsync(expectedOldRoom);

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomCoordsSpecification>()))
                .ReturnsAsync(expectedNewRoom);

            _repoMock.Object.Update(expectedOldRoom);
            _repoMock.Object.Update(expectedNewRoom);

            // Act
            await _sut.MoveActiveForEditAsync(expectedCoordX, expectedCoordY);

            var actualOldRoomStatus = expectedOldRoom.IsActiveForEdit;
            var actualNewRoomStatus = expectedNewRoom.IsActiveForEdit;
            var actualNewRoomCoordX = expectedNewRoom.CoordX;
            var actualNewRoomCoordY = expectedNewRoom.CoordY;

            // Assert
            _repoMock.Verify(x => x.Update(expectedOldRoom), Times.Once);
            _repoMock.Verify(x => x.Update(expectedNewRoom), Times.Once);
            actualOldRoomStatus.Should().BeFalse();
            actualNewRoomStatus.Should().BeTrue();
            actualNewRoomCoordX.Should().Be(expectedCoordX);
            actualNewRoomCoordY.Should().Be(expectedCoordY);
        }

        [Fact]
        public async Task MoveActiveForEditAsync_ShouldThrowException_WhenNoActiveForEditRoomInDb()
        {
            // Arrange
            var expectedCoordX = 10;
            var expectedCoordY = 10;

            var expectedNewRoom = _fixture.Build<Room>()
                .With(x => x.CoordX, expectedCoordX)
                .With(x => x.CoordY, expectedCoordY)
                .With(x => x.IsActiveForEdit, false)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomEditSpecification>()))
                .ThrowsAsync(new ArgumentNullException("roomToReturn", "No room matches provided specification."));

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomIdSpecification>()))
                .ReturnsAsync(expectedNewRoom);

            // Act
            var action = async () => await _sut.MoveActiveForEditAsync(expectedCoordX, expectedCoordY);

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No room matches provided specification. (Parameter 'roomToReturn')");
            _repoMock.Verify(x => x.Update(expectedNewRoom), Times.Never);
        }

        [Fact]
        public async Task MoveActiveForEditAsync_ShouldThrowException_WhenNoRoomWithProvidedCoordsInDb()
        {
            // Arrange
            var expectedOldRoom = _fixture.Build<Room>()
                .With(x => x.IsActiveForEdit, true)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomEditSpecification>()))
                .ReturnsAsync(expectedOldRoom);

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomCoordsSpecification>()))
                .ThrowsAsync(new ArgumentNullException("roomToReturn", "No room matches provided specification."));
            // Act
            var action = async () => await _sut.MoveActiveForEditAsync(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No room matches provided specification. (Parameter 'roomToReturn')");
            _repoMock.Verify(x => x.Update(expectedOldRoom), Times.Never);
        }
    }
}
