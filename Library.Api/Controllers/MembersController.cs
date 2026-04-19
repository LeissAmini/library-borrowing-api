using Library.Api.DTOs;
using Library.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace Library.Api.Controllers;

[ApiController]
[Route("api/members")]
public class MembersController : ControllerBase
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    [HttpGet]
    public async Task<ActionResult<List<MemberResponse>>> GetMembers()
    {
        var members = await _memberService.GetMembersAsync();
        return Ok(members);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MemberResponse>> GetMemberById(Guid id)
    {
        var member = await _memberService.GetMemberByIdAsync(id);

        if (member is null)
        {
            return NotFound(new { error = "Member not found." });
        }

        return Ok(member);
    }

    [HttpPost]
    public async Task<ActionResult<MemberResponse>> CreateMember([FromBody] CreateMemberRequest request)
    {
        var validationError = ValidateMemberRequest(request);
        if (validationError is not null)
        {
            return BadRequest(new { error = validationError });
        }

        var created = await _memberService.CreateMemberAsync(request);

        return CreatedAtAction(nameof(GetMemberById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<MemberResponse>> UpdateMember(Guid id, [FromBody] CreateMemberRequest request)
    {
        var validationError = ValidateMemberRequest(request);
        if (validationError is not null)
        {
            return BadRequest(new { error = validationError });
        }

        var updated = await _memberService.UpdateMemberAsync(id, request);

        if (updated is null)
        {
            return NotFound(new { error = "Member not found." });
        }

        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMember(Guid id)
    {
        var deleted = await _memberService.DeleteMemberAsync(id);

        if (!deleted)
        {
            return NotFound(new { error = "Member not found." });
        }

        return NoContent();
    }

    private static string? ValidateMemberRequest(CreateMemberRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName))
        {
            return "FullName is required.";
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return "Email is required.";
        }

        try
        {
            _ = new MailAddress(request.Email);
        }
        catch
        {
            return "Email must be valid.";
        }

        return null;
    }
}