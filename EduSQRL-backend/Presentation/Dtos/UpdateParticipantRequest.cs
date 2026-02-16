namespace Presentation.Dtos;

public record UpdateParticipantRequest
{
    public required Guid Id { get; init; } 
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }

    public required Guid RoleId { get; init; }
    public required byte[] RowVersion { get; init; }
}
