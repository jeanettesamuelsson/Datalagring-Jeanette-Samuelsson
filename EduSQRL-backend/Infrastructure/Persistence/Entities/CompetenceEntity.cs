namespace Infrastructure.Persistence.Entities;

public class CompetenceEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!; 
    public byte[] Concurrency { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public virtual ICollection<ParticipantEntity> Instructors { get; set; } = [];
}
