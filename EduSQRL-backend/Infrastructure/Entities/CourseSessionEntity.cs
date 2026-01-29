
namespace Infrastructure.Entities;

public class CourseSessionEntity
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public DateTime StartDate { get; set; } 
    public DateTime EndDate { get; set; }   
    public int Capacity { get; set; }      
    public byte[] Concurrency { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }

    // Navigation Properties
    public CourseEntity Course { get; set; } = null!;
}
