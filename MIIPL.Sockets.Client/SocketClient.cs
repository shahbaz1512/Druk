using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using MIIPL.Common;
using System.Threading;
using System.Configuration;
using MaxiSwitch.Logger;

namespace MIIPL.Sockets
{
    public class SocketClientEventArgs : EventArgs
    {
        public string ID
        {
            get;
            private set;
        }
        public SocketClientEventArgs(string id)
        {
            this.ID = id;
        }
    }
    public class SocketClientDataReceivedEventArgs : SocketClientEventArgs
    {
        public byte[] Message
        {
            get;
            private set;
        }
        public SocketClientDataReceivedEventArgs(string id, byte[] msg)
            : base(id)
        {
            this.Message = msg;
        }
    }
    public class SocketClientDataSendFailedEventArgs : SocketClientEventArgs
    {
        public byte[] Message
        {
            get;
            private set;
        }
        public SocketClientDataSendFailedEventArgs(string id, byte[] msg)
            : base(id)
        {
            this.Message = msg;
        }
    }
    public delegate void SocketClientEventHandler(object sender, SocketClientEventArgs e);
    public delegate void SocketClientDataReceivedEventHandler(object sender, SocketClientDataReceivedEventArgs e);
    public delegate void SocketClientDataSendFailedEventHandler(object sender, SocketClientDataSendFailedEventArgs e);

    public class ClientObject
    {
        // Client socket.
        private Socket workSocket = null;

        public Socket Socket
        {
            get { return workSocket; }
            set { workSocket = value; }
        }
        // Size of receive buffer.
        public const int BufferSize = 256;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();

        public string IP
        {
            get
            {
                try
                {
                    return workSocket.GetRemoteID();
                }
                catch //(Exception ex)
                {
                    return string.Empty;
                }
            }
        }

        public string TerminalName
        {
            get
            {
                if (!String.IsNullOrEmpty(this.IP))
                    return "Terminal : " + IP;
                return string.Empty;
            }
        }
    }

    public enum SocketClientLocationEnum : int
    {
        ServerSide = 0,
        ClientSide = 1,
    }


    public interface iSocketClientExtender
    {
        SocketClient Client { get; set; }
        void OnConnected(object sender, SocketClientEventArgs e);
        void OnDisconnected(object sender, SocketClientEventArgs e);
        void OnDataReceived(object sender, SocketClientDataReceivedEventArgs e);
        void OnSendComplete(object sender, SocketClientEventArgs e);
        void OnSendFailed(object sender, SocketClientDataSendFailedEventArgs e);
        void OnSendHandShakeMsg(object sender, SocketClientEventArgs e);
        void OnUnableToConnect(object sender, SocketClientEventArgs e);
    }
    public class SocketClient //: IDisposable
    {
        #region Constuctor
        public SocketClient()
            : this(SocketClientLocationEnum.ClientSide)
        {
        }
        public SocketClient(SocketClientLocationEnum pLocation)
        {
            RaiseAsyncEvents = false;
            CheckConnDelay = 60;
            //CheckConnIntervel = 60 * 5;
            MaxWaitingDelay = 10;
            Location = pLocation;
            InitConnDelay = 60;
            IdleTime = TimeSpan.FromSeconds(0);
            RetryToConnect = true;
            IDType = IDTypeEnum.Remote;
        }
        #endregion

        #region Member Variables
        private SocketClientLocationEnum location = SocketClientLocationEnum.ClientSide;
        //private List<string> pendingItems = null;
        private Socket client;						// Server connection
        private byte[] m_byBuff = new byte[8192];	// Recieved data buffer
        private string remoteHostIP = "";
        private int remoteHostPort = 0;
        #endregion

