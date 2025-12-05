using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using PulseTrack.Presentation.Navigation;

namespace PulseTrack.Presentation.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ObservableCollection<NavigationItemViewModel> _navigationItems = new();

    public MainWindowViewModel(IEnumerable<INavigationSection> navigationSections)
    {
        ArgumentNullException.ThrowIfNull(navigationSections);

        foreach (INavigationSection section in navigationSections)
        {
            section.ViewModelChanged += OnNavigationSectionViewModelChanged;
            _navigationItems.Add(new NavigationItemViewModel(section));
        }

        NavigationItems = new ReadOnlyObservableCollection<NavigationItemViewModel>(_navigationItems);
        SelectedNavigationItem = NavigationItems.FirstOrDefault();
    }

    public ReadOnlyObservableCollection<NavigationItemViewModel> NavigationItems { get; }

    [ObservableProperty]
    private NavigationItemViewModel? _selectedNavigationItem;

    [ObservableProperty]
    private ViewModelBase? _currentViewModel;

    partial void OnSelectedNavigationItemChanged(NavigationItemViewModel? oldValue, NavigationItemViewModel? newValue)
    {
        if (ReferenceEquals(oldValue, newValue))
        {
            return;
        }

        if (oldValue is not null)
        {
            oldValue.IsSelected = false;
        }

        if (newValue is not null)
        {
            newValue.IsSelected = true;
            CurrentViewModel = newValue.Content;
        }
        else
        {
            CurrentViewModel = null;
        }
    }

    private void OnNavigationSectionViewModelChanged(object? sender, EventArgs e)
    {
        NavigationItemViewModel? selected = SelectedNavigationItem;

        if (selected is not null && ReferenceEquals(selected.Section, sender))
        {
            CurrentViewModel = selected.Content;
        }
    }
}

