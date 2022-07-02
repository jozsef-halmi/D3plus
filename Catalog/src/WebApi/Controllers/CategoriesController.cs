using Catalog.Application.Categorys.Commands.CreateCategory;
using Catalog.Application.Categorys.Commands.DeleteCategory;
using Catalog.Application.Categorys.Commands.UpdateCategory;
using Catalog.Application.TodoLists.Queries.GetCategories;
using Catalog.WebApi.Helpers;
using Catalog.WebApi.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebApi.Controllers;

public class CategoriesController : ApiControllerBase
{
    private readonly LinkGenerator _linkGenerator;

    public CategoriesController(LinkGenerator linkGenerator, ISender mediator) : base(mediator)
    {
        _linkGenerator = linkGenerator;
    }

    [HttpGet]
    public async Task<ActionResult<HateoasResponse<CategoriesVm>>> GetCategories()
    {
        return HateoasHelper.CreateLinksForCategories(HttpContext, _linkGenerator, await _mediator.Send(new GetCategoriesQuery()));
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateCategoryCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateCategoryCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteCategoryCommand() {  Id = id });

        return NoContent();
    }
}