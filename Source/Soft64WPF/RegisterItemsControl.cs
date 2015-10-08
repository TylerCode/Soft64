using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Soft64WPF
{
    public class RegisterItemsControl : ItemsControl
    {
        public RegisterItemsControl() : base()
        {

        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return false;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ContentPresenter();
        }
    }
}
