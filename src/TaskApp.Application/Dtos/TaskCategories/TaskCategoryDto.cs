namespace TaskApp.Application.Dtos.TaskCategories
{
    public sealed class TaskCategoryDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int TasksCount { get; set; }

    }
}
