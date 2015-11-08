using Soft64;
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
            m_VMemStream = new WeakReference<Stream>(Machine.Current.DeviceCPU.VirtualMemoryStream);
        }

        public dynamic VirtualMemoryAddress
        {
            get { return m_VMemPosition; }

            set
            {
                m_VMemPosition = Convert.ToInt64(value);
            }
        }

        public dynamic ReadVirtualMemory (dynamic count)
        {
            Stream stream = null;

            Int32 _count = Convert.ToInt32(count);

            if (m_VMemStream.TryGetTarget(out stream))
            {
                Byte[] buffer = new Byte[_count];
                stream.Read(buffer, 0, _count);
                return buffer;
            }
            else
            {
                return null;
            }
        }

    }
}
