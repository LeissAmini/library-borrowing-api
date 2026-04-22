using Library.Api.Data;
using Library.Api.Models;

namespace Library.Api.Repositories;

public interface IBorrowRepository
{
  Task<IEnumerable<BorrowRecord>> GetAllBorrowRecordsAsync();
  Task<BorrowRecord?> GetBorrowRecordByMemberIdAsync(Guid memberId);
  Task<BorrowRecord> AddBorrowRecordAsync(BorrowRecord borrowRecord);
  Task UpdateBorrowRecordAsync(BorrowRecord borrowRecord);
}