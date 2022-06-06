using Carting.WebApi.Application.Common.Exceptions;
using Carting.WebApi.Application.Common.Interfaces;
using Carting.WebApi.Domain.Entities;
using MediatR;

namespace Carting.WebApi.Application.Carts.Commands.RemoveItemFromCartCommand;

public record RemoveItemFromCartCommand : IRequest<string>
{
    public string CartId { get; init; }
    public int Id { get; init; }
}

public class RemoveItemFromCartCommandHandler : IRequestHandler<RemoveItemFromCartCommand, string>
{
    private readonly ICartingDbContext _context;

    public RemoveItemFromCartCommandHandler(ICartingDbContext context)
    {
        _context = context;
    }


    public Task<string> Handle(RemoveItemFromCartCommand request, CancellationToken cancellationToken)
    {
        var cart = _context.Get<Cart>(request.CartId);
        if (cart == null)
            throw new NotFoundException(typeof(Cart).Name, request.CartId);

        var cartItem = cart.Items.FirstOrDefault(i => i.Id == request.Id);

        if (cartItem == null)
            throw new NotFoundException(typeof(CartItem).Name, request.Id);

        cart.Items.Remove(cartItem);

        _context.Update(cart);

        return Task.FromResult(cart.Id);
    }
}
