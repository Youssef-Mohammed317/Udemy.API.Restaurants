using Restaurants.Domain.Entities.Base;
using Restaurants.Domain.ValueObjects;

namespace Restaurants.Domain.Entities;

public class Restaurant : BaseEntity<int>
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool HasDelivery { get; set; }

    public string? ContactEmail { get; set; }
    public string? ContactNumber { get; set; }
    #region ObjectValues
    public Address? Address { get; set; }
    #endregion
    #region Relations
    public int CategoryId { get; set; }
    public Category Category { get; set; } = default!;
    public List<Dish> Dishes { get; set; } = new();

    public string OwnerId { get; set; } = default!;
    public User Owner { get; set; } = default!;
    #endregion


}
