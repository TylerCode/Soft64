/*
Soft64 - C# N64 Emulator
Copyright (C) Soft64 Project @ Codeplex
Copyright (C) 2013 - 2015 Bryan Perris
	
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
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Soft64.Engines
{
    /// <summary>
    /// Runs all tasks on a single thread
    /// </summary>
    /// <remarks>
    /// Warning: There should be just one self looping task after other tasks have finished, otherwise
    /// there is no way to ensure the loop will ever end and execute the tasks after it.  The core
    /// scheduler expects the engine to instantly kill all tasks when stopping the machine.  No resources
    /// need to be cleaned up inside the scheduler because the machine the machine already implements a clean
    /// disposal pattern.
    /// </remarks>
    public sealed class SingleCoreScheduler : CoreTaskScheduler
    {
        private Thread m_SingleThread;
        private Action m_CallChain;

        public SingleCoreScheduler()
            : base()
        {
        }

        protected override System.Threading.Thread GetTaskThread(Task task)
        {
            Action taskAction = new Action(() =>
            {
                this.TryExecuteTask(task);
            });

            if (m_CallChain == null)
            {
                m_CallChain = taskAction;
            }
            else
            {
                MulticastDelegate.Combine(m_CallChain, taskAction);
            }

            if (m_SingleThread == null)
            {
                RuntimeHelpers.PrepareMethod(this.GetType().GetMethod(
                    "ThreadCall",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic
                    ).MethodHandle);

                m_SingleThread = new Thread(ThreadCall);
            }

            return m_SingleThread;
        }

        [STAThread]
        private void ThreadCall(Object obj)
        {
            m_CallChain();
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}