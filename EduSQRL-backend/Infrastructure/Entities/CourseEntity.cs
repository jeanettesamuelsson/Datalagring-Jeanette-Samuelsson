
namespace Infrastructure.Entities;

public class CourseEntity
{
    public Guid Id { get; set; }
    public string CourseName { get; set; } = null!;
    public string CourseCode { get; set; } = null!;
    public string Description { get; set; } = null!;
    public byte[] Concurrency { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
}
