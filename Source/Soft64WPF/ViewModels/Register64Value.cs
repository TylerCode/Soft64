using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Soft64WPF.ViewModels
{
    public sealed class Register64Value : RegisterValueViewModel
    {
        public Register64Value()
        {
            Value = "0000000000000000";
        }

        protected override string MakeString(dynamic value)
        {
            UInt64 _value = value;
            return _value.ToString("X16");
        }

        protected override dynamic ParseValue(string value) =>
            UInt64.Parse(value, NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture);

        protected override bool ValueCheck(string value)
        {
            UInt64 _value = 0;
            return UInt64.TryParse(value, NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out _value);
        } 
    }
}
