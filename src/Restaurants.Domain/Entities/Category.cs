using Restaurants.Domain.Entities.Base;

namespace Restaurants.Domain.Entities;

public class Category : BaseEntity<int>
{
    public string Name { get; set; } = default!;
    #region Relations
    public List<Restaurant> Restaurants { get; set; } = new();
    #endregion

}
