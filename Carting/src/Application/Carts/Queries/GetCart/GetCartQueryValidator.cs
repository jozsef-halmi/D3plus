using Carting.Application.Carts.Queries.GetCart;
using Carting.Domain.ValueObjects;
using FluentValidation;

namespace Carting.Application.Carts.Commands.AddItemToCart;

public class GetCartQueryCommandValidator : AbstractValidator<GetCartQuery>
{
    public GetCartQueryCommandValidator()
    {
        RuleFor(v => v.CartId)
            .NotEmpty();
    }
}
