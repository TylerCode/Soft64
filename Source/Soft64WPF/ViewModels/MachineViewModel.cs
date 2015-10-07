using System;
using System.Windows;
using Soft64;

namespace Soft64WPF.ViewModels
{
    public sealed class MachineViewModel : QuickDependencyObject
    {
        private static MachineViewModel s_CurrentModel = null;
        public static MachineViewModel CurrentModel => s_CurrentModel;

        public MachineViewModel()
        {
            s_CurrentModel = this;

            SetValue(CurrentMachinePK, Machine.Current);
            SetValue(CartridgePK, new CartridgeViewModel());
            SetValue(DeviceRcpPK, new RcpViewModel());
            SetValue(DeviceCpuPK, new CpuViewModel());
            SetValue(EnginePK, new EmulatorEngineViewModel());
        }

        #region DP - Current Machine
        private static readonly DependencyPropertyKey CurrentMachinePK= RegDPKey<MachineViewModel, Machine>("CurrentMachine");
        public static readonly DependencyProperty CurrentMachineProperty = CurrentMachinePK.DependencyProperty;
        public Machine CurrentMachine => GetValue(CurrentMachineProperty);
        #endregion

        #region DP - Cartridge
        private static readonly DependencyPropertyKey CartridgePK = RegDPKey<MachineViewModel, CartridgeViewModel>("Cartridge");
        public static readonly DependencyProperty CartridgeProperty = CartridgePK.DependencyProperty;
        public CartridgeViewModel Cartridge => GetValue(CartridgeProperty);
        #endregion

        #region DP - DeviceRCP
        private static readonly DependencyPropertyKey DeviceRcpPK = RegDPKey<MachineViewModel, RcpViewModel>("DeviceRcp");
        public static readonly DependencyProperty DeviceRcpProperty = DeviceRcpPK.DependencyProperty;
        public RcpViewModel DeviceRcp => GetValue(DeviceRcpProperty);
        #endregion

        #region DP - DeviceCPU
        private static readonly DependencyPropertyKey DeviceCpuPK = RegDPKey<MachineViewModel, CpuViewModel>("DeviceCpu");
        public static readonly DependencyProperty DeviceCpuProperty = DeviceCpuPK.DependencyProperty;
        public CpuViewModel DeviceCpu => GetValue(DeviceCpuProperty);
        #endregion

        #region DP - Engine
        private static readonly DependencyPropertyKey EnginePK = RegDPKey<MachineViewModel, EmulatorEngineViewModel>("Engine");
        public static readonly DependencyProperty EngineProperty = EnginePK.DependencyProperty;
        public EmulatorEngineViewModel Engine => GetValue(EngineProperty);
        #endregion

        #region WeakEvents

        public event EventHandler<MachineEventNotificationArgs> MachineEventNotification
        {
            add
            {
                WeakEventManager<Machine, MachineEventNotificationArgs>
                    .AddHandler(CurrentMachine, "MachineEventNotification", value);
            }

            remove
            {
                WeakEventManager<Machine, MachineEventNotificationArgs>
                    .RemoveHandler(CurrentMachine, "MachineEventNotification", value);
            }
        }

        #endregion
    }
}