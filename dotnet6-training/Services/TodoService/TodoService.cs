using dotnet6_training.Data.Repository;
using dotnet6_training.Models.Enums;
using dotnet6_training.Models.OperationResult;

namespace dotnet6_training.Services.TodoService;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _todoRepository;
    public TodoService(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<IResult<List<TodoResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var todos = await _todoRepository.Get().Data.ToListAsync(cancellationToken);

        var response = todos.ToResponse();

        return new Result<List<TodoResponse>>(response).StatusCode(ApiStatusCode.Success).ToResult();
    }

    public async Task<IResult<TodoResponse>> Find(int todoId, CancellationToken cancellationToken)
    {
        var todo = await _todoRepository.Get(todoId, cancellationToken);

        //TODO:Automation Checking Todo Item will be implementing by FluentValidation

        TodoResponse response = todo.Data;

        return new Result<TodoResponse>(response).StatusCode(ApiStatusCode.Success).ToResult();
    }

    public async Task<IResult<TodoResponse>> New(CreateTodoRequest request, CancellationToken cancellationToken)
    {
        //TODO:Automation Checking Todo Item will be implementing by FluentValidation

        var newTodo = await _todoRepository.Add(request, cancellationToken);

        TodoResponse response = newTodo.Data;

        return new Result<TodoResponse>(response).StatusCode(ApiStatusCode.Success).ToResult();
    }

    public async Task<IResult<TodoResponse>> Modify(EditTodoRequest request, CancellationToken cancellationToken)
    {
        //TODO:Automation Checking Todo Item will be implementing by FluentValidation

        var todo = await _todoRepository.Update(request, cancellationToken);

        TodoResponse response = todo.Data;

        return new Result<TodoResponse>(response).StatusCode(ApiStatusCode.Success).ToResult();
    }

    public async Task<IResult> Delete(int todoId, CancellationToken cancellationToken)
    {
        var todo = await _todoRepository.Get(todoId, cancellationToken);

        await _todoRepository.Delete(todo.Data, cancellationToken);

        return new Result().StatusCode(ApiStatusCode.Success).ToResult();
    }
}