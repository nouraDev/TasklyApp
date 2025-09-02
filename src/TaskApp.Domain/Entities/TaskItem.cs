using TaskApp.Domain.Enums;
using TaskApp.Domain.Events;
using TaskApp.Domain.ObjectValues;
using TaskApp.Domain.Shared;

namespace TaskApp.Domain.Entities
{
    public class TaskItem : BaseEntity
    {
        public TaskTitle Title { get; private set; }
        public string Description { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public DueDate? DueDate { get; private set; }
        public PriorityCategory PriorityCategory { get; private set; }
        public Guid CategoryId { get; private set; }
        public TaskCategory Category { get; private set; }
        private TaskItem() { }

        public TaskItem(TaskTitle title, string description, PriorityCategory priority, Guid listId, DueDate? dueDate = null)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            PriorityCategory = priority;
            CategoryId = listId;
            IsCompleted = false;
            DueDate = dueDate;
        }

        public void MarkAsCompleted(Guid completedByUserId)
        {
            if (IsCompleted) return;

            IsCompleted = true;

            AddDomainEvent(new TaskCompletedEvent(Id, completedByUserId));
        }
    }
}
