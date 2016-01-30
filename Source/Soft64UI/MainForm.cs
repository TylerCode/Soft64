using CefSharp;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using NLog;
using NLog.Config;
using NLog.Targets;
using Soft64;
using Soft64.MipsR4300;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;

namespace Soft64UI
{
    public sealed class MainForm : CEFWindowForm
    {
        private BootBreakMode m_BreakOnBootMode;
        private static MainForm s_Current;
        private ScriptEngine m_PyEngine;

        public enum BootBreakMode
        {
            None,
            Pre,
            Post
        }

        public MainForm() : base()
        {
            s_Current = this;
            m_PyEngine = Python.CreateEngine();
            m_PyEngine.Runtime.LoadAssembly(typeof(Machine).Assembly);
            m_PyEngine.Runtime.LoadAssembly(typeof(Console).Assembly);
            m_PyEngine.Runtime.LoadAssembly(typeof(MessageBox).Assembly);
            m_PyEngine.Runtime.LoadAssembly(typeof(NLog.Logger).Assembly);
        }

        protected override void InitializeComponent()
        {
            this.Text = "Soft64 Emulator UI 1.0";
            TargetUrl = MakeLocalUrl("Main");

            base.InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            /* Uncomment to show the developer tools to debug scripts */
            //HostBrowser.FrameLoadStart += HostBrowser_FrameLoadStart;
        }

        private void HostBrowser_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            ShowDevTools();
        }

        protected override void RegisterJSObjects()
        {
            HostBrowser.RegisterJsObject("n64Memory", new N64MemoryScriptObject());

            base.RegisterJSObjects();
        }

        public void TestBytes(dynamic a)
        {
            Console.ResetColor();
        }

        public static void OnLogMessage(String logger, String level, String message)
        {
            s_Current?.LogMessage(logger, level, message);
        }

        private void LogMessage(String logger, String level, String message)
        {
            ExecuteJSCallback("emulog", logger.Substring(logger.LastIndexOf('.') + 1), level, message);
        }

        public void RunEmu()
        {
            if (m_BreakOnBootMode == BootBreakMode.Post)
            {
                Machine.Current.MachineEventNotification += (o, e) =>
                {
                    if (e.EventType == MachineEventType.Booted)
                        Machine.Current.Pause();
                };
            }

            Machine.Current.Run();
        }

        public void ExecutePython(String script)
        {
            try
            {
                m_PyEngine.Execute(script);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Python Runtime Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void InsertRomFile(String stringBuffer)
        {
            Byte[] buffer = Convert.FromBase64String(stringBuffer);
            VirtualCartridge cart = new VirtualCartridge(new MemoryStream(buffer));
            Machine.Current.CartridgeInsert(cart);
        }

        public String LoadHtml(String url)
        {
            String fullpath = Environment.CurrentDirectory + "\\HTMLUI\\" + url;

            var reader = File.OpenText(fullpath);
            return reader.ReadToEnd();
        }

        public void SetDebugStart()
        {
            m_BreakOnBootMode = BootBreakMode.Post;
        }
    }
}
