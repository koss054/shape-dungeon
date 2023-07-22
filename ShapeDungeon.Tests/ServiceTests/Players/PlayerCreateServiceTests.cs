//#nullable disable
//using Moq;
//using NUnit.Framework;
//using ShapeDungeon.Data;
//using ShapeDungeon.DTOs.Players;
//using ShapeDungeon.Interfaces.Services.Players;
//using ShapeDungeon.Repos;
//using ShapeDungeon.Services.Players;
//using System.Threading.Tasks;

//namespace ShapeDungeon.Tests.ServiceTests.Players
//{
//    internal class PlayerCreateServiceTests
//    {
//        private Mock<IPlayerRepositoryOld> _repoMock;
//        private Mock<IUnitOfWork> _unitOfWorkMock;
//        private IPlayerCreateService _service;

//        [SetUp]
//        public void Test_Initialize()
//        {
//            _repoMock = new Mock<IPlayerRepositoryOld>();
//            _unitOfWorkMock = new Mock<IUnitOfWork>();
//            _service = new PlayerCreateService(_repoMock.Object, _unitOfWorkMock.Object);
//        }

//        [Test]
//        public async Task CreatePlayer_WithUniqueName_CreatesNewPlayer()
//        {
//            // Arrange
//            var uniqueName = "Minecraft Steve Lvl.64";
//            var playerDto = new PlayerDto { Name = uniqueName };

//            _repoMock
//                .Setup(x => x.DoesNameExist(playerDto.Name))
//                .ReturnsAsync(false);

//            // Act
//            var isCreated = await _service.CreatePlayerAsync(playerDto);

//            // Assert
//            Assert.IsTrue(isCreated);
//        }

//        [Test]
//        public async Task CreatePlayer_WithExistingName_DoesNotCreateNewPlayer()
//        {
//            // Arrange
//            var existingName = "Minecraft Steve Lvl.64";
//            var playerDto = new PlayerDto { Name = existingName };

//            _repoMock
//                .Setup(x => x.DoesNameExist(playerDto.Name))
//                .ReturnsAsync(true);

//            // Act
//            var isCreated = await _service.CreatePlayerAsync(playerDto);

//            // Assert
//            Assert.IsFalse(isCreated);
//        }
//    }
//}
