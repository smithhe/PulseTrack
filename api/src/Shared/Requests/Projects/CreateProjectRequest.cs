namespace PulseTrack.Shared.Requests.Projects
{
    public record CreateProjectRequest(string Name, string? Color, string? Icon, bool IsInbox);
}
