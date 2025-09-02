using TaskApp.Domain.Enums;

namespace TaskApp.Application.Dtos.Tasks
{
    public class TaskItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public PriorityCategory PriorityCategory { get; set; }
        public Guid ListId { get; set; }
        public string ListName { get; set; }
        public Guid UserId { get; set; }
    }
}
