using FluentValidation;
using Carting.WebApi.Domain.Extensions;
namespace Carting.WebApi.Application.Carts.Commands.Update;

public class UpdateCommandValidator : AbstractValidator<UpdateItemCommand>
{
    public UpdateCommandValidator()
    {
        RuleFor(v => v.CartId)
            .NotEmpty();

        RuleFor(v => v.Name)
         .NotEmpty()
         .MaximumLength(50);

        RuleFor(v => v.Price)
         .GreaterThanOrEqualTo(0);

        RuleFor(v => v.CurrencyCode)
         .NotEmpty();
    }
}
