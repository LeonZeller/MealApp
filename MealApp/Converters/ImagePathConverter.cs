using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealApp.Converters
{
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string path && !string.IsNullOrWhiteSpace(path) && File.Exists(path))
            {
                return ImageSource.FromFile(path);
            }

            return ImageSource.FromFile("mealpageicon.png");
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
