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

namespace Soft64UI
{
    public partial class ChromeHostForm : Form
    {
        private ChromiumWebBrowser m_HostBrowser;
        private DebugCallbacks m_DebugCallbacks = new DebugCallbacks();

        public ChromeHostForm()
        {
            InitializeComponent();

            m_DebugCallbacks.ShowTools += M_DebugCallbacks_ShowTools;
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
            m_HostBrowser.LoadingStateChanged += M_HostBrowser_LoadingStateChanged;

            /* Register JS Objects */
            m_HostBrowser.RegisterJsObject("debugCallbacks", m_DebugCallbacks);
        }

        private void M_DebugCallbacks_ShowTools(object sender, EventArgs e)
        {
            m_HostBrowser.ShowDevTools();
        }

        private void M_HostBrowser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
        }
    }
}
