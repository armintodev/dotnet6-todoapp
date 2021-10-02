using FluentValidation;
using FluentValidation.Results;

namespace dotnet6_training.Models.Validators;

public class EditTodoRequestValidator : AbstractValidator<EditTodoRequest>
{
    public EditTodoRequestValidator()
    {
        RuleFor(_ => _.Id)
            .NotEqual(0).WithMessage("Id must be have value");
    }

    public override async Task<ValidationResult> ValidateAsync(ValidationContext<EditTodoRequest> context, CancellationToken cancellation = default)
    {
        await base.ValidateAsync(context, cancellation);

        var createResult = new CreateTodoRequestValidator();
        var createValidator = createResult.Validate(new CreateTodoRequest(context.InstanceToValidate.Title, context.InstanceToValidate.Description));

        var editResult = new EditTodoRequestValidator();
        var editValidator = editResult.Validate(context.InstanceToValidate);

        List<ValidationFailure> errors = new();

        errors.AddRange(editValidator.Errors);
        errors.AddRange(createValidator.Errors);

        return new ValidationResult(errors);
    }
}