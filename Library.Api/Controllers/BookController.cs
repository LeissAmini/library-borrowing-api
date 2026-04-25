using Library.Api.DTOs;
using Library.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

[ApiController]
[Route("api/books")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<ActionResult<List<BookResponse>>> GetBooks()
    {
        var books = await _bookService.GetBooksAsync();
        return Ok(books);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BookResponse>> GetBookById(Guid id)
    {
        var book = await _bookService.GetBookByIdAsync(id);

        if (book is null)
        {
            return NotFound(new { error = "Book not found." });
        }

        return Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<BookResponse>> CreateBook([FromBody] CreateBookRequest request)
    {
        var validationError = ValidateBookRequest(request);
        if (validationError is not null)
        {
            return BadRequest(new { error = validationError });
        }
        var created = await _bookService.CreateBookAsync(request);

        return CreatedAtAction(nameof(GetBookById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BookResponse>> UpdateBook(Guid id, [FromBody] CreateBookRequest request)
    {
        var validationError = ValidateBookRequest(request);
        if (validationError is not null)
        {
            return BadRequest(new { error = validationError });
        }

        var updated = await _bookService.UpdateBookAsync(id, request);

        if (updated is null)
        {
            return NotFound(new { error = "Book not found." });
        }

        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        var deleted = await _bookService.DeleteBookAsync(id);

        if (!deleted)
        {
            return NotFound(new { error = "Book not found." });
        }

        return NoContent();
    }

    private static string? ValidateBookRequest(CreateBookRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return "Title is required.";
        }
        if (string.IsNullOrWhiteSpace(request.Author))
        {
            return "Author is required.";
        }

        if (string.IsNullOrWhiteSpace(request.ISBN))
        {
            return "ISBN is required.";
        }

        //  Totalcopies and Available copies rules 
        if (request.TotalCopies <= 0)
        {
            return "Total Copies must be greater than 0.";
        }
        if (request.AvailableCopies < 0)
        {
            return "Available Copies must be greater than or equal to 0.";
        }
        if (request.AvailableCopies > request.TotalCopies)
        {
            return "Available Copies must not exceed TotalCopies.";
        }

        return null;
    }
}