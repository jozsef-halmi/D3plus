using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected ISender _mediator;

    public ApiControllerBase(ISender mediator)
    {
        _mediator = mediator;
    }
}
