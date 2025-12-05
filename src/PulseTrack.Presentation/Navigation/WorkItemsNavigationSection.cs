using System;
using PulseTrack.Presentation.ViewModels;

namespace PulseTrack.Presentation.Navigation;

/// <summary>
/// Navigation entry that maps to the Work Items experience.
/// </summary>
public sealed class WorkItemsNavigationSection : INavigationSection
{
    private readonly WorkItemsViewModel _viewModel;

    public WorkItemsNavigationSection(WorkItemsViewModel viewModel)
    {
        _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
    }

    public string Key => "work-items";

    public string Title => "Work Items";

    public string? IconGlyph => "\uE7C3";

    public string? Description => "Plan and track tasks";

    public ViewModelBase ViewModel
    {
        get
        {
            _viewModel.EnsureLoaded();
            return _viewModel;
        }
    }
}

