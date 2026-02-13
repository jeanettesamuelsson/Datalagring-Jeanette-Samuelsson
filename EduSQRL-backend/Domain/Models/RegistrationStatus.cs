

namespace Domain.Models;

public enum RegistrationStatus
{

    Registered = 1,   
    Waitlisted = 2,  // Registered on waitlist if course session is full
    Cancelled = 3     
}
