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
using System.Linq;
using System.Windows;
using Soft64;
using Soft64WPF.Windows;

namespace Soft64WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Closed += mainWindow_Closed;
            mainWindow.Show();

            base.OnStartup(e);
        }

        private void mainWindow_Closed(object sender, EventArgs e)
        {
            Machine.Current.Stop();
            Machine.Current.Dispose();

            var childWindows = from window in Application.Current.Windows.Cast<Window>()
                               where window.IsLoaded
                               select window;

            foreach (Window win in childWindows)
            {
                win.Close();
            }
        }
    }

    public static class EntryPoint
    {
        [STAThread]
        public static void Main()
        {
            /* Create an instance of the emulator machine */
            Machine machine = new Machine();

            App app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}