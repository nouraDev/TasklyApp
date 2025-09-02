using MediatR;
using TaskApp.Application.Common;
using TaskApp.Domain.Entities;
using TaskApp.Domain.Interfaces.Geniric;
using TaskApp.Domain.ObjectValues;

namespace TaskApp.Application.Commands.Tasks.CreateTaskItem
{
    public class CreateTaskItemCommandHandler
        : IRequestHandler<CreateTaskItemCommand, Response<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateTaskItemCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<Guid>> Handle(CreateTaskItemCommand request, CancellationToken cancellationToken)
        {
            var categoryExists = _unitOfWork.TaskListRepository.GetAll().Any(c => c.Id == request.ListId && c.UserId == request.UserId);
            if (!categoryExists)
                return Response.Failure<Guid>(Error.NotFound("Category.NotFound", "Task category not found."));

            // Create TaskItem
            var taskItem = new TaskItem(
                new TaskTitle(request.Title),
                request.Description,
                request.PriorityCategory,
                request.ListId,
                request.DueDate is not null ? new DueDate(request.DueDate.Value) : null
            );

            await _unitOfWork.TaskItemRepository.AddAsync(taskItem);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response.Success(taskItem.Id);
        }
    }
}
