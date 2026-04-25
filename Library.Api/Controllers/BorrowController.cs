using System.Runtime.InteropServices;
using Library.Api.DTOs;
using Library.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

[ApiController]
[Route("api/borrowRecords")]
public class BorrowController : ControllerBase
{
  private readonly IBorrowService _borrowService;

  public BorrowController(IBorrowService borrowService)
  {
    _borrowService = borrowService;
  }

  [HttpGet("all")]
  public async Task<ActionResult<IEnumerable<BorrowRecordResult>>> GetBorrowRecords()
  {
    var borrowRecords = await _borrowService.GetAllBorrowRecordsAsync();
    return Ok(borrowRecords);
  }

  [HttpGet("member/{memberId}")]
  public async Task<ActionResult<IEnumerable<BorrowRecordResult>>> GetBorrowRecordsByMemberId(Guid memberId)
  {
    var borrowRecords = await _borrowService.GetBorrowRecordsByMemberIdAsync(memberId);
    return Ok(borrowRecords);
  }

  [HttpPost("borrow")]
  public async Task<ActionResult<BorrowRecordResult>> BorrowBook([FromBody] BorrowRequest request)
  {
    Console.WriteLine("BorrowBook called with BookId: {0}, MemberId: {1}, BorrowDate: {2}", request.BookId, request.MemberId, request.BorrowDate);
    var borrowRecord = await _borrowService.BorrowBookAsync(request);
    return Ok(borrowRecord);
  }

  [HttpPost("return")]
  public async Task<ActionResult<BorrowRecordResult>> ReturnBook([FromBody] ReturnRequest request)
  {
    var borrowRecord = await _borrowService.ReturnBookAsync(request);
    return Ok(borrowRecord);
  }
}