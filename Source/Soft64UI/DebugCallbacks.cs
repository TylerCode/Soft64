using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64UI
{
    public class DebugCallbacks
    {
        public event EventHandler ShowTools;

        public void ShowDevTools()
        {
            OnShowTools();
        }

        private void OnShowTools()
        {
            ShowTools?.Invoke(this, new EventArgs());
        }
    }
}
