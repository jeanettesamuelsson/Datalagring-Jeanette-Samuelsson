using Domain.Models;

namespace Presentation.Dtos.Registration;

public sealed record RegistrationDto(

    Guid Id,
    string ParticipantName,
    string CourseName,
    DateTime StartDate,
    RegistrationStatus Status,
    byte[] RowVersion

);
