using FluentValidation;
using Restful.API.Models;


namespace Restful.API.Validator
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Id).NotNull().WithMessage("Product ID is required.").GreaterThan(0).WithMessage("Product ID must be greater than 0");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required.").Length(5, 50).WithMessage("Product name must be between 5 and 50 characters");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Product name is required.").Length(5, 500).WithMessage("Product name must be between 5 and 500 characters");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Product name is required.").GreaterThan(0).WithMessage("Product price must be greater than zero.");
        }
    }
}
