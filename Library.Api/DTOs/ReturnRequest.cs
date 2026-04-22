namespace Library.Api.DTOs
{
  public class ReturnRequest
  {
    public Guid BorrowRecordId { get; set; }
    public DateTime ReturnDate { get; set; }
  }
}