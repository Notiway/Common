using Notiway.Common.Core.Enums;

namespace Notiway.Common.Core.Models;
public struct Results<T>
{
    public static implicit operator Results<T>((IReadOnlyCollection<T>? values, int count) tuple) => new(tuple.values, tuple.count);
    public static implicit operator Results<T>((IReadOnlyCollection<T>? values, int count, string? paginationToken) tuple) => new(tuple.values, tuple.count, tuple.paginationToken);
    public static implicit operator Results<T>((IReadOnlyCollection<T>? values, string? paginationToken) tuple) => new(tuple.values, tuple.paginationToken);
    public static implicit operator Results<T>(List<T>? values) => new(values);
    public static implicit operator Results<T>(Processing response) => new(response);

    public Processing Processing { get; private set; } = Processing.Error;

    public bool IsSuccess { get; private set; }

    public readonly bool IsFailure => !IsSuccess;

    public readonly bool IsPaginated => PaginationToken != null;

    public IReadOnlyCollection<T> Values { get; private set; } = Array.Empty<T>();

    public int Count { get; private set; }

    public string? PaginationToken { get; private set; }

    public Results(IReadOnlyCollection<T>? values, int count, string? paginationToken) : this(values, count)
    {
        if(IsSuccess && !string.IsNullOrEmpty(paginationToken))
        {
            PaginationToken = paginationToken;
        }
    }

    public Results(IReadOnlyCollection<T>? values, string? paginationToken) : this(values)
    {
        if(IsSuccess && !string.IsNullOrEmpty(paginationToken))
        {
            PaginationToken = paginationToken;
        }
    }

    public Results(IReadOnlyCollection<T>? values, int count)
    {
        if(values is not null)
        {
            Processing = Processing.Success;
            Values = values;
            Count = count;
            IsSuccess = true;
        }
    }

    public Results(IReadOnlyCollection<T>? values)
    {
        if(values is not null)
        {
            Processing = Processing.Success;
            Values = values;
            Count = values.Count;
            IsSuccess = true;
        }
    }

    public Results(Processing processing)
    {
        Processing = processing;
        IsSuccess = processing == Processing.Success;
    }

    public Results(IReadOnlyCollection<T>? values, int count, Processing processing)
    {
        if(values is not null)
        {
            Processing = Processing.Success;
            Values = values;
            Count = count;
            IsSuccess = true;
        }
        else
        {
            Processing = processing;
            IsSuccess = processing == Processing.Success;
        }
    }
}
