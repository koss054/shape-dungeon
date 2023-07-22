////#nullable disable
////using Moq;
////using NUnit.Framework;
////using ShapeDungeon.Data;
////using ShapeDungeon.DTOs.Players;
////using ShapeDungeon.Entities;
////using ShapeDungeon.Helpers.Enums;
////using ShapeDungeon.Interfaces.Services.Players;
////using ShapeDungeon.Repos;
////using ShapeDungeon.Services.Players;
////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Threading.Tasks;

////namespace ShapeDungeon.Tests.ServiceTests.Players
////{
////    internal class PlayerScoutServiceTests
////    {
////        private Mock<IPlayerRepositoryOld> _repoMock;
////        private Mock<IUnitOfWork> _unitOfWorkMock;
////        private IPlayerScoutService _service;

////        [SetUp]
////        public void Test_Initialize()
////        {
////            _repoMock = new Mock<IPlayerRepositoryOld>();
////            _unitOfWorkMock = new Mock<IUnitOfWork>();
////            _service = new PlayerScoutService(_repoMock.Object, _unitOfWorkMock.Object);
////        }

////        [Test]
////        public async Task GetActiveScoutEnergy_WithActivePlayer_ReturnsCurrentScoutEnergy()
////        {
////            // Arrange
////            var expectedScoutEnergy = 5;

////            _repoMock
////                .Setup(x => x.GetActiveScoutEnergy())
////                .ReturnsAsync(expectedScoutEnergy);

////            // Act
////            var returnedScoutEnergy = await _service.GetActiveScoutEnergyAsync();

////            // Assert
////            Assert.AreEqual(expectedScoutEnergy, returnedScoutEnergy);
////        }

////        [Test]
////        public async Task GetActiveScoutEnergy_WithNoActivePlayer_ReturnsZeroScoutEnergy()
////        {
////            // Arrange
////            var expectedScoutEnergy = 0;

////            // Act
////            var returnedScoutEnergy = await _service.GetActiveScoutEnergyAsync();

////            // Assert
////            Assert.AreEqual(expectedScoutEnergy, returnedScoutEnergy);
////        }

////        [Test]
////        public async Task UpdateActiveScoutEnergy_ValidInfoReduceAction_ReturnsUpdatedActiveScoutEnergy()
////        {
////            // Arrange
////            var initialScoutEnergy = 3;
////            var expectedScoutEnergy = 2;
////            var scoutAction = PlayerScoutAction.Reduce;
////            var activePlayer = new Player
////            {
////                IsActive = true,
////                CurrentScoutEnergy = initialScoutEnergy,
////            };

////            _repoMock
////                .Setup(x => x.GetActive())
////                .ReturnsAsync(activePlayer);

////            // Act
////            var updatedScoutEnergy = await _service.UpdateActiveScoutEnergyAsync(scoutAction);

////            // Assert
////            Assert.AreEqual(updatedScoutEnergy, expectedScoutEnergy);
////        }

////        [Test]
////        public async Task UpdateActiveScoutEnergy_ValidInfoRefillAction_ReturnsUpdatedActiveScoutEnergy()
////        {
////            // Arrange
////            var playerAgility = 5;
////            var initialScoutEnergy = 3;
////            var expectedScoutEnergy = 5;
////            var scoutAction = PlayerScoutAction.Refill;
////            var activePlayer = new Player
////            {
////                IsActive = true,
////                CurrentScoutEnergy = initialScoutEnergy,
////                Agility = playerAgility,
////            };

////            _repoMock
////                .Setup(x => x.GetActive())
////                .ReturnsAsync(activePlayer);

////            // Act
////            var updatedScoutEnergy = await _service.UpdateActiveScoutEnergyAsync(scoutAction);

////            // Assert
////            Assert.AreEqual(updatedScoutEnergy, expectedScoutEnergy);
////        }

////        [Test]
////        public async Task UpdateActiveScountEnergy_InvalidScoutAction_ReturnsMinusOne()
////        {
////            // Arrange
////            var playerAgility = 5;
////            var initialScoutEnergy = 3;
////            var expectedResult = -1;
////            var scoutAction = 54;
////            var activePlayer = new Player
////            {
////                IsActive = true,
////                CurrentScoutEnergy = initialScoutEnergy,
////                Agility = playerAgility,
////            };

////            _repoMock
////                .Setup(x => x.GetActive())
////                .ReturnsAsync(activePlayer);

////            // Act
////            var returnValue = await _service.UpdateActiveScoutEnergyAsync((PlayerScoutAction)scoutAction);

////            // Assert
////            Assert.AreEqual(expectedResult, returnValue);
////        }

////        [Test]
////        public void UpdateActiveScoutEnergy_WithNoActivePlayer_ThrowsNullReferenceException()
////        {
////            // Arrange
////            var scoutAction = PlayerScoutAction.Refill;

////            // Act
////            var updateScountEnergyTask = _service.UpdateActiveScoutEnergyAsync(scoutAction);

////            // Assert
////            Assert.That(async () => await updateScountEnergyTask,
////                                    Throws.TypeOf<NullReferenceException>());
////        }
////    }
////}
