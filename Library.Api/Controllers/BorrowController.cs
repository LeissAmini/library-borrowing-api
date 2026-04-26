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
    try
    {
      var borrowRecords = await _borrowService.GetBorrowRecordsByMemberIdAsync(memberId);
      return Ok(borrowRecords);
    }
    catch (KeyNotFoundException ex)
    {
      return NotFound(ex.Message);
    }
    
  }

  [HttpPost("borrow")]
  public async Task<ActionResult<BorrowRecordResult>> BorrowBook([FromBody] BorrowRequest request)
  {
    try
    {
      var borrowRecord = await _borrowService.BorrowBookAsync(request);
      return Ok(borrowRecord);
    }
    catch (KeyNotFoundException ex)
    {
      return NotFound(ex.Message);
    }
    catch (InvalidOperationException ex)
    {
      return Conflict(ex.Message);
    }
  }

  [HttpPost("return")]
  public async Task<ActionResult<BorrowRecordResult>> ReturnBook([FromBody] ReturnRequest request)
  {
    try
    {
      var borrowRecord = await _borrowService.ReturnBookAsync(request);
      return Ok(borrowRecord);
    }
    catch (KeyNotFoundException ex)
    {
      return NotFound(ex.Message);
    }
    catch (InvalidOperationException ex)
    {
      return Conflict(ex.Message);
    }
  }
}