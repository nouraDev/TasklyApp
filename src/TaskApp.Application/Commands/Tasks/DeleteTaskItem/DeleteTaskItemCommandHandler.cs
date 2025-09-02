using MediatR;
using TaskApp.Application.Common;
using TaskApp.Domain.Entities;
using TaskApp.Domain.Interfaces.Geniric;

namespace TaskApp.Application.Commands.Tasks.DeleteTaskItem
{
    public class DeleteTaskItemCommandHandler : IRequestHandler<DeleteTaskItemCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTaskItemCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(DeleteTaskItemCommand request, CancellationToken cancellationToken)
        {
            TaskItem? task = await _unitOfWork.TaskItemRepository.GetByIdAsync(request.TaskId);
            if (task is null)
                return Response.Failure(Error.NotFound("Task.NotFound", "Task not found."));

            _unitOfWork.TaskItemRepository.DeleteAsync(task);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response.Success();
        }
    }
}
