using System.Windows;
using Soft64.MipsR4300.Debugging;
using Soft64WPF;
using Soft64;

namespace Soft64WPF.ViewModels
{
    public class CpuViewModel : QuickDependencyObject
    {
        internal CpuViewModel()
        {
            SetValue(DebuggerPK, new MipsDebuggerViewModel());
            SetValue(VirtualMemoryPK, StreamViewModel.NewModelFromStream(CPU.VirtualMemoryStream));
            SetValue(DebugVirtualMemoryPK, StreamViewModel.NewModelFromStream(new VMemViewStream()));
            SetValue(TlbCachePK, new TlbCacheViewModel());
            SetValue(StatePK, new ExecutionStateViewModel());
        }

        #region DP - Debugger
        private static readonly DependencyPropertyKey DebuggerPK = RegDPKey<CpuViewModel, MipsDebuggerViewModel>("Debugger");
        public static readonly DependencyProperty DebuggerProperty = DebuggerPK.DependencyProperty;
        public MipsDebuggerViewModel Debugger => GetValue(DebuggerProperty);
        #endregion

        #region DP - VirtualMemory
        private static readonly DependencyPropertyKey VirtualMemoryPK = RegDPKey<CpuViewModel, StreamViewModel>("VirtualMemory");
        public static readonly DependencyProperty VirtualMemoryProperty = VirtualMemoryPK.DependencyProperty;
        public StreamViewModel VirtualMemory => GetValue(VirtualMemoryProperty);
        #endregion

        #region DP - DebugVirtualMemory
        private static readonly DependencyPropertyKey DebugVirtualMemoryPK = RegDPKey<CpuViewModel, StreamViewModel>("DebugVirtualMemory");
        public static readonly DependencyProperty DebugVirtualMemoryProperty = DebugVirtualMemoryPK.DependencyProperty;
        public StreamViewModel DebugVirtualMemory => GetValue(DebugVirtualMemoryProperty);
        #endregion

        #region DP - TLB Cache
        private static readonly DependencyPropertyKey TlbCachePK = RegDPKey<CpuViewModel, TlbCacheViewModel>("TlbCache");
        public static readonly DependencyProperty TlbCacheProperty = TlbCachePK.DependencyProperty;
        public TlbCacheViewModel TlbCache => GetValue(TlbCacheProperty);
        #endregion

        #region DP - Execution State
        private static readonly DependencyPropertyKey StatePK = RegDPKey<CpuViewModel, ExecutionStateViewModel>("State");
        public static readonly DependencyProperty StateProperty = StatePK.DependencyProperty;
        public ExecutionStateViewModel State => GetValue(StateProperty);
        #endregion

        public CPUProcessor CPU => MachineViewModel.CurrentModel.CurrentMachine.DeviceCPU;
    }
}