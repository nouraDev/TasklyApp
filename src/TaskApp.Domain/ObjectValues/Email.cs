namespace TaskApp.Domain.ObjectValues
{
    public class Email
    {
        public string Value { get; private set; }

        public Email() { }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email cannot be empty");

            if (!value.Contains("@"))
                throw new ArgumentException("Email must be valid");

            Value = value;
        }
    }

}
