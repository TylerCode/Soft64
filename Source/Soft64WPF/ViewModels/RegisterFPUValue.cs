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
    public sealed class RegisterFPUValue : RegisterValueViewModel
    {
        /* TODO: Use tagging support to switch value mode of FPU registers */

        public RegisterFPUValue()
        {
            Value = "0.000000";
        }

        protected override string MakeString(dynamic value)
        {
            Double d = value;
            return d.ToString();
        }

        protected override dynamic ParseValue(string value)
        {
            return Double.Parse(value, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture);
        }

        protected override bool ValueCheck(string value)
        {
            Double d;
            return Double.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out d);
        }
    }
}
