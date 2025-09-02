using AutoMapper;
using MediatR;
using TaskApp.Application.Common;
using TaskApp.Domain.Entities;
using TaskApp.Domain.Interfaces.Geniric;

namespace TaskApp.Application.Commands.Tasks.UpdateTaskItem
{
    public class UpdateTaskItemCommandHandler : IRequestHandler<UpdateTaskItemCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateTaskItemCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response> Handle(UpdateTaskItemCommand request, CancellationToken cancellationToken)
        {
            TaskItem? task = await _unitOfWork.TaskItemRepository.GetByIdAsync(request.TaskId);
            if (task is null)
                return Response.Failure(Error.NotFound("Task.NotFound", "Task not found."));

            _mapper.Map(request, task);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response.Success();
        }
    }
}
