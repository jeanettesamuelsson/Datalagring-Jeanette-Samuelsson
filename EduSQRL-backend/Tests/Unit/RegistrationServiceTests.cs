using Application.Abstractions.Persistence;
using Application.Modules.Registrations;
using Application.Modules.Registrations.Input;
using Domain.Models;
using NSubstitute;

namespace Tests.Unit;

public class RegistrationServiceTests
{
    // private fields
    private readonly IRegistrationRepository _registrationsRepoMock;
    private readonly IUnitOfWork _uowMock;
    private readonly RegistrationService _service;

    // --- CONSTRUCTOR ---
    public RegistrationServiceTests()
    {
        _registrationsRepoMock = Substitute.For<IRegistrationRepository>();
        _uowMock = Substitute.For<IUnitOfWork>();

        // create service and send in mocks
        _service = new RegistrationService(
            _registrationsRepoMock,
            _uowMock
        );
    }

    // --- helper method for creating test data ---
    private CreateRegistrationInput CreateTestInput()
    {
        return new CreateRegistrationInput(
            ParticipantId: Guid.NewGuid(),
            CourseSessionId: Guid.NewGuid()
        );
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnGuid_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var input = CreateTestInput();

        // Setup: Registration does not exist (returnfalse)

        _registrationsRepoMock.AlreadyExistsAsync(
            input.ParticipantId,
            input.CourseSessionId,
            Arg.Any<CancellationToken>()
        ).Returns(false);

        // Act
        var resultId = await _service.CreateAsync(input, CancellationToken.None);

        // Assert
        
        //check that result guid is not empty

        Assert.NotEqual(Guid.Empty, resultId);

        // check that AddAsync was called with registration with the correct data

        await _registrationsRepoMock.Received(1).AddAsync(
            Arg.Is<Registration>(r =>
                r.ParticipantId == input.ParticipantId &&
                r.CourseSessionId == input.CourseSessionId &&
                r.Status == RegistrationStatus.Pending),
            Arg.Any<CancellationToken>()
        );

        // check that saving was called once

        await _uowMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowArgumentException_WhenParticipantIsAlreadyRegistered()
    {
        // Arrange
        var input = CreateTestInput();

        // Setup: Registration already exists (returns true)

        _registrationsRepoMock.AlreadyExistsAsync(
            input.ParticipantId,
            input.CourseSessionId,
            Arg.Any<CancellationToken>()
        ).Returns(true);

        // Act & Assert

        // Check that exception is thrown with the correct message

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.CreateAsync(input, CancellationToken.None));

        Assert.Equal("Already Registered!", exception.Message);

        // Check that AddAsync and SaveChanges were never called since validation failed

        await _registrationsRepoMock.DidNotReceive().AddAsync(Arg.Any<Registration>(), Arg.Any<CancellationToken>());
        await _uowMock.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}