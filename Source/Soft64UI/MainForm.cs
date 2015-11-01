using CefSharp;
using NLog;
using NLog.Config;
using NLog.Targets;
using Soft64;
using Soft64.MipsR4300;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

using JSCallback = System.Action<System.Object[]>;

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
            ExecuteJSCallback("cartridge", e.NewCartridge?.RomImage.Name,
                e.NewCartridge?.RomImage.Serial.Serial,
                e.NewCartridge?.RomImage.CRC1.ToString("X4"),
                e.NewCartridge?.RomImage.CRC2.ToString("X4"),
                e.NewCartridge?.RomImage.BootRomInformation.CIC.ToString(),
                e.NewCartridge?.RomImage.Region.ToString());
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

        public void InsertRomFile(String stringBuffer)
        {
            Byte[] buffer = Convert.FromBase64String(stringBuffer);
            VirtualCartridge cart = new VirtualCartridge(new MemoryStream(buffer));
            Machine.Current.CartridgeInsert(cart);
        }
    }
}
