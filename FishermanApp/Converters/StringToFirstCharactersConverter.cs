using CommunityToolkit.Maui.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Converters
{
    public class StringToFirstCharactersConverter : BaseConverter<object, object, object>
    {
        public override object DefaultConvertReturnValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override object DefaultConvertBackReturnValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override object ConvertFrom(object value, object parameter, CultureInfo culture)
        {
            return value.ToString().Substring(0, Convert.ToInt32(parameter));
        }

        public override object ConvertBackTo(object value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
