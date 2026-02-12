namespace Restaurants.Application.Restaurants.Dtos;

public class CreateRestaurantDto
{
    //[StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = default!;
    //[Required(ErrorMessage = "Description field is required")]
    public string Description { get; set; } = default!;
    public bool HasDelivery { get; set; }
    public int CategoryId { get; set; }
    //[EmailAddress(ErrorMessage = "Please provide a valid email address")]
    public string? ContactEmail { get; set; }
    //[Phone(ErrorMessage = "please provide a valid phone number")]
    public string? ContactNumber { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    //[RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Please provide a valid postal code (XX-XXX).")]
    public string? PostalCode { get; set; }
}
