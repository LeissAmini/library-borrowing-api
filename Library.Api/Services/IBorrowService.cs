using Library.Api.DTOs;
using Library.Api.Models;

namespace Library.Api.Services
{
    public interface IBorrowService
    {
        Task<BorrowRecordResult> BorrowBookAsync(BorrowRequest request);
        Task<BorrowRecordResult> ReturnBookAsync(ReturnRequest request);
        Task<IEnumerable<BorrowRecordResult>> GetBorrowRecordsByMemberIdAsync(Guid memberId);
        Task<IEnumerable<BorrowRecordResult>> GetAllBorrowRecordsAsync();
    }
}