using CommunityToolkit.Maui.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Converters
{
    public class NegativeAndPositiveNumberConverter : BaseConverter<object, object>
    {
        public override object DefaultConvertReturnValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override object DefaultConvertBackReturnValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override object ConvertFrom(object value, CultureInfo culture)
        {
            int number = (int)value;

            return number < 0 ? number : "+" + number;
        }

        public override object ConvertBackTo(object value, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
