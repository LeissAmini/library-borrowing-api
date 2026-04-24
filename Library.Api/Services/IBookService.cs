using Library.Api.DTOs;
namespace Library.Api.Services;

public interface IBookService
{
    Task<List<BookResponse>> GetBooksAsync();
    Task<BookResponse?> GetBookByIdAsync(Guid id);
    Task<BookResponse> CreateBookAsync(CreateBookRequest request);
    Task<BookResponse?> UpdateBookAsync(Guid id, CreateBookRequest request);
    Task<bool> DeleteBookAsync(Guid id);
}