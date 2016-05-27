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
        /// Start up the emulator core on the current thread calling this method
        /// </summary>
        void Run();

        /// <summary>
        /// Start up the emulator core in an asynchronous matter.
        /// </summary>
        void RunAsync();
    }

    public interface ISoft64Result
    {

    }

    public interface ISoft64Error
    {
        String Message { get; }
        Int32 Code { get; }

    }
}
