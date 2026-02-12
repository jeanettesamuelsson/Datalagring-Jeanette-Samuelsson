
namespace Domain.Models;

public sealed record Location(

    Guid Id,
    string Name,
    byte[] RowVersion 

);
