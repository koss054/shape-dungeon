//#nullable disable
//using Moq;
//using NUnit.Framework;
//using ShapeDungeon.DTOs.Rooms;
//using ShapeDungeon.Entities;
//using ShapeDungeon.Interfaces.Services.Rooms;
//using ShapeDungeon.Repos;
//using ShapeDungeon.Services.Rooms;
//using System.Threading.Tasks;

//namespace ShapeDungeon.Tests.ServiceTests.Rooms
//{
//    internal class CheckRoomNeighborsServiceTests
//    {
//        private Mock<IRoomRepositoryOld> _repoMock;
//        private ICheckRoomNeighborsService _service;

//        [SetUp]
//        public void Test_Initialize()
//        {
//            _repoMock = new Mock<IRoomRepositoryOld>();
//            _service = new CheckRoomNeighborsService(_repoMock.Object);
//        }

//        [Test]
//        public async Task SetDtoNeighbors_WithNeighborsOnAllSides_SetsCorrectRoomNeighbors()
//        {
//            // Arrange
//            var coordX = 0;
//            var coordY = 0;

//            _repoMock
//                .Setup(x => x.GetByCoords(coordX, coordY))
//                .ReturnsAsync(new Room
//                {
//                    CanGoLeft = true,
//                    CanGoRight = true,
//                    CanGoUp = true,
//                    CanGoDown = true,
//                });

//            _repoMock
//                .Setup(x => x.GetByCoords(-1, 0))
//                .ReturnsAsync(new Room());

//            _repoMock
//                .Setup(x => x.GetByCoords(1, 0))
//                .ReturnsAsync(new Room());

//            _repoMock
//                .Setup(x => x.GetByCoords(0, 1))
//                .ReturnsAsync(new Room());

//            _repoMock
//                .Setup(x => x.GetByCoords(-0, -1))
//                .ReturnsAsync(new Room());

//            // Act
//            var room = await _service.SetDtoNeighborsAsync(coordX, coordY);

//            // Assert
//            Assert.IsTrue(room.HasLeftNeighbor);
//            Assert.IsTrue(room.HasRightNeighbor);
//            Assert.IsTrue(room.HasUpNeighbor);
//            Assert.IsTrue(room.HasDownNeighbor);
//        }

//        [Test]
//        public async Task SetDtoNeighbors_WithHorizontalNeighbors_SetsCorrectRoomNeighbors()
//        {
//            // Arrange
//            var coordX = 0;
//            var coordY = 0;

//            _repoMock
//                .Setup(x => x.GetByCoords(coordX, coordY))
//                .ReturnsAsync(new Room
//                {
//                    CanGoLeft = true,
//                    CanGoRight = true,
//                    CanGoUp = false,
//                    CanGoDown = false,
//                });

//            _repoMock
//                .Setup(x => x.GetByCoords(-1, 0))
//                .ReturnsAsync(new Room());

//            _repoMock
//                .Setup(x => x.GetByCoords(1, 0))
//                .ReturnsAsync(new Room());

//            // Act
//            var room = await _service.SetDtoNeighborsAsync(coordX, coordY);

//            // Assert
//            Assert.IsTrue(room.HasLeftNeighbor);
//            Assert.IsTrue(room.HasRightNeighbor);
//            Assert.IsFalse(room.HasUpNeighbor);
//            Assert.IsFalse(room.HasDownNeighbor);
//        }

//        [Test]
//        public async Task SetDtoNeighbors_WithVerticalNeighbors_SetsCorrectRoomNeighbors()
//        {
//            // Arrange
//            var coordX = 0;
//            var coordY = 0;

//            _repoMock
//                .Setup(x => x.GetByCoords(coordX, coordY))
//                .ReturnsAsync(new Room
//                {
//                    CanGoLeft = false,
//                    CanGoRight = false,
//                    CanGoUp = true,
//                    CanGoDown = true,
//                });

//            _repoMock
//                .Setup(x => x.GetByCoords(0, 1))
//                .ReturnsAsync(new Room());

//            _repoMock
//                .Setup(x => x.GetByCoords(-0, -1))
//                .ReturnsAsync(new Room());

//            // Act
//            var room = await _service.SetDtoNeighborsAsync(coordX, coordY);

//            // Assert
//            Assert.IsFalse(room.HasLeftNeighbor);
//            Assert.IsFalse(room.HasRightNeighbor);
//            Assert.IsTrue(room.HasUpNeighbor);
//            Assert.IsTrue(room.HasDownNeighbor);
//        }

//        [Test]
//        public async Task SetDtoNeighbors_WithOneNeighbor_SetsCorrectRoomNeighbors()
//        {
//            // Arrange
//            var coordX = 0;
//            var coordY = 0;

//            _repoMock
//                .Setup(x => x.GetByCoords(coordX, coordY))
//                .ReturnsAsync(new Room
//                {
//                    CanGoLeft = false,
//                    CanGoRight = false,
//                    CanGoUp = false,
//                    CanGoDown = true,
//                });

//            _repoMock
//                .Setup(x => x.GetByCoords(-0, -1))
//                .ReturnsAsync(new Room());

//            // Act
//            var room = await _service.SetDtoNeighborsAsync(coordX, coordY);

//            // Assert
//            Assert.IsFalse(room.HasLeftNeighbor);
//            Assert.IsFalse(room.HasRightNeighbor);
//            Assert.IsFalse(room.HasUpNeighbor);
//            Assert.IsTrue(room.HasDownNeighbor);
//        }

//        [Test]
//        public void SetHasNeighborsProperties_WithCorrectData_ReturnsExpectedDto()
//        {
//            // Arrange
//            var room = new RoomDto();
//            var roomNav = new RoomNavDto
//            {
//                HasLeftNeighbor = false,
//                HasRightNeighbor = true,
//                HasDownNeighbor = true,
//                HasUpNeighbor = false,
//            };

//            // Act
//            room = _service.SetHasNeighborsProperties(room, roomNav);

//            // Assert
//            Assert.AreEqual(room.HasLeftNeighbor, roomNav.HasLeftNeighbor);
//            Assert.AreEqual(room.HasRightNeighbor, roomNav.HasRightNeighbor);
//            Assert.AreEqual(room.HasDownNeighbor, roomNav.HasDownNeighbor);
//            Assert.AreEqual(room.HasUpNeighbor, roomNav.HasUpNeighbor);
//        }
//    }
//}
