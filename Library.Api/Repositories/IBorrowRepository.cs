using Library.Api.Models;

namespace Library.Api.Repositories;

public interface IBorrowRepository
{
    Task<IEnumerable<BorrowRecord>> GetAllBorrowRecordsAsync();
    Task<IEnumerable<BorrowRecord>> GetBorrowRecordsByMemberIdAsync(Guid memberId);
    Task<BorrowRecord?> GetBorrowRecordByIdAsync(Guid borrowRecordId);
    Task<BorrowRecord?> GetActiveBorrowRecordByIdAndMemberAsync(Guid borrowRecordId, Guid memberId);
    Task<bool> HasActiveBorrowRecordAsync(Guid bookId, Guid memberId);
    Task<Book?> GetBookByIdAsync(Guid bookId);
    Task<Member?> GetMemberByIdAsync(Guid memberId);
    Task AddBorrowRecordAsync(BorrowRecord borrowRecord);
    Task SaveChangesAsync();
}