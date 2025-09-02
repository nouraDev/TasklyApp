using System.Text.Json.Serialization;

namespace TaskApp.Application.Common
{
    public class Response
    {
        public Response(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None ||
                !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid error", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public Error Error { get; }

        public static Response Success() => new(true, Error.None);

        public static Response<TValue> Success<TValue>(TValue value) =>
            new(value, true, Error.None);

        public static Response Failure(Error error) => new(false, error);

        public static Response<TValue> Failure<TValue>(Error error) =>
            new(default, false, error);
    }

    public class Response<TValue> : Response
    {
        private readonly TValue? _value;

        public Response(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        [JsonIgnore]
        public TValue Value => _value!;

        [JsonPropertyName("data")]
        public TValue? Data => IsSuccess ? _value : default;

        public static implicit operator Response<TValue>(TValue? value) =>
            value is not null ? Success(value) : Failure<TValue>(Error.NullValue);

        public static Response<TValue> ValidationFailure(Error error) =>
            new(default, false, error);
    }
}
