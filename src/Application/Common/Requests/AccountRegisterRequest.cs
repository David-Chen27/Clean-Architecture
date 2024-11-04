using Clean_Architecture.Domain.ValueObjects;

namespace Clean_Architecture.Application.Common.Requests;

public class AccountRegisterRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string UserName { get; init; }
    public string? Title { get; init; }
    public string? TelEx { get; init; }
    public Sex Sex { get; init; } = Sex.Null;
    public string? IdNumber { get; init; }
    public DateTime Birthday { get; init; }
    public string? Contact { get; init; }
    public string? Post { get; init; }
}
