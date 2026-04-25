using Microsoft.EntityFrameworkCore;
using Library.Api.Models;

namespace Library.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Book> Books => Set<Book>();
        public DbSet<Member> Members => Set<Member>();
        public DbSet<BorrowRecord> BorrowRecords => Set<BorrowRecord>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BorrowRecord>()
                .HasOne<Book>()
                .WithMany()
                .HasForeignKey(br => br.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BorrowRecord>()
                .HasOne<Member>()
                .WithMany()
                .HasForeignKey(br => br.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BorrowRecord>()
                .Ignore(br => br.IsActive);
        }
    }
}
