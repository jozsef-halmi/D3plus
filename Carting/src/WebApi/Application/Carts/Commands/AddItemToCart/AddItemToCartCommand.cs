using Carting.WebApi.Application.Common.Exceptions;
using Carting.WebApi.Application.Common.Interfaces;
using Carting.WebApi.Domain.Entities;
using Carting.WebApi.Domain.Extensions;
using MediatR;

namespace Carting.WebApi.Application.Carts.Commands.AddItemToCart;

public record AddItemToCartCommand : IRequest<string>
{
    public AddItemToCartCommand(string cartId, int id, string name, WebImage? webImage, string currencyCode, decimal price, int quantity)
    {
        CartId = cartId ?? throw new ArgumentNullException(nameof(cartId));
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        WebImage = webImage;
        CurrencyCode = currencyCode ?? throw new ArgumentNullException(nameof(currencyCode));
        Price = price;
        Quantity = quantity;
    }

    public string CartId { get; }
    public int Id { get; }
    public string Name { get; }
    public WebImage? WebImage { get; }
    public string CurrencyCode { get; }
    public decimal Price { get; }
    public int Quantity { get; }
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
        if (_context.Get<Cart>(request?.CartId) == null)
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
