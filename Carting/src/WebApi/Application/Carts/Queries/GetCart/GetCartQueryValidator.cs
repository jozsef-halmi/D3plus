using FluentValidation;

namespace Carting.WebApi.Application.Carts.Queries.GetCart;

public class GetCartQueryCommandValidator : AbstractValidator<GetCartQuery>
{
    public GetCartQueryCommandValidator()
    {
        RuleFor(v => v.CartId)
            .NotEmpty();
    }
}
