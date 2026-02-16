

namespace Domain.Models;

public enum RegistrationStatus
{
    Pending = 1,
    Registered = 2,   
    Waitlisted = 3,  // Registered on waitlist if course session is full
    Cancelled = 4    
}
