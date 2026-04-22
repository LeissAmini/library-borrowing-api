using Library.Api.DTOs;
using Library.Api.Models;
using Library.Api.Repositories;

namespace Library.Api.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRepository _borrowRepository;

        public BorrowService(IBorrowRepository borrowRepository)
        {
            _borrowRepository = borrowRepository;
        }

        public async Task<BorrowRecordResult> BorrowBookAsync(BorrowRequest request)
        {
            // Create a new borrow record
            var borrowRecord = new BorrowRecord
            {
                Id = Guid.NewGuid(),
                BookId = request.BookId,
                MemberId = request.MemberId,
                BorrowDate = request.BorrowDate,
                ReturnDate = null,
                Status = true
            };

            var created = await _borrowRepository.AddBorrowRecordAsync(borrowRecord);
            return MapToResult(created);
        }

        public async Task<BorrowRecordResult> ReturnBookAsync(ReturnRequest request)
        {
            // Return book by updating the existing borrow record
            var record = await _borrowRepository.GetBorrowRecordByMemberIdAsync(request.BorrowRecordId);
            if (record == null)
                throw new KeyNotFoundException($"Borrow record {request.BorrowRecordId} not found.");

            if (!record.Status)
                throw new InvalidOperationException("This book has already been returned.");

            record.ReturnDate = request.ReturnDate;
            record.Status = false;

            await _borrowRepository.UpdateBorrowRecordAsync(record);
            return MapToResult(record);
        }

        public async Task<IEnumerable<BorrowRecordResult>> GetBorrowRecordsByMemberIdAsync(Guid memberId)
        {
            // Get all records and filter by member ID (since we don't have a direct query method in the repository)
            var records = await _borrowRepository.GetAllBorrowRecordsAsync();
            return records.Where(r => r.MemberId == memberId).Select(MapToResult);
        }

        public async Task<IEnumerable<BorrowRecordResult>> GetAllBorrowRecordsAsync()
        {
            // Get all borrow records
            var records = await _borrowRepository.GetAllBorrowRecordsAsync();
            return records.Select(MapToResult);
        }

        private static BorrowRecordResult MapToResult(BorrowRecord record) => new BorrowRecordResult
        {
            Id = record.Id,
            BookId = record.BookId,
            MemberId = record.MemberId,
            BorrowDate = record.BorrowDate,
            ReturnDate = record.ReturnDate
        };
    }
}