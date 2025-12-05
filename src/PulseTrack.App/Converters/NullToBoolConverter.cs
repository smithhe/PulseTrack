using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace PulseTrack.App.Converters;

public sealed class NullToBoolConverter : IValueConverter
{
    public bool Invert { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool result = value is not null;
        return Invert ? !result : result;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

