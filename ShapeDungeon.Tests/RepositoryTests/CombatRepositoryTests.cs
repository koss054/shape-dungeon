using AutoFixture;
using FluentAssertions;
using ShapeDungeon.Data;
using ShapeDungeon.Entities;
using ShapeDungeon.Exceptions;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Repos;
using ShapeDungeon.Specifications;
using ShapeDungeon.Specifications.Combats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ShapeDungeon.Tests.RepositoryTests
{
    // Not testing Update and AddAsync, since they are direct calls to EFC.
    public class CombatRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly ICombatRepository _sut;
        private readonly IFixture _fixture;

        public CombatRepositoryTests()
        {
            _context = ContextGenerator.Generate();
            _sut = new CombatRepository(_context);
            _fixture = new Fixture();
        }

        [Theory]
        [MemberData(nameof(GetMultipleByAsync_ValidItemsData))]
        public async Task GetMultipleByAsync_ShouldReturnCombats_WhenSpecificationMatchesEntitiesInDb(
            int expectedCount, bool[] areActive, ISpecification<Combat> specification)
        {
            // Arrange
            var combats = new List<Combat>();
            for (int i = 0; i < areActive.Length; i++)
            {
                var combat = _fixture.Build<Combat>()
                    .With(x => x.Id, Guid.NewGuid())
                    .With(x => x.IsActive, areActive[i])
                    .Create();

                combats.Add(combat);
            }

            await _context.Combats.AddRangeAsync(combats);
            await _context.SaveChangesAsync();

            // Act
            var actualCombats = await _sut.GetMultipleByAsync(specification);

            // Assert
            actualCombats.Count().Should().Be(expectedCount);
        }

        [Theory]
        [MemberData(nameof(GetMultipleByAsync_InvalidItemsData))]
        public async Task GetMultipleByAsync_ShouldReturnEmptyList_WhenSpecificationDoesNotMatchEntitiesInDb(
            bool[] areActive, ISpecification<Combat> specification)
        {
            // Arrange
            var combats = new List<Combat>();
            for (int i = 0; i < areActive.Length; i++)
            {
                var combat = _fixture.Build<Combat>()
                    .With(x => x.Id, Guid.NewGuid())
                    .With(x => x.IsActive, areActive[i])
                    .Create();

                combats.Add(combat);
            }

            await _context.Combats.AddRangeAsync(combats);
            await _context.SaveChangesAsync();

            // Act
            var actualEnemies = await _sut.GetMultipleByAsync(specification);

            // Assert
            actualEnemies.Count().Should().Be(0);
        }

        [Theory]
        [MemberData(nameof(GetFirstAsync_ValidItemsData))]
        public async Task GetFirstAsync_ShouldReturnCombat_WhenSpecificationMatchesEntityInDb(
            bool isActive, ISpecification<Combat> specification)
        {
            // Arrange
            var expectedCombat = _fixture.Build<Combat>()
                .With(x => x.Id, Guid.NewGuid())
                .With(x => x.IsActive, isActive)
                .Create();

            await _context.Combats.AddAsync(expectedCombat);
            await _context.SaveChangesAsync();

            // Act
            var actualCombat = await _sut.GetFirstAsync(specification);

            // Assert
            actualCombat.Should().As<Combat>();
            actualCombat.Should().BeEquivalentTo(expectedCombat);
        }

        [Theory]
        [MemberData(nameof(GetFirstAsync_InvalidItemsData))]
        public async Task GetFirstAsync_ShouldThrowException_WhenSpecificationDoesNotMatchEntityInDb(
            bool isActive, ISpecification<Combat> specification)
        {
            // Arrange
            var expectedCombat = _fixture.Build<Combat>()
                .With(x => x.Id, Guid.NewGuid())
                .With(x => x.IsActive, isActive)
                .Create();

            await _context.Combats.AddAsync(expectedCombat);
            await _context.SaveChangesAsync();

            // Act
            var action = async () => await _sut.GetFirstAsync(specification);

            // Assert
            await action.Should().ThrowAsync<NoActiveCombatException>()
                .WithMessage("bruh");
        }

        [Theory]
        [MemberData(nameof(IsValidByAsync_ValidItemsData))]
        public async Task IsValidByAsync_ShouldReturnTrue_WhenSpecificationMatchesEntityInDb(
            bool isActive, ISpecification<Combat> specification)
        {
            // Arrange
            var combat = _fixture.Build<Combat>()
                .With(x => x.Id, Guid.NewGuid())
                .With(x => x.IsActive, isActive)
                .Create();

            await _context.Combats.AddAsync(combat);
            await _context.SaveChangesAsync();

            // Act
            var actualBool = await _sut.IsValidByAsync(specification);

            // Assert
            actualBool.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(IsValidByAsync_InvalidItemsData))]
        public async Task IsValidByAsync_ShouldReturnFalse_WhenSpecificationDoesNotMatchEntityInDb(
            bool isActive, ISpecification<Combat> specification)
        {
            // Arrange
            var combat = _fixture.Build<Combat>()
                .With(x => x.Id, Guid.NewGuid())
                .With(x => x.IsActive, isActive)
                .Create();

            await _context.Combats.AddAsync(combat);
            await _context.SaveChangesAsync();

            // Act
            var actualBool = await _sut.IsValidByAsync(specification);

            // Assert
            actualBool.Should().BeFalse();
        }

        public static IEnumerable<object[]> GetMultipleByAsync_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    3,                                              // Expected Returned Combats Count
                    new bool[] { true, true, false, false, true },  // Combat IsActive
                    new CombatIsActiveSpecification(),              // Repository Specification
                },
            };

        public static IEnumerable<object[]> GetMultipleByAsync_InvalidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    new bool[] { false, false, false }, // Combat IsActive
                    new CombatIsActiveSpecification(),  // Repository Specification
                },
            };

        public static IEnumerable<object[]> GetFirstAsync_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    true,                               // Combat IsActive
                    new CombatIsActiveSpecification(),  // Repository Specification
                },
            };

        public static IEnumerable<object[]> GetFirstAsync_InvalidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    false,                              // Combat IsActive
                    new CombatIsActiveSpecification(),  // Repository Specification
                },
            };

        public static IEnumerable<object[]> IsValidByAsync_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    true,                               // Combat IsActive
                    new CombatIsActiveSpecification(),  // Repository Specification
                },
            };

        public static IEnumerable<object[]> IsValidByAsync_InvalidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    false,                              // Combat IsActive
                    new CombatIsActiveSpecification(),  // Repository Specification
                },
            };
    }
}
