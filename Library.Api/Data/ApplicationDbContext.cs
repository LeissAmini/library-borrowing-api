using Library.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<BorrowRecord> BorrowRecords => Set<BorrowRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BorrowRecord>()
            .HasOne(br => br.Member)
            .WithMany(m => m.BorrowRecords)
            .HasForeignKey(br => br.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BorrowRecord>()
            .HasOne(br => br.Book)
            .WithMany()
            .HasForeignKey(br => br.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BorrowRecord>()
            .Ignore(br => br.IsActive);
    }
}
