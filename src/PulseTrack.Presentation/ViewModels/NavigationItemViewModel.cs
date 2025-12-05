using System;
using CommunityToolkit.Mvvm.ComponentModel;
using PulseTrack.Presentation.Navigation;

namespace PulseTrack.Presentation.ViewModels;

/// <summary>
/// View-model used by the shell sidebar to represent an available navigation section.
/// </summary>
public sealed partial class NavigationItemViewModel : ObservableObject
{
    public NavigationItemViewModel(INavigationSection section)
    {
        Section = section ?? throw new ArgumentNullException(nameof(section));
    }

    public INavigationSection Section { get; }

    public string Key => Section.Key;

    public string Title => Section.Title;

    public string? IconGlyph => Section.IconGlyph;

    public string? Description => Section.Description;

    public ViewModelBase Content => Section.ViewModel;

    [ObservableProperty]
    private bool _isSelected;
}

