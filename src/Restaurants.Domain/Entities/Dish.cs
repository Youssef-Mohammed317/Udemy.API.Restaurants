using Restaurants.Domain.Entities.Base;

namespace Restaurants.Domain.Entities;

public class Dish : BaseEntity<int>
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }

    public int? KiloCalories { get; set; }

    #region Relations
    public int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = default!;
    #endregion

}
