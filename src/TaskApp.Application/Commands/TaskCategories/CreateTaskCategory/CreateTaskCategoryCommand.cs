using MediatR;
using TaskApp.Application.Common;

namespace TaskApp.Application.Commands.TaskCategories.CreateTaskCategory
{
    public record CreateTaskCategoryCommand(string Name, Guid UserId, string? Color) : IRequest<Response<Guid>>;
}
