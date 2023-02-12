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

    public async Task<ResponseResult<IReadOnlyList<TodoResponse>>> ReadOnlyGetAll(CancellationToken cancellationToken)
    {
        var todos = await _todoRepository.ReadOnlyGet(cancellationToken);

        var response = todos.Data.ToResponse();

        return new ResponseResult<IReadOnlyList<TodoResponse>>(response).StatusCode(ApiStatusCode.Success).ToResult();
    }

    public async Task<ResponseResult<List<TodoResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var todos = await _todoRepository.Get().Data.OrderByDescending(_ => _.CreateDate).ToListAsync(cancellationToken);

        var response = todos.ToResponse();

        return new ResponseResult<List<TodoResponse>>(response).StatusCode(ApiStatusCode.Success).ToResult();
    }

    public async Task<ResponseResult<TodoResponse>> Find(int todoId, CancellationToken cancellationToken)
    {
        var todo = await _todoRepository.Get(todoId, cancellationToken);

        //TODO:Automation Checking Todo Item will be implementing by FluentValidation

        TodoResponse response = todo.Data;

        return new ResponseResult<TodoResponse>(response).StatusCode(ApiStatusCode.Success).ToResult();
    }

    public async Task<ResponseResult<TodoResponse>> New(CreateTodoRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateTodoRequestValidator();
        var validation = await validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
            return new ResponseResult<TodoResponse>(request).StatusCode(ApiStatusCode.Failed)
                .WithMessage(validation.Errors.First().ErrorMessage).ToResult();

        var newTodo = await _todoRepository.Add(request, cancellationToken);

        TodoResponse response = newTodo.Data;

        return new ResponseResult<TodoResponse>(response).StatusCode(ApiStatusCode.Success).ToResult();
    }

    public async Task<ResponseResult<TodoResponse>> Modify(EditTodoRequest request, CancellationToken cancellationToken)
    {
        var validator = new EditTodoRequestValidator();
        var validation = await validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
            return new ResponseResult<TodoResponse>(request).StatusCode(ApiStatusCode.Failed)
                .WithMessage(validation.Errors.First().ErrorMessage).ToResult();

        var todo = await _todoRepository.Update(request, cancellationToken);

        TodoResponse response = todo.Data;

        return new ResponseResult<TodoResponse>(response).StatusCode(ApiStatusCode.Success).ToResult();
    }

    public async Task<ResponseResult> Delete(int todoId, CancellationToken cancellationToken)
    {
        if (todoId.Equals(0))
            return new ResponseResult().StatusCode(ApiStatusCode.Failed)
                .WithMessage("todoId can't be 0").ToResult();

        var todo = await _todoRepository.Get(todoId, cancellationToken);

        await _todoRepository.Delete(todo.Data, cancellationToken);

        return new ResponseResult().StatusCode(ApiStatusCode.Success).ToResult();
    }
}