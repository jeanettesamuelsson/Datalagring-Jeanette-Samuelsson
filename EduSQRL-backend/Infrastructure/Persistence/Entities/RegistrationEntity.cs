using Application.Abstractions.Persistence;
using Domain.Models;

namespace Infrastructure.Persistence.Entities;

public class RegistrationEntity : IEntity<Guid>
{
    public Guid Id { get; set; }

    public byte[] Concurrency { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public RegistrationStatus Status { get; set; }
    public Guid ParticipantId { get; set; } //FK
    public Guid CourseSessionId { get; set; } //FK

    // navigation properties
    public ParticipantEntity Participant { get; set; } = null!;
    public CourseSessionEntity CourseSession { get; set; } = null!;
}
