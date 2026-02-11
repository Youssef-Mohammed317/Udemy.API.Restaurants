using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurants.Application.Services.Abstractions;

public interface IServiceMaanager
{
    IRestaurantService RestaurantService { get; }
}
public interface IRestaurantService
{

}

