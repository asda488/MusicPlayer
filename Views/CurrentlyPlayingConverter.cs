using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace MusicPlayer.Views;
public class CurrentlyPlayingConverter : IMultiValueConverter  {
    public static readonly CurrentlyPlayingConverter Instance = new();
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values[0] is int sid && values[1] is int cpid && targetType.IsAssignableTo(typeof(bool)))
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