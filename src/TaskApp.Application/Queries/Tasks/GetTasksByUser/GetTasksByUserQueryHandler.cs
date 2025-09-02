using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskApp.Application.Dtos.Tasks;
using TaskApp.Domain.Interfaces.Geniric;

namespace TaskApp.Application.Queries.Tasks.GetTasksByUser
{
    public class GetTasksByUserQueryHandler
        : IRequestHandler<GetTasksByUserQuery, IReadOnlyList<TaskItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTasksByUserQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<TaskItemDto>> Handle(GetTasksByUserQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _unitOfWork.TaskItemRepository
                .GetAll()
                .AsNoTracking()
                .Where(t => t.Category.UserId == request.UserId && !t.IsCompleted)
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

            return tasks;
        }
    }
}
