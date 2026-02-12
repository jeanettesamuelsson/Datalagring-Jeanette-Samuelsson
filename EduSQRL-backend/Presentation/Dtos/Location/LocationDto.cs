namespace Presentation.Dtos.Location;

public sealed record LocationDto(

    Guid Id,
    string Name,
    byte[] RowVersion

);
