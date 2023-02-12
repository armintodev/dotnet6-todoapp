using dotnet6_training.Models.OperationResult;

namespace dotnet6_training.Services.TodoService;

public interface ITodoService
{
    Task<ResponseResult<IReadOnlyList<TodoResponse>>> ReadOnlyGetAll(CancellationToken cancellationToken);
    Task<ResponseResult<List<TodoResponse>>> GetAll(CancellationToken cancellationToken);
    Task<ResponseResult<TodoResponse>> Find(int todoId, CancellationToken cancellationToken);
    Task<ResponseResult<TodoResponse>> New(CreateTodoRequest request, CancellationToken cancellationToken);
    Task<ResponseResult<TodoResponse>> Modify(EditTodoRequest request, CancellationToken cancellationToken);
    Task<ResponseResult> Delete(int todoId, CancellationToken cancellationToken);
}