namespace Library.Api.Models;

public class Member
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<BorrowRecord> BorrowRecords { get; set; } = new();
}