using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp.WinForms;
using CefSharp;
using Soft64;
using System.IO;
using NLog;
using NLog.Targets;

namespace Soft64UI
{
    public partial class ChromeHostForm : Form
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private BootBreakMode m_BreakOnBootMode;
        private ChromiumWebBrowser m_HostBrowser;
        private static ChromeHostForm s_Current;

        public enum BootBreakMode
        {
            None,
            Pre,
            Post
        }

        public ChromeHostForm()
        {
            s_Current = this;
            InitializeComponent();

            MethodCallTarget target = new MethodCallTarget();
            target.ClassName = this.GetType().AssemblyQualifiedName;
            target.MethodName = "OnLogMessage";
            target.Parameters.Add(new MethodCallParameter("${level}"));
            target.Parameters.Add(new MethodCallParameter("${message}"));
            NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Trace);

        }

        protected override void OnLoad(EventArgs e)
        {
            /* Initalize the CEF system */
            CefSettings settings = new CefSettings();
            settings.WindowlessRenderingEnabled = true;
            settings.CefCommandLineArgs.Add(new KeyValuePair<String, String>("process-per-site", "1"));
            Cef.Initialize(settings, true, false);

            /* Create an instance of the Chrome browser host */
            m_HostBrowser = new ChromiumWebBrowser($"{Environment.CurrentDirectory}/HTMLUI/Start.html");
            Controls.Add(m_HostBrowser);

            /* Register JS Objects */
            m_HostBrowser.RegisterJsObject("mainWindow", this);

            Machine.Current.DeviceRCP.Interface_Parallel.CartridgeChanged += Interface_Parallel_CartridgeChanged;
        }

        private void Interface_Parallel_CartridgeChanged(object sender, Soft64.RCP.CartridgeChangedEventArgs e)
        {
            m_HostBrowser.ExecuteScriptAsync(
            $@"$('#cartrigeInfo').html(' \
            Name: {e.NewCartridge?.RomImage.Name} \
            <br /> \
            ID: {e.NewCartridge?.RomImage.Serial.Serial } \
            <br /> \
            CRC1: {e.NewCartridge?.RomImage.CRC1:X4} \
            <br /> \
            CRC2: {e.NewCartridge?.RomImage.CRC2:X4} \
            <br /> \
            Detected CIC: {e.NewCartridge?.RomImage.BootRomInformation.CIC.ToString()} \
            <br /> \
            Region: {e.NewCartridge?.RomImage.Region.ToString()} \
            <br /> \
            ');");
        }

        public static void OnLogMessage(String level, String message)
        {
            s_Current?.LogMessage(level, message);
        }

        private void LogMessage(String level, String message)
        {
            /* TODO: Expose message to Javascript */
        }

        protected override void OnClosed(EventArgs e)
        {
            Cef.Shutdown();
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

        public void InsertRomFile(String stringBuffer)
        {
            Byte[] buffer = Convert.FromBase64String(stringBuffer);
            VirtualCartridge cart = new VirtualCartridge(new MemoryStream(buffer));
            Machine.Current.CartridgeInsert(cart);
        }

        public void ShowDevTools()
        {
            m_HostBrowser.ShowDevTools();
        }
    }
}
