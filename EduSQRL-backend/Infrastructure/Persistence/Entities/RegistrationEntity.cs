using Application.Abstractions.Persistence;
using Domain.Models;

namespace Infrastructure.Persistence.Entities;

internal class RegistrationEntity : IEntity<Guid>
{
    public Guid Id { get; set; }

    public byte[] Concurrency { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public RegistrationStatus Status { get; set; }

    //connection to participant and course session
    public Guid ParticipantId { get; set; } 
    public Guid CourseSessionId { get; set; } 
    public ParticipantEntity Participant { get; set; } = null!;
    public CourseSessionEntity CourseSession { get; set; } = null!;
}
