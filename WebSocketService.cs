using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;
using SuperWebSocket;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace CloudDemo
{
    partial class WebSocketService : ServiceBase
    {
        private static List<string> tasks = new List<string>();
        private IBootstrap m_Bootstrap;
        //private BackgroundWorker receiveWorker;

        public WebSocketService()
        {
            InitializeComponent();
            this.m_Bootstrap = BootstrapFactory.CreateBootstrap();
        }
        protected override void OnStart(string[] args)
        {
            if (this.m_Bootstrap.Initialize())
            {

                this.m_Bootstrap.Start();
                foreach (IWorkItem server in m_Bootstrap.AppServers)
                {
                    //装载事件
                    WebSocketLoader.Setup(server);
                    if (server.State == ServerState.Running)
                    {
                        LogHelper.Info(string.Format("{0} has been started", server.Name));
                    }
                    else
                    {
                        LogHelper.Info(string.Format("{0} failed to start", server.Name));
                    }
                }
            }
        }

        protected override void OnStop()
        {
            foreach (IWorkItem server in m_Bootstrap.AppServers)
            {
                server.Stop();
            }
            this.m_Bootstrap.Stop();
        }
        protected override void OnShutdown()
        {
            foreach (IWorkItem server in m_Bootstrap.AppServers)
            {
                server.Stop();
            }
            this.m_Bootstrap.Stop();
        }
    }
}
