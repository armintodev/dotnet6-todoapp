using FluentValidation;

namespace dotnet6_training.Models.Validators;

public class CreateTodoRequestValidator : AbstractValidator<CreateTodoRequest>
{
    public CreateTodoRequestValidator()
    {
        RuleFor(_ => _.Title)
            .NotEmpty().WithMessage("Title must be have value")
            .Length(2, 200).WithMessage("The Title must have Between 2 and 200 characters");

        RuleFor(_ => _.Description)
            .NotEmpty().WithMessage("Description must be have value")
            .MinimumLength(5).WithMessage("The Description minimum value is 5 character");
    }
}