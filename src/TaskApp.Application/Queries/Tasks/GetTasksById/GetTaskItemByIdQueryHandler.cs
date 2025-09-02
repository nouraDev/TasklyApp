using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskApp.Application.Dtos.Tasks;
using TaskApp.Domain.Interfaces.Geniric;

namespace TaskApp.Application.Queries.Tasks.GetTasksById
{
    public class GetTaskItemByIdQueryHandler
        : IRequestHandler<GetTaskItemByIdQuery, TaskItemDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTaskItemByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TaskItemDto?> Handle(GetTaskItemByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.TaskItemRepository.GetAll()
                .AsNoTracking()
                .Where(ts => ts.Id == request.TaskId)
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

                }).FirstOrDefaultAsync();
        }
    }
}
