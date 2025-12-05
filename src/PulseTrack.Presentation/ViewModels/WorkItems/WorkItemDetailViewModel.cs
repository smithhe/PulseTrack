using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PulseTrack.Presentation.ViewModels.WorkItems;

/// <summary>
/// ViewModel that renders the details for a single work item.
/// </summary>
public sealed partial class WorkItemDetailViewModel : ViewModelBase
{
    public event EventHandler? BackRequested;

    [ObservableProperty]
    private WorkItemListItemViewModel? _workItem;

    public ObservableCollection<string> Notes { get; } = new();

    public bool HasNotes => Notes.Count > 0;

    public bool HasNoNotes => !HasNotes;

    public IRelayCommand BackCommand { get; }

    public WorkItemDetailViewModel()
    {
        Notes.CollectionChanged += OnNotesChanged;
        BackCommand = new RelayCommand(() => BackRequested?.Invoke(this, EventArgs.Empty));
    }

    public void Load(WorkItemListItemViewModel workItem)
    {
        WorkItem = workItem ?? throw new ArgumentNullException(nameof(workItem));
        Notes.Clear();
    }

    private void OnNotesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(HasNotes));
        OnPropertyChanged(nameof(HasNoNotes));
    }
}

