using Library.Api.Data;
using Library.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Repositories;

public class BookRepository : IBookRepository
{
    private readonly ApplicationDbContext _context;

    public BookRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> GetAllAsync()
    {
        return await _context.Books.ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(Guid id)
    {
        return await _context.Books.FindAsync(id);
    }

    public async Task<Book> AddAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<Book> UpdateAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book is null)
        {
            return false;
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return true;
    }
}