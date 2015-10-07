using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using Soft64.MipsR4300;

using ObservableEntries = System.Collections.ObjectModel.ObservableCollection<Soft64WPF.ViewModels.TlbModelEntry>;

namespace Soft64WPF.ViewModels
{
    public sealed class TlbCacheViewModel : QuickDependencyObject
    {
        public TlbCacheViewModel()
        {
            SetValue(TlbEntriesPK, new ObservableEntries());
        }

        public void Load()
        {
            PageMask = TlbCache.PageMask;
            EntryHi = TlbCache.EntryHi;
            EntryLo0 = TlbCache.EntryLo0;
            EntryLo1 = TlbCache.EntryLo1;
            Index = TlbCache.Index;
            Wired = TlbCache.Wired;
            Random = TlbCache.Random;
            BadVAddress = TlbCache.BadVAddr;

            TlbEntries.Clear();

            for (Int32 i = 0; i < TlbCache.Count; i++)
            {
                TlbEntries.Add(new TlbModelEntry(i, TlbCache[i]));
            }
        }

        public void Store()
        {
            TlbCache.PageMask = PageMask;
            TlbCache.EntryHi = EntryHi;
            TlbCache.EntryLo0 = EntryLo0;
            TlbCache.EntryLo1 = EntryLo1;
            TlbCache.Index = Index;
            TlbCache.Wired = Wired;
            TlbCache.Random = Random;
            TlbCache.BadVAddr = BadVAddress;

            /* TODO: Sync entry collection to real cache */
        }

        #region DP - TLBEntries
        private static readonly DependencyPropertyKey TlbEntriesPK = RegDPKey<TlbCacheViewModel, ObservableEntries>("TlbEntries");
        public static readonly DependencyProperty TlbEntriesProperty = TlbEntriesPK.DependencyProperty;
        public ObservableEntries TlbEntries => GetValue(TlbEntriesProperty);
        #endregion

        #region DP - PageMask
        public static readonly DependencyProperty PageMaskProperty = RegDP<TlbCacheViewModel, UInt64>("PageMask");
        public UInt64 PageMask { get { return GetValue(PageMaskProperty); } set { SetValue(PageMaskProperty, value); } }
        #endregion

        #region DP - EntryHi
        public static readonly DependencyProperty EntryHiProperty = RegDP<TlbCacheViewModel, UInt64>("EntryHi");
        public UInt64 EntryHi { get { return GetValue(EntryHiProperty); } set { SetValue(EntryHiProperty, value); } }
        #endregion

        #region DP - EntryLo0
        public static readonly DependencyProperty EntryLo0Property = RegDP<TlbCacheViewModel, UInt64>("EntryLo0");
        public UInt64 EntryLo0 { get { return GetValue(EntryLo0Property); } set { SetValue(EntryLo0Property, value); } }
        #endregion

        #region DP - EntryLo1
        public static readonly DependencyProperty EntryLo1Property = RegDP<TlbCacheViewModel, UInt64>("EntryLo1");
        public UInt64 EntryLo1 { get { return GetValue(EntryLo1Property); } set { SetValue(EntryLo1Property, value); } }
        #endregion

        #region DP - Index
        public static readonly DependencyProperty IndexProperty = RegDP<TlbCacheViewModel, UInt64>("Index");
        public UInt64 Index { get { return GetValue(IndexProperty); } set { SetValue(IndexProperty, value); } }
        #endregion

        #region DP - Wired
        public static readonly DependencyProperty WiredProperty = RegDP<TlbCacheViewModel, UInt64>("Wired");
        public UInt64 Wired { get { return GetValue(WiredProperty); } set { SetValue(WiredProperty, value); } }
        #endregion

        #region DP - Random
        public static readonly DependencyProperty RandomProperty = RegDP<TlbCacheViewModel, UInt64>("Random");
        public UInt64 Random { get { return GetValue(RandomProperty); } set { SetValue(RandomProperty, value); } }
        #endregion

        #region DP - BadVAddress
        public static readonly DependencyProperty BadVAddressProperty = RegDP<TlbCacheViewModel, UInt64>("BadVAddress");
        public UInt64 BadVAddress { get { return GetValue(BadVAddressProperty); } set { SetValue(BadVAddressProperty, value); } }
        #endregion

        #region Weak Events

        public event EventHandler<TLBCacheChangeEventArgs> TLBChanged
        {
            add
            {
                WeakEventManager<TLBCache, TLBCacheChangeEventArgs>
                    .AddHandler(TlbCache, "CacheChanged", value);
            }

            remove
            {
                WeakEventManager<TLBCache, TLBCacheChangeEventArgs>
                    .RemoveHandler(TlbCache, "CacheChanged", value);
            }
        }

        #endregion

        public TLBCache TlbCache => MachineViewModel.CurrentModel.CurrentMachine.DeviceCPU.Tlb;
    }
}