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
            return base.GetValue(property);
        }

        protected static DependencyPropertyKey RegDPKey<TOwner, T>(String name, PropertyMetadata pMetadata)
        {
            return DependencyProperty.RegisterReadOnly(name, typeof(T), typeof(TOwner), pMetadata);
        }

        protected static DependencyPropertyKey RegDPKey<TOwner, T>(String name)
        {
            return RegDPKey<TOwner, T>(name, new PropertyMetadata());
        }

        protected static DependencyProperty RegDP<TOwner, T>(String name)
        {
            return DependencyProperty.Register(name, typeof(T), typeof(TOwner), new PropertyMetadata());
        }


        protected void SetOrClearDP(DependencyPropertyKey key, Boolean condition, Object value)
        {
            if (condition)
                SetValue(key, value);
            else
                ClearValue(key);
        }
    }
}
