#nullable disable
using Moq;
using NUnit.Framework;
using ShapeDungeon.Data;
using ShapeDungeon.DTOs.Rooms;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Repos;
using ShapeDungeon.Services.Rooms;
using System.Threading.Tasks;

namespace ShapeDungeon.Tests.ServiceTests
{
    internal class RoomCreateServiceTests
    {
        private Mock<IRoomRepository> _repoMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private IRoomCreateService _service;

        [SetUp]
        public void Test_Initialize()
        {
            _repoMock = new Mock<IRoomRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _service = new RoomCreateService(_repoMock.Object, _unitOfWorkMock.Object);
        }

        [Test]
        public async Task InitializeRoomFromStartRoom_WithDirectionLeft_ReturnsExpectedRoomDto()
        {
            // Arrange
            var expectedDto = new RoomDetailsDto()
            {
                CanGoRight = true,
                CoordX = -1,
                CoordY = -0,
            };

            var direction = RoomDirection.Left;

            _repoMock
                .Setup(x => x.GetActiveForEditCoordX())
                .ReturnsAsync(0);

            _repoMock
                .Setup(x => x.GetActiveForEditCoordY())
                .ReturnsAsync(0);

            // Act
            var roomDto = await _service.InitializeRoomAsync(direction);

            // Assert
            Assert.AreEqual(roomDto.CanGoRight, expectedDto.CanGoRight);
            Assert.AreEqual(roomDto.CoordX, expectedDto.CoordX);
            Assert.AreEqual(roomDto.CoordY, expectedDto.CoordY);
        }
    }
}
