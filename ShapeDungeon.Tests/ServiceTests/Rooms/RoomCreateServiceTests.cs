//#nullable disable
//using Moq;
//using NUnit.Framework;
//using ShapeDungeon.Data;
//using ShapeDungeon.DTOs.Rooms;
//using ShapeDungeon.Entities;
//using ShapeDungeon.Helpers.Enums;
//using ShapeDungeon.Interfaces.Services.Rooms;
//using ShapeDungeon.Repos;
//using ShapeDungeon.Services.Rooms;
//using System;
//using System.Threading.Tasks;

//namespace ShapeDungeon.Tests.ServiceTests.Rooms
//{
//    internal class RoomCreateServiceTests
//    {
//        private Mock<IRoomRepositoryOld> _repoMock;
//        private Mock<IUnitOfWork> _unitOfWorkMock;
//        private IRoomCreateService _service;

//        [SetUp]
//        public void Test_Initialize()
//        {
//            _repoMock = new Mock<IRoomRepositoryOld>();
//            _unitOfWorkMock = new Mock<IUnitOfWork>();
//            _service = new RoomCreateService(_repoMock.Object, _unitOfWorkMock.Object);
//        }

//        [Test]
//        public async Task CreateStartRoom_WithValidDto_ReturnsExpectedDetails()
//        {
//            // Arrange
//            var expectedCoordX = 0;
//            var expectedCoordY = 0;
//            var expectedRoom = new Room()
//            {
//                IsActiveForMove = false,
//                IsActiveForScout = false,
//                IsActiveForEdit = true,
//                CanGoLeft = true,
//                CanGoRight = true,
//                CanGoUp = true,
//                CanGoDown = true,
//                IsStartRoom = true,
//                IsSafeRoom = false,
//                IsEnemyRoom = false,
//                IsEndRoom = false,
//            };

//            RoomDetailsDto roomDto = expectedRoom;
//            roomDto.CoordX = 100000;
//            roomDto.CoordY = -1000000;

//            // Act
//            var createdRoom = await _service.CreateAsync(roomDto);

//            // Assert
//            Assert.IsTrue(createdRoom.IsStartRoom);
//            Assert.AreEqual(createdRoom.CoordX, expectedCoordX);
//            Assert.AreEqual(createdRoom.CoordY, expectedCoordY);
//        }

//        [Test]
//        public async Task CreateRoomThatIsNotStart_WithValidDto_ReturnsExpectedDetails()
//        {
//            // Arrange
//            var expectedCoordX = 11110;
//            var expectedCoordY = -1212120;
//            var roomDto = new RoomDetailsDto()
//            {
//                IsActiveForEdit = true,
//                CanGoLeft = true,
//                CanGoRight = true,
//                CanGoUp = true,
//                CanGoDown = true,
//                IsStartRoom = false,
//                IsSafeRoom = false,
//                IsEnemyRoom = false,
//                IsEndRoom = false,
//                CoordX = expectedCoordX,
//                CoordY = expectedCoordY,
//            };

//            // Act
//            var createdRoom = await _service.CreateAsync(roomDto);

//            // Assert
//            Assert.IsFalse(createdRoom.IsStartRoom);
//            Assert.AreEqual(createdRoom.CoordX, expectedCoordX);
//            Assert.AreEqual(createdRoom.CoordY, expectedCoordY);
//        }

//        [Test]
//        public async Task InitializeRoomFromStartRoom_WithDirectionLeft_ReturnsExpectedRoomDto()
//        {
//            // Arrange
//            var expectedDto = new RoomDetailsDto()
//            {
//                CanGoRight = true,
//                CoordX = -1,
//                CoordY = 0,
//            };

//            var direction = RoomDirection.Left;

//            _repoMock
//                .Setup(x => x.GetActiveForEditCoordX())
//                .ReturnsAsync(0);

//            _repoMock
//                .Setup(x => x.GetActiveForEditCoordY())
//                .ReturnsAsync(0);

//            // Act
//            var roomDto = await _service.InitializeRoomAsync(direction);

