using MediatR;
using TaskApp.Application.Common;

namespace TaskApp.Application.Commands.TaskCategories.UpdateTaskCategory
{
    public record UpdateTaskCategoryCommand(Guid Id, string? Name, string? Color) : IRequest<Response>;
}
