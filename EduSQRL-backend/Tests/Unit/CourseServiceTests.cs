using NSubstitute;
using Xunit;
using Application.Modules.Courses;
using Application.Modules.Courses.Input;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Domain.Persistence;

namespace Tests.Unit;

public class CourseServiceTests
{
    private readonly ICourseRepository _courseRepoMock;
    private readonly IUnitOfWork _uowMock;
    private readonly CourseService _service;

    public CourseServiceTests()
    {
        _courseRepoMock = Substitute.For<ICourseRepository>();
        _uowMock = Substitute.For<IUnitOfWork>();

        _service = new CourseService(
            _courseRepoMock,
            _uowMock
        );
    }

    private CreateCourseInput CreateTestInput()
    {
        return new CreateCourseInput(
            CourseName: "Tests 101",
            CourseCode: "TTT-101",
            Description: "Beskrivning"
        );
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnGuidAndSave_WhenInputIsValid()
    {
        // Arrange
        var input = CreateTestInput();

        // Act
        var resultId = await _service.CreateAsync(input, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, resultId);

        // check that addAsync was called with a Course object that has the correct properties
        await _courseRepoMock.Received(1).AddAsync(
            Arg.Is<Course>(c =>
                c.CourseName == input.CourseName &&
                c.CourseCode == input.CourseCode &&
                c.RowVersion.Length == 0),   // empty RowVersion for new entity
            Arg.Any<CancellationToken>()
        );

        // Check that Unit of Work saves the changes
        await _uowMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedCourse_WhenCourseExists()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var oldVersion = new byte[] { 1, 1, 1 };
        var newVersion = new byte[] { 2, 2, 2 };

        var input = new UpdateCourseInput(
            Id: courseId,
            CourseName: "Uppdaterat namn",
            CourseCode: "TDD102",
            Description: "Ny beskrivning",
            RowVersion: oldVersion
        );

        var existingCourse = new Course(
            Id: courseId,
            CourseName: "Gammalt namn",
            CourseCode: "TDD101",
            Description: "Gammal beskrivning",
            RowVersion: oldVersion
        );

        var updatedCourse = existingCourse with
        {
            CourseName = input.CourseName,
            RowVersion = newVersion // new row version from database 
        };

        // Setup: return existing course on first call, then updated course on second call (after update)

        _courseRepoMock.GetByIdAsync(courseId, Arg.Any<CancellationToken>())
            .Returns(existingCourse, updatedCourse);

        // Act

        var result = await _service.UpdateAsync(input, CancellationToken.None);

        // Assert

        // check result is not null and has the updated course name

        Assert.NotNull(result);
        Assert.Equal(input.CourseName, result.CourseName);

        // Check that repository was called and the right RowVersion was used (concurrency check)

        await _courseRepoMock.Received(1).UpdateAsync(
            Arg.Is<Course>(c => c.RowVersion == oldVersion),
            Arg.Any<CancellationToken>()
        );

        // check that the course was saved with uow

        await _uowMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}