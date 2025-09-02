using MediatR;
using TaskApp.Application.Dtos.TaskCategories;

namespace TaskApp.Application.Queries.TaskCategories.GetTaskCategoryById
{
    public record GetTaskCategoryByIdQuery(Guid Id) : IRequest<TaskCategoryDto>;
}
