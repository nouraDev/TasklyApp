using TaskApp.Domain.Shared;

namespace TaskApp.Domain.Entities
{
    public class TaskCategory : BaseEntity
    {
        public string Name { get; private set; }
        public string? Color { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public ICollection<TaskItem> Tasks { get; set; }


        private TaskCategory() { }

        public TaskCategory(string name, Guid userId, string? color = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("List name cannot be empty");

            Id = Guid.NewGuid();
            Name = name;
            UserId = userId;
            Color = color;
        }
    }

}
