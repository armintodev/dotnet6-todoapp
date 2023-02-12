using dotnet6_training.Models.OperationResult;

namespace dotnet6_training.Data.Repository;

public interface ITodoRepository
{
    Task<IResponseResult<IReadOnlyList<Todo>>> ReadOnlyGet(CancellationToken cancellationToken);
    IResponseResult<IQueryable<Todo>> Get();
    Task<IResponseResult<Todo>> Get(int todoId, CancellationToken cancellationToken);
    Task<IResponseResult<Todo>> Add(Todo todo, CancellationToken cancellationToken);
    Task<IResponseResult<Todo>> Update(Todo todo, CancellationToken cancellationToken);
    Task<IResponseResult> Delete(Todo todo, CancellationToken cancellationToken);
    Task Save(CancellationToken cancellationToken);
}