using Domain.Persistence;

namespace Infrastructure.Persistence.Entities;

public class LocationEntity : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!; 
    public byte[] Concurrency { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }

    // navigation properties
    public virtual ICollection<CourseSessionEntity> CourseSessions { get; set; } = [];

}
