using Microsoft.EntityFrameworkCore;
using Library.Api.Models;

namespace Library.Api.Data
{
  public  class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}
    public DbSet<BorrowRecord> BorrowRecords => Set<BorrowRecord>();
  }
}
