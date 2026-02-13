using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Modules.Registrations.Input;

public sealed record UpdateRegistrationInput(

    Guid Id,
    RegistrationStatus Status, 
    byte[] RowVersion        
    
);
