using NSubstitute;
using Xunit;
using Application.Modules.Locations;
using Application.Modules.Locations.Input;
using Application.Abstractions.Persistence;
using Domain.Models;

namespace Tests.Unit;

public class LocationServiceTests
{
    // private fields
    private readonly ILocationRepository _locationRepoMock;
    private readonly IUnitOfWork _uowMock;
    private readonly LocationService _service;

    // --- CONSTRUCTOR ---
    public LocationServiceTests()
    {
        _locationRepoMock = Substitute.For<ILocationRepository>();
        _uowMock = Substitute.For<IUnitOfWork>();

        // create service and send in mocks
        _service = new LocationService(
            _locationRepoMock,
            _uowMock
        );
    }

  

    [Fact]
    public async Task CreateAsync_ShouldReturnGuid_WhenLocationIsCreated()
    {
        // Arrange
        var input = new CreateLocationInput(Name: "TestCity");

        // Act
        var resultId = await _service.CreateAsync(input, CancellationToken.None);

        // Assert

        // check that a valid Guid is returned

        Assert.NotEqual(Guid.Empty, resultId);

        // check that AddAsync is called with a Location object that has the correct properties

        await _locationRepoMock.Received(1).AddAsync(
            Arg.Is<Location>(l =>
                l.Name == input.Name &&
                l.Id == resultId &&
                l.RowVersion.Length == 0),   // RowVersion should be empty for a new entity
            Arg.Any<CancellationToken>()
        );

        // Check that Unit of Work saves the changes
        await _uowMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowArgumentException_WhenLocationDoesNotExist()
    {
        // Arrange
        var locationId = Guid.NewGuid();
        var rowVersion = new byte[] { 1, 2, 3 };

        // Setup: GetByIdAsync return null (location does not exist)
        _locationRepoMock.GetByIdAsync(locationId, Arg.Any<CancellationToken>())
            .Returns((Location?)null);

        // Act & Assert

        // check that exception is thrown with the correct message when location is not found

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.DeleteAsync(locationId, rowVersion, CancellationToken.None));

        Assert.Equal("Location not found", exception.Message);

        // Kontrollera att DeleteAsync och SaveChanges aldrig anropades
        await _locationRepoMock.DidNotReceive().DeleteAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        await _uowMock.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}