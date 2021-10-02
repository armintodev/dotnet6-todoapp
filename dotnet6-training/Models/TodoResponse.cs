namespace dotnet6_training.Models;

public record TodoResponse(int Id, string Title, string Description, string CreateDate)
{
    public static implicit operator TodoResponse(CreateTodoRequest request)
    {
        return new(0,
            request.Title,
            request.Description,
            DateTime.Now.ToString());
    }

    public static implicit operator TodoResponse(EditTodoRequest request)
    {
        return new(request.Id,
            request.Title,
            request.Description,
            DateTime.Now.ToString());
    }

}