namespace PulseTrack.Shared.Responses;

public record ResponseBase(bool IsSuccess, string? Reason = null)
{
    public static ResponseBase Success(string? reason = null) => new(true, reason);

    public static ResponseBase Failure(string reason) => new(false, reason);
}

