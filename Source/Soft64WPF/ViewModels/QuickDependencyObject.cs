using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Soft64WPF.ViewModels
{
    public abstract class QuickDependencyObject : DependencyObject
    {
        /* Hide the old member and use a dynamic version to skip using casts */
        protected new dynamic GetValue(DependencyProperty property)
        {
            return GetValue(property);
        }

        protected static DependencyPropertyKey RegDPKey<TOwner, T>(String name)
        {
            return DependencyProperty.RegisterReadOnly(name, typeof(T), typeof(TOwner), new PropertyMetadata());
        }
    }
}
