using dotnet6_training.Models.Configuration;
using dotnet6_training.Models.OperationResult;
using dotnet6_training.Services.CacheService;

using Hangfire;

using Microsoft.Extensions.Options;

namespace dotnet6_training.Data.Repository;

public class TodoRepository : ITodoRepository
{
    private readonly CacheSetting _cacheSetting;
    private readonly string cacheKey = $"{typeof(Todo)}";
    private readonly Func<CacheSide, ICacheService> _cacheService;
    private readonly TodoContext _context;
    public TodoRepository(TodoContext context,
        Func<CacheSide, ICacheService> cacheService,
        IOptionsMonitor<CacheSetting> cacheSetting)
    {
        _context = context;
        _cacheService = cacheService;
        _cacheSetting = cacheSetting.CurrentValue;
    }

    public async Task<IResponseResult<IReadOnlyList<Todo>>> ReadOnlyGet(CancellationToken cancellationToken)
    {
        if (_cacheService(_cacheSetting.CacheSide).TryGet(cacheKey, out IReadOnlyList<Todo> cachedList) is false)
        {
            cachedList = await _context.Todos.AsNoTracking().ToListAsync();
            _cacheService(_cacheSetting.CacheSide).Set(cacheKey, cachedList);
        }

        return new ResponseResult<IReadOnlyList<Todo>>(cachedList).StatusCode(ApiStatusCode.Success).ToResult();
    }

    public IResponseResult<IQueryable<Todo>> Get()
    {
        return new ResponseResult<IQueryable<Todo>>(_context.Todos).StatusCode(ApiStatusCode.Success).ToResult();
    }

    public async Task<IResponseResult<Todo>> Get(int todoId, CancellationToken cancellationToken)
    {
        return new ResponseResult<Todo>(await _context.Todos.SingleOrDefaultAsync(_ => _.Id.Equals(todoId), cancellationToken)).StatusCode(ApiStatusCode.Success).ToResult();
    }

    public async Task<IResponseResult<Todo>> Add(Todo todo, CancellationToken cancellationToken)
    {
        var operation = await _context.Todos.AddAsync(todo, cancellationToken);
        await this.Save(cancellationToken);

        BackgroundJob.Enqueue(() => RefreshCache());

        return new ResponseResult<Todo>(operation.Entity).StatusCode(ApiStatusCode.Success).ToResult();
    }

    public async Task<IResponseResult<Todo>> Update(Todo todo, CancellationToken cancellationToken)
    {
        var updatedTodo = _context.Todos.Update(todo);
        await this.Save(cancellationToken);

        BackgroundJob.Enqueue(() => RefreshCache());

        return new ResponseResult<Todo>(updatedTodo.Entity).StatusCode(ApiStatusCode.Success).ToResult();
    }

    public async Task<IResponseResult> Delete(Todo todo, CancellationToken cancellationToken)
    {
        _context.Todos.Remove(todo);
        await this.Save(cancellationToken);

        BackgroundJob.Enqueue(() => RefreshCache());

        return new ResponseResult().StatusCode(ApiStatusCode.Success).ToResult();
    }

    public async Task Save(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RefreshCache()
    {
        _cacheService(_cacheSetting.CacheSide).Remove(cacheKey);
        var cachedList = await _context.Todos.ToListAsync();
        _cacheService(_cacheSetting.CacheSide).Set(cacheKey, cachedList);
    }
}