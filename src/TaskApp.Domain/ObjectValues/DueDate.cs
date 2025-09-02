namespace TaskApp.Domain.ObjectValues
{
    public class DueDate
    {
        public DateTime Value { get; }
        protected DueDate() { }

        public DueDate(DateTime value)
        {
            if (value < DateTime.UtcNow.Date)
                throw new ArgumentException("Due date cannot be in the past");
            Value = value;
        }

        public override bool Equals(object obj) => obj is DueDate other && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
    }

}
