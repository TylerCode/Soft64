/*
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
using System.Windows;
using Soft64;
using Soft64.RCP;

namespace Soft64WPF.ViewModels
{
    public sealed class CartridgeViewModel : QuickDependencyObject
    {
        internal CartridgeViewModel()
        {
            SetValue(NamePK, "<Slot Empty>");

            CartridgeChanged += CartridgeViewModel_CartridgeChanged;
        }

        private void CartridgeViewModel_CartridgeChanged(object sender, CartridgeChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Boolean set = e.NewCartridge != null;
                SetOrClearDP(NamePK, set, e.NewCartridge?.ToString());
                SetOrClearDP(HeaderPK, set, e.NewCartridge?.GetCartridgeInfo());
                SetOrClearDP(IsCICSkippedPK, set, e.NewCartridge?.LockoutKey == null);
                SetOrClearDP(IsCartridgeInsertedPK, set, set);
            });
        }

        #region DP - Name
        private static readonly DependencyPropertyKey NamePK = RegDPKey<CartridgeViewModel, String>("Name");
        public static readonly DependencyProperty NameProperty = NamePK.DependencyProperty;
        public String Name => GetValue(NameProperty);
        #endregion

        #region DP - Header
        private static readonly DependencyPropertyKey HeaderPK = RegDPKey<CartridgeViewModel, CartridgeInfo>("Header");
        public static readonly DependencyProperty HeaderProperty = HeaderPK.DependencyProperty;
        public CartridgeInfo Header => GetValue(HeaderProperty);
        #endregion

        #region DP - IsCICSkipped
        private static readonly DependencyPropertyKey IsCICSkippedPK = RegDPKey<CartridgeViewModel, Boolean>("IsCICSkipped");
        public static readonly DependencyProperty IsCICSkippedProperty = IsCICSkippedPK.DependencyProperty;
        public Boolean IsCICSkipped => GetValue(IsCICSkippedProperty);
        #endregion

        #region DP - IsCartridgeInserted
        private static readonly DependencyPropertyKey IsCartridgeInsertedPK = RegDPKey<CartridgeViewModel, Boolean>("IsCartridgeInserted");
        public static readonly DependencyProperty IsCartridgeInsertedProperty = IsCartridgeInsertedPK.DependencyProperty;
        public Boolean IsCartridgeInserted => GetValue(IsCartridgeInsertedProperty);
        #endregion

        #region WeakEvents

        public event EventHandler<CartridgeChangedEventArgs> CartridgeChanged
        {
            add
            {
                if (PI != null)
                WeakEventManager<ParallelInterface, CartridgeChangedEventArgs>
                    .AddHandler(PI, "CartridgeChanged", value);
            }

            remove
            {
                if (PI != null)
                    WeakEventManager<ParallelInterface, CartridgeChangedEventArgs>
                    .RemoveHandler(PI, "CartridgeChanged", value);
            }
        }

        #endregion

        private ParallelInterface PI => MachineViewModel.CurrentModel.CurrentMachine?.DeviceRCP.Interface_Parallel;
    }
}