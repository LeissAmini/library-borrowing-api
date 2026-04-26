using Library.Api.DTOs;
using Library.Api.Models;
using Library.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Library.Api.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRepository _borrowRepository;
        private readonly IMemoryCache _cache;

        public BorrowService(IBorrowRepository borrowRepository, IMemoryCache cache)
        {
            _borrowRepository = borrowRepository;
            _cache = cache;
        }

        public async Task<BorrowRecordResult> BorrowBookAsync(BorrowRequest request)
        {
            var member = await _borrowRepository.GetMemberByIdAsync(request.MemberId);
            if (member == null)
            {
                throw new KeyNotFoundException($"Member {request.MemberId} not found.");
            }

            var book = await _borrowRepository.GetBookByIdAsync(request.BookId);
            if (book == null)
            {
                throw new KeyNotFoundException($"Book {request.BookId} not found.");
            }

            var alreadyBorrowed = await _borrowRepository.HasActiveBorrowRecordAsync(request.BookId, request.MemberId);
            if (alreadyBorrowed)
            {
                throw new InvalidOperationException("This member already has an active borrow for this book.");
            }

            if (book.AvailableCopies <= 0)
            {
                throw new InvalidOperationException("This book is currently unavailable.");
            }

            book.AvailableCopies -= 1;

            var record = new BorrowRecord
            {
                Id = Guid.NewGuid(),
                BookId = request.BookId,
                MemberId = request.MemberId,
                BorrowDate = request.BorrowDate == default ? DateTime.UtcNow : request.BorrowDate,
                ReturnDate = null
            };

            await _borrowRepository.AddBorrowRecordAsync(record);
            try
            {
                await _borrowRepository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new InvalidOperationException("This book is currently unavailable.");
            }
            _cache.Remove("all_books");
            return MapToResult(record);
        }

        public async Task<BorrowRecordResult> ReturnBookAsync(ReturnRequest request)
        {
            var record = await _borrowRepository.GetActiveBorrowRecordByIdAndMemberAsync(request.BorrowRecordId, request.MemberId);
            if (record == null)
            {
                throw new InvalidOperationException("No active borrow record found for this book and member.");
            }

            var book = await _borrowRepository.GetBookByIdAsync(record.BookId);
            if (book == null)
            {
                throw new KeyNotFoundException($"Book {record.BookId} not found.");
            }

            record.ReturnDate = request.ReturnDate == default ? DateTime.UtcNow : request.ReturnDate;
            record.Status = "Returned";
            book.AvailableCopies += 1;

            await _borrowRepository.SaveChangesAsync();
            _cache.Remove("all_books");
            return MapToResult(record);
        }

        public async Task<IEnumerable<BorrowRecordResult>> GetBorrowRecordsByMemberIdAsync(Guid memberId)
        {
            var member = await _borrowRepository.GetMemberByIdAsync(memberId);
            if (member == null)
                throw new KeyNotFoundException($"Member {memberId} not found.");

            var records = await _borrowRepository.GetBorrowRecordsByMemberIdAsync(memberId);
            return records.Select(MapToResult);
        }

        public async Task<IEnumerable<BorrowRecordResult>> GetAllBorrowRecordsAsync()
        {
            var records = await _borrowRepository.GetAllBorrowRecordsAsync();
            return records.Select(MapToResult);
        }

        private static BorrowRecordResult MapToResult(BorrowRecord record) => new BorrowRecordResult
        {
            Id = record.Id,
            BookId = record.BookId,
            MemberId = record.MemberId,
            BorrowDate = record.BorrowDate,
            ReturnDate = record.ReturnDate,
            Status = record.Status
        };
    }
}