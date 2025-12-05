using System;
using PulseTrack.Presentation.ViewModels;
using PulseTrack.Presentation.ViewModels.WorkItems;

namespace PulseTrack.Presentation.Navigation;

/// <summary>
/// Navigation entry that maps to the Work Items experience.
/// </summary>
public sealed class WorkItemsNavigationSection : INavigationSection
{
    private readonly WorkItemsViewModel _backlogViewModel;
    private readonly WorkItemDetailViewModel _detailViewModel;
    private ViewModelBase _currentViewModel;

    public WorkItemsNavigationSection(
        WorkItemsViewModel backlogViewModel,
        WorkItemDetailViewModel detailViewModel)
    {
        _backlogViewModel = backlogViewModel ?? throw new ArgumentNullException(nameof(backlogViewModel));
        _detailViewModel = detailViewModel ?? throw new ArgumentNullException(nameof(detailViewModel));

        _currentViewModel = _backlogViewModel;
        _backlogViewModel.WorkItemOpened += OnWorkItemOpened;
        _detailViewModel.BackRequested += OnBackRequested;
        _backlogViewModel.EnsureLoaded();
    }

    public string Key => "work-items";

    public string Title => "Work Items";

    public string? IconGlyph => "\uE7C3";

    public string? Description => "Plan and track tasks";

    public ViewModelBase ViewModel => _currentViewModel;

    public event EventHandler? ViewModelChanged;

    private void OnWorkItemOpened(object? sender, WorkItemListItemViewModel e)
    {
        _detailViewModel.Load(e);
        _currentViewModel = _detailViewModel;
        _backlogViewModel.ResetSelection();
        ViewModelChanged?.Invoke(this, EventArgs.Empty);
    }

    private void OnBackRequested(object? sender, EventArgs e)
    {
        _currentViewModel = _backlogViewModel;
        ViewModelChanged?.Invoke(this, EventArgs.Empty);
    }
}

