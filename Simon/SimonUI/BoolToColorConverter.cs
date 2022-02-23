using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;


namespace SimonUI
{
    internal class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string parameterString = parameter as string;
            if (!string.IsNullOrEmpty(parameterString))
            {
                string[] parameters = parameterString.Split(new char[] { '|' });
                var darkString = parameters[0].Split(new char[] { ',' });
               
                var darkR = GetColorByte(darkString[0]);
                var darkG = GetColorByte(darkString[1]);
                var darkB = GetColorByte(darkString[2]);

                var dark = Color.FromRgb(darkR, darkG, darkB);
                SolidColorBrush darkColor = new SolidColorBrush(dark);

                var lightString = parameters[1].Split(new char[] { ',' });

                var lightR = GetColorByte(lightString[0]);
                var lightG = GetColorByte(lightString[1]);
                var lightB = GetColorByte(lightString[2]);

                var light = Color.FromRgb(lightR, lightG, lightB);
                SolidColorBrush lightColor = new SolidColorBrush(light);

                if ((bool)value)
                {
                    return lightColor;
                }

                return darkColor;
            }
            return Binding.DoNothing;
        }

        private byte GetColorByte(string byteString)
        {
            int value;
            int.TryParse(byteString, out value);
            return (byte)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
