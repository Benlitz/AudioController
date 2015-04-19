using System;
using System.Globalization;
using WpfEx.Views.ValueConverters;

namespace AudioController
{
    public class VirtualKeyConverter : TypedValueConverter<VirtualKeyConverter, VirtualKey, string, object>
    {
        protected override string ProcessTypedConversion(VirtualKey value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == 0 ? string.Empty : value.ToString().Substring(4);
        }

        protected override VirtualKey ProcessTypedBackwardConversion(string value, Type targetType, object parameter, CultureInfo culture)
        {
            VirtualKey result;
            return Enum.TryParse("KEY_" + value, out result) ? result : 0;
        }
    }
}