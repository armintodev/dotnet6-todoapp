namespace dotnet6_training.Models
{
    public record CreateTodoRequest(string Title, string Description);
    public record EditTodoRequest(int Id, string Title, string Description);
}