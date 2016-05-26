using System;
using System.ComponentModel;
using System.Globalization;
using PunyUrl.Domain.Entities;

namespace PunyUrl.Domain.Converters
{
    public class SmartUrlConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return SmartUrl.Parse(value as string);
        }
    }
}