
namespace Application.Abstractions.Persistence;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
}
