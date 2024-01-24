using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace MusicPlayer.Views;
public class CurrentlyPlayingConverter : IValueConverter {
    public static readonly CurrentlyPlayingConverter Instance = new();
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int sid && parameter is int cpid && targetType.IsAssignableTo(typeof(bool)))
        {
            return sid == cpid;
        }
        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
      throw new NotSupportedException();
    }
}