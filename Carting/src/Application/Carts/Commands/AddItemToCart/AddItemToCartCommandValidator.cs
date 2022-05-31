using Carting.Domain.ValueObjects;
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

        RuleFor(v => v.Price)
         .GreaterThanOrEqualTo(0);

        RuleFor(v => v.CurrencyCode)
         .NotEmpty()
         .Must(v => Currency.From(v) != null);
    }
}
