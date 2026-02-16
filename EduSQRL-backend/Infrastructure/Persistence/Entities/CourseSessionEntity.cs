using Application.Abstractions.Persistence;

namespace Infrastructure.Persistence.Entities;

public class CourseSessionEntity : IEntity<Guid>
{
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; } 
    public DateTime EndDate { get; set; }   
    public int Capacity { get; set; }      
    public byte[] Concurrency { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }

    public Guid CourseId { get; set; } //FK
    public Guid LocationId { get; set; } //FK

    // Navigation Properties
    public CourseEntity Course { get; set; } = null!;

    public LocationEntity Location { get; set; } = null!;

    public ICollection<RegistrationEntity> Registrations { get; set; } = [];
}