        #region SocketClientExtender
        private iSocketClientExtender socketClientExtender = null;
        public iSocketClientExtender SocketClientExtender
        {
            get { return socketClientExtender; }
            set
            {
                socketClientExtender = value;
                if (socketClientExtender != null)
                    socketClientExtender.Client = this;
            }
        }
        void SocketClientExtender_OnConnected(SocketClientEventArgs e)
        {
            try
            {
                if (this.SocketClientExtender != null)
                {
                    SocketClientEventHandler d = new SocketClientEventHandler(this.SocketClientExtender.OnConnected);
                    d.BeginInvoke(this, e, null, null);
                }
            }
            catch (Exception ex)
            {
                this.WriteLogEntry("Error occured in SocketClientExtender_OnConnected : " + ex.Message);
            }
        }
        void SocketClientExtender_OnDisconnected(SocketClientEventArgs e)
        {
            try
            {
                if (this.SocketClientExtender != null)
                {
                    SocketClientEventHandler d = new SocketClientEventHandler(this.SocketClientExtender.OnDisconnected);
                    d.BeginInvoke(this, e, null, null);
                }
            }
            catch (Exception ex)
            {
                this.WriteLogEntry("Error occured in SocketClientExtender_OnDisconnected : " + ex.Message);
            }
        }
        void SocketClientExtender_OnDataReceived(SocketClientDataReceivedEventArgs e)
        {
            try
            {
                if (this.SocketClientExtender != null)
                {
                    SocketClientDataReceivedEventHandler d = new SocketClientDataReceivedEventHandler(this.SocketClientExtender.OnDataReceived);
                    d.BeginInvoke(this, e, null, null);
                }
            }
            catch (Exception ex)
            {
                this.WriteLogEntry("Error occured in SocketClientExtender_OnDataReceived : " + ex.Message);
            }
        }
        void SocketClientExtender_OnSendComplete(SocketClientEventArgs e)
        {
            try
            {
                if (this.SocketClientExtender != null)
                {
                    SocketClientEventHandler d = new SocketClientEventHandler(this.SocketClientExtender.OnSendComplete);
                    d.BeginInvoke(this, e, null, null);
                }
            }
            catch (Exception ex)
            {
                this.WriteLogEntry("Error occured in SocketClientExtender_OnSendComplete : " + ex.Message);
            }
        }
        void SocketClientExtender_OnSendFailed(SocketClientDataSendFailedEventArgs e)
        {
            try
            {
                if (this.SocketClientExtender != null)
                {
                    SocketClientDataSendFailedEventHandler d = new SocketClientDataSendFailedEventHandler(this.SocketClientExtender.OnSendFailed);
                    d.BeginInvoke(this, e, null, null);
                }
            }
            catch (Exception ex)
            {
                this.WriteLogEntry("Error occured in SocketClientExtender_OnSendFailed : " + ex.Message);
            }
        }
        void SocketClientExtender_OnSendHandShakeMsg(SocketClientEventArgs e)
        {
            try
            {
                if (this.SocketClientExtender != null)
                {
                    SocketClientEventHandler d = new SocketClientEventHandler(this.SocketClientExtender.OnSendHandShakeMsg);
                    d.BeginInvoke(this, e, null, null);
                }
            }
            catch (Exception ex)
            {
                this.WriteLogEntry("Error occured in SocketClientExtender_OnSendHandShakeMsg : " + ex.Message);
            }
        }
        void SocketClientExtender_OnUnableToConnect(SocketClientEventArgs e)
        {
            try
            {
                if (this.SocketClientExtender != null)
                {
                    SocketClientEventHandler d = new SocketClientEventHandler(this.SocketClientExtender.OnUnableToConnect);
                    d.BeginInvoke(this, e, null, null);
                }
            }
            catch (Exception ex)
            {
                this.WriteLogEntry("Error occured in SocketClientExtender_OnSendHandShakeMsg : " + ex.Message);
            }
        }
        #endregion

        #region Events
        public bool RaiseAsyncEvents { get; set; }
        public event SocketClientEventHandler Connected = null;
        public event SocketClientEventHandler Disconnected = null;
        public event SocketClientDataReceivedEventHandler DataReceived = null;
        public event SocketClientEventHandler SendComplete = null;
        public event SocketClientDataSendFailedEventHandler SendFailed = null;
        public event SocketClientEventHandler SendHandShakeMsg = null;
        public event SocketClientEventHandler UnableToConnect = null;
        private void OnConnected(string pID)
        {
            IsRunning = true;
            UpdateConnTimer(this.CheckConnDelay);
            if (this.Connected != null)
            {
                SocketClientExtender_OnConnected(new SocketClientEventArgs(pID));
                if (this.RaiseAsyncEvents)
                {
                    foreach (SocketClientEventHandler action in this.Connected.GetInvocationList())
                        action.BeginInvoke(this, new SocketClientEventArgs(pID), null, null);
                }
                else
                    this.Connected(this, new SocketClientEventArgs(pID));
            }
        }
        private void OnDisconnected(string pID)
        {
            WriteLogEntry("Disconnected..");
            IsRunning = false;
            UpdateConnTimer(this.InitConnDelay);
            if (this.Disconnected != null)
            {
                SocketClientExtender_OnDisconnected(new SocketClientEventArgs(pID));
                if (this.RaiseAsyncEvents)
                {
                    foreach (SocketClientEventHandler action in this.Disconnected.GetInvocationList())
                        action.BeginInvoke(this, new SocketClientEventArgs(pID), null, null);
                }
                else
                    this.Disconnected(this, new SocketClientEventArgs(pID));
            }
        }
        private void OnDataReceived(string pID, byte[] pMsg)
        {
            //mutex.WaitOne();
            WaitingForReplay = false;
            //mutex.ReleaseMutex();
            if (this.DataReceived != null)
            {
                SocketClientExtender_OnDataReceived(new SocketClientDataReceivedEventArgs(pID, pMsg));
                if (this.RaiseAsyncEvents)
                {
                    foreach (SocketClientDataReceivedEventHandler action in this.DataReceived.GetInvocationList())
                        action.BeginInvoke(this, new SocketClientDataReceivedEventArgs(pID, pMsg), null, null);
                }
                else
                    this.DataReceived(this, new SocketClientDataReceivedEventArgs(pID, pMsg));
            }
        }
        private void OnSendComplete(string pID)
        {
            if (this.SendComplete != null)
            {
                SocketClientExtender_OnSendComplete(new SocketClientEventArgs(pID));
                if (this.RaiseAsyncEvents)
                {
                    foreach (SocketClientEventHandler action in this.SendComplete.GetInvocationList())
                        action.BeginInvoke(this, new SocketClientEventArgs(pID), null, null);
                }
                else
                    this.SendComplete(this, new SocketClientEventArgs(pID));
            }
        }
        private void OnSendFailed(string pID, byte[] pMsg)
        {
            if (this.SendFailed != null)
            {
                SocketClientExtender_OnSendFailed(new SocketClientDataSendFailedEventArgs(pID, pMsg));
                if (this.RaiseAsyncEvents)
                {
                    foreach (SocketClientDataSendFailedEventHandler action in this.SendFailed.GetInvocationList())
                        action.BeginInvoke(this, new SocketClientDataSendFailedEventArgs(pID, pMsg), null, null);
                }
                else
                    this.SendFailed(this, new SocketClientDataSendFailedEventArgs(pID, pMsg));
            }
        }
        private void OnSendHandShakeMsg(string pID)
        {
            if (this.SendHandShakeMsg != null)
            {
                SocketClientExtender_OnSendHandShakeMsg(new SocketClientEventArgs(pID));
                if (this.RaiseAsyncEvents)
                {
                    foreach (SocketClientEventHandler action in this.SendHandShakeMsg.GetInvocationList())
                        action.BeginInvoke(this, new SocketClientEventArgs(pID), null, null);
                }
                else
                    this.SendHandShakeMsg(this, new SocketClientEventArgs(pID));
            }
        }
        private void OnUnableToConnect(string pID)
        {
            if (this.UnableToConnect != null)
            {
                SocketClientExtender_OnUnableToConnect(new SocketClientEventArgs(pID));
                if (this.RaiseAsyncEvents)
                {
                    foreach (SocketClientEventHandler action in this.UnableToConnect.GetInvocationList())
                        action.BeginInvoke(this, new SocketClientEventArgs(pID), null, null);
                }
                else
                    this.UnableToConnect(this, new SocketClientEventArgs(pID));
            }
        }
        #endregion

        #region Logger
        private iLogger logger = null;
        public iLogger Logger
        {
            get { return logger; }
            set { logger = value; }
        }
        private void WriteLogEntry(string msg)
        {
            //if (this.Client != null)
            {
                if (this.Logger != null)
                {
                    this.Logger.WriteEntry(this, msg);
                }
            }
        }
        //private iLogger connLogger = null;
        //public iLogger ConnLogger
        //{
        //    get { return connLogger; }
        //    set { connLogger = value; }
        //}
        //private void WriteConnectionLogEntry(string msg)
        //{
        //    //if (this.Client != null)
        //    {
        //        if (this.ConnLogger != null)
        //        {
        //            this.ConnLogger.WriteEntry(this, msg);
        //        }
        //    }
        //}
        #endregion

        #region Properties
        public SocketClientLocationEnum Location
        {
            get { return location; }
            private set { location = value; }
        }
        public Socket Client
        {
            get { return client; }
            set { client = value; }
        }
        public string RemoteHostIP
        {
            get { return remoteHostIP; }
            set { remoteHostIP = value; }
        }
        public int RemoteHostPort
        {
            get { return remoteHostPort; }
            set { remoteHostPort = value; }
        }
        public int CheckConnDelay { get; set; }
        //public int CheckConnIntervel { get; set; }
        public int MaxWaitingDelay { get; set; }
        public enum IDTypeEnum
        {
            Remote,
            Local,
        }
        static object onjlock = new object();
        private IDTypeEnum idtype = IDTypeEnum.Remote;
        public IDTypeEnum IDType
        {
            get
            {
                lock (onjlock)
                {
                    return idtype;
                }
            }
            set { idtype = value; }
        }
        //public IDTypeEnum IDType 
        //{
        //    lock(onjlock)
        //{
        //    get;
        
        //set; 
        //}
        //}
        private string id = string.Empty;
        public string ID
        {
            get
            {
                if (id == string.Empty)
                {
                    if (this.Client != null)
                    {
                        if (IDType == IDTypeEnum.Remote)
                            id = this.Client.GetRemoteID();
                        else
                            id = this.Client.GetLocalID();
                    }
                }
                return id;
                //return string.Empty;
            }
        }
        public string TerminalName
        {
            get
            {
                if (!String.IsNullOrEmpty(this.ID))
                    return "Terminal : " + ID;
                return string.Empty;
            }
        }
        public int InitConnDelay { get; set; }
        public bool IsRunning
        {
            get;
            private set;
        }
        public TimeSpan IdleTime { get; private set; }
        public bool RetryToConnect { get; set; }
        #endregion        // The port number for the remote device.

        #region Public Methods
        public void StartClient()
        {
            this.InitConnDelay = 5;
            InternalStartClient();
        }
        public void StopClient()
        {
            this.InitConnDelay = -1;
            InternalStopClient();
        }
        public void Send(byte[] data)
        {
            try
            {
                if (!InternalSend(data))
                {
                    if (this.IDType == IDTypeEnum.Remote)
                        OnSendFailed(Client.GetRemoteID(), data);
                    else
                        OnSendFailed(Client.GetLocalID(), data);
                }
            }
            catch (Exception ex)
            {
                HsmLogger.WriteTransLog(null,string.Format("Exception occured in Send : {0}", ex.Message));
                throw ex;
            }
        }
        public void SendWithTimeOut(byte[] data)
        {
            ExecuteTerminalCommandRequest(new TerminalCommandRequestWithArgsDelegate(this.Send), data);
        }
        #endregion

