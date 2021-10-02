namespace dotnet6_training.Models;

public record CreateTodoRequest(string Title, string Description) : BaseRequest();
public record EditTodoRequest(int Id, string Title, string Description) : BaseRequest();