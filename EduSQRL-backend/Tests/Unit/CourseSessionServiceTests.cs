using NSubstitute;
using Xunit;
using Application.Modules.CourseSessions;
using Application.Modules.CourseSessions.Input;
using Application.Modules.Courses;

using Domain.Models;
using Domain.RepositoryInterfaces;
using Domain.Persistence;

namespace Tests.Unit;

public class CourseSessionServiceTests
{
    
    // private fields
    private readonly ICourseSessionRepository _sessionRepoMock;
    private readonly ICourseRepository _courseRepoMock;
    private readonly ILocationRepository _locationRepoMock;
    private readonly IUnitOfWork _uowMock;
    private readonly CourseSessionService _service;

    // --- CONSTRUCTOR ---
    public CourseSessionServiceTests()
    {
        _sessionRepoMock = Substitute.For<ICourseSessionRepository>();
        _courseRepoMock = Substitute.For<ICourseRepository>();
        _locationRepoMock = Substitute.For<ILocationRepository>();
        _uowMock = Substitute.For<IUnitOfWork>();

        // create service and send in mocks

        _service = new CourseSessionService(
            _sessionRepoMock,
            _courseRepoMock,
            _locationRepoMock,
            _uowMock
        );


    }

    // --- helper method for creating test data
    
    private CreateCourseSessionInput CreateTestInput()
    {
        return new CreateCourseSessionInput(
            CourseId: Guid.NewGuid(),
            LocationId: Guid.NewGuid(),
            CourseName: "Testkurs",
            LocationName: "Testplats",
            StartDate: DateTime.Now.AddDays(7),
            EndDate: DateTime.Now.AddDays(14),
            Capacity: 15
        );
    }

   
    [Fact]
    public async Task CreateAsync_ShouldReturnGuid_WhenCourseAndLocationExist()
    {
        // Arrange
        var input = CreateTestInput();

        _courseRepoMock.GetByIdAsync(input.CourseId, Arg.Any<CancellationToken>())
     .Returns(new Course(
         input.CourseId,
         "Programmering 1",
         "PRG01",
         "En kurs", 
         []             
     ));

        _locationRepoMock.GetByIdAsync(input.LocationId, Arg.Any<CancellationToken>())
            .Returns(new Location(input.LocationId, "Nyköping", []));

        // Act
        var resultId = await _service.CreateAsync(input, CancellationToken.None);

        // Assert

        // check ID not empty

        Assert.NotEqual(Guid.Empty, resultId);

        // check that repo and uow was called once

        await _sessionRepoMock.Received(1).AddAsync(Arg.Any<CourseSession>(), Arg.Any<CancellationToken>());
        await _uowMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    
    [Fact]
    public async Task CreateAsync_ShouldThrowArgumentException_WhenCourseIsMissing()
    {
        // 1. Arrange

        var input = CreateTestInput();

        // return null when repo.GetById is called

        _courseRepoMock.GetByIdAsync(input.CourseId, Arg.Any<CancellationToken>())
            .Returns((Course?)null);

        // 2. Act & 3. Assert

        // check that exception is thrown and correct exception.message

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.CreateAsync(input, CancellationToken.None));

        Assert.Equal($"Course with ID {input.CourseId} does not exist", exception.Message);

        // check that repo and uow was never called 

        await _sessionRepoMock.DidNotReceive().AddAsync(Arg.Any<CourseSession>(), Arg.Any<CancellationToken>());
        await _uowMock.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

   
}