using Catalog.Application.Common.Mappings;
using Catalog.Domain.Entities;

namespace Catalog.Application.TodoLists.Queries.ExportTodos;

public class TodoItemRecord : IMapFrom<TodoItem>
{
    public string? Title { get; set; }

    public bool Done { get; set; }
}
