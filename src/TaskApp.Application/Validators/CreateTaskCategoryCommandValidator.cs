using FluentValidation;
using TaskApp.Application.Commands.TaskCategories.CreateTaskCategory;

namespace TaskApp.Application.Validators
{
    public class CreateTaskCategoryCommandValidator : AbstractValidator<CreateTaskCategoryCommand>
    {
        public CreateTaskCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
