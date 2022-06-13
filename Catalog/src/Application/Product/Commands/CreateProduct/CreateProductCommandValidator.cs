using FluentValidation;

namespace Catalog.Application.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

        RuleFor(v => v.Price)
           .GreaterThanOrEqualTo(0).WithMessage("Price should be a positive integer");

        RuleFor(v => v.Amount)
            .GreaterThan(0).WithMessage("Amount should be positive number");
    }
}
