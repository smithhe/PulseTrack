using System;
using PulseTrack.Presentation.ViewModels;

namespace PulseTrack.Presentation.Navigation;

/// <summary>
/// Describes an item that can appear in the main navigation shell.
/// </summary>
public interface INavigationSection
{
    /// <summary>
    /// Unique identifier for the section (used for selection/state).
    /// </summary>
    string Key { get; }

    /// <summary>
    /// Display name shown in the sidebar.
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Optional icon glyph (e.g., Segoe Fluent icon code) displayed with the title.
    /// </summary>
    string? IconGlyph { get; }

    /// <summary>
    /// Optional short description used in tooltips.
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// The ViewModel that should be displayed when the section is active.
    /// </summary>
    ViewModelBase ViewModel { get; }

    /// <summary>
    /// Raised when the view model instance changes so bindings can update.
    /// </summary>
    event EventHandler? ViewModelChanged;
}

