﻿using System.Linq;
using System.Reflection;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using FoodDiary.API;
using FoodDiary.API.Controllers.v1;
using FoodDiary.Domain.Dtos;
using FoodDiary.Domain.Entities;
using FoodDiary.Domain.Services;
using FoodDiary.Infrastructure.Services;
using FoodDiary.UnitTests.Customizations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FoodDiary.UnitTests.Controllers
{
    public class NotesControllerTests
    {
        private readonly ILoggerFactory _loggerFactory;

        private readonly IMapper _mapper;

        private readonly Mock<INoteService> _noteServiceMock;

        private readonly IFixture _fixture;

        public NotesControllerTests()
        {
            var serviceCollection = new ServiceCollection()
                .AddLogging()
                .AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)))
                .AddTransient<ICaloriesService, CaloriesService>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            _loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            _mapper = serviceProvider.GetService<IMapper>();
            _noteServiceMock = new Mock<INoteService>();
            _fixture = SetupFixture();
        }

        private IFixture SetupFixture()
        {
            var _fixture = new Fixture();
            _fixture.Customize(new FixtureWithCircularReferencesCustomization());
            return _fixture;
        }

        public NotesController NotesController => new NotesController(
            _loggerFactory,
            _mapper,
            _noteServiceMock.Object);

        [Fact]
        public async void GetNotes_ReturnsFilteredNotes_WhenModelStateIsValid()
        {
            var request = _fixture.Create<NotesSearchRequestDto>();
            var notes = _fixture.CreateMany<Note>();
            _noteServiceMock.Setup(s => s.SearchNotesAsync(request, default))
                .ReturnsAsync(notes);

            var result = await NotesController.GetNotes(request, default);

            _noteServiceMock.Verify(s => s.SearchNotesAsync(request, default), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void GetNotes_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            var request = _fixture.Create<NotesSearchRequestDto>();
            var notes = _fixture.CreateMany<Note>();
            var controller = NotesController;
            controller.ModelState.AddModelError(_fixture.Create<string>(), _fixture.Create<string>());

            var result = await controller.GetNotes(request, default);

            _noteServiceMock.Verify(s => s.SearchNotesAsync(request, default), Times.Never);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async void CreateNote_CreatesNoteSuccessfully_WhenNoteDataIsValid()
        {
            var note = _fixture.Create<NoteCreateEditDto>();
            var validationResult = _fixture.Build<ValidationResultDto>()
                .With(r => r.IsValid, true)
                .Create();
            _noteServiceMock.Setup(s => s.ValidateNoteDataAsync(note, default))
                .ReturnsAsync(validationResult);

            var result = await NotesController.CreateNote(note, default);

            _noteServiceMock.Verify(s => s.ValidateNoteDataAsync(note, default), Times.Once);
            _noteServiceMock.Verify(s => s.CreateNoteAsync(It.IsNotNull<Note>(), default), Times.Once);
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async void CreateNote_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            var note = _fixture.Create<NoteCreateEditDto>();
            var controller = NotesController;
            controller.ModelState.AddModelError("error", "error");

            var result = await controller.CreateNote(note, default);

            _noteServiceMock.Verify(s => s.ValidateNoteDataAsync(note, default), Times.Never);
            _noteServiceMock.Verify(s => s.CreateNoteAsync(It.IsNotNull<Note>(), default), Times.Never);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async void CreateNote_ReturnsBadRequest_WhenNoteDataIsInvalid()
        {
            var note = _fixture.Create<NoteCreateEditDto>();
            var validationResult = _fixture.Build<ValidationResultDto>()
               .With(r => r.IsValid, false)
               .Create();
            _noteServiceMock.Setup(s => s.ValidateNoteDataAsync(note, default))
                .ReturnsAsync(validationResult);

            var result = await NotesController.CreateNote(note, default);

            _noteServiceMock.Verify(s => s.ValidateNoteDataAsync(note, default), Times.Once);
            _noteServiceMock.Verify(s => s.CreateNoteAsync(It.IsNotNull<Note>(), default), Times.Never);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async void EditNote_UpdatesNoteSuccessfully_WhenRequestedNoteExists()
        {
            var noteId = _fixture.Create<int>();
            var noteData = _fixture.Create<NoteCreateEditDto>();
            var noteForUpdate = _fixture.Create<Note>();
            var validationResult = _fixture.Build<ValidationResultDto>()
               .With(r => r.IsValid, true)
               .Create();
            _noteServiceMock.Setup(s => s.ValidateNoteDataAsync(noteData, default))
                .ReturnsAsync(validationResult);
            _noteServiceMock.Setup(s => s.GetNoteByIdAsync(noteId, default))
                .ReturnsAsync(noteForUpdate);

            var result = await NotesController.EditNote(noteId, noteData, default);

            _noteServiceMock.Verify(s => s.ValidateNoteDataAsync(noteData, default), Times.Once);
            _noteServiceMock.Verify(s => s.GetNoteByIdAsync(noteId, default), Times.Once);
            _noteServiceMock.Verify(s => s.EditNoteAsync(noteForUpdate, default), Times.Once);
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async void EditNote_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            var noteId = _fixture.Create<int>();
            var noteData = _fixture.Create<NoteCreateEditDto>();
            var controller = NotesController;
            controller.ModelState.AddModelError("error", "error");

            var result = await controller.EditNote(noteId, noteData, default);

            _noteServiceMock.Verify(s => s.ValidateNoteDataAsync(noteData, default), Times.Never);
            _noteServiceMock.Verify(s => s.GetNoteByIdAsync(noteId, default), Times.Never);
            _noteServiceMock.Verify(s => s.EditNoteAsync(It.IsAny<Note>(), default), Times.Never);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async void EditNote_ReturnsBadRequest_WhenNoteDataIsInvalid()
        {
            var noteId = _fixture.Create<int>();
            var noteData = _fixture.Create<NoteCreateEditDto>();
            var validationResult = _fixture.Build<ValidationResultDto>()
               .With(r => r.IsValid, false)
               .Create();
            _noteServiceMock.Setup(s => s.ValidateNoteDataAsync(noteData, default))
                .ReturnsAsync(validationResult);

            var result = await NotesController.EditNote(noteId, noteData, default);

            _noteServiceMock.Verify(s => s.ValidateNoteDataAsync(noteData, default), Times.Once);
            _noteServiceMock.Verify(s => s.GetNoteByIdAsync(noteId, default), Times.Never);
            _noteServiceMock.Verify(s => s.EditNoteAsync(It.IsAny<Note>(), default), Times.Never);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async void EditNote_ReturnsNotFound_WhenRequestedNoteDoesNotExist()
        {
            var noteId = _fixture.Create<int>();
            var noteData = _fixture.Create<NoteCreateEditDto>();
            var validationResult = _fixture.Build<ValidationResultDto>()
              .With(r => r.IsValid, true)
              .Create();
            _noteServiceMock.Setup(s => s.ValidateNoteDataAsync(noteData, default))
                .ReturnsAsync(validationResult);
            _noteServiceMock.Setup(s => s.GetNoteByIdAsync(noteId, default))
                .ReturnsAsync(null as Note);

            var result = await NotesController.EditNote(noteId, noteData, default);

            _noteServiceMock.Verify(s => s.ValidateNoteDataAsync(noteData, default), Times.Once);
            _noteServiceMock.Verify(s => s.GetNoteByIdAsync(noteId, default), Times.Once);
            _noteServiceMock.Verify(s => s.EditNoteAsync(It.IsAny<Note>(), default), Times.Never);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void DeleteNote_DeletesNoteSuccessfully_WhenRequestedNoteExists()
        {
            var noteForDelete = _fixture.Create<Note>();
            _noteServiceMock.Setup(s => s.GetNoteByIdAsync(noteForDelete.Id, default))
                .ReturnsAsync(noteForDelete);

            var result = await NotesController.DeleteNote(noteForDelete.Id, default);

            _noteServiceMock.Verify(s => s.GetNoteByIdAsync(noteForDelete.Id, default), Times.Once);
            _noteServiceMock.Verify(s => s.DeleteNoteAsync(noteForDelete, default), Times.Once);
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async void DeleteNote_ReturnsNotFound_WhenRequestedNoteDoesNotExists()
        {
            var noteForDeleteId = _fixture.Create<int>();
            _noteServiceMock.Setup(s => s.GetNoteByIdAsync(noteForDeleteId, default))
                .ReturnsAsync(null as Note);

            var result = await NotesController.DeleteNote(noteForDeleteId, default);

            _noteServiceMock.Verify(s => s.GetNoteByIdAsync(noteForDeleteId, default), Times.Once);
            _noteServiceMock.Verify(s => s.DeleteNoteAsync(It.IsAny<Note>(), default), Times.Never);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void DeleteNotes_DeletesNotesSuccessfully_WhenAllRequestedNotesAreFetched()
        {
            var notesForDelete = _fixture.CreateMany<Note>();
            var notesForDeleteIds = notesForDelete.Select(n => n.Id);

            _noteServiceMock.Setup(s => s.GetNotesByIdsAsync(notesForDeleteIds, default))
                .ReturnsAsync(notesForDelete);
            _noteServiceMock.Setup(s => s.AllNotesFetched(notesForDeleteIds, notesForDelete))
                .Returns(true);

            var result = await NotesController.DeleteNotes(notesForDeleteIds, default);

            _noteServiceMock.Verify(s => s.GetNotesByIdsAsync(notesForDeleteIds, default), Times.Once);
            _noteServiceMock.Verify(s => s.AllNotesFetched(notesForDeleteIds, notesForDelete), Times.Once);
            _noteServiceMock.Verify(s => s.DeleteNotesAsync(notesForDelete, default), Times.Once);
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async void DeleteNotes_ReturnsBadRequest_WhenNotAllRequestedNotesAreFetched()
        {
            var notesForDelete = _fixture.CreateMany<Note>();
            var notesForDeleteIds = notesForDelete.Select(n => n.Id);

            _noteServiceMock.Setup(s => s.GetNotesByIdsAsync(notesForDeleteIds, default))
                .ReturnsAsync(notesForDelete);
            _noteServiceMock.Setup(s => s.AllNotesFetched(notesForDeleteIds, notesForDelete))
                .Returns(false);

            var result = await NotesController.DeleteNotes(notesForDeleteIds, default);

            _noteServiceMock.Verify(s => s.GetNotesByIdsAsync(notesForDeleteIds, default), Times.Once);
            _noteServiceMock.Verify(s => s.AllNotesFetched(notesForDeleteIds, notesForDelete), Times.Once);
            _noteServiceMock.Verify(s => s.DeleteNotesAsync(notesForDelete, default), Times.Never);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async void MoveNote_MovesNoteSuccessfully_WhenNoteCanBeMoved()
        {
            var moveRequest = _fixture.Create<NoteMoveRequestDto>();
            var noteForMove = _fixture.Create<Note>();

            _noteServiceMock.Setup(s => s.GetNoteByIdAsync(moveRequest.NoteId, default))
                .ReturnsAsync(noteForMove);
            _noteServiceMock.Setup(s => s.NoteCanBeMovedAsync(noteForMove, moveRequest, default))
                .ReturnsAsync(true);

            var result = await NotesController.MoveNote(moveRequest, default);

            _noteServiceMock.Verify(s => s.GetNoteByIdAsync(moveRequest.NoteId, default), Times.Once);
            _noteServiceMock.Verify(s => s.NoteCanBeMovedAsync(noteForMove, moveRequest, default), Times.Once);
            _noteServiceMock.Verify(s => s.MoveNoteAsync(noteForMove, moveRequest, default), Times.Once);
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async void MoveNote_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            var moveRequest = _fixture.Create<NoteMoveRequestDto>();
            var controller = NotesController;
            controller.ModelState.AddModelError("error", "error");

            var result = await controller.MoveNote(moveRequest, default);

            _noteServiceMock.Verify(s => s.GetNoteByIdAsync(moveRequest.NoteId, default), Times.Never);
            _noteServiceMock.Verify(s => s.NoteCanBeMovedAsync(It.IsAny<Note>(), moveRequest, default), Times.Never);
            _noteServiceMock.Verify(s => s.MoveNoteAsync(It.IsAny<Note>(), moveRequest, default), Times.Never);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async void MoveNote_ReturnsNotFound_WhenNoteForMoveDoesNotExist()
        {
            var moveRequest = _fixture.Create<NoteMoveRequestDto>();

            _noteServiceMock.Setup(s => s.GetNoteByIdAsync(moveRequest.NoteId, default))
                .ReturnsAsync(null as Note);

            var result = await NotesController.MoveNote(moveRequest, default);

            _noteServiceMock.Verify(s => s.GetNoteByIdAsync(moveRequest.NoteId, default), Times.Once);
            _noteServiceMock.Verify(s => s.NoteCanBeMovedAsync(It.IsAny<Note>(), moveRequest, default), Times.Never);
            _noteServiceMock.Verify(s => s.MoveNoteAsync(It.IsAny<Note>(), moveRequest, default), Times.Never);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void MoveNote_ReturnsBadRequest_WhenNoteCannotBeMoved()
        {
            var moveRequest = _fixture.Create<NoteMoveRequestDto>();
            var noteForMove = _fixture.Create<Note>();

            _noteServiceMock.Setup(s => s.GetNoteByIdAsync(moveRequest.NoteId, default))
                .ReturnsAsync(noteForMove);
            _noteServiceMock.Setup(s => s.NoteCanBeMovedAsync(noteForMove, moveRequest, default))
                .ReturnsAsync(false);

            var result = await NotesController.MoveNote(moveRequest, default);

            _noteServiceMock.Verify(s => s.GetNoteByIdAsync(moveRequest.NoteId, default), Times.Once);
            _noteServiceMock.Verify(s => s.NoteCanBeMovedAsync(noteForMove, moveRequest, default), Times.Once);
            _noteServiceMock.Verify(s => s.MoveNoteAsync(noteForMove, moveRequest, default), Times.Never);
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
