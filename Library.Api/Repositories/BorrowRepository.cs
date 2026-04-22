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
      return await _context.BorrowRecords.ToListAsync();
    }

    public async Task<BorrowRecord?> GetBorrowRecordByMemberIdAsync(Guid memberId)
    {
      return await _context.BorrowRecords.FindAsync(memberId);
    }

    public async Task<BorrowRecord> AddBorrowRecordAsync(BorrowRecord borrowRecord)
    {
      _context.BorrowRecords.Add(borrowRecord);
      await _context.SaveChangesAsync();
      return borrowRecord;
    }

    public async Task UpdateBorrowRecordAsync(BorrowRecord borrowRecord)
    {
      _context.BorrowRecords.Update(borrowRecord);
      await _context.SaveChangesAsync();
    }
  }
}