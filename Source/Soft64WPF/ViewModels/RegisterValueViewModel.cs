using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64WPF.ViewModels
{
    public abstract class RegisterValueViewModel : IDataErrorInfo, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public String Value { get; set; }

        public String Error => null;

        public String this[String columnName]
        {
            get
            {
                if (columnName.Equals("Value"))
                {
                    if (ValueCheck(Value))
                    {
                        return null;
                    }
                    else
                    {
                        return "Invalid value input";
                    }
                }

                return null;
            }
        }

        public dynamic RegValue
        {
            get { return ParseValue(Value); }
            set { Value = MakeString(value); }
        }

        protected void OnPropertyChange(String name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected abstract Boolean ValueCheck(String value);

        protected abstract dynamic ParseValue(String value);

        protected abstract String MakeString(dynamic value);
    }
}
