using Carting.Application.TodoLists.Queries.ExportTodos;

namespace Carting.Application.Common.Interfaces;

public interface ICsvFileBuilder
{
    byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
}
