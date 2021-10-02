using dotnet6_training.Models.OperationResult;

namespace dotnet6_training.Services.TodoService;

public interface ITodoService
{
    Task<Result<List<TodoResponse>>> GetAll(CancellationToken cancellationToken);
    Task<Result<TodoResponse>> Find(int todoId, CancellationToken cancellationToken);
    Task<Result<TodoResponse>> New(CreateTodoRequest request, CancellationToken cancellationToken);
    Task<Result<TodoResponse>> Modify(EditTodoRequest request, CancellationToken cancellationToken);
    Task<Result> Delete(int todoId, CancellationToken cancellationToken);
}