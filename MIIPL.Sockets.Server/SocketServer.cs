using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using MIIPL.Common;
using System.Collections.Generic;
using MaxiSwitch.Logger;

namespace MIIPL.Sockets
{
    public class SocketServerEventArgs : EventArgs
    {
        public string ID
        {
            get;
            private set;
        }

        public SocketServerEventArgs(string id)
        {
            this.ID = id;
        }
    }
    public class SocketServerIsValidIPEventArgs : SocketServerEventArgs
    {
        public string IPAddress { get; private set; }
        public bool Cancel { get; set; }

        public SocketServerIsValidIPEventArgs(string id, string ipAddress)
            : base(id)
        {
            this.IPAddress = ipAddress;
            this.Cancel = false;
        }

        public SocketServerIsValidIPEventArgs(string id, bool cancel)
            : base(id)
        {
            this.Cancel = cancel;
        }
    }

    public delegate void SocketServerEventHandler(object sender, EventArgs e);
    public delegate void SocketServerIsValidIPEventHandler(object sender, SocketServerIsValidIPEventArgs e);

    public class SocketServer
    {
        private readonly object connectedSocketsSyncHandle = new object();
        private delegate void SendDelegate(byte[] data);
        public static int Default_HostPort = 9000;

        #region Logger
        private iLogger logger = null;
        public iLogger Logger
        {
            get { return logger; }
            set { logger = value; }
        }
        private void WriteLogEntry(string msg)
        {
            if (this.Logger != null)
            {
                this.Logger.WriteEntry(this, msg);
            }
        }
        #endregion

        #region Events
        public bool RaiseAsyncEvents { get; set; }
        #region Server Events
        public event SocketServerEventHandler Started = null;
        public event SocketServerEventHandler Stopped = null;
        public event SocketServerIsValidIPEventHandler IsValidIP = null;

        private void OnStarted()
        {
            UpdateConnTimer(this.CheckConnDelay);
            this.IsListening = true;
            if (this.Started != null)
            {
                this.Started(this, new EventArgs());
            }
        }
        private void OnStopped()
        {
            this.IsListening = false;
            if (this.Stopped != null)
            {
                this.Stopped(this, new EventArgs());
            }
        }
        private bool OnIsValidIP(string pID, string ipAddress)
        {
            bool flag = false;
            if (this.IsValidIP != null)
            {
                SocketServerIsValidIPEventArgs e = new SocketServerIsValidIPEventArgs(pID, ipAddress);
                IsValidIP(this, e);
                flag = e.Cancel;
            }
            return flag;
        }
        #endregion
        #region Client Events
        public event SocketClientEventHandler Connected = null;
        public event SocketClientEventHandler Disconnected = null;
        public event SocketClientDataReceivedEventHandler DataReceived = null;
        public event SocketClientEventHandler SendComplete = null;
        public event SocketClientDataSendFailedEventHandler SendFailed = null;
        public event SocketClientEventHandler SendHandShakeMsg = null;
        public event SocketClientEventHandler UnableToConnect = null;
        #endregion
        #endregion
        

        #region Member Variables
        private string hostIP = string.Empty;
        private int hostPort = Default_HostPort;
        private List<SocketClient> clientList = null;
        private Socket listener = null;
        private SocketClientEventHandler client_disconnected = null;
        #endregion

        #region Public Properties
        public string HostIP
        {
            get { return hostIP; }
            set { hostIP = value; }
        }
        public int HostPort
        {
            get { return hostPort; }
            set { hostPort = value; }
        }
        public List<SocketClient> ClientList
        {
            get
            {
                if (clientList == null)
                    clientList = new List<SocketClient>();
                return clientList;
            }
            private set
            {
                clientList = value;
            }
        }
        public Socket Listener
        {
            get { return listener; }
            set { listener = value; }
        }
        public int CheckConnDelay { get; set; }
        public int MaxWaitingDelay { get; set; }
        public bool IsListening
        {
            get;
            private set;
        }
        public int InitConnDelay { get; set; }
        private bool IsDisposed { get; set; }
        #endregion

        // Thread signal.
        public ManualResetEvent allDone = new ManualResetEvent(false);

        #region Constructor
        public SocketServer()
        {
            this.IsListening = false;
            RaiseAsyncEvents = true;
            CheckConnDelay = 60;
            //CheckConnIntervel = 60 * 5;
            MaxWaitingDelay = 60;
            InitConnDelay = 5;
            //disconnectOnNoResponse = true;
            IsDisposed = false;
            this.client_disconnected = new SocketClientEventHandler(state_Disconnected);
        }
        #endregion

        #region Public Methods

