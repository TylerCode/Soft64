using CefSharp;
using NLog;
using NLog.Targets;
using Soft64;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64UI
{
    public sealed class MainForm : CEFWindowForm
    {
        private BootBreakMode m_BreakOnBootMode;
        private static MainForm s_Current;

        public enum BootBreakMode
        {
            None,
            Pre,
            Post
        }

        public MainForm() : base()
        {
            s_Current = this;

            MethodCallTarget target = new MethodCallTarget();
            target.ClassName = this.GetType().AssemblyQualifiedName;
            target.MethodName = "OnLogMessage";
            target.Parameters.Add(new MethodCallParameter("${level}"));
            target.Parameters.Add(new MethodCallParameter("${message}"));
            NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Trace);
        }

        protected override void InitializeComponent()
        {
            this.Text = "Soft64 Emulator UI 1.0";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(883, 542);
            TargetUrl = MakeLocalUrl("Main");

            base.InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Machine.Current.DeviceRCP.Interface_Parallel.CartridgeChanged += Interface_Parallel_CartridgeChanged;
        }

        private void Interface_Parallel_CartridgeChanged(object sender, Soft64.RCP.CartridgeChangedEventArgs e)
        {
            HostBrowser.ExecuteScriptAsync(
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
    }
}
