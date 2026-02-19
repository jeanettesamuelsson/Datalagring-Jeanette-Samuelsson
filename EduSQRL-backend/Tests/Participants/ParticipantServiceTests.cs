using Application.Abstractions.Persistence;
using Application.Modules.Participants;
using Application.Modules.Roles;
using Domain.Models;
using Domain.Participants.ValueObjects;
using NSubstitute;




namespace Tests.Participants;

public class ParticipantServiceTests
{
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnParticipant_WhenParticipantExists()
    {
        // --- ARRANGE ---

        // create mocks

        var repoMock = Substitute.For<IParticipantRepository>();
        var uowMock = Substitute.For<IUnitOfWork>(); 
        var roleServiceMock = Substitute.For<IRoleService>();

        // create service and send in mocks

        var service = new ParticipantService(repoMock, uowMock, roleServiceMock);

        var participantId = Guid.NewGuid();
       
        var phone = new PhoneNumber("070-123 45 67");

        // create participant
        var expectedParticipant = new Participant(
            participantId,               // Id
            "Nisse",                      // FirstName
            "Nosig",                      // LastName
            "nisse@ekorre.se",            // Email
            phone,                        // PhoneNumber
            Guid.NewGuid(),               // RoleId
            "Student",                    // RoleName
            DateTime.Now,                 // Created
            new byte[0]                   // RowVersion 
        );

        
        repoMock.GetByIdAsync(participantId).Returns(expectedParticipant);

        // --- ACT  ---
       
        var result = await service.GetByIdAsync(participantId, CancellationToken.None);

        // --- ASSERT  ---
       
        // check result not null

        Assert.NotNull(result);

        // check that the expected participant is the returning participant

        Assert.Equal(expectedParticipant.FirstName, result.FirstName);
        Assert.Equal(participantId, result.Id);

        // assert that repository was called once with right id and any ct
        await repoMock.Received(1).GetByIdAsync(participantId, Arg.Any<CancellationToken>());
    }


    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenParticipantDoesNotExist()
    {
        // --- ARRANGE ---

        // create mocks

        var repoMock = Substitute.For<IParticipantRepository>();
        var uowMock = Substitute.For<IUnitOfWork>();
        var roleServiceMock = Substitute.For<IRoleService>();

        var service = new ParticipantService(repoMock, uowMock, roleServiceMock);

        var nonExistentId = Guid.NewGuid();

        // return null when GetByIdAsync(nonExistentId) is called

        repoMock.GetByIdAsync(nonExistentId, Arg.Any<CancellationToken>())
                .Returns((Participant?)null);

        // --- ACT ---

        var result = await service.GetByIdAsync(nonExistentId, CancellationToken.None);

        // ---  ASSERT ---

        // result should be null

        Assert.Null(result);

        // repository was called once with the right id

        await repoMock.Received(1).GetByIdAsync(nonExistentId, Arg.Any<CancellationToken>());
    }
}
