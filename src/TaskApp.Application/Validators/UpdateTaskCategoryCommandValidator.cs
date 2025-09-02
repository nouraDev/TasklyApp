using FluentValidation;
using TaskApp.Application.Commands.TaskCategories.UpdateTaskCategory;

namespace TaskApp.Application.Validators
{
    public class UpdateTaskCategoryCommandValidator : AbstractValidator<UpdateTaskCategoryCommand>
    {
        public UpdateTaskCategoryCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Category Id is required.");
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        }
    }
}
