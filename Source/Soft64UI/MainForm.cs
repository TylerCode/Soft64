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
using System.Web.UI;

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
            target.Parameters.Add(new MethodCallParameter("${logger}"));
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
            StringBuilder htmlBuilder = new StringBuilder();
            HtmlTextWriter writer = new HtmlTextWriter(new StringWriter(htmlBuilder));
            writer.WriteEncodedText($"Name: {e.NewCartridge?.RomImage.Name}");
            writer.WriteBreak();
            writer.WriteEncodedText($"ID: {e.NewCartridge?.RomImage.Serial.Serial }");
            writer.WriteBreak();
            writer.WriteEncodedText($"CRC1: {e.NewCartridge?.RomImage.CRC1:X4} ");
            writer.WriteBreak();
            writer.WriteEncodedText($"CRC2: {e.NewCartridge?.RomImage.CRC2:X4}");
            writer.WriteBreak();
            writer.WriteEncodedText($"Detected CIC: {e.NewCartridge?.RomImage.BootRomInformation.CIC.ToString()}");
            writer.WriteBreak();
            writer.WriteEncodedText($"Region: {e.NewCartridge?.RomImage.Region.ToString()}");
            writer.Flush();

            HostBrowser.ExecuteScriptAsync($" $('#cartrigeInfo').html('{htmlBuilder.ToString()}'); ");
        }

        public static void OnLogMessage(String logger, String level, String message)
        {
            s_Current?.LogMessage(logger, level, message);
        }

        private void LogMessage(String logger, String level, String message)
        {
            logger = logger.Substring(logger.LastIndexOf('.') + 1);
            String spanClass = "logmessage";

            switch (level.ToLower())
            {
                default:
                case "trace":
                case "info": break;
                case "fatal": spanClass = "logmessage_fatal"; break;
                case "error": spanClass = "logmessage_error"; break;
                case "warning": spanClass = "logmessage_warning"; break;
                case "debug": spanClass = "logmessage_debug"; break;
            }


            StringBuilder htmlBuilder = new StringBuilder();
            HtmlTextWriter writer = new HtmlTextWriter(new StringWriter(htmlBuilder));
            writer.WriteBeginTag("span");
            writer.WriteAttribute("class", spanClass);
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteEncodedText($"{logger}: {message}");
            writer.WriteEndTag("span");
            writer.WriteBreak();
            writer.Flush();


            HostBrowser.ExecuteScriptAsync($" $('#emulog').append('{htmlBuilder.ToString()}'); ");
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
