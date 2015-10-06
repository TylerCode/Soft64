using System;
using System.Windows;
using Soft64;

namespace Soft64WPF.ViewModels
{
    public sealed class MachineViewModel : QuickDependencyObject
    {
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
        public RcpViewModel DeviceCpu => GetValue(DeviceCpuProperty);
        #endregion

        private static readonly DependencyPropertyKey EnginePropertyKey =
            DependencyProperty.RegisterReadOnly("Engine", typeof(EmulatorEngineViewModel), typeof(MachineViewModel),
            new PropertyMetadata());

        public static readonly DependencyProperty EngineProperty =
            EnginePropertyKey.DependencyProperty;

        public MachineViewModel()
        {
            /* Important: Prevent crashes if the machine hasn't been created yet */
            if (Machine.Current == null)
                return;

            SetValue(CurrentMachinePropertyKey, Machine.Current);
            SetValue(CartridgePropertyKey, new CartridgeViewModel(this));
            SetValue(RcpPropertyKey, new RcpViewModel(this));
            SetValue(CpuPropertyKey, new CpuViewModel(this));
            SetValue(EnginePropertyKey, new EmulatorEngineViewModel(this));
        }

        public EmulatorEngineViewModel Engine
        {
            get { return (EmulatorEngineViewModel)GetValue(EngineProperty); }
        }

        public event EventHandler<MachineEventNotificationArgs> MachineEventNotification
        {
            add
            {
                WeakEventManager<Machine, MachineEventNotificationArgs>.AddHandler(CurrentMachine,  "MachineEventNotification", value);
            }

            remove
            {
                WeakEventManager<Machine, MachineEventNotificationArgs>.RemoveHandler(CurrentMachine, "MachineEventNotification", value);
            }
        }
    }
}