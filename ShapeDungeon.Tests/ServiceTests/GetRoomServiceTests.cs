#nullable disable
using Moq;
using NUnit.Framework;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Repos;
using ShapeDungeon.Services.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeDungeon.Tests.ServiceTests
{
    internal class GetRoomServiceTests
    {
        private Mock<IRoomRepository> _repoMock;
        private IGetRoomService _service;

        [SetUp]
        public void Test_Initialize()
        {
            _repoMock = new Mock<IRoomRepository>();
            _service = new GetRoomService(_repoMock.Object);
        }

        // These tests can't return an empty room.
        // The start room will have the Move, Scout, Edit props set to true - DB is seeded with it.
        // Existing room properties cannot be updated.

        [Test]
        public async Task GetActiveForMove_HasMandatoryStartRoom_ReturnsActiveForMoveDto()
        {
            // Arrange
            var expectedCoordX = 10;
            var expectedCoordY = 123091;

            _repoMock
                .Setup(x => x.GetActiveForMove())
                .ReturnsAsync(new Room()
                {
                    IsActiveForMove = true,
                    CoordX = expectedCoordX,
                    CoordY = expectedCoordY
                });

            // Act
            var roomDto = await _service.GetActiveForMoveAsync();

            // Assert
            Assert.IsTrue(roomDto.IsActiveForMove);
            Assert.AreEqual(roomDto.CoordX, expectedCoordX);
            Assert.AreEqual(roomDto.CoordY, roomDto.CoordY);
        }

        [Test]
        public async Task GetActiveForScout_HasMandatoryStartRoom_ReturnsActiveForMoveDto()
        {
            // Arrange
            var expectedCoordX = 45645645;
            var expectedCoordY = -444456386;

            _repoMock
                .Setup(x => x.GetActiveForScout())
                .ReturnsAsync(new Room()
                {
                    IsActiveForScout = true,
                    CoordX = expectedCoordX,
                    CoordY = expectedCoordY
                });

            // Act
            var roomDto = await _service.GetActiveForScoutAsync();

            // Assert
            Assert.IsTrue(roomDto.IsActiveForScout);
            Assert.AreEqual(roomDto.CoordX, expectedCoordX);
            Assert.AreEqual(roomDto.CoordY, roomDto.CoordY);
        }

        [Test]
        public async Task GetActiveForEdit_HasMandatoryStartRoom_ReturnsActiveForMoveDto()
        {
            // Arrange
            var expectedCoordX = 4444;
            var expectedCoordY = 54;

            _repoMock
                .Setup(x => x.GetActiveForEdit())
                .ReturnsAsync(new Room()
                {
                    IsActiveForEdit = true,
                    CoordX = expectedCoordX,
                    CoordY = expectedCoordY
                });

            // Act
            var roomDto = await _service.GetActiveForEditAsync();

            // Assert
            Assert.IsTrue(roomDto.IsActiveForEdit);
            Assert.AreEqual(roomDto.CoordX, expectedCoordX);
            Assert.AreEqual(roomDto.CoordY, roomDto.CoordY);
        }
    }
}
