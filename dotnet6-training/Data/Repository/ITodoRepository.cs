using dotnet6_training.Models.OperationResult;

namespace dotnet6_training.Data.Repository;

public interface ITodoRepository
{
    IResult<IQueryable<Todo>> Get();
    Task<IResult<Todo>> Get(int todoId, CancellationToken cancellationToken);
    Task<IResult<Todo>> Add(Todo todo, CancellationToken cancellationToken);
    Task<IResult<Todo>> Update(Todo todo, CancellationToken cancellationToken);
    Task<IResult> Delete(Todo todo, CancellationToken cancellationToken);
    Task Save(CancellationToken cancellationToken);
}