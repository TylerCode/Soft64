using CefSharp;
using CefSharp.WinForms;
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
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(883, 642);
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
            JSCallback _callback = (args) => callback?.ExecuteAsync(args);
            eventName = eventName.ToLower();

            if (!m_JSCallbacks.ContainsKey(eventName))
                m_JSCallbacks.Add(eventName, _callback);
            else
            {
                m_JSCallbacks[eventName] = (JSCallback)Delegate.Combine(m_JSCallbacks[eventName], _callback);
            }
        }

        protected void ExecuteJSCallback(String eventName, params Object[] arguments)
        {
            eventName = eventName.ToLower();
            if (m_JSCallbacks.ContainsKey(eventName))
                m_JSCallbacks[eventName]?.Invoke(arguments);
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

        public static String MakeLocalUrl(String pageName)
        {
            return $"{Environment.CurrentDirectory}\\HTMLUI\\{pageName}.html";
        }
    }
}
