using FluentValidation;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Dtos.Validators;

public class CreateRestaurantValidator : AbstractValidator<CreateRestaurantDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateRestaurantValidator(IUnitOfWork unitOfWork)
    {
        //RuleFor(dto => dto).CustomAsync(CheckDto);

        RuleFor(dto => dto.Name)
            .Length(3, 100);

        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(dto => dto.ContactEmail)
            .EmailAddress()
            .WithMessage("Please provide a valid email address.")
            .MustAsync(CheckEmailExist)
            .WithMessage("This contact email already exists.");

        RuleFor(dto => dto.PostalCode)
            .Matches(@"^\d{2}-\d{3}$").WithMessage("Please provide a valid postal code (XX-XXX).");

        RuleFor(dto => dto.ContactNumber)
            .Matches(@"^(010|011|012|015)\d{8}$")
            .WithMessage("Please provide a valid Egyptian number.")
            .MustAsync(CheckNumberExist)
            .WithMessage("This contact number already exists.");


        RuleFor(dto => dto.CategoryId)
            .MustAsync(CheckCategoryExists)
            .WithMessage("Insert a valid Category");

        _unitOfWork = unitOfWork;
    }
    // MustAsync Approach one prop at time
    private async Task<bool> CheckNumberExist(string number, CancellationToken token)
    {
        var isExist = await _unitOfWork.RestaurantRepository.IsContactNumberExist(number);
        return !isExist;
    }

    private async Task<bool> CheckEmailExist(string email, CancellationToken token)
    {
        var isExist = await _unitOfWork.RestaurantRepository.IsContactEmailExist(email);
        return !isExist;
    }
    private async Task<bool> CheckCategoryExists(int categoryId, CancellationToken token)
    {

        var category = await _unitOfWork.CategoryRepository.GetById(categoryId);
        return category != null;
    }

    // CustomAsync Approach can check more than one and combine props also
    //private async Task CheckDto(CreateRestaurantDto dto, ValidationContext<CreateRestaurantDto> context, CancellationToken token)
    //{
    //    var isNumberExist = await _unitOfWork.RestaurantRepository.IsContactNumberExist(dto.ContactNumber);
    //    if (isNumberExist)
    //    {
    //        context.AddFailure(nameof(dto.ContactNumber), "This contact number already exists.");
    //    }
    //    var isEmailExist = await _unitOfWork.RestaurantRepository.IsContactEmailExist(dto.ContactNumber);
    //    if (isEmailExist)
    //    {
    //        context.AddFailure(nameof(dto.ContactEmail), "This contact email already exists.");
    //    }
    //    var category = await _unitOfWork.CategoryRepository.GetById(dto.CategoryId);
    //    if (category != null)
    //    {
    //        context.AddFailure(nameof(dto.CategoryId), "Insert a valid Category");
    //    }
    //}



}
