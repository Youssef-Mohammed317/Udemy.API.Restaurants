namespace Restaurants.Domain.Entities.Base;

public abstract class BaseEntity<TKey> : IAuditableEntity
{
    public TKey Id { get; set; } = default!;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
