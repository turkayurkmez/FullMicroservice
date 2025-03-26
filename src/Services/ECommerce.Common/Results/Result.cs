namespace ECommerce.Common.Results
{
    public record Result
    {
        public bool IsSuccess { get; init; }
        public string Message { get; init; } = string.Empty;
        public IEnumerable<string> Errors { get; init; } = Enumerable.Empty<string>();

        public static Result Success() => new Result { IsSuccess = true };
        public static Result Success(string message) => new Result { IsSuccess = true, Message = message };
        public static Result Failure(IEnumerable<string> errors) => new Result { IsSuccess = false, Errors = errors };
        public static Result Failure(string message) => new Result { IsSuccess = false, Message = message };

        public static Result Failure(string message, IEnumerable<string> errors) => new Result { IsSuccess = false, Message = message, Errors = errors };
    }

    public record Result<T> : Result
    {
        public T? Data { get; init; }
        public static Result<T> Success(T data) => new Result<T> { IsSuccess = true, Data = data };
        public static new Result<T> Success() => new Result<T> { IsSuccess = true };
        public static new Result<T> Success(string message) => new Result<T> { IsSuccess = true, Message = message };
        public static new Result<T> Failure(IEnumerable<string> errors) => new Result<T> { IsSuccess = false, Errors = errors };
        public static new Result<T> Failure(string message) => new Result<T> { IsSuccess = false, Message = message };
        public static new Result<T> Failure(string message, IEnumerable<string> errors) => new Result<T> { IsSuccess = false, Message = message, Errors = errors };
    }
}
