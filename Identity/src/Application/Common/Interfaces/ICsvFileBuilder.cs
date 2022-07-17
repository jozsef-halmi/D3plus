using Identity.Application.TodoLists.Queries.ExportTodos;

namespace Identity.Application.Common.Interfaces;

public interface ICsvFileBuilder
{
    byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
}
