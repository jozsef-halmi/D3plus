using Catalog.Application.Categorys.Commands.CreateCategory;
using Catalog.Application.Categorys.Commands.DeleteCategory;
using Catalog.Application.Categorys.Commands.UpdateCategory;
using Catalog.Application.TodoLists.Queries.GetCategories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebApi.Controllers;

public class CategoriesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<CategoriesVm>> GetCategories()
    {
        return await Mediator.Send(new GetCategoriesQuery());
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateCategoryCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateCategoryCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteCategoryCommand() {  Id = id });

        return NoContent();
    }
}