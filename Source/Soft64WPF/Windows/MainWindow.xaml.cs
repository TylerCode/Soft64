﻿/*
Soft64 - C# N64 Emulator
Copyright (C) Soft64 Project @ Codeplex
Copyright (C) 2013 - 2014 Bryan Perris

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>
*/

using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using Microsoft.Win32;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using Soft64;
using Soft64.Debugging;
using Soft64.MipsR4300;
using Soft64WPF.ViewModels;
using Soft64WPF.Helper;

namespace Soft64WPF.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private BootBreakMode m_BreakOnBootMode;
        private MachineViewModel m_MachineVM;

        public enum BootBreakMode
        {
            None,
            Pre,
            Post
        }

        public MainWindow()
        {
            InitializeComponent();
            m_MachineVM = (MachineViewModel)FindResource("machineVM");

            xamlControl_EmuRunButton.Click += xamlControl_EmuRunButton_Click;
            xamlControl_MainCartBrowseFileButton.Click += xamlControl_MainCartBrowseFileButton_Click;
            xaml_ButtonScript.Click += xaml_ButtonScript_Click;
            xaml_ButtonToolCPUDebugger.Click += xaml_ButtonToolCPUDebugger_Click;
            xaml_ButtonToolMemoryEditor.Click += xaml_ButtonToolMemoryEditor_Click;
            xamlControl_EmuRunPostDebugButton.Click += xamlControl_EmuRunPostDebugButton_Click;

            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            Loaded += MainWindow_Loaded;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        /* This is where emulator is started */
        private void RunEmu()
        {
            if (m_BreakOnBootMode == BootBreakMode.Post)
            {
                m_MachineVM.MachineEventNotification += (o, e) =>
                {
                    if (e.EventType == MachineEventType.Booted)
                        Machine.Current.Pause();
                };
            }

            Machine.Current.Run();
        }

        #region WPF Event Handlers

        void xamlControl_EmuRunPostDebugButton_Click(object sender, RoutedEventArgs e)
        {
            m_BreakOnBootMode = BootBreakMode.Post;
            RunEmu();
        }

        /* Window load event */
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ((Paragraph)xaml_LogRichBox.Document.Blocks.FirstBlock).LineHeight = 3;

                var target = new WpfRichTextBoxTarget
                {
                    Name = "RichText",
                    Layout = @"[${level:uppercase=true}] :: ${callsite:className=true:fileName=false:includeSourcePath=false:methodName=false:cleanNamesOfAnonymousDelegates=false} :: ${message} ${exception:innerFormat=tostring:maxInnerExceptionLevel=10:separator=,:format=tostring}",
                    ControlName = xaml_LogRichBox.Name,
                    FormName = GetType().Name,
                    AutoScroll = true,
                    MaxLines = 100000,
                    UseDefaultRowColoringRules = false,
                };
                var asyncWrapper = new AsyncTargetWrapper { Name = "RichTextAsync", WrappedTarget = target };

                target.RowColoringRules.Clear();
                target.RowColoringRules.Add(new WpfRichTextBoxRowColoringRule("level == LogLevel.Fatal", "White", "Red", FontStyles.Normal, FontWeights.Bold));
                target.RowColoringRules.Add(new WpfRichTextBoxRowColoringRule("level == LogLevel.Error", "Red", "Transparent", FontStyles.Italic, FontWeights.Bold));
                target.RowColoringRules.Add(new WpfRichTextBoxRowColoringRule("level == LogLevel.Warn", "Orange", "Transparent"));
                target.RowColoringRules.Add(new WpfRichTextBoxRowColoringRule("level == LogLevel.Info", "Black", "Transparent"));
                target.RowColoringRules.Add(new WpfRichTextBoxRowColoringRule("level == LogLevel.Debug", "Gray", "Transparent"));
                target.RowColoringRules.Add(new WpfRichTextBoxRowColoringRule("level == LogLevel.Trace", "White", "Transparent", FontStyles.Normal, FontWeights.Normal));

                var cpuLogTarget = new FileTarget();
                cpuLogTarget.Name = "instTraceFile";
                cpuLogTarget.Layout = "${message}";
                cpuLogTarget.FileName = "${basedir}/logs/cpuTrace.txt";
                cpuLogTarget.KeepFileOpen = false;
                cpuLogTarget.DeleteOldFileOnStartup = true;
                cpuLogTarget.Encoding = Encoding.ASCII;

                LogManager.Configuration.AddTarget(asyncWrapper.Name, asyncWrapper);
                LogManager.Configuration.AddTarget(cpuLogTarget.Name, cpuLogTarget);

                var cpulogrule = new LoggingRule("Soft64.MipsR4300.Interpreter", LogLevel.Trace, cpuLogTarget);
                cpulogrule.Final = true;

                LogManager.Configuration.LoggingRules.Add(cpulogrule);
                LogManager.Configuration.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, asyncWrapper));

                LogManager.ReconfigExistingLoggers();
            });
        }

        private void xaml_ButtonScript_Click(object sender, RoutedEventArgs e)
        {
            PyWindow window = new PyWindow();
            window.Show();
        }

        /* Exception leak handler - Show a message box when uncaught exceptions occur */
        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Runtime Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void xamlControl_MainCartBrowseFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = @"(N64 Cartridge ROM (*.z64;*.n64;*.v64;*.bin)|*.n64;*.z64;*.v64;*.bin|All Files (*.*)|*.*;";
            String romdir = Machine.Current.GetUIConfig<String>("RecentRomDirectory");

            if (Directory.Exists(romdir))
                dlg.InitialDirectory = romdir;

            if (dlg.ShowDialog() == true)
            {
                Machine.Current.SetUIConfig<String>("RecentRomDirectory", Path.GetDirectoryName(dlg.FileName));
                Machine.Current.CartridgeInsertFile(dlg.FileName);
            }
        }

        private void xamlControl_EmuRunButton_Click(object sender, RoutedEventArgs e)
        {
            RunEmu();
        }

        private void xaml_ButtonEjectCartridge_Click(object sender, RoutedEventArgs e)
        {
            Machine.Current.CartridgeEject();
        }

        private void xaml_ButtonToolMemoryEditor_Click(object sender, RoutedEventArgs e)
        {
            MemoryEditorWindow window = new MemoryEditorWindow();
            window.Show();
        }

        private void xaml_ButtonToolCPUDebugger_Click(object sender, RoutedEventArgs e)
        {
            CPUDebugger win = new CPUDebugger();
            win.Show();
        }

        #endregion
    }
}