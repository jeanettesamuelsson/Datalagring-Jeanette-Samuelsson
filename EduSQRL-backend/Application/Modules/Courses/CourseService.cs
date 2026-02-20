using Application.Abstractions.Persistence;
using Application.Modules.Courses.Input;
using Application.Modules.Courses.Output;
using Domain.Models;



namespace Application.Modules.Courses;

public class CourseService(
    
    ICourseRepository courses, 
    IUnitOfWork uow
    ) : ICourseService


{
    // map from Domain to output
    private static CourseOutput ToOutputModel(Course c) => new(
        c.Id,
        c.CourseName,
        c.CourseCode,
        c.Description,
        c.RowVersion

        );

    // create 
    public async Task<Guid> CreateAsync(CreateCourseInput input, CancellationToken ct)
    {

        var courseId = Guid.NewGuid();
        
        var course = new Course(
            Id: courseId,
            CourseName: input.CourseName,
            CourseCode: input.CourseCode,
            Description: input.Description,
            RowVersion: Array.Empty<byte>()

            );

        await courses.AddAsync(course, ct);

        await uow.SaveChangesAsync(ct);

        return courseId;

    }

    // read (all and by id)
    public async Task<IReadOnlyList<CourseOutput>> GetAllCoursesAsync(CancellationToken ct = default)
    {
        var list = await courses.ListAsync(ct);

        return [.. list.Select(ToOutputModel)];
    }

    public async Task<CourseOutput?> GetByIdAsync(Guid courseId, CancellationToken ct)
    {
        var course = await courses.GetByIdAsync(courseId, ct);

        return course is null ? null : ToOutputModel(course);
    }


    //update
    public async Task<CourseOutput?> UpdateAsync(UpdateCourseInput input, CancellationToken ct)
    {
        var course = await courses.GetByIdAsync(input.Id, ct);
        if (course is null)
            return null;

        // mapper

        var updatedCourse = course with
        {
            CourseName = input.CourseName,
            CourseCode = input.CourseCode,
            Description = input.Description,
            RowVersion = input.RowVersion 
        };

        await courses.UpdateAsync(updatedCourse, ct);
        await uow.SaveChangesAsync(ct);

        // get updated and return

        var result = await courses.GetByIdAsync(input.Id, ct);
        return result is null ? null : ToOutputModel(result);


    }
    

   
    //delete
    public async Task DeleteAsync(Guid courseId, byte[] rowVersion, CancellationToken ct = default)
    {
        var course = await courses.GetByIdAsync(courseId, ct)
                       ?? throw new ArgumentException("Course not found");

        await courses.UpdateAsync(course with { RowVersion = rowVersion }, ct);

        await courses.DeleteAsync(courseId, ct);

        await uow.SaveChangesAsync(ct);
    }


}
