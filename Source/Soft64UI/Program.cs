using CefSharp;
using NLog;
using NLog.Config;
using NLog.Targets;
using Soft64;
using Soft64.MipsR4300;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Soft64UI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Machine machine = new Machine();

            ConfigureLogging();

            Cef.Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            Cef.Shutdown();
        }

        private static void ConfigureLogging()
        {
            var config = new LoggingConfiguration();

            FileTarget cpuTraceLogTarget = new FileTarget();
            cpuTraceLogTarget.Layout = "${message}";
            cpuTraceLogTarget.FileName = "${basedir}/logs/cpuTrace.txt";
            cpuTraceLogTarget.KeepFileOpen = false;
            cpuTraceLogTarget.DeleteOldFileOnStartup = true;
            cpuTraceLogTarget.Encoding = Encoding.ASCII;

            MethodCallTarget target = new MethodCallTarget();
            target.ClassName = typeof(MainForm).AssemblyQualifiedName;
            target.MethodName = "OnLogMessage";
            target.Parameters.Add(new MethodCallParameter("${logger}"));
            target.Parameters.Add(new MethodCallParameter("${level}"));
            target.Parameters.Add(new MethodCallParameter("${message}"));

            var cpuLogRule = new LoggingRule(typeof(Interpreter).AssemblyQualifiedName, LogLevel.Debug, cpuTraceLogTarget);
            cpuLogRule.Final = true;

            var emuLogRule = new LoggingRule("*", LogLevel.Trace, target);


            config.LoggingRules.Add(cpuLogRule);
            config.LoggingRules.Add(emuLogRule);


            LogManager.Configuration = config;
        }
    }
}
