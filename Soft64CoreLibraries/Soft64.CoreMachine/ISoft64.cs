using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64.CoreMachine
{
    /// <summary>
    /// Main interface a for an instance of a Soft64 emulator core
    /// </summary>
    /// <remarks>
    /// This interface provides the main API for the Soft64 emulator core.
    /// Emulator configuration and controls, and state are exposed through this
    /// type of interface.
    /// </remarks>
    public interface ISoft64
    {
        /// <summary>
        /// Start up the emulator core and begin emulator execution
        /// </summary>
        void Start();
    }
}
