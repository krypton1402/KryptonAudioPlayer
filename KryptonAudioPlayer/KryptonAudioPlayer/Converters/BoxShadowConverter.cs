using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KryptonAudioPlayer.Converters
{
    public class BoxShadowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                var parts = str.Split(' ');
                if (parts.Length == 4 && float.TryParse(parts[0], out var offsetX)
                    && float.TryParse(parts[1], out var offsetY)
                    && float.TryParse(parts[2], out var blur)
                    && Color.TryParse(parts[3], out var color))
                {
                    return new BoxShadow
                    {
                        OffsetX = offsetX,
                        OffsetY = offsetY,
                        Blur = blur,
                        Color = color
                    };
                }
            }

            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

