using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Modules.Locations.Input;

public sealed record UpdateLocationInput(

    Guid Id,
    string Name,
    byte[] RowVersion 

);