namespace PulseTrack.Shared.Requests.Projects
{
    public record UpdateProjectRequest(string Name, string? Color, string? Icon, bool IsInbox);
}
