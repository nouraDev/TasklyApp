using MediatR;
using TaskApp.Application.Dtos.TaskCategories;

namespace TaskApp.Application.Queries.TaskCategories.GetTaskCategoriesByUser
{
    public record GetTaskCategoriesByUserQuery(Guid UserId) : IRequest<IReadOnlyList<TaskCategoryDto>>;
}
