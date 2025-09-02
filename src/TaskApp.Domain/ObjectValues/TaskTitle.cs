namespace TaskApp.Domain.ObjectValues
{
    public class TaskTitle
    {
        public string Value { get; }

        public TaskTitle(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Title cannot be empty");

            if (value.Length > 100)
                throw new ArgumentException("Title is too long");

            Value = value;
        }

        public override bool Equals(object obj)
            => obj is TaskTitle other && Value == other.Value;

        public override int GetHashCode() => Value.GetHashCode();
    }

}
