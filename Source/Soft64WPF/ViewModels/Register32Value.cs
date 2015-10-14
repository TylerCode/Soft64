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
    public sealed class Register32Value : RegisterValueViewModel
    {
        public Register32Value()
        {
            Value = "00000000";
        }

        protected override string MakeString(dynamic value)
        {
            UInt32 _value = value;
            return _value.ToString("X8");
        }

        protected override dynamic ParseValue(string value) =>
            UInt32.Parse(value, NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture);

        protected override bool ValueCheck(string value)
        {
            UInt32 _value = 0;
            return UInt32.TryParse(value, NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out _value);
        } 
    }
}
