namespace Library.Api.Models;

public class BorrowRecord
{
    public Guid Id { get; set; }

    public Guid MemberId { get; set; }
    public Member Member { get; set; } = null!;
}