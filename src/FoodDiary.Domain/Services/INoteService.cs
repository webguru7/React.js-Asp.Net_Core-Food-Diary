using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FoodDiary.Domain.Dtos;
using FoodDiary.Domain.Entities;

namespace FoodDiary.Domain.Services
{
    public interface INoteService
    {
        Task<Note> GetNoteByIdAsync(int id, CancellationToken cancellationToken);

        Task<IEnumerable<Note>> GetNotesByPageIdAsync(int pageId, CancellationToken cancellationToken);

        Task<IEnumerable<Note>> GetNotesByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken);

        Task<bool> IsNoteDataValidAsync(NoteCreateEditDto noteData);

        Task<Note> CreateNoteAsync(Note note, CancellationToken cancellationToken);

        Task<Note> EditNoteAsync(Note note, CancellationToken cancellationToken);

        Task<Note> DeleteNoteAsync(Note note, CancellationToken cancellationToken);

        bool AllNotesFetched(IEnumerable<int> requestedIds, IEnumerable<Note> fetchedNotes);

        Task DeleteNotesAsync(IEnumerable<Note> notes, CancellationToken cancellationToken);

        Task<Note> MoveNoteAsync(NoteMoveRequestDto moveRequest, CancellationToken cancellationToken);
    }
}