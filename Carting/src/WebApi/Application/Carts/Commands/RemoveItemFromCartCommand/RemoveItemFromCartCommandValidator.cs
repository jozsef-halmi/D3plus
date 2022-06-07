using FluentValidation;

namespace Carting.WebApi.Application.Carts.Commands.RemoveItemFromCartCommand;

public class RemoveItemFromCartCommandValidator : AbstractValidator<RemoveItemFromCartCommand>
{
    public RemoveItemFromCartCommandValidator()
    {
        RuleFor(v => v.CartId)
            .NotEmpty();
    }
}
