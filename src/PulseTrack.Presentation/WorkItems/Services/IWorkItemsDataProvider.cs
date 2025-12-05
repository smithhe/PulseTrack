using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PulseTrack.Presentation.WorkItems.Models;

namespace PulseTrack.Presentation.WorkItems.Services;

/// <summary>
/// Contract the UI uses to load work item summaries.
/// </summary>
public interface IWorkItemsDataProvider
{
    Task<IReadOnlyList<WorkItemListItem>> GetWorkItemsAsync(CancellationToken cancellationToken);
}

