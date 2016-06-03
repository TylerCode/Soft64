using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64.RCP
{
    public class CartridgeHeapStream : MemorySection
    {
        private Cartridge m_Cartridge;

        public CartridgeHeapStream(Cartridge cart) : base(0x100000, 0x05000000)
        {
            m_Cartridge = cart;
        }

        protected override void Allocate(int size)
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return base.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            base.Write(buffer, offset, count);
        }

    }
}
