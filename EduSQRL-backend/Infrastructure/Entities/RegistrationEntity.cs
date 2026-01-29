

namespace Infrastructure.Entities;

internal class RegistrationEntity
{
    public Guid Id { get; set; }

    //connection to participant and course session
    public Guid ParticipantId { get; set; } 
    public Guid CourseSessionId { get; set; } 

    public byte[] Concurrency { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }

    public ParticipantEntity Participant { get; set; } = null!;
    public CourseSessionEntity CourseSession { get; set; } = null!;
}
