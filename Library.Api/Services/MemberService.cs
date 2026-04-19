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
            Email = m.Email,
            MembershipDate = m.MembershipDate
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
            Email = member.Email,
            MembershipDate = member.MembershipDate
        };
    }

    public async Task<MemberResponse> CreateMemberAsync(CreateMemberRequest request)
    {
        var member = new Member
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email,
            MembershipDate = DateTime.UtcNow
        };

        var created = await _memberRepository.AddAsync(member);

        return new MemberResponse
        {
            Id = created.Id,
            FullName = created.FullName,
            Email = created.Email,
            MembershipDate = created.MembershipDate
        };
    }

    public async Task<MemberResponse?> UpdateMemberAsync(Guid id, CreateMemberRequest request)
    {
        var member = await _memberRepository.GetByIdAsync(id);

        if (member is null)
        {
            return null;
        }

        member.FullName = request.FullName;
        member.Email = request.Email;

        var updated = await _memberRepository.UpdateAsync(member);

        return new MemberResponse
        {
            Id = updated.Id,
            FullName = updated.FullName,
            Email = updated.Email,
            MembershipDate = updated.MembershipDate
        };
    }

    public async Task<bool> DeleteMemberAsync(Guid id)
    {
        return await _memberRepository.DeleteAsync(id);
    }
}