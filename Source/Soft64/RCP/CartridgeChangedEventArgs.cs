using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64.RCP
{
    public sealed class CartridgeChangedEventArgs : EventArgs
    {
        private Cartridge m_NewCartridge;

        public CartridgeChangedEventArgs(Cartridge newCartridge)
        {
            m_NewCartridge = newCartridge;
        }

        public Cartridge NewCartridge
        {
            get { return m_NewCartridge; }
        }
    }
}
