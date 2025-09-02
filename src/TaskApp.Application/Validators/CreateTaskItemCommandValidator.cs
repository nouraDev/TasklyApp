using FluentValidation;
using TaskApp.Application.Commands.Tasks.CreateTaskItem;

namespace TaskApp.Application.Validators
{
    public class CreateTaskItemCommandValidator : AbstractValidator<CreateTaskItemCommand>
    {
        public CreateTaskItemCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.ListId).NotEmpty();
        }
    }
}
