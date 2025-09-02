using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskApp.Application.Commands.TaskCategories.CreateTaskCategory;
using TaskApp.Application.Commands.TaskCategories.DeleteTaskCategory;
using TaskApp.Application.Commands.TaskCategories.UpdateTaskCategory;
using TaskApp.Application.Queries.TaskCategories.GetTaskCategoriesByUser;
using TaskApp.Application.Queries.TaskCategories.GetTaskCategoryById;

namespace TaskApp.WepApi.Controllers.TaskCategory
{
    [ApiController]
    [Route("api/categories")]
    public class TaskCategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TaskCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CreateTaskCategoryCommand command)
        {
            var res = await _mediator.Send(command);
            return res.IsSuccess ? Ok(res) : BadRequest(res);

        }

        [HttpPatch("update/{id:Guid}")]
        public async Task<IActionResult> UpdateCategoryAsync(Guid id, [FromBody] UpdateTaskCategoryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest(new { error = "Id in route does not match Id in body." });
            }

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return NoContent();

        }
        [HttpDelete("delete/{id:Guid}")]
        public async Task<IActionResult> DeleteCategoryAsync(Guid id)
        {
            var command = new DeleteTaskCategoryCommand(id);

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return NoContent();
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var category = await _mediator.Send(new GetTaskCategoryByIdQuery(id), cancellationToken);

            if (category == null)
                return NotFound(new { error = "Task category not found." });

            return Ok(category);
        }

        [HttpGet("user/{userId:Guid}")]
        public async Task<IActionResult> GetByUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            var categories = await _mediator.Send(new GetTaskCategoriesByUserQuery(userId), cancellationToken);

            return Ok(categories);
        }
    }
}
