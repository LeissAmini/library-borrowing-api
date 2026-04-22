using Library.Api.DTOs;
using Library.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

[ApiController]
[Route("api")]
public class BorrowController : ControllerBase
{
  private readonly IBorrowService _borrowService;

  public BorrowController(IBorrowService borrowService)
  {
    _borrowService = borrowService;
  }

  [HttpGet("")]
}