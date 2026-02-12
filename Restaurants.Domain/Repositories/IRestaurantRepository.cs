using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IUnitOfWork
{
    IRestaurantRepository RestaurantRepository { get; }
    IDishRepository DishRepository { get; }
    ICategoryRepository CategoryRepository { get; }

    Task<int> CommitAsync();
}
public interface IRepository<TEntity, TKey>
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetById(TKey id);
    void Update(TEntity entity);
    Task CreateAsync(TEntity entity);
    void Delete(TEntity entity);
}
public interface IRestaurantRepository : IRepository<Restaurant, int>
{
    Task<bool> IsContactEmailExist(string email);
    Task<bool> IsContactNumberExist(string number);
}
public interface ICategoryRepository : IRepository<Category, int>
{
}
public interface IDishRepository : IRepository<Dish, int>
{
}
