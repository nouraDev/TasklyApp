using MediatR;
using TaskApp.Application.Common;
using TaskApp.Domain.Entities;
using TaskApp.Domain.Interfaces.Geniric;

namespace TaskApp.Application.Commands.Tasks.MarkTaskItemCompleted
{
    public class MarkTaskItemCompletedCommandHandler
        : IRequestHandler<MarkTaskItemCompletedCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public MarkTaskItemCompletedCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<Response> Handle(MarkTaskItemCompletedCommand request, CancellationToken cancellationToken)
        {
            TaskItem? task = await _unitOfWork.TaskItemRepository.GetByIdAsync(request.TaskId);
            if (task is null)
                return Response.Failure(Error.NotFound("Task.NotFound", "Task not found."));

            task.MarkAsCompleted(request.UserId);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response.Success();
        }
    }
}
