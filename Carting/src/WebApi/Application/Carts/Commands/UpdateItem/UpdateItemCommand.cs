using Carting.WebApi.Application.Common.Exceptions;
using Carting.WebApi.Application.Common.Interfaces;
using Carting.WebApi.Domain.Entities;
using Carting.WebApi.Domain.Extensions;
using MediatR;

namespace Carting.WebApi.Application.Carts.Commands.Update;

public record UpdateCommand : IRequest<string>
{
    public string CartId { get; init; }
    public int Id { get; init; }
    public string Name { get; init; }
    public WebImage? WebImage { get; init; }
    public string CurrencyCode { get; init; }
    public decimal Price { get; init; }
    public int Quantity { get; init; }
}

public class UpdateCommandHandler : IRequestHandler<UpdateCommand, string>
{
    private readonly ICartingDbContext _context;

    public UpdateCommandHandler(ICartingDbContext context)
    {
        _context = context;
    }


    public Task<string> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var cart = _context.Get<Cart>(request.CartId);
        if (cart == null)
            throw new NotFoundException(typeof(Cart).Name, request.CartId);

        var cartItem = cart.Items.FirstOrDefault(i => i.Id == request.Id);

        if (cartItem == null)
            throw new NotFoundException(typeof(CartItem).Name, request.Id);

        cartItem.Name = request.Name;
        cartItem.WebImage = request.WebImage;
        cartItem.Currency = request.CurrencyCode.ToCurrency();
        cartItem.Price = request.Price;
        cartItem.Quantity = request.Quantity;

        _context.Update(cart);

        return Task.FromResult(cart.Id);
    }
}
