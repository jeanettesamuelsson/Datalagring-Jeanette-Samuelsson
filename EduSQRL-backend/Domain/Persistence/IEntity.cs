
namespace Domain.Persistence;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
}
