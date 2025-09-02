using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskApp.Application.Dtos.Tasks;
using TaskApp.Domain.Interfaces.Geniric;

namespace TaskApp.Application.Queries.Tasks.GetCompletedTasks
{
    public class GetCompletedTasksQueryHandler
        : IRequestHandler<GetCompletedTasksQuery, IReadOnlyList<TaskItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCompletedTasksQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<TaskItemDto>> Handle(GetCompletedTasksQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.TaskItemRepository
                 .GetAll()
                 .Where(t => t.Category.UserId == request.UserId && t.IsCompleted)
                 .Select(ts => new TaskItemDto
                 {
                     Id = ts.Id,
                     Title = ts.Title.Value,
                     Description = ts.Description,
                     IsCompleted = ts.IsCompleted,
                     CreatedAt = ts.CreatedAt,
                     DueDate = ts.DueDate.Value,
                     CompletedAt = ts.CompletedAt,
                     PriorityCategory = ts.PriorityCategory,
                     ListId = ts.CategoryId,
                     ListName = ts.Category.Name,
                     UserId = ts.Category.UserId

                 })
                 .ToListAsync(cancellationToken);

        }
    }
}