        bool RestartListening = true;
        public void StartListening()
        {
            RestartListening = true;
            InternalStartListening();
        }

        private void InternalStartListening()
        {
            try
            {
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                if (ipHostInfo.AddressList.Length > 1 && !string.IsNullOrEmpty(this.HostIP))
                {
                    foreach (IPAddress tipAddress in ipHostInfo.AddressList)
                    {
                        if (tipAddress.ToString() == this.HostIP)
                        {
                            ipAddress = tipAddress;
                            break;
                        }
                    }
                }

                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, this.HostPort);
                this.Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.Listener.Bind(localEndPoint);
                this.Listener.Listen(100);
                OnStarted();
                //UpdateConnTimer(this.InitConnDelay);
                HsmLogger.WriteTransLog(null, "Waiting for a connection...");
                IsDisposed = false;
                StartAccept(this.Listener);
            }
            catch (Exception e)
            {
                HsmLogger.WriteTransLog(null, string.Format("Exception occured in StartListening : {0}", e.Message));
            }
        }
        private void UpdateConnTimer(int connDelay)
        {
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            TimerCallback tcb = CheckListening;
            if (ConnTimer == null)
            {
                //ConnTimer = new System.Threading.Timer(tcb, this.Listener, connDelay * 1000, connDelay * 1000);
                if (connDelay == -1)
                    ConnTimer = new System.Threading.Timer(tcb, this.Listener, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                else
                    ConnTimer = new System.Threading.Timer(tcb, this.Listener, connDelay * 1000, connDelay * 1000);
            }

            else
            {
                //ConnTimer.Change(connDelay * 1000, connDelay * 1000);
                if (connDelay == -1)
                    ConnTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                else
                    ConnTimer.Change(connDelay * 1000, connDelay * 1000);
            }

        }
        public void StopListening()
        {
            RestartListening = false;
            InternalStopListening();
        }
        private void StartAccept(Socket listener)
        {
            if (RestartListening)
                // Start an asynchronous socket to listen for connections.
                listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
        }
        public void InternalStopListening()
        {
            if (this.Listener != null)
            {
                IsDisposed = true;
                this.Listener.Shutdown();
                this.Listener = null;
                CloseAllConnections();

                if (ConnTimer != null)
                {
                    ConnTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                    ConnTimer.Dispose();
                    ConnTimer = null;
                }
                OnStopped();
            }
        }
        public void Send(string pID, byte[] data)
        {
            SocketClient state = null;
            lock (connectedSocketsSyncHandle)
            {
                state = ClientList.Find(m => m.ID == pID);
            }
            if (state != null)
            {
                SendDelegate sd = new SendDelegate(state.Send);
                sd.BeginInvoke(data, null, null);
            }
        }
        public void SendWithTimeOut(string pID, byte[] data, int timeout)
        {
            SocketClient state = null;
            lock (connectedSocketsSyncHandle)
            {
                state = ClientList.Find(m => m.ID == pID);
            }
            if (state != null)
            {
                state.MaxWaitingDelay = timeout;
                SendDelegate sd = new SendDelegate(state.SendWithTimeOut);
                sd.BeginInvoke(data, null, null);
            }
        }
        #endregion

        #region Private Methods
        private void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();
            bool flag = false;
            Socket listener = (Socket)ar.AsyncState;
            if (IsDisposed) return;
            try
            {
                // Get the socket that handles the client request.
                Socket handler = listener.EndAccept(ar);

                //// New Method
                string pID = handler.GetRemoteID();

                if (!OnIsValidIP(pID, handler.RemoteIP()))
                {
                    CreateClientConnection(pID, handler);
                }
                else
                {
                    HsmLogger.WriteTransLog(null, "Connection Request From Invalid Terminal: " + pID + " Cannot be Processed");
                }
                flag = true;

            }
            catch (ObjectDisposedException ex)
            {
                HsmLogger.WriteTransLog(null, "Error occured in AcceptCallback : Socket disposed");
            }
            catch (Exception ex)
            {
                flag = true;
                HsmLogger.WriteTransLog(null, string.Format("Error occured in AcceptCallback : {0}", ex.Message));
            }
            finally
            {
                if (flag)
                    StartAccept(listener);
            }
        }
        private void CreateClientConnection(string IPAddress, Socket workSocket)
        {
            try
            {
                SocketClient state = null;
                lock (connectedSocketsSyncHandle)
                {
                    RemoveClient(IPAddress);
                    state = new SocketClient(SocketClientLocationEnum.ServerSide) { Client = workSocket };
                    ClientList.Add(state);
                }
                if (state != null)
                {
                    state.MaxWaitingDelay = this.MaxWaitingDelay;
                    state.CheckConnDelay = this.CheckConnDelay;
                    state.InitConnDelay = this.InitConnDelay;
                    //Raise Connected Event

                    HsmLogger.WriteTransLog(null, string.Format("Client # {0} ({1})connected", state.TerminalName, state.Location));

                    WireEvents(state);
                    state.StartClient();
                }
            }
            catch (Exception ex)
            {
                HsmLogger.WriteTransLog(null, string.Format("Exception occured in NewConnection : {0}", ex.Message));
                RemoveClient(IPAddress);
            }
        }
        private bool RemoveClient(string pID)
        {
            try
            {
                //lock (connectedSocketsSyncHandle)
                {
                    SocketClient state = ClientList.Find(m => m.ID == pID);
                    if (state != null)
                    {
                        UnwireEvents(state);
                        state.StopClient();
                        lock (connectedSocketsSyncHandle)
                        {
                            HsmLogger.WriteTransLog(null, string.Format("Existing Client {0} ({1}), disconnected", pID, state.Location));
                            ClientList.Remove(state);
                        }
                        //state.Dispose();
                        state = null;
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                HsmLogger.WriteTransLog(null, string.Format("Exception occured in RemoveClient : {0}", ex.Message));
                return false;
            }
        }
        private void WireEvents(SocketClient state)
        {
            state.CheckConnDelay = this.CheckConnDelay;
            //state.CheckConnIntervel = this.CheckConnIntervel;
            state.MaxWaitingDelay = this.MaxWaitingDelay;
            state.Logger = this.Logger;

            if (this.Connected != null) state.Connected += this.Connected;
            if (this.client_disconnected != null) state.Disconnected += this.client_disconnected;
            if (this.Disconnected != null) state.Disconnected += this.Disconnected;
            if (this.DataReceived != null) state.DataReceived += this.DataReceived;
            if (this.SendComplete != null) state.SendComplete += this.SendComplete;
            if (this.SendFailed != null) state.SendFailed += this.SendFailed;
            if (this.SendHandShakeMsg != null) state.SendHandShakeMsg += this.SendHandShakeMsg;
            if (this.UnableToConnect != null) state.UnableToConnect += this.UnableToConnect;
        }

        void state_Disconnected(object sender, SocketClientEventArgs e)
        {
            RemoveClient(e.ID);
        }
        private void UnwireEvents(SocketClient state)
        {
            if (this.Connected != null) state.Connected -= this.Connected;
            if (this.client_disconnected != null) state.Disconnected -= this.client_disconnected;
            if (this.Disconnected != null) state.Disconnected -= this.Disconnected;
            if (this.DataReceived != null) state.DataReceived -= this.DataReceived;
            if (this.SendComplete != null) state.SendComplete -= this.SendComplete;
            if (this.SendFailed != null) state.SendFailed -= this.SendFailed;
            if (this.SendHandShakeMsg != null) state.SendHandShakeMsg -= this.SendHandShakeMsg;
            if (this.UnableToConnect != null) state.UnableToConnect -= this.UnableToConnect;
        }
        private void CloseAllConnections()
        {
            try
            {
                int i = 0;
                while (i < this.ClientList.Count)
                {
                    if (!RemoveClient(this.ClientList[i].ID))
                        i++;
                }
                //foreach (SocketClient client in this.ClientList)
                //{
                //    RemoveClient(client.IP);
                //}
            }
            catch (Exception ex)
            {
                HsmLogger.WriteTransLog(null, string.Format("Exception occured in CloseAllConnections : {0}", ex.Message));
                throw ex;
            }
        }
        #endregion

        #region Check the Terminal Connectivity in regular intervals
        System.Threading.Timer ConnTimer = null;
        public void CheckListening(object connInfo)
        {
            try
            {
               // WriteLogEntry("Checking Listining Mode ...");//: " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss:fff"));
                Socket sock = (Socket)connInfo;
                if (sock == null || !this.IsListening)
                {
                    HsmLogger.WriteTransLog(null, "Server is not listening.");
                    HsmLogger.WriteTransLog(null, "Restarting Server");
                    InternalStartListening();
                }
                else
                {
                    HsmLogger.WriteTransLog(null, "Server is listening.");
                }
            }
            catch (Exception ex)
            {
                HsmLogger.WriteTransLog(null, "Error occured in 'CheckListening'");
                HsmLogger.WriteTransLog(null, "Error : " + ex.Message);
                HsmLogger.WriteTransLog(null, "StackTrace : " + ex.StackTrace);
            }
        }
        #endregion
    }
}
