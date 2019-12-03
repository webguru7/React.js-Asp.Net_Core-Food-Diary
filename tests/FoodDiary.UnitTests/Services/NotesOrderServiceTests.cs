using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using FoodDiary.Domain.Dtos;
using FoodDiary.Domain.Entities;
using FoodDiary.Domain.Repositories;
using FoodDiary.Domain.Services;
using FoodDiary.Infrastructure.Services;
using FoodDiary.UnitTests.Customizations;
using Moq;
using Xunit;

namespace FoodDiary.UnitTests.Services
{
    public class NotesOrderServiceTests
    {
        private readonly Mock<INoteRepository> _noteRepositoryMock;
        private readonly IFixture _fixture;

        public NotesOrderServiceTests()
        {
            _noteRepositoryMock = new Mock<INoteRepository>();
            _fixture = SetupFixture();
        }

        private IFixture SetupFixture()
        {
            var _fixture = new Fixture();
            _fixture.Customize(new FixtureWithCircularReferencesCustomization());
            return _fixture;
        }

        public INotesOrderService NotesOrderService => new NotesOrderService(_noteRepositoryMock.Object);

        [Theory]
        [AutoData]
        public async void GetOrderForNewNoteAsync_ReturnsOneMoreThanMaxDisplayOrder(int expectedMaxDisplayOrder)
        {
            var note = _fixture.Create<Note>();
            _noteRepositoryMock.Setup(r => r.GetMaxDisplayOrderFromQueryAsync(It.IsAny<IQueryable<Note>>(), default))
                .ReturnsAsync(expectedMaxDisplayOrder);

            var result = await NotesOrderService.GetOrderForNewNoteAsync(note, default);

            _noteRepositoryMock.Verify(r => r.GetQueryWithoutTracking(), Times.Once);
            _noteRepositoryMock.Verify(r => r.GetMaxDisplayOrderFromQueryAsync(It.IsAny<IQueryable<Note>>(), default), Times.Once);
            result.Should().Be(expectedMaxDisplayOrder + 1);
        }

        [Fact]
        public async void ReorderNotesOnDeleteAsync_RecalculatesDisplayOrders_OfNotesWithoutDeleted()
        {
            var noteForDelete = _fixture.Create<Note>();
            var notesForReorder = _fixture.CreateMany<Note>().ToList();
            var expectedNoteOrders = GetExpectedNoteOrders(notesForReorder);

            _noteRepositoryMock.Setup(r => r.GetListFromQueryAsync(It.IsAny<IQueryable<Note>>(), default))
                .ReturnsAsync(notesForReorder);

            await NotesOrderService.ReorderNotesOnDeleteAsync(noteForDelete, default);
            var resultNoteOrders = GetResultNoteOrders(notesForReorder);

            _noteRepositoryMock.Verify(r => r.GetQuery(), Times.Once);
            _noteRepositoryMock.Verify(r => r.GetListFromQueryAsync(It.IsAny<IQueryable<Note>>(), default), Times.Once);
            _noteRepositoryMock.Verify(r => r.UpdateRange(notesForReorder), Times.Once);
            resultNoteOrders.Should().Contain(expectedNoteOrders);
        }

        [Fact]
        public async void ReorderNotesOnDeleteRangeAsync_RecalculatesDisplayOrders_OfNotesWithoutDeleted()
        {
            var notesForDelete = _fixture.CreateMany<Note>();
            var notesForReorder = _fixture.CreateMany<Note>().ToList();
            var expectedNoteOrders = GetExpectedNoteOrders(notesForReorder);

            _noteRepositoryMock.Setup(r => r.GetListFromQueryAsync(It.IsAny<IQueryable<Note>>(), default))
                .ReturnsAsync(notesForReorder);

            await NotesOrderService.ReorderNotesOnDeleteRangeAsync(notesForDelete, default);
            var resultNoteOrders = GetResultNoteOrders(notesForReorder);

            _noteRepositoryMock.Verify(r => r.GetQuery(), Times.Once);
            _noteRepositoryMock.Verify(r => r.GetListFromQueryAsync(It.IsAny<IQueryable<Note>>(), default), Times.Once);
            _noteRepositoryMock.Verify(r => r.UpdateRange(notesForReorder), Times.Once);
            resultNoteOrders.Should().Contain(expectedNoteOrders);
        }

        [Fact]
        public async void ReorderNotesOnMoveAsync_RecalculatesDisplayOrders_OfNotesFromSourceMealWithoutMoved_And_NotesFromDestMealWithMoved()
        {
            var noteForMove = _fixture.Create<Note>();
            var moveRequest = _fixture.Create<NoteMoveRequestDto>();
            var notesForReorderFromSourceMeal = _fixture.CreateMany<Note>().ToList();
            var notesForReorderFromDestMeal = _fixture.CreateMany<Note>().ToList();
            var expectedNoteOrdersFromSourceMeal = GetExpectedNoteOrders(notesForReorderFromSourceMeal);
            var expectedNoteOrdersFromDestMeal = GetExpectedNoteOrders(notesForReorderFromDestMeal, moveRequest.Position);

            _noteRepositoryMock.SetupSequence(r => r.GetListFromQueryAsync(It.IsAny<IQueryable<Note>>(), default))
                .ReturnsAsync(notesForReorderFromSourceMeal)
                .ReturnsAsync(notesForReorderFromDestMeal);

            await NotesOrderService.ReorderNotesOnMoveAsync(noteForMove, moveRequest, default);
            var resultNoteOrdersFromSourceMeal = GetResultNoteOrders(notesForReorderFromSourceMeal);
            var resultNoteOrdersFromDestMeal = GetResultNoteOrders(notesForReorderFromDestMeal);

            _noteRepositoryMock.Verify(r => r.GetQuery(), Times.Exactly(2));
            _noteRepositoryMock.Verify(r => r.GetListFromQueryAsync(It.IsAny<IQueryable<Note>>(), default), Times.Exactly(2));
            _noteRepositoryMock.Verify(r => r.UpdateRange(notesForReorderFromSourceMeal), Times.Once);
            _noteRepositoryMock.Verify(r => r.UpdateRange(notesForReorderFromDestMeal), Times.Once);

            resultNoteOrdersFromSourceMeal.Should().Contain(expectedNoteOrdersFromSourceMeal);
            resultNoteOrdersFromDestMeal.Should().Contain(expectedNoteOrdersFromDestMeal);
        }

        private List<int> GetExpectedNoteOrders(IEnumerable<Note> notesForReorder, int initialOrderValue = -1)
        {
            var expectedNoteOrders = new List<int>();
            int curIndex = initialOrderValue;
            foreach (var note in notesForReorder)
                expectedNoteOrders.Add(++curIndex);
            return expectedNoteOrders;
        }

        private List<int> GetResultNoteOrders(IEnumerable<Note> notesForReorder)
        {
            return notesForReorder.Select(n => n.DisplayOrder).ToList();
        }
    }
}