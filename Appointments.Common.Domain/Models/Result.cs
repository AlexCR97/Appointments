namespace Appointments.Common.Domain.Models;

public sealed class Result<TException>
    where TException : Exception
{
    public TException Exception { get; }

    private Result(TException exception)
    {
        Exception = exception;
    }

    public bool IsFailure => Exception is not null;

    public static Result<TException> Failure(TException exception)
    {
        return new Result<TException>(exception);
    }

    public static Result<TException> Success()
    {
        return new Result<TException>(null!);
    }
}

public sealed class Result<TException, TResult>
    where TException : Exception
{
    public TException Exception { get; }
    public TResult Data { get; }

    private Result(TException exception, TResult data)
    {
        Exception = exception;
        Data = data;
    }

    public bool IsFailure => Exception is not null;

    public static Result<TException, object> Failure(TException exception)
    {
        return new Result<TException, object>(exception, null!);
    }

    public static Result<TException, TResult> Success(TResult? data = default)
    {
        return new Result<TException, TResult>(null!, data!);
    }
}
