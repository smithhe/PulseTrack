using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PulseTrack.Presentation.ViewModels.WorkItems;
using PulseTrack.Presentation.WorkItems.Models;
using PulseTrack.Presentation.WorkItems.Services;

namespace PulseTrack.Presentation.ViewModels;

/// <summary>
/// View-model responsible for displaying and filtering the work items list + detail view.
/// </summary>
public sealed partial class WorkItemsViewModel : ViewModelBase
{
    private readonly IWorkItemsDataProvider _dataProvider;
    private readonly ObservableCollection<WorkItemListItemViewModel> _items = new();
    private readonly List<WorkItemListItemViewModel> _itemsCache = new();
    private CancellationTokenSource? _refreshCancellationSource;

    public WorkItemsViewModel(IWorkItemsDataProvider dataProvider)
    {
        _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));

        WorkItems = new ReadOnlyObservableCollection<WorkItemListItemViewModel>(_items);
        RefreshCommand = new AsyncRelayCommand(RefreshAsync);
    }

    public ReadOnlyObservableCollection<WorkItemListItemViewModel> WorkItems { get; }

    [ObservableProperty]
    private WorkItemListItemViewModel? _selectedWorkItem;

    [ObservableProperty]
    private string? _searchText;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string? _errorMessage;

    [ObservableProperty]
    private bool _hasVisibleItems;

    public IAsyncRelayCommand RefreshCommand { get; }

    public bool HasNoVisibleItems => !HasVisibleItems;

    /// <summary>
    /// Loads data once when the view/model is first shown.
    /// </summary>
    public void EnsureLoaded()
    {
        if (_items.Count == 0 && !IsLoading)
        {
            RefreshCommand.Execute(null);
        }
    }

    private async Task RefreshAsync()
    {
        _refreshCancellationSource?.Cancel();
        _refreshCancellationSource?.Dispose();
        _refreshCancellationSource = new CancellationTokenSource();

        CancellationToken cancellationToken = _refreshCancellationSource.Token;

        try
        {
            IsLoading = true;
            ErrorMessage = null;

            IReadOnlyList<WorkItemListItem> rawItems =
                await _dataProvider.GetWorkItemsAsync(cancellationToken).ConfigureAwait(true);

            _itemsCache.Clear();
            foreach (WorkItemListItem raw in rawItems)
            {
                _itemsCache.Add(new WorkItemListItemViewModel(raw));
            }

            ApplyFilters();

            if (_items.Count > 0)
            {
                SelectedWorkItem ??= _items[0];
            }
        }
        catch (OperationCanceledException)
        {
            // Swallow cancellations triggered by a newer refresh.
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            if (_refreshCancellationSource is { } current && current.Token == cancellationToken)
            {
                current.Dispose();
                _refreshCancellationSource = null;
            }

            IsLoading = false;
        }
    }

    partial void OnSearchTextChanged(string? oldValue, string? newValue)
    {
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        IEnumerable<WorkItemListItemViewModel> query = _itemsCache;

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            string term = SearchText.Trim();
            query = query.Where(item =>
                item.Title.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                (item.OwnerDisplayName?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false) ||
                item.ProjectName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                item.Key.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        WorkItemListItemViewModel? previousSelection = SelectedWorkItem;

        _items.Clear();
        foreach (WorkItemListItemViewModel item in query)
        {
            _items.Add(item);
        }

        HasVisibleItems = _items.Count > 0;

        if (_items.Count == 0)
        {
            SelectedWorkItem = null;
        }
        else if (previousSelection is null)
        {
            SelectedWorkItem = _items[0];
        }
        else
        {
            SelectedWorkItem = _items.FirstOrDefault(i => i.Id == previousSelection.Id) ?? _items[0];
        }
    }

    partial void OnHasVisibleItemsChanged(bool oldValue, bool newValue)
    {
        OnPropertyChanged(nameof(HasNoVisibleItems));
    }
}

