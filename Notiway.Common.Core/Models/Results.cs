using Notiway.Common.Core.Enums;

namespace Notiway.Common.Core.Models;
public struct Results<T>
{
    public static implicit operator Results<T>((IEnumerable<T>? values, int count) tuple) => new(tuple.values, tuple.count);
    public static implicit operator Results<T>((IEnumerable<T>? values, int count, string? paginationToken) tuple) => new(tuple.values, tuple.count, tuple.paginationToken);
    public static implicit operator Results<T>((IEnumerable<T>? values, string? paginationToken) tuple) => new(tuple.values, tuple.paginationToken);
    public static implicit operator Results<T>(List<T>? values) => new(values);
    public static implicit operator Results<T>(Processing response) => new(response);

    public Processing Processing { get; private set; } = Processing.Error;

    public bool IsSuccess { get; private set; } = false;

    public readonly bool IsFailure => !IsSuccess;

    public readonly bool IsPaginated => PaginationToken != null;

    public IEnumerable<T> Values { get; private set; } = default!;

    public int Count { get; private set; }

    public string? PaginationToken { get; private set; }

    public Results(IEnumerable<T>? values, int count, string? paginationToken)
    {
        if(values is not null)
        {
            Processing = Processing.Success;
            Values = values;
            Count = count;
            IsSuccess = true;
        }
        if(!string.IsNullOrEmpty(paginationToken))
        {
            PaginationToken = paginationToken;
        }
    }
    public Results(IEnumerable<T>? values, string? paginationToken)
    {
        if(values is not null)
        {
            Processing = Processing.Success;
            Values = values;
            Count = values.Count();
            IsSuccess = true;
        }
        if(!string.IsNullOrEmpty(paginationToken))
        {
            PaginationToken = paginationToken;
        }
    }

    public Results(IEnumerable<T>? values, int count)
    {
        if(values is not null)
        {
            Processing = Processing.Success;
            Values = values;
            Count = count;
            IsSuccess = true;
        }
    }

    public Results(IEnumerable<T>? values)
    {
        if(values is not null)
        {
            Processing = Processing.Success;
            Values = values;
            Count = values.Count();
            IsSuccess = true;
        }
    }

    public Results(Processing processing)
    {
        Processing = processing;
        IsSuccess = processing == Processing.Success;
    }

    public Results(IEnumerable<T>? values, int count, Processing processing)
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