        #region private Methods
        private void InternalStartClient()
        {
            // Connect to a remote device.
            try
            {
                if (this.InitConnDelay == -1) return;
                if (this.Location == SocketClientLocationEnum.ClientSide)
                {
                    // Close the socket if it is still open
                    InternalStopClient();
                    IPHostEntry ipHostInfo = Dns.Resolve(this.RemoteHostIP);
                    IPAddress ipAddress = ipHostInfo.AddressList[0];
                    IPEndPoint remoteEP = new IPEndPoint(ipAddress, this.RemoteHostPort);
                    // Create a TCP/IP socket.
                    Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    Client.Blocking = false;
                    // Connect to the remote endpoint.
                    Client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), Client);

                    if (RetryToConnect)
                        UpdateConnTimer(this.InitConnDelay);
                    //else
                    //    UpdateConnTimer(-1);
                }
                else
                {
                    string pID = string.Empty;

                    if (this.IDType == IDTypeEnum.Remote)
                        pID = Client.GetRemoteID();
                    else
                        pID = Client.GetLocalID();

                    HsmLogger.WriteTransLog(null, "Remote ID : " + pID + "IDType :" + this.IDType.ToString());
                    if (!String.IsNullOrEmpty(pID))
                        OnConnected(pID);
                    WaitForData(this.Client);
                    UpdateConnTimer(this.CheckConnDelay);
                }
            }
            catch (Exception e)
            {
                HsmLogger.WriteTransLog(null, string.Format("Exception occured in 'StartClient' : {0}", e.Message));
            }
        }
        private void InternalStopClient()
        {
            try
            {
                if (stateTimer != null)
                {
                    stateTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                    stateTimer.Dispose();
                    stateTimer = null;
                }

                if (ConnTimer != null)
                {
                    ConnTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                    ConnTimer.Dispose();
                    ConnTimer = null;
                }

                if (Client != null)
                {
                    string pID = string.Empty;

                    if (this.IDType == IDTypeEnum.Remote)
                        pID = Client.GetRemoteID();
                    else
                        pID = Client.GetLocalID();

                    Client.Shutdown();
                    Client = null;
                    if (!String.IsNullOrEmpty(pID))
                        OnDisconnected(pID);
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void UpdateConnTimer(int connDelay)
        {
            //AutoResetEvent autoEvent = new AutoResetEvent(false);
            if (ConnTimer == null)
            {
                TimerCallback tcb = CheckConnectivity;
                if (connDelay == -1)
                    ConnTimer = new System.Threading.Timer(tcb, this.Client, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                else
                    ConnTimer = new System.Threading.Timer(tcb, this.Client, connDelay * 1000, connDelay * 1000);
            }
            else
            {
                if (connDelay == -1)
                    ConnTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                else
                    ConnTimer.Change(connDelay * 1000, connDelay * 1000);
            }
        }
        private String response = String.Empty;
        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;
                // Complete the connection.
                handler.EndConnect(ar);
                if (handler.Connected)
                {
                    if (this.IDType == IDTypeEnum.Remote)
                    {
                        HsmLogger.WriteTransLog(null, string.Format("Client connected to {0}", handler.RemoteEndPoint.ToString()));
                        OnConnected(handler.GetRemoteID());
                    }
                    else
                    {
                        HsmLogger.WriteTransLog(null, string.Format("Client connected to {0}", handler.LocalEndPoint.ToString()));
                        OnConnected(handler.GetLocalID());
                    }
                    WaitForData(handler);
                }
                else
                    HsmLogger.WriteTransLog(null, "Unable to connect to Host");
            }
            catch (ObjectDisposedException e)
            {
                OnUnableToConnect("");
                HsmLogger.WriteTransLog(null, "Error occured in ConnectCallback first : " + e.Message);
            }
            catch (Exception e)
            {
                HsmLogger.WriteTransLog(null, "Error occured in ConnectCallback : " + e.Message);
                OnUnableToConnect("");
            }
        }
        public AsyncCallback m_pfnCallBack;
        private IAsyncResult oldAynchResult;
        private void WaitForData(Socket handler)
        {
            try
                {
                if (handler != null)
                {
                    if (m_pfnCallBack == null)
                        m_pfnCallBack = new AsyncCallback(OnDataReceived);

                    ClientObject state = new ClientObject();
                    state.Socket = handler;

                    oldAynchResult = handler.BeginReceive(state.buffer, 0, state.buffer.Length, SocketFlags.None, m_pfnCallBack, state);
                }
                }
            catch (Exception e)
            {
                HsmLogger.WriteTransLog(null, string.Format("Error occured in WaitForData : {0}", e.Message));
                InternalCloseConnection();
            }
        }
        private DateTime LastReceivedTime;
        private void OnDataReceived(IAsyncResult ar)
        {
            if (ar == oldAynchResult)
            {
                ClientObject state = (ClientObject)ar.AsyncState;
                Socket handler = state.Socket;

                try
                {
                    if (!handler.Connected)
                    {
                        //handler.Close();
                        InternalCloseConnection();
                        return;
                    }
                    byte[] buff = GetRecievedData(ar);
                    if (buff.Length > 0)
                    {
                        response = ByteArrayToASCII(buff);
                        // Encoding.ASCII.GetString(buff, 0, buff.Length);
                        if (this.IDType == IDTypeEnum.Remote)
                        {
                            if (!CommonConfiguration.ConfigHID)
                            {
                                HsmLogger.WriteTransLog(null, string.Format("Received  << {0} : {1}", handler.GetRemoteID(), Encoding.ASCII.GetString(buff, 0, buff.Length)));
                            }
                            else
                            {
                                HsmLogger.WriteTransLog(null, string.Format("Received  << {0} : {1}", handler.GetRemoteID(), ""));
                            }
                           
                        }
                        else
                        {
                            if (!CommonConfiguration.ConfigHID)
                            {
                                HsmLogger.WriteTransLog(null, string.Format("Received HSM << {0} : {1}", handler.GetLocalID(), Encoding.ASCII.GetString(buff, 0, buff.Length)));
                            }
                            else
                            {
                                HsmLogger.WriteTransLog(null, string.Format("Received HSM << {0} : {1}", handler.GetLocalID(), ""));
                            }
                            
                        }
                        //response));

                        LastReceivedTime = DateTime.Now;
                        if (this.IDType == IDTypeEnum.Remote)
                            OnDataReceived(handler.GetRemoteID(), buff);//response);
                        else
                            OnDataReceived(handler.GetLocalID(), buff);//response);

                        WaitForData(handler);
                    }
                    else
                    {
                        InternalCloseConnection();
                    }
                }
                catch (Exception e)
                {
                    HsmLogger.WriteTransLog(null, string.Format("Error occured in OnDataReceived : {0}", e.Message));
                    InternalCloseConnection();
                }
            }
            else
                WaitForData(this.Client);
        }
        public void StopReceive(IAsyncResult ar)
        {
            ClientObject state = (ClientObject)ar.AsyncState;
            Socket handler = state.Socket;
            int nBytesRec = 0;
            try
            {
                nBytesRec = handler.EndReceive(ar);
            }
            catch { }
        }
        public byte[] GetRecievedData(IAsyncResult ar)
        {
            ClientObject state = (ClientObject)ar.AsyncState;
            Socket handler = state.Socket;
            int nBytesRec = 0;
            try
            {
                nBytesRec = handler.EndReceive(ar);
            }
            catch { }
            byte[] byReturn = new byte[nBytesRec];
            Array.Copy(state.buffer, byReturn, nBytesRec);

            // Check for any remaining data and display it
            // This will improve performance for large packets 
            // but adds nothing to readability and is not essential

            int nToBeRead = handler.Available;
            byte[] byData = null;
            if (nToBeRead > 0)
            {
                byData = new byte[nToBeRead];
                handler.Receive(byData);
                // Append byData to byReturn here
            }
         
            byte[] finalBytes = new byte[nBytesRec + nToBeRead];
            Array.Copy(byReturn, finalBytes, nBytesRec);
            if (nToBeRead > 0)
                Array.Copy(byData, 0, finalBytes, nBytesRec, nToBeRead);
            //
            //return byReturn;
            return finalBytes;
        }

        #region Sync Send
        private bool InternalSend(byte[] Message)
        {
            //Message = Message.Replace("\0", "");
            if (Message == null || Message.Length == 0)
            {
                HsmLogger.WriteTransLog(null, "Message length must be > 0");
                return false;
            }

            // Check we are connected
            if (Client == null)
            {
                HsmLogger.WriteTransLog(null, "Connection not initialized");
                return false;
                //ConnectToServer();
            }
            if (!Client.Connected)
            {
                HsmLogger.WriteTransLog(null, "Must be connected to Send a message");
                return false;
            }

            // Read the message from the text box and send it
            try
            {
                // Convert to byte array and send.
                Byte[] byteDateLine = Message; // ASCIIToByteArray(Message);// Encoding.ASCII.GetBytes(Message.ToCharArray());
                Client.Send(byteDateLine, byteDateLine.Length, SocketFlags.None);
                if (this.IDType == IDTypeEnum.Remote)
                {
                    HsmLogger.WriteTransLog(null, string.Format("Send  >> {0} : {1}", Client.GetRemoteID(), Encoding.ASCII.GetString(Message)));
                    OnSendComplete(Client.GetRemoteID());
                }
                else
                {
                    HsmLogger.WriteTransLog(null, string.Format("Send HSM >> {0} : {1}", Client.GetLocalID(), Encoding.ASCII.GetString(Message)));                  
                    OnSendComplete(Client.GetLocalID());
                }
                return true;
            }
            catch (Exception ex)
            {
                HsmLogger.WriteTransLog(null, ex.Message + "Send Message Failed!");
                return false;
            }
        }
        #endregion
        #endregion

        #region Check the Terminal Connectivity in regular intervals
        System.Threading.Timer ConnTimer = null;
        public void CheckConnectivity(object connInfo)
        {
            try
            {
                if (this.InitConnDelay == -1) return;

                IdleTime = DateTime.Now - LastReceivedTime;

                if (IdleTime.TotalSeconds < CheckConnDelay) return;

                // WriteLogEntry("Checking Connectivity...");// : " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss:fff"));
                Socket sock = (Socket)connInfo;
                if (sock == null || !sock.Connected)
                {
                    WriteLogEntry("Connection is not initialized / disconnected...");
                    WriteLogEntry("Reconnecting to Server");
                    InternalStartClient();
                }
                else
                {
                    if (this.IDType == IDTypeEnum.Remote)
                        OnSendHandShakeMsg(sock.GetRemoteID());
                    else
                        OnSendHandShakeMsg(sock.GetLocalID());
                }
            }
            catch (Exception ex)
            {
                WriteLogEntry("Error occured in 'CheckConnectivity'");
                WriteLogEntry("Error : " + ex.Message);
                WriteLogEntry("StackTrace : " + ex.StackTrace);
            }
        }
        #endregion

        #region Send Terminal Request to Server and wait for reply
        //private static Mutex mutex = new Mutex();
        private bool WaitingForReplay = false;
        System.Threading.Timer stateTimer = null;
        private object obj = new object();
        private delegate void TerminalCommandRequestDelegate();
        private delegate void TerminalCommandRequestWithArgsDelegate(byte[] msg);
        private void ExecuteTerminalCommandRequest(TerminalCommandRequestWithArgsDelegate cmd, byte[] msg)
        {
            if (cmd == null) return;
            //mutex.WaitOne();
            try
            {
                if (WaitingForReplay)
                {
                    WriteLogEntry("Already one request is pending. Please try after some time..");
                    //mutex.ReleaseMutex();
                    return;
                }
                else
                {
                    cmd.Invoke(msg);

                    AutoResetEvent autoEvent = new AutoResetEvent(false);
                    TimerCallback tcb = CheckStatus;
                    WaitingForReplay = true;
                    if (stateTimer == null)
                        stateTimer = new System.Threading.Timer(tcb, autoEvent, MaxWaitingDelay * 1000, System.Threading.Timeout.Infinite);
                    else
                        stateTimer.Change(MaxWaitingDelay * 1000, System.Threading.Timeout.Infinite);
                }
            }
            catch (Exception ex)
            {
                WriteLogEntry("Error occured in 'ReceiveSFTPDetails'");
                WriteLogEntry("Error : " + ex.Message);
            }
            finally
            {
                //mutex.ReleaseMutex();
            }
        }
        private void CheckStatus(Object stateInfo)
        {
            try
            {
                //mutex.WaitOne();
                if (WaitingForReplay)
                {
                    InternalCloseConnection();
                }
            }
            catch //(Exception ex)
            {

            }
            finally
            {
                //mutex.ReleaseMutex();
            }
        }
        private void InternalCloseConnection()
        {
            WaitingForReplay = false;
            if (this.location == SocketClientLocationEnum.ClientSide)
            {
                WriteLogEntry("Disconnecting from Server. Because Server is not responding.");
                InternalStopClient();
                //WriteLogEntry("Disconnected from Server");
                if (this.InitConnDelay != -1)
                {
                    WriteLogEntry("Reconnecting to Server");
                    InternalStartClient();
                }
            }
            else
            {
                WriteLogEntry(string.Format("Server closing Terminal({0}) connection. Because Terminal is not responding.", this.ID));
                StopClient();
            }
        }
        #endregion

        public int Receive(byte[] buffer, int offset, int size, int timeout)
        {
            Socket socket = this.Client;
            int startTickCount = Environment.TickCount;
            int received = 0;  // how many bytes is already received
            do
            {
                if (Environment.TickCount > startTickCount + timeout)
                    throw new Exception("Timeout.");
                try
                {
                    received += socket.Receive(buffer, offset + received, size - received, SocketFlags.None);
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.WouldBlock ||
                        ex.SocketErrorCode == SocketError.IOPending ||
                        ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                    {
                        // socket buffer is probably empty, wait and try again
                        Thread.Sleep(30);
                    }
                    else
                        throw ex;  // any serious error occurr
                }
            } while (received < size);
            return received;
        }
        public byte[] Receive()
        {
            Socket socket = this.Client;
            int nBytesRec = 256;
            byte[] byReturn = new byte[nBytesRec];
            nBytesRec = socket.Receive(byReturn, socket.Available, SocketFlags.None);//, byReturn.Length, SocketFlags.None);

            int nToBeRead = socket.Available;
            byte[] byData = null;
            if (nToBeRead > 0)
            {
                byData = new byte[nToBeRead];
                socket.Receive(byData);
            }

            byte[] finalBytes = new byte[nBytesRec + nToBeRead];
            Array.Copy(byReturn, finalBytes, nBytesRec);
            if (nToBeRead > 0)
                Array.Copy(byData, 0, finalBytes, nBytesRec, nToBeRead);

            return finalBytes;
        }

        // Displays sending with a connected socket
        // using the overload that takes a buffer, message size, and socket flags.
        public int SendReceiveTest3(string message)
        {
            Socket server = this.Client;
            byte[] msg = ASCIIToByteArray(message);// Encoding.UTF8.GetBytes("This is a test");
            byte[] bytes = new byte[256];
            try
            {
                // Blocks until send returns.
                int i = server.Send(msg, msg.Length, SocketFlags.None);
                HsmLogger.WriteTransLog(null, string.Format("Sent {0} bytes.", i));

                // Get reply from the server.
                int byteCount = server.Receive(bytes, server.Available,
                                                   SocketFlags.None);
                if (byteCount > 0)
                    WriteLogEntry(ByteArrayToASCII(bytes));
            }
            catch (SocketException e)
            {
                HsmLogger.WriteTransLog(null, string.Format("{0} Error code: {1}.", e.Message, e.ErrorCode));
                return (e.ErrorCode);
            }
            return 0;
        }
        public static string ByteArrayToASCII(byte[] byteArray)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < byteArray.Length; i++)
                sb.Append((char)byteArray[i]);
            return sb.ToString();
            //System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            //return encoding.GetString(byteArray);
        }
        public static byte[] ASCIIToByteArray(string str)
        {
            char[] chars = str.ToCharArray();
            byte[] result = new byte[chars.Length];
            for (int i = 0; i < chars.Length; i++)
                result[i] = (byte)chars[i];
            return result;
            //System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            //return encoding.GetBytes(str);
        }
    }
}
