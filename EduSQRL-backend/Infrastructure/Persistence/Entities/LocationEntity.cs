using Application.Abstractions.Persistence;

namespace Infrastructure.Persistence.Entities;

public class LocationEntity : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!; 
    public byte[] Concurrency { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }

    public virtual ICollection<CourseSessionEntity> CourseSessions { get; set; } = [];

}
