namespace Restaurants.Domain.Repositories;

public interface IUnitOfWork
{
    IRestaurantRepository RestaurantRepository { get; }
    IDishRepository DishRepository { get; }
    ICategoryRepository CategoryRepository { get; }

    Task<int> CommitAsync();
}
