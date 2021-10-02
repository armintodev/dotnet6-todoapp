using dotnet6_training.Data.Repository;
using dotnet6_training.Models.OperationResult;
using dotnet6_training.Models.Validators;

namespace dotnet6_training.Services.TodoService;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _todoRepository;
    public TodoService(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<Result<List<TodoResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var todos = await _todoRepository.Get().Data.OrderByDescending(_ => _.CreateDate).ToListAsync(cancellationToken);

        var response = todos.ToResponse();

        return new Result<List<TodoResponse>>(response).StatusCode(ApiStatusCode.Success).ToResult();
    }

    public async Task<Result<TodoResponse>> Find(int todoId, CancellationToken cancellationToken)
    {
        var todo = await _todoRepository.Get(todoId, cancellationToken);

        //TODO:Automation Checking Todo Item will be implementing by FluentValidation

        TodoResponse response = todo.Data;

        return new Result<TodoResponse>(response).StatusCode(ApiStatusCode.Success).ToResult();
    }

    public async Task<Result<TodoResponse>> New(CreateTodoRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateTodoRequestValidator();
        var validation = await validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
            return new Result<TodoResponse>(request).StatusCode(ApiStatusCode.Failed)
                .WithMessage(validation.Errors.First().ErrorMessage).ToResult();

        var newTodo = await _todoRepository.Add(request, cancellationToken);

        TodoResponse response = newTodo.Data;

        return new Result<TodoResponse>(response).StatusCode(ApiStatusCode.Success).ToResult();
    }

    public async Task<Result<TodoResponse>> Modify(EditTodoRequest request, CancellationToken cancellationToken)
    {
        var validator = new EditTodoRequestValidator();
        var validation = await validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
            return new Result<TodoResponse>(request).StatusCode(ApiStatusCode.Failed)
                .WithMessage(validation.Errors.First().ErrorMessage).ToResult();

        var todo = await _todoRepository.Update(request, cancellationToken);

        TodoResponse response = todo.Data;

        return new Result<TodoResponse>(response).StatusCode(ApiStatusCode.Success).ToResult();
    }

    public async Task<Result> Delete(int todoId, CancellationToken cancellationToken)
    {
        if (todoId.Equals(0))
            return new Result().StatusCode(ApiStatusCode.Failed)
                .WithMessage("todoId can't be 0").ToResult();

        var todo = await _todoRepository.Get(todoId, cancellationToken);

        await _todoRepository.Delete(todo.Data, cancellationToken);

        return new Result().StatusCode(ApiStatusCode.Success).ToResult();
    }
}