using Soft64;
using Soft64.MipsR4300.Debugging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64UI
{
    public class N64MemoryScriptObject
    {
        private WeakReference<Stream> m_VMemStream;
        private Int64 m_VMemPosition;

        public N64MemoryScriptObject()
        {
            m_VMemStream = new WeakReference<Stream>(new VMemViewStream());
        }

        public dynamic VirtualMemoryAddress
        {
            get { return m_VMemPosition; }

            set
            {
                m_VMemPosition = (Int64)(UInt32)(UInt64)value;
            }
        }

        public dynamic GetPC()
        {
            return Machine.Current.DeviceCPU.State.PC;
        }

        public dynamic ReadVirtualMemory (dynamic count)
        {
            Stream stream = null;

            Int32 _count = Convert.ToInt32(count);

            if (m_VMemStream.TryGetTarget(out stream))
            {
                Byte[] buffer = new Byte[_count];
                stream.Position = m_VMemPosition;
                stream.Read(buffer, 0, _count);
                return buffer;
            }
            else
            {
                return null;
            }
        }

        public void WriteVirtualMemoryByte(dynamic b)
        {
            Stream stream = null;

            if (m_VMemStream.TryGetTarget(out stream))
            {
                stream.Position = this.m_VMemPosition;
                stream.WriteByte(Convert.ToByte(b));
            }
        }
    }
}
