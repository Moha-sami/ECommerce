namespace ECommerce.Application.common;

public class Result
{
    public bool IsSuccess { get; private set; }

    public IReadOnlyList<Error> Errors { get; private set; }

    protected Result(bool isSuccess, IReadOnlyList<Error> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }
    // Factory methods for creating Result instances with no data
    public static Result Success() => new Result(true, Array.Empty<Error>());
    public static Result Failure(Error error) => new Result(false, new[] { error });
    public static Result Failure(IReadOnlyList<Error> errors) => new Result(false, errors);
}
    // Factory methods for creating Result instances with data
    public class Result<TValue> : Result
    {
        private readonly TValue _value;

        public TValue Data => IsSuccess ? _value : throw new InvalidOperationException("Cannot access data from a failed result.");

        private Result(TValue Value) : base(true, Array.Empty<Error>())
         {
            _value = Value;
         }
        private Result(Error error) : base(false, new[] {error})
        {
            _value = default;
        }
        private Result(IReadOnlyList<Error> errors) : base(false, errors)
        {
            _value = default;
        }
        public static Result<TValue> Ok(TValue value) => new Result<TValue>(value);
        public static Result<TValue> Fail(Error error) => new Result<TValue>(error);
        public static Result<TValue> Fail(IReadOnlyList<Error> errors) => new Result<TValue>(errors);

    }


