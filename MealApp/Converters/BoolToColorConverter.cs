using System.Globalization;

namespace MealApp.Converters
{
    public class BoolToColorConverter : BindableObject, IValueConverter
    {
        public static readonly BindableProperty TrueColorProperty =
            BindableProperty.Create(nameof(TrueColor), typeof(Color), typeof(BoolToColorConverter), Colors.Purple);

        public static readonly BindableProperty FalseColorProperty =
            BindableProperty.Create(nameof(FalseColor), typeof(Color), typeof(BoolToColorConverter), Colors.Gray);

        public Color TrueColor
        {
            get => (Color)GetValue(TrueColorProperty);
            set => SetValue(TrueColorProperty, value);
        }

        public Color FalseColor
        {
            get => (Color)GetValue(FalseColorProperty);
            set => SetValue(FalseColorProperty, value);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = value is bool boolValue && boolValue;
            if (parameter?.ToString() == "inverse") b = !b;
            return b ? TrueColor : FalseColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}