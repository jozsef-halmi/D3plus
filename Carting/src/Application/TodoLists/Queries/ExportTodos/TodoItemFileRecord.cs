using Carting.Application.Common.Mappings;
using Carting.Domain.Entities;

namespace Carting.Application.TodoLists.Queries.ExportTodos;

public class TodoItemRecord : IMapFrom<TodoItem>
{
    public string? Title { get; set; }

    public bool Done { get; set; }
}
