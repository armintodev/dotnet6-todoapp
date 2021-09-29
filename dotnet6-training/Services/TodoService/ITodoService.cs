using dotnet6_training.Models.OperationResult;

namespace dotnet6_training.Services.TodoService;

public interface ITodoService
{
    Task<IResult<List<TodoResponse>>> GetAll(CancellationToken cancellationToken);
    Task<IResult<TodoResponse>> Find(int todoId, CancellationToken cancellationToken);
    Task<IResult<TodoResponse>> New(CreateTodoRequest request, CancellationToken cancellationToken);
    Task<IResult<TodoResponse>> Modify(EditTodoRequest request, CancellationToken cancellationToken);
    Task<IResult> Delete(int todoId, CancellationToken cancellationToken);
}