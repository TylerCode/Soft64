using Soft64.MipsR4300;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Soft64WPF.ViewModels
{
    public sealed class Cop0RegistersViewModel : QuickDependencyObject
    {
        internal Cop0RegistersViewModel()
        {
            SetValue(CountPK, new Register32Value());
            SetValue(ComparePK, new Register32Value());
            SetValue(BadVAddressPK, new Register64Value());
            SetValue(EntryHiPK, new Register64Value());
            SetValue(EntryLo0PK, new Register64Value());
            SetValue(EntryLo1PK, new Register64Value());
            SetValue(IndexPK, new Register64Value());
            SetValue(PageMaskPK, new Register64Value());
            SetValue(RandomPK, new Register64Value());
            SetValue(WiredPK, new Register64Value());
            SetValue(ContextPK, new Register64Value());
            SetValue(EPCPK, new Register64Value());
            SetValue(PRIdPK, new Register64Value());
            SetValue(ConfigPK, new Register64Value());
            SetValue(LLAddrPK, new Register64Value());
            SetValue(WatchLoPK, new Register64Value());
            SetValue(WatchHiPK, new Register64Value());
            SetValue(XContextPK, new Register64Value());
            SetValue(ECCPK, new Register64Value());
            SetValue(CacheErrPK, new Register64Value());
            SetValue(TagLoPK, new Register64Value());
            SetValue(TagHiPK, new Register64Value());
            SetValue(ErrorEPCPK, new Register64Value());
        }

        public void Load()
        {
            Count.RegValue = Regs.Count;
            Compare.RegValue = Regs.Compare;
            BadVAddress.RegValue = Regs.BadVAddr;
            EntryHi.RegValue = Regs.EntryHi;
            EntryLo0.RegValue = Regs.EntryLo0;
            EntryLo1.RegValue = Regs.EntryLo1;
            Index.RegValue = Regs.Index;
            PageMask.RegValue = Regs.PageMask;
            Random.RegValue = Regs.Random;
            Wired.RegValue = Regs.Wired;
            Context.RegValue = Regs.Context;
            EPC.RegValue = Regs.EPC;
            PRId.RegValue = Regs.PRId;
            Config.RegValue = Regs.Config;
            LLAddr.RegValue = Regs.LLAddr;
            WatchLo.RegValue = Regs.WatchLo;
            WatchHi.RegValue = Regs.WatchHi;
            XContext.RegValue = Regs.XContext;
            ECC.RegValue = Regs.ECC;
            CacheErr.RegValue = Regs.CacheErr;
            TagLo.RegValue = Regs.TagLo;
            TagHi.RegValue = Regs.TagHi;
            ErrorEPC.RegValue = Regs.ErrorEPC;
        }

        public void Store()
        {
            Regs.Count = Count.RegValue;
            Regs.Compare = Compare.RegValue;
            Regs.BadVAddr = BadVAddress.RegValue;
            Regs.EntryHi = EntryHi.RegValue;
            Regs.EntryLo0 = EntryLo0.RegValue;
            Regs.EntryLo1 = EntryLo1.RegValue;
            Regs.Index = Index.RegValue;
            Regs.PageMask = PageMask.RegValue;
            Regs.Random = Random.RegValue;
            Regs.Wired = Wired.RegValue;
            Regs.Context = Context.RegValue;
            Regs.EPC = EPC.RegValue;
            Regs.PRId = PRId.RegValue;
            Regs.Config = Config.RegValue;
            Regs.LLAddr = LLAddr.RegValue;
            Regs.WatchLo = WatchLo.RegValue;
            Regs.WatchHi = WatchHi.RegValue;
            Regs.XContext = XContext.RegValue;
            Regs.ECC = ECC.RegValue;
            Regs.CacheErr = CacheErr.RegValue;
            Regs.TagLo = TagLo.RegValue;
            Regs.TagHi = TagHi.RegValue;
            Regs.ErrorEPC = ErrorEPC.RegValue;
        }

        #region DP - Count
        private static readonly DependencyPropertyKey CountPK = RegDPKey<Cop0RegistersViewModel, Register32Value>("Count");
        public static readonly DependencyProperty CountProperty = CountPK.DependencyProperty;
        public Register32Value Count => GetValue(CountProperty);
        #endregion

        #region DP - Compare
        private static readonly DependencyPropertyKey ComparePK = RegDPKey<Cop0RegistersViewModel, Register32Value>("Compare");
        public static readonly DependencyProperty CompareProperty = ComparePK.DependencyProperty;
        public Register32Value Compare => GetValue(CompareProperty);
        #endregion

        #region DP - Bad Virtual Address
        private static readonly DependencyPropertyKey BadVAddressPK = RegDPKey<Cop0RegistersViewModel, Register64Value>("BadVAddress");
        public static readonly DependencyProperty BadVAddressProperty = BadVAddressPK.DependencyProperty;
        public Register64Value BadVAddress => GetValue(BadVAddressProperty);
        #endregion

        #region DP - Entry Hi
        private static readonly DependencyPropertyKey EntryHiPK = RegDPKey<Cop0RegistersViewModel, Register64Value>("EntryHi");
        public static readonly DependencyProperty EntryHiProperty = EntryHiPK.DependencyProperty;
        public Register64Value EntryHi => GetValue(EntryHiProperty);
        #endregion

        #region DP - Entry Lo 0
        private static readonly DependencyPropertyKey EntryLo0PK = RegDPKey<Cop0RegistersViewModel, Register64Value>("EntryLo0");
        public static readonly DependencyProperty EntryLo0Property = EntryLo0PK.DependencyProperty;
        public Register64Value EntryLo0 => GetValue(EntryLo0Property);
        #endregion

        #region DP - Entry Lo 1
        private static readonly DependencyPropertyKey EntryLo1PK = RegDPKey<Cop0RegistersViewModel, Register64Value>("EntryLo1");
        public static readonly DependencyProperty EntryLo1Property = EntryLo1PK.DependencyProperty;
        public Register64Value EntryLo1 => GetValue(EntryLo1Property);
        #endregion

        #region DP - Index
        private static readonly DependencyPropertyKey IndexPK = RegDPKey<Cop0RegistersViewModel, Register64Value>("Index");
        public static readonly DependencyProperty IndexProperty = IndexPK.DependencyProperty;
        public Register64Value Index => GetValue(IndexProperty);
        #endregion

        #region DP - PageMask
        private static readonly DependencyPropertyKey PageMaskPK = RegDPKey<Cop0RegistersViewModel, Register64Value>("PageMask");
        public static readonly DependencyProperty PageMaskProperty = PageMaskPK.DependencyProperty;
        public Register64Value PageMask => GetValue(PageMaskProperty);
        #endregion

        #region DP - Random
        private static readonly DependencyPropertyKey RandomPK = RegDPKey<Cop0RegistersViewModel, Register64Value>("Random");
        public static readonly DependencyProperty RandomProperty = RandomPK.DependencyProperty;
        public Register64Value Random => GetValue(RandomProperty);
        #endregion

        #region DP - Wired
        private static readonly DependencyPropertyKey WiredPK = RegDPKey<Cop0RegistersViewModel, Register64Value>("Wired");
        public static readonly DependencyProperty WiredProperty = WiredPK.DependencyProperty;
        public Register64Value Wired => GetValue(WiredProperty);
        #endregion

        #region DP - Context
        private static readonly DependencyPropertyKey ContextPK = RegDPKey<Cop0RegistersViewModel, Register64Value>("Context");
        public static readonly DependencyProperty ContextProperty = ContextPK.DependencyProperty;
        public Register64Value Context => GetValue(ContextProperty);
        #endregion

        #region DP - EPC
        private static readonly DependencyPropertyKey EPCPK = RegDPKey<Cop0RegistersViewModel, Register64Value>("EPC");
        public static readonly DependencyProperty EPCProperty = EPCPK.DependencyProperty;
        public Register64Value EPC => GetValue(EPCProperty);
        #endregion

        #region DP - PRId
        private static readonly DependencyPropertyKey PRIdPK = RegDPKey<Cop0RegistersViewModel, Register64Value>("PRId");
        public static readonly DependencyProperty PRIdProperty = PRIdPK.DependencyProperty;
        public Register64Value PRId => GetValue(PRIdProperty);
        #endregion

        #region DP - Config
        private static readonly DependencyPropertyKey ConfigPK = RegDPKey<Cop0RegistersViewModel, Register64Value>("Config");
        public static readonly DependencyProperty ConfigProperty = ConfigPK.DependencyProperty;
        public Register64Value Config => GetValue(ConfigProperty);
        #endregion

        #region DP - LLAddr
        private static readonly DependencyPropertyKey LLAddrPK = RegDPKey<Cop0RegistersViewModel, Register64Value>("LLAddr");
        public static readonly DependencyProperty LLAddrProperty = LLAddrPK.DependencyProperty;
        public Register64Value LLAddr => GetValue(LLAddrProperty);
        #endregion

        #region DP - WatchLo
        private static readonly DependencyPropertyKey WatchLoPK = RegDPKey<Cop0RegistersViewModel, Register64Value>("WatchLo");
        public static readonly DependencyProperty WatchLoProperty = WatchLoPK.DependencyProperty;
        public Register64Value WatchLo => GetValue(WatchLoProperty);
        #endregion

        #region DP - WatchHi
        private static readonly DependencyPropertyKey WatchHiPK = RegDPKey<Cop0RegistersViewModel, Register64Value>("WatchHi");
        public static readonly DependencyProperty WatchHiProperty = WatchHiPK.DependencyProperty;
        public Register64Value WatchHi => GetValue(WatchHiProperty);
        #endregion

        #region DP - XContext
        private static readonly DependencyPropertyKey XContextPK = RegDPKey<Cop0RegistersViewModel, Register64Value>("XContext");
        public static readonly DependencyProperty XContextProperty = XContextPK.DependencyProperty;
        public Register64Value XContext => GetValue(XContextProperty);
        #endregion

        #region DP - ECC
        private static readonly DependencyPropertyKey ECCPK = RegDPKey<Cop0RegistersViewModel, Register64Value>("ECC");
        public static readonly DependencyProperty ECCProperty = ECCPK.DependencyProperty;
        public Register64Value ECC => GetValue(ECCProperty);
        #endregion

        #region DP - CacheErr
        private static readonly DependencyPropertyKey CacheErrPK = RegDPKey<Cop0RegistersViewModel, Register64Value>("CacheErr");
        public static readonly DependencyProperty CacheErrProperty = CacheErrPK.DependencyProperty;
        public Register64Value CacheErr => GetValue(CacheErrProperty);
        #endregion

        #region DP - TagLo
        private static readonly DependencyPropertyKey TagLoPK = RegDPKey<Cop0RegistersViewModel, Register64Value>("TagLo");
        public static readonly DependencyProperty TagLoProperty = TagLoPK.DependencyProperty;
        public Register64Value TagLo => GetValue(TagLoProperty);
        #endregion

        #region DP - TagHi
        private static readonly DependencyPropertyKey TagHiPK = RegDPKey<Cop0RegistersViewModel, Register64Value>("TagHi");
        public static readonly DependencyProperty TagHiProperty = TagHiPK.DependencyProperty;
        public Register64Value TagHi => GetValue(TagHiProperty);
        #endregion

        #region DP - ErrorEPC
        private static readonly DependencyPropertyKey ErrorEPCPK = RegDPKey<Cop0RegistersViewModel, Register64Value>("ErrorEPC");
        public static readonly DependencyProperty ErrorEPCProperty = ErrorEPCPK.DependencyProperty;
        public Register64Value ErrorEPC => GetValue(ErrorEPCProperty);
        #endregion

        CP0Registers Regs => MachineViewModel.CurrentModel?.CurrentMachine?.DeviceCPU.State.CP0Regs;
    }
}
