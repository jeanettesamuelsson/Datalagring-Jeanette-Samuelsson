using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Abstractions.Persistence;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
}
