using Library.Api.Models;

namespace Library.Api.Repositories;

public interface IMemberRepository
{
    Task<List<Member>> GetAllAsync();
    Task<Member?> GetByIdAsync(Guid id);
    Task<Member> AddAsync(Member member);
    Task<Member> UpdateAsync(Member member);
    Task<bool> DeleteAsync(Guid id);
}