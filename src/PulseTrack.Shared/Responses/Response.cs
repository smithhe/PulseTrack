namespace PulseTrack.Shared.Responses;

public record Response<T>(bool IsSuccess, string? Reason, T? Data) : ResponseBase(IsSuccess, Reason)
{
    public static Response<T> Success(T data, string? reason = null) => new(true, reason, data);

    public new static Response<T> Failure(string reason) => new(false, reason, default);
}

