﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using Soft64.MipsR4300;

namespace Soft64WPF.ViewModels
{
    public sealed class TlbCacheViewModel : MachineComponentViewModel
    {
        public TlbCacheViewModel(MachineViewModel parentMachineModel)
            : base(parentMachineModel)
        {
            Refresh();

            WeakEventManager<TLBCache, TLBCacheChangeEventArgs>.AddHandler(
                parentMachineModel.TargetMachine.DeviceCPU.Tlb,
                "CacheChanged",
                TlbChange);

            TlbEntries.CollectionChanged += TlbEntries_CollectionChanged;
        }

        private void TlbEntries_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                /* Update the real TLB cache */
            }
        }

        public void Refresh()
        {
            ReadRegs();
            ReadEntries();
        }

        private void TlbChange(Object sender, TLBCacheChangeEventArgs a)
        {
            Dispatcher.InvokeAsync(() =>
            {
                TLBCache cache = (TLBCache)sender;

                if (a.Index < 0)
                {
                    /* Do a full reload */
                    ReadEntries();
                    return;
                }

                if (cache[a.Index] != null)
                {
                    TlbEntries.Add(new TlbModelEntry(a.Index, cache[a.Index]));
                }
                else
                {
                    foreach (var entry in TlbEntries)
                    {
                        if (entry.EntryIndex == a.Index)
                        {
                            TlbEntries.Remove(entry);
                        }
                    }
                }
            });
        }

        private void ReadRegs()
        {
            TLBCache cache = ParentMachine.TargetMachine.DeviceCPU.Tlb;

            PageMask = cache.PageMask;
            EntryHi = cache.EntryHi;
            EntryLo0 = cache.EntryLo0;
            EntryLo1 = cache.EntryLo1;
            Index = cache.Index;
            Wired = cache.Wired;
            Random = cache.Random;
            BadVAddress = cache.BadVAddr;
        }

        private void ReadEntries()
        {
            TLBCache cache = ParentMachine.TargetMachine.DeviceCPU.Tlb;
        }

        private static void UpdateRegister(DependencyObject o, DependencyPropertyChangedEventArgs a)
        {
            TlbCacheViewModel model = o as TlbCacheViewModel;

            if (model != null)
            {
                TLBCache cache = model.ParentMachine.TargetMachine.DeviceCPU.Tlb;

                switch (a.Property.Name)
                {
                    case "PageMask": cache.PageMask = (UInt64)a.NewValue; break;
                    case "EntryHi": cache.EntryHi = (UInt64)a.NewValue; break;
                    case "EntryLo0": cache.EntryLo0 = (UInt64)a.NewValue; break;
                    case "EntryLo1": cache.EntryLo1 = (UInt64)a.NewValue; break;
                    case "Index": cache.Index = (UInt64)a.NewValue; break;
                    default: break;
                }
            }
        }

        private static readonly DependencyPropertyKey TlbEntriesPropertyKey =
        DependencyProperty.RegisterReadOnly("TlbEntries", typeof(ObservableCollection<TlbModelEntry>), typeof(TlbCacheViewModel),
        new PropertyMetadata(new ObservableCollection<TlbModelEntry>()));

        public static readonly DependencyProperty TlbEntriesProperty =
            TlbEntriesPropertyKey.DependencyProperty;

        public static readonly DependencyProperty PageMaskProperty =
            DependencyProperty.Register("PageMask", typeof(UInt64), typeof(TlbCacheViewModel),
            new PropertyMetadata(UpdateRegister));

        public static readonly DependencyProperty EntryHiProperty =
            DependencyProperty.Register("EntryHi", typeof(UInt64), typeof(TlbCacheViewModel),
            new PropertyMetadata(UpdateRegister));

        public static readonly DependencyProperty EntryLo0Property =
            DependencyProperty.Register("EntryLo0", typeof(UInt64), typeof(TlbCacheViewModel),
            new PropertyMetadata(UpdateRegister));

        public static readonly DependencyProperty EntryLo1Property =
            DependencyProperty.Register("EntryLo1", typeof(UInt64), typeof(TlbCacheViewModel),
            new PropertyMetadata(UpdateRegister));

        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(UInt64), typeof(TlbCacheViewModel),
            new PropertyMetadata(UpdateRegister));

        private static readonly DependencyPropertyKey WiredPropertyKey =
            DependencyProperty.RegisterReadOnly("Wired", typeof(UInt64), typeof(TlbCacheViewModel),
            new PropertyMetadata());

        public static readonly DependencyProperty WiredProperty =
           WiredPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey RandomPropertyKey =
            DependencyProperty.RegisterReadOnly("Random", typeof(UInt64), typeof(TlbCacheViewModel),
            new PropertyMetadata());

        public static readonly DependencyProperty RandomProperty =
            RandomPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey BadVAddressPropertyKey =
            DependencyProperty.RegisterReadOnly("BadVAddress", typeof(UInt64), typeof(TlbCacheViewModel),
            new PropertyMetadata());

        public static readonly DependencyProperty BadVAddressProperty =
            BadVAddressPropertyKey.DependencyProperty;

        public ObservableCollection<TlbModelEntry> TlbEntries
        {
            get { return (ObservableCollection<TlbModelEntry>)GetValue(TlbEntriesProperty); }
            private set { SetValue(TlbEntriesPropertyKey, value); }
        }

        public UInt64 PageMask
        {
            get { return (UInt64)GetValue(PageMaskProperty); }
            set { SetValue(PageMaskProperty, value); }
        }

        public UInt64 EntryHi
        {
            get { return (UInt64)GetValue(EntryHiProperty); }
            set { SetValue(EntryHiProperty, value); }
        }

        public UInt64 EntryLo0
        {
            get { return (UInt64)GetValue(EntryLo0Property); }
            set { SetValue(EntryLo0Property, value); }
        }

        public UInt64 EntryLo1
        {
            get { return (UInt64)GetValue(EntryLo1Property); }
            set { SetValue(EntryLo1Property, value); }
        }

        public UInt64 Index
        {
            get { return (UInt64)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        public UInt64 Wired
        {
            get { return (UInt64)GetValue(WiredProperty); }
            private set { SetValue(WiredPropertyKey, value); }
        }

        public UInt64 Random
        {
            get { return (UInt64)GetValue(RandomProperty); }
            private set { SetValue(RandomPropertyKey, value); }
        }

        public UInt64 BadVAddress
        {
            get { return (UInt64)GetValue(BadVAddressProperty); }
            private set { SetValue(BadVAddressPropertyKey, value); }
        }
    }

    public sealed class TlbModelEntry
    {
        private Int32 m_EntryIndex;
        private TLBEntry m_AssociatedEntry;

        public TlbModelEntry(Int32 index, TLBEntry entry)
        {
            m_EntryIndex = index;
            m_AssociatedEntry = entry;
        }

        public TLBEntry AssociatedEntry
        {
            get { return m_AssociatedEntry; }
        }

        public Int32 EntryIndex
        {
            get { return m_EntryIndex; }
        }
    }
}