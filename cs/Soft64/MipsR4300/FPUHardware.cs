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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Soft64.MipsR4300
{
    public static class FPUHardware
    {
        private const Int32 _MCW_RC = 0x00000300;
        private const Int32 _MCW_EM = 0x0008001f;
        private const Int32 _EM_INEXACT = 0x00000001;
        private const Int32 _EM_UNDERFLOW = 0x00000002;
        private const Int32 _EM_OVERFLOW = 0x00000004;
        private const Int32 _EM_ZERODIVIDE = 0x00000008;
        private const Int32 _EM_INVALID = 0x00000010;

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint _controlfp(uint newControl, uint mask);

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint _statusfp();

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint _clearfp();


        public static void SetRoundingMode(FPURoundMode mode)
        {
            _controlfp((UInt32)mode, _MCW_RC);
        }

        public static void ClearFPUExceptionMask()
        {
            _controlfp(0, _MCW_EM);
        }

        public static Boolean CheckFPUException()
        {
            return _statusfp() != 0;
        }

        public static FPUExceptionType GetFPUException()
        {
            UInt32 status = _statusfp();
            _clearfp();

            if (status != 0)
            {
                if ((status & _EM_INEXACT) == _EM_INEXACT)
                {
                    return FPUExceptionType.Inexact;
                }
                else if ((status & _EM_INVALID) == _EM_INVALID)
                {
                    return FPUExceptionType.Invalid;
                }
                else if ((status & _EM_OVERFLOW) == _EM_OVERFLOW)
                {
                    return FPUExceptionType.Overflow;
                }
                else if ((status & _EM_UNDERFLOW) == _EM_UNDERFLOW)
                {
                    return FPUExceptionType.Underflow;
                }
                else if ((status & _EM_ZERODIVIDE) == _EM_ZERODIVIDE)
                {
                    return FPUExceptionType.DivideByZero;
                }
                else
                {
                    return FPUExceptionType.Unimplemented;
                }
            }
            else
            {
                return FPUExceptionType.Unimplemented;
            }
        }
    }
}
