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
    [System.ComponentModel.DesignerCategory("Code")]
    public class CEFWindowForm : Form
    {
        private ChromiumWebBrowser m_HostBrowser;
        private String m_TargetUrl;

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
