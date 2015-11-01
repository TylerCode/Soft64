using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;

namespace Soft64UI
{
    public delegate void JSCallback(Object[] arguments);

    [System.ComponentModel.DesignerCategory("Code")]
    public class CEFWindowForm : Form
    {
        private ChromiumWebBrowser m_HostBrowser;
        private String m_TargetUrl;
        private Dictionary<String, JSCallback> m_JSCallbacks = new Dictionary<string, JSCallback>();

        public CEFWindowForm(String url)
        {
            m_TargetUrl = url;
            InitializeComponent();
        }

        public CEFWindowForm()
        {
            InitializeComponent();
        }

        protected virtual void InitializeComponent()
        {
        }

        protected virtual void RegisterJSObjects()
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            m_HostBrowser = new ChromiumWebBrowser(m_TargetUrl);
            Controls.Add(m_HostBrowser);

            m_HostBrowser.RegisterJsObject("currentForm", this);
            RegisterJSObjects();

            base.OnLoad(e);
        }

        public void On(String eventName, IJavascriptCallback callback)
        {
            if (!m_JSCallbacks.ContainsKey(eventName))
                m_JSCallbacks.Add(eventName.ToLower(), (args) => callback?.ExecuteAsync(args));
        }

        protected void ExecuteJSCallback(String eventName, params Object[] arguments)
        {
            m_JSCallbacks[eventName.ToLower()]?.Invoke(arguments);
        }

        protected void RemoveJSCallback(String eventName)
        {
            m_JSCallbacks.Remove(eventName);
        }

        public void ShowDevTools()
        {
            m_HostBrowser?.ShowDevTools();
        }

        public String TargetUrl
        {
            get { return m_TargetUrl; }
            set { m_TargetUrl = value; }
        }

        public ChromiumWebBrowser HostBrowser => m_HostBrowser;

        protected static String MakeLocalUrl(String pageName)
        {
            return $"{Environment.CurrentDirectory}\\HTMLUI\\{pageName}.html";
        }
    }
}
