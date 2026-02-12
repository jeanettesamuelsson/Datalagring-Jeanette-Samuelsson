namespace Presentation.Dtos.Location;

public sealed record UpdateLocationRequest(

    Guid Id,
    string Name,
    byte[] RowVersion 

);
