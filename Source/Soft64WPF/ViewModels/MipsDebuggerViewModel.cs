﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Soft64.MipsR4300.Debugging;

namespace Soft64WPF.ViewModels
{
    public class MipsDebuggerViewModel : QuickDependencyObject
    {
        private MipsDebugger m_Debugger;

        public event EventHandler Finished;

        public MipsDebuggerViewModel()
        {
            if (MachineViewModel.CurrentModel.CurrentMachine == null)
                return;

            m_Debugger = new MipsDebugger();
            Disassembly = new ObservableCollection<DisassembledInstruction>();

            WeakEventManager<MipsDebugger, EventArgs>.AddHandler(
            m_Debugger,
            "CodeScanned",
            (o, e) =>
            {
                Dispatcher.InvokeAsync(() =>
                {
                    Disassembly.Clear();

                    foreach (var l in m_Debugger.Disassembly)
                    {
                        Disassembly.Add(l);
                    }

                    Dispatcher.InvokeAsync(() =>
                    {
                        var e1 = Finished;

                        if (e1 != null)
                            e1(this, new EventArgs());

                    }, DispatcherPriority.Render);
                });
            });
        }

        private readonly static DependencyPropertyKey DisassemblyPropertyKey =
            DependencyProperty.RegisterReadOnly("Disassembly", typeof(ObservableCollection<DisassembledInstruction>), typeof(MipsDebuggerViewModel),
            new PropertyMetadata());

        public readonly static DependencyProperty DisassemblyProperty =
            DisassemblyPropertyKey.DependencyProperty;

        public ObservableCollection<DisassembledInstruction> Disassembly
        {
            get { return (ObservableCollection<DisassembledInstruction>)GetValue(DisassemblyProperty); }
            private set {SetValue(DisassemblyPropertyKey, value); }
        }

        public MipsDebugger Debugger
        {
            get { return m_Debugger; }
        }

    }
}
