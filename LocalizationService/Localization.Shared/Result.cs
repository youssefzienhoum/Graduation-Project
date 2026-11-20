using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Shared
{
    public enum ErrorType
    {
        None = 0,
        Validation,   // 400
        NotFound,     // 404
        Conflict,     // 409
        Unavailable   // 503
    }

    public sealed class Result<T>
    {
        public bool IsSuccess { get; private init; }
        public T? Value { get; private init; }
        public ErrorType ErrorType { get; private init; }
        public string? Error { get; private init; }

        public static Result<T> Success(T value) => new() { IsSuccess = true, Value = value };

        public static Result<T> Failure(ErrorType type, string error) =>
            new() { IsSuccess = false, ErrorType = type, Error = error };
    }
}
