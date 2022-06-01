using FluentValidation;

namespace Carting.Application.Carts.Commands.AddItemToCart;

public class RemoveItemFromCartCommandValidator : AbstractValidator<RemoveItemFromCartCommand>
{
    public RemoveItemFromCartCommandValidator()
    {
        RuleFor(v => v.CartId)
            .NotEmpty();
    }
}
