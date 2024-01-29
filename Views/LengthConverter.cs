using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace MusicPlayer.Views;
public class LengthConverter : IValueConverter {
    public static readonly LengthConverter Instance = new();
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int length && targetType.IsAssignableTo(typeof(string)))
        {
            return new string($"{length/60}:{(length%60).ToString().PadLeft(2, '0')}");
        }
        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
      throw new NotSupportedException();
    }
}