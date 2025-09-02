namespace TaskApp.Application.Dtos.Tasks
{
    public sealed class TasksStasDto
    {
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int UpComingTasks { get; set; }
        public int OverdueTasks { get; set; }
        public int TodayTasks { get; set; }
    }
}
