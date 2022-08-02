using Carting.WebApi.Application.Common.Exceptions;
using Carting.WebApi.Application.Common.Interfaces;
using Carting.WebApi.Domain.Entities;
using Carting.WebApi.Domain.Extensions;
using MediatR;

namespace Carting.WebApi.Application.Carts.Commands.AddItemToCart;

public record AddItemToCartCommand : IRequest<string>
{
    public string CartId { get; init; }
    public int Id { get; init; }
    public string Name { get; init; }
    public WebImage? WebImage { get; init; }
    public string CurrencyCode { get; init; }
    public decimal Price { get; init; }
    public int Quantity { get; init; }
}

public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand, string>
{
    private readonly ICartingDbContext _context;

    public AddItemToCartCommandHandler(ICartingDbContext context)
    {
        _context = context;
    }


    public Task<string> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        if (_context.Get<Cart>(request.CartId) == null)
        {
            _context.Insert(new Cart()
            {
                Id = request.CartId,
            }, cancellationToken);
        }

        var cart = _context.Get<Cart>(request.CartId);

        if (cart.Items != null && cart.Items.Any(item => item.Id == request.Id))
            throw new ItemAlreadyAddedToCartException();

        cart.Items?.Add(new CartItem()
        {
            Id = request.Id,
            Name = request.Name,
            Currency = request.CurrencyCode.ToCurrency(),
            Price = request.Price,
            Quantity = request.Quantity,
            WebImage = request.WebImage,
        });

        _context.Update(cart);

        return Task.FromResult(cart.Id);
    }
}
