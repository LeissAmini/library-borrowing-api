using Library.Api.Data;
using Library.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Repositories
{
  public class BorrowRepository : IBorrowRepository
  {
    private readonly ApplicationDbContext _context;

    public BorrowRepository(ApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<BorrowRecord>> GetAllBorrowRecordsAsync()
    {
      return await _context.BorrowRecords
      .AsNoTracking()
      .ToListAsync();
    }

    public async Task<IEnumerable<BorrowRecord>> GetBorrowRecordsByMemberIdAsync(Guid memberId)
    {
      return await _context.BorrowRecords
      .AsNoTracking()
      .Where(br => br.MemberId == memberId)
      .ToListAsync();
    }

    public async Task<BorrowRecord?> GetBorrowRecordByIdAsync(Guid borrowRecordId)
    {
      return await _context.BorrowRecords.FindAsync(borrowRecordId);
    }

    public async Task<BorrowRecord?> GetActiveBorrowRecordByIdAndMemberAsync(Guid borrowRecordId, Guid memberId)
    {
      return await _context.BorrowRecords
        .FirstOrDefaultAsync(br =>
          br.Id == borrowRecordId &&
          br.MemberId == memberId &&
          br.ReturnDate == null);
    }

    public async Task<bool> HasActiveBorrowRecordAsync(Guid bookId, Guid memberId)
    {
      return await _context.BorrowRecords
        .AnyAsync(br =>
          br.BookId == bookId &&
          br.MemberId == memberId &&
          br.ReturnDate == null);
    }

    public async Task<Book?> GetBookByIdAsync(Guid bookId)
    {
      return await _context.Books.FindAsync(bookId);
    }

    public async Task<Member?> GetMemberByIdAsync(Guid memberId)
    {
      return await _context.Members.FindAsync(memberId);
    }

    public Task AddBorrowRecordAsync(BorrowRecord borrowRecord)
    {
      _context.BorrowRecords.Add(borrowRecord);
      return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
      await _context.SaveChangesAsync();
    }
  }
}