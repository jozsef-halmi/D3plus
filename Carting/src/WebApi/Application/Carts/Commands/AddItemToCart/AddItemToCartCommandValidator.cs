using Carting.WebApi.Domain.ValueObjects;
using FluentValidation;

namespace Carting.WebApi.Application.Carts.Commands.AddItemToCart;

public class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
{
    public AddItemToCartCommandValidator()
    {
        RuleFor(v => v.CartId)
            .NotEmpty();

        RuleFor(v => v.Name)
         .NotEmpty()
         .MaximumLength(50);

        RuleFor(v => v.Price)
         .GreaterThanOrEqualTo(0);

        RuleFor(v => v.CurrencyCode)
         .NotEmpty()
         .Must(v => Currency.From(v) != null);
    }
}
