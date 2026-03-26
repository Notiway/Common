using Notiway.Common.Core.Enums;

namespace Notiway.Common.Core.Models;
public struct Result<T>
{
    public static implicit operator Result<T>(T value) => new(value);
    public static implicit operator Result<T>(Processing response) => new(response);

    public Processing Processing { get; private set; } = Processing.Error;

    public bool IsSuccess { get; private set; }

    public readonly bool IsFailure => !IsSuccess;

    public T Value { get; private set; } = default!;

    public Result(T? value) : this(value, Processing.NotFound) { }

    public Result(Processing processing)
    {
        Processing = processing;
        IsSuccess = processing == Processing.Success;
    }

    public Result(T? value, Processing processing)
    {
        if(value is not null)
        {
            Processing = Processing.Success;
            Value = value;
            IsSuccess = true;
        }
        else
        {
            Processing = processing;
            IsSuccess = processing == Processing.Success;
        }
    }
}