//            // Assert
//            Assert.AreEqual(roomDto.CanGoRight, expectedDto.CanGoRight);
//            Assert.AreEqual(roomDto.CoordX, expectedDto.CoordX);
//            Assert.AreEqual(roomDto.CoordY, expectedDto.CoordY);
//        }

//        [Test]
//        public async Task InitializeRoomFromStartRoom_WithDirectionRight_ReturnsExpectedRoomDto()
//        {
//            // Arrange
//            var expectedDto = new RoomDetailsDto()
//            {
//                CanGoLeft = true,
//                CoordX = 1,
//                CoordY = 0,
//            };

//            var direction = RoomDirection.Right;

//            _repoMock
//                .Setup(x => x.GetActiveForEditCoordX())
//                .ReturnsAsync(0);

//            _repoMock
//                .Setup(x => x.GetActiveForEditCoordY())
//                .ReturnsAsync(0);

//            // Act
//            var roomDto = await _service.InitializeRoomAsync(direction);

//            // Assert
//            Assert.AreEqual(roomDto.CanGoLeft, expectedDto.CanGoLeft);
//            Assert.AreEqual(roomDto.CoordX, expectedDto.CoordX);
//            Assert.AreEqual(roomDto.CoordY, expectedDto.CoordY);
//        }

//        [Test]
//        public async Task InitializeRoomFromStartRoom_WithDirectionTop_ReturnsExpectedRoomDto() // Why named the direction top? Gotta rename it at some point :D
//        {
//            // Arrange
//            var expectedDto = new RoomDetailsDto()
//            {
//                CanGoDown = true,
//                CoordX = 0,
//                CoordY = 1,
//            };

//            var direction = RoomDirection.Top;

//            _repoMock
//                .Setup(x => x.GetActiveForEditCoordX())
//                .ReturnsAsync(0);

//            _repoMock
//                .Setup(x => x.GetActiveForEditCoordY())
//                .ReturnsAsync(0);

//            // Act
//            var roomDto = await _service.InitializeRoomAsync(direction);

//            // Assert
//            Assert.AreEqual(roomDto.CanGoDown, expectedDto.CanGoDown);
//            Assert.AreEqual(roomDto.CoordX, expectedDto.CoordX);
//            Assert.AreEqual(roomDto.CoordY, expectedDto.CoordY);
//        }

//        [Test]
//        public async Task InitializeRoomFromStartRoom_WithDirectionBottom_ReturnsExpectedRoomDto()  // Direction name same as comment above.
//        {
//            // Arrange
//            var expectedDto = new RoomDetailsDto()
//            {
//                CanGoUp = true,
//                CoordX = 0,
//                CoordY = -1,
//            };

//            var direction = RoomDirection.Bottom;

//            _repoMock
//                .Setup(x => x.GetActiveForEditCoordX())
//                .ReturnsAsync(0);

//            _repoMock
//                .Setup(x => x.GetActiveForEditCoordY())
//                .ReturnsAsync(0);

//            // Act
//            var roomDto = await _service.InitializeRoomAsync(direction);

//            // Assert
//            Assert.AreEqual(roomDto.CanGoUp, expectedDto.CanGoUp);
//            Assert.AreEqual(roomDto.CoordX, expectedDto.CoordX);
//            Assert.AreEqual(roomDto.CoordY, expectedDto.CoordY);
//        }

//        [Test]
//        public void InitializeRoomFromStartRoom_WithInvalidDirection_ThrowsArgumentOutOfRangeException()
//        {
//            // Arrange
//            var direction = 9999999999;

//            _repoMock
//                .Setup(x => x.GetActiveForEditCoordX())
//                .ReturnsAsync(0);

//            _repoMock
//                .Setup(x => x.GetActiveForEditCoordY())
//                .ReturnsAsync(0);

//            // Act
//            var initializationTask = _service.InitializeRoomAsync((RoomDirection)direction);

//            // Assert
//            Assert.That(async () => await initializationTask,
//                                    Throws.TypeOf<ArgumentOutOfRangeException>());
//        }
//    }
//}
