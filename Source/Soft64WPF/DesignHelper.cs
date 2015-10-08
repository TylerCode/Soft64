using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Soft64;

namespace Soft64WPF
{
    public static class DesignHelper
    {
        public static void MakeTestMachine(DependencyObject o)
        {
            if (DesignerProperties.GetIsInDesignMode(o))
            {
                Machine machine = new Machine();
            }
        }
    }
}
