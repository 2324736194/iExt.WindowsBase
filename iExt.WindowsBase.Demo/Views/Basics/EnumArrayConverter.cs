using System;
using System.Globalization;
using System.Windows.Data;

namespace iExt.WindowsBase.Demo.Views.Basics
{
    public class EnumArrayConverter : IValueConverter
    {       
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumType = default(Type);
            switch (value)
            {
                case Enum e:
                    enumType = e.GetType();
                    break;
                case Type type:
                    if (type.IsEnum)
                    {

                        enumType = type;
                    }
                    break;
            }

            if (null != enumType)
            {
                return Enum.GetValues(enumType);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}