using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskApp.Application.Commands.Tasks.CreateTaskItem;
using TaskApp.Application.Commands.Tasks.DeleteTaskItem;
using TaskApp.Application.Commands.Tasks.MarkTaskItemCompleted;
using TaskApp.Application.Commands.Tasks.UpdateTaskItem;
using TaskApp.Application.Dtos.Tasks;
using TaskApp.Application.Queries.Tasks.GetCompletedTasks;
using TaskApp.Application.Queries.Tasks.GetOverdueTasks;
using TaskApp.Application.Queries.Tasks.GetStats;
using TaskApp.Application.Queries.Tasks.GetTasksById;
using TaskApp.Application.Queries.Tasks.GetTasksByUser;

namespace TaskApp.WepApi.Controllers.TaskItem
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskItemController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskItemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Commands

        [HttpPost("create")]
        public async Task<IActionResult> CreateTaskItemAsync([FromBody] CreateTaskItemCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess
                ? CreatedAtAction(nameof(CreateTaskItemAsync), new { id = result.Value }, result)
                : BadRequest(result.Error);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateTaskItemAsync(Guid id, [FromBody] UpdateTaskItemCommand command)
        {
            if (id != command.TaskId)
                return BadRequest("Task ID mismatch");

            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}/complete")]
        public async Task<IActionResult> MarkTaskItemCompletedAsync(Guid id, [FromBody] MarkTaskItemCompletedCommand command)
        {
            command = new MarkTaskItemCompletedCommand(id, command.UserId);
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("delete/{id:Guid}")]
        public async Task<IActionResult> DeleteTaskItemAsync(Guid id)
        {
            var result = await _mediator.Send(new DeleteTaskItemCommand(id));
            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }

        #endregion

        #region Queries

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetTaskItemByIdAsyc(Guid id)
        {
            var task = await _mediator.Send(new GetTaskItemByIdQuery(id));
            if (task == null)
                return NotFound(new { error = "Task not found." });

            return Ok(task);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetTasksByUserAsync([FromQuery] Guid userId)
        {
            var tasks = await _mediator.Send(new GetTasksByUserQuery(userId));
            return Ok(tasks);
        }

        [HttpGet("archived")]
        public async Task<IActionResult> GetArchivedTasksAsync([FromQuery] Guid userId)
        {
            var tasks = await _mediator.Send(new GetArchivedTasksQuery(userId));
            return Ok(tasks);
        }

        [HttpGet("completed")]
        public async Task<IActionResult> GetCompletedTasksAsync([FromQuery] Guid userId)
        {
            var tasks = await _mediator.Send(new GetCompletedTasksQuery(userId));
            return Ok(tasks);
        }
        [HttpGet("stats")]
        public async Task<ActionResult<TasksStasDto>> GetStatsAsync([FromQuery] Guid userId, CancellationToken cancellationToken)
        {
            if (userId == null)
            {
                return BadRequest(new { Message = "UserId is required" });
            }

            var query = new GetStatsQuery(userId);
            var stats = await _mediator.Send(query, cancellationToken);

            stats ??= new TasksStasDto();

            return Ok(stats);
        }

        #endregion
    }
}
