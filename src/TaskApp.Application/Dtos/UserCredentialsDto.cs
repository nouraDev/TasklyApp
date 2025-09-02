namespace TaskApp.Application.Dtos
{
    public record UserCredentialsDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
