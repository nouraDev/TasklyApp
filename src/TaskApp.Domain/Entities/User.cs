using TaskApp.Domain.Defaults;
using TaskApp.Domain.ObjectValues;
using TaskApp.Domain.Shared;

namespace TaskApp.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public Email Email { get; set; }
        public ICollection<TaskCategory> Categories { get; set; }

        private User() { }

        public User(string name, string password, Email email)
        {
            Id = Guid.NewGuid();
            Name = name;
            Password = password;
            Email = email;
            Categories = DefaultCategories.Names
                            .Select(c => new TaskCategory(c, Id, ListColors.GetRandomColor()))
                            .ToList();
        }
    }

}
