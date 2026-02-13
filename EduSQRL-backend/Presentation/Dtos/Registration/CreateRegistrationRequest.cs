namespace Presentation.Dtos.Registration;

public sealed record CreateRegistrationRequest(

    Guid ParticipantId,
    Guid CourseSessionId

);