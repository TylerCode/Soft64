using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Soft64Binding.WPF
{
    public class RegisterFloatValue : IDataErrorInfo, INotifyPropertyChanged
    {
        private String m_Value = "0.000000";

        public String Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public string Error
        {
            get { return null; }
        }

        internal Double RegValue
        {
            get
            {
                Double v = 0;
                Double.TryParse(m_Value, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out v);
                return v;
            }

            set
            {
                m_Value = value.ToString("0.######");
                OnPropertyChange("Value");
            }
        }

        public string this[string columnName]
        {
            get 
            {
                Double v = 0;

                if (columnName.Equals("Value"))
                {
                    if (Double.TryParse(m_Value, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out v))
                    {
                        return null;
                    }
                    else
                    {
                        return "Value must be in valid decimal value";
                    }
                }

                return null;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChange(String name)
        {
            var e = PropertyChanged;

            if (e != null)
            {
                e(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion
    }
}
