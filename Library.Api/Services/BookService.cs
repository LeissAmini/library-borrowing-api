using Library.Api.DTOs;
using Library.Api.Models;
using Library.Api.Repositories;

namespace Library.Api.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<List<BookResponse>> GetBooksAsync()
    {
        var books = await _bookRepository.GetAllAsync();

        return books.Select(bk => new BookResponse
        {
            Id = bk.Id,
            Title = bk.Title,
            Author = bk.Author,
            ISBN = bk.ISBN,
            TotalCopies = bk.TotalCopies,
            AvailableCopies = bk.AvailableCopies
        }).ToList();
    }

    public async Task<BookResponse?> GetBookByIdAsync(Guid id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book is null)
        {
            return null;
        }

        return new BookResponse
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            ISBN = book.ISBN,
            TotalCopies = book.TotalCopies,
            AvailableCopies = book.AvailableCopies
            
        };
    }

    public async Task<BookResponse> CreateBookAsync(CreateBookRequest request)
    {
        ValidateBookRules(request);
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Author = request.Author,
            ISBN = request.ISBN,
            TotalCopies = request.TotalCopies,
            AvailableCopies = request.AvailableCopies
   
        };

        var created = await _bookRepository.AddAsync(book);

        return new BookResponse
        {
            Id = created.Id,
            Title = created.Title,
            Author = created.Author,
            ISBN = created.ISBN,
            TotalCopies = created.TotalCopies,
            AvailableCopies = created.AvailableCopies
        };
    }

    public async Task<BookResponse?> UpdateBookAsync(Guid id, CreateBookRequest request)
    {
        ValidateBookRules(request);
        var book = await _bookRepository.GetByIdAsync(id);

        if (book is null)
        {
            return null;
        }

        book.Title = request.Title;
        book.Author = request.Author;
        book.ISBN = request.ISBN;
        book.TotalCopies = request.TotalCopies;
        book.AvailableCopies = request.AvailableCopies;

        
        var updated = await _bookRepository.UpdateAsync(book);

        return new BookResponse
        {
            Id = updated.Id,
            Title = updated.Title,
            Author = updated.Author,
            ISBN = updated.ISBN,
            TotalCopies = updated.TotalCopies,
            AvailableCopies = updated.AvailableCopies
          
        };
    }
    public async Task<bool> DeleteBookAsync(Guid id)
    {
        return await _bookRepository.DeleteAsync(id);
    }
    private static void ValidateBookRules(CreateBookRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new ArgumentException("Title is required.");

        if (string.IsNullOrWhiteSpace(request.Author))
            throw new ArgumentException("Author is required.");

        if (string.IsNullOrWhiteSpace(request.ISBN))
            throw new ArgumentException("ISBN is required.");

        if (request.TotalCopies <= 0)
            throw new ArgumentException("Total Copies must be greater than 0.");

        if (request.AvailableCopies < 0)
            throw new ArgumentException("Available Copies must be greater than or equal to 0.");

        if (request.AvailableCopies > request.TotalCopies)
            throw new ArgumentException("Available Copies must not exceed TotalCopies.");
    }
}