using Library.Api.DTOs;

namespace Library.Api.Services;

public interface IMemberService
{
    Task<List<MemberResponse>> GetMembersAsync();
    Task<MemberResponse?> GetMemberByIdAsync(Guid id);
    Task<MemberResponse> CreateMemberAsync(CreateMemberRequest request);
}