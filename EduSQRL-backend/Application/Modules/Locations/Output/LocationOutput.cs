using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Modules.Locations.Output;

public sealed record LocationOutput(

    Guid Id,
    string Name,
    byte[] RowVersion

);
