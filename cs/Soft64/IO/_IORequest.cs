﻿/*
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

namespace Soft64.IO
{
    public class _IORequest
    {
        private Int64 m_Offset;
        private Int64 m_Length;

        public _IORequest(Int64 offset, Int64 length)
        {
            m_Offset = offset;
            m_Length = length;
        }

        public Int64 Offset
        {
            get { return m_Offset; }
        }

        public Int64 Length
        {
            get { return m_Length; }
        }

        public Int64 ReadEnd
        {
            get { return Offset + Length; }
        }
    }
}