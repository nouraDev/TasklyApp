using MediatR;
using TaskApp.Application.Common;

namespace TaskApp.Application.Commands.TaskCategories.DeleteTaskCategory
{
    public record DeleteTaskCategoryCommand(Guid Id) : IRequest<Response>;
}
