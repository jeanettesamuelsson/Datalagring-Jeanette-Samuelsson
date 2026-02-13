using Domain.Models;

namespace Presentation.Dtos.Registration;

public sealed record UpdateRegistrationRequest(

    Guid Id,
    RegistrationStatus Status, 
    byte[] RowVersion   
    
);
