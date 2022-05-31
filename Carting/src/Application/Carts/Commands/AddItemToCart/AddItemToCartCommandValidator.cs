using FluentValidation;

namespace Carting.Application.Carts.Commands.AddItemToCart;

public class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
{
    public AddItemToCartCommandValidator()
    {
        RuleFor(v => v.CartId)
            .NotEmpty();

        RuleFor(v => v.Name)
         .NotEmpty()
         .MaximumLength(200);
    }
}
