namespace dotnet6_training.Models;

/// <summary>
/// the todo domain model
/// </summary>
public class Todo
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreateDate { get; set; }

    //you can use this rather than extension method in below class
    //public void Edit(this Todo todo, string title, string description)
    //{
    //    todo.Title = title;
    //    todo.Description = description;
    //    todo.CreateDate = DateTime.Now;
    //}

    /// <summary>
    /// convert dto to domain model
    /// </summary>
    /// <param name="request">todo request information</param>
    public static implicit operator Todo(CreateTodoRequest request)
    {
        return new Todo
        {
            Title = request.Title,
            Description = request.Description,
            CreateDate = DateTime.Now
        };
    }

    /// <summary>
    /// convert dto to domain model
    /// </summary>
    /// <param name="request">todo request information</param>
    public static implicit operator Todo(EditTodoRequest request)
    {
        return new Todo
        {
            Id = request.Id,
            Title = request.Title,
            Description = request.Description,
            CreateDate = DateTime.Now
        };
    }

    /// <summary>
    /// convert domain model to response
    /// </summary>
    /// <param name="todo">single of a todo domain model</param>
    public static implicit operator TodoResponse(Todo todo)
    {
        return new(todo.Id,
            todo.Title,
            todo.Description,
            todo.CreateDate.ToString());
    }
}

public static class ConvertExtensions
{
    public static List<TodoResponse> ToResponse(this List<Todo> todos)
    {
        return todos.Select(_ => new TodoResponse(_.Id, _.Title, _.Description, _.CreateDate.ToString())).ToList();
    }

    public static IReadOnlyList<TodoResponse> ToResponse(this IReadOnlyList<Todo> todos)
    {
        return todos.Select(_ => new TodoResponse(_.Id, _.Title, _.Description, _.CreateDate.ToString())).ToList();
    }

    /// <summary>
    /// edit this todo domain model
    /// </summary>
    /// <param name="todo">this todo domain model</param>
    /// <param name="title">the title of todo</param>
    /// <param name="description">the description of todo</param>
    public static void Edit(this Todo todo, string title, string description)
    {
        todo.Title = title;
        todo.Description = description;
        todo.CreateDate = DateTime.Now;
    }
}
