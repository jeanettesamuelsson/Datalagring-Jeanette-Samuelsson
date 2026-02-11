namespace Presentation.Dtos;

public record CreateParticipantRequest
{
    public required string FirstName { get; init; } 
    public required string LastName { get; init; } 
    public required string Email { get; init; } 
    public required string PhoneNumber { get; init; } 

    public required List<string> Roles { get; init; }
}
