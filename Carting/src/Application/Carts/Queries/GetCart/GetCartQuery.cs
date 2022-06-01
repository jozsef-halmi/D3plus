using AutoMapper;
using Carting.Application.Common.Exceptions;
using Carting.Application.Common.Interfaces;
using Carting.Domain.Entities;
using MediatR;

namespace Carting.Application.Carts.Queries.GetCart;

public record GetCartQuery : IRequest<CartDto>
{
    public string CartId { get; init; }
}

public class GetCartQueryHandler : IRequestHandler<GetCartQuery, CartDto>
{
    private readonly ICartingDbContext _context;
    private readonly IMapper _mapper;

    public GetCartQueryHandler(ICartingDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CartDto> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = _context.Get<Cart>(request.CartId);
        if (cart == null)
            throw new NotFoundException(typeof(Cart).Name, request.CartId);
        return _mapper.Map<Cart, CartDto>(cart);
    }
}
