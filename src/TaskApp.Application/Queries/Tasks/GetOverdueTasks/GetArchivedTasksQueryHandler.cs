using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskApp.Application.Dtos.Tasks;
using TaskApp.Domain.Interfaces.Geniric;

namespace TaskApp.Application.Queries.Tasks.GetOverdueTasks
{
    public class GetArchivedTasksQueryHandler
        : IRequestHandler<GetArchivedTasksQuery, IReadOnlyList<TaskItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetArchivedTasksQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<TaskItemDto>> Handle(GetArchivedTasksQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.TaskItemRepository
                .GetAll()
                .Where(t => t.Category.UserId == request.UserId && t.DueDate != null &&
                            t.DueDate.Value < DateTime.UtcNow &&
                            t.IsCompleted)
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
                .ToListAsync();

        }

    }
}
