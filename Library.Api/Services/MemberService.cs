using Library.Api.DTOs;
using Library.Api.Models;
using Library.Api.Repositories;

namespace Library.Api.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;

    public MemberService(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<List<MemberResponse>> GetMembersAsync()
    {
        var members = await _memberRepository.GetAllAsync();

        return members.Select(m => new MemberResponse
        {
            Id = m.Id,
            FullName = m.FullName,
            Email = m.Email
        }).ToList();
    }

    public async Task<MemberResponse?> GetMemberByIdAsync(Guid id)
    {
        var member = await _memberRepository.GetByIdAsync(id);

        if (member is null)
        {
            return null;
        }

        return new MemberResponse
        {
            Id = member.Id,
            FullName = member.FullName,
            Email = member.Email
        };
    }

    public async Task<MemberResponse> CreateMemberAsync(CreateMemberRequest request)
    {
        var member = new Member
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email
        };

        var created = await _memberRepository.AddAsync(member);

        return new MemberResponse
        {
            Id = created.Id,
            FullName = created.FullName,
            Email = created.Email
        };
    }
}