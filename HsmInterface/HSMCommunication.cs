using MIIPL.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using MIIPL.Common;
using MaxiSwitch.Logger;

namespace MaxiSwitch.HSMManager.HsmInterface
{
    public class HSMCommunication
    {
        private int ResponseTime = 0;
        public HSMCommunication(string _HsmIp, int _HsmPort)//
        {
            try
            {
                HSM.HSMIP = _HsmIp;
                HSM.HSMPort = _HsmPort;
            }
            catch (Exception ex)
            { }
        }

        #region Server
        private SocketServer bancsServer = null;
        public SocketServer BancsServer
        {
            get
            {
                if (bancsServer == null)
                {
                    bancsServer = new SocketServer();
                    bancsServer.Started += new SocketServerEventHandler(AcquirerServer_Started);
                    bancsServer.Stopped += new SocketServerEventHandler(AcquirerServer_Stopped);
                    bancsServer.IsValidIP += new SocketServerIsValidIPEventHandler(AcquirerServer_IsValidIP);
                    bancsServer.Connected += new SocketClientEventHandler(AcquirerServer_Connected);
                    bancsServer.Disconnected += new SocketClientEventHandler(AcquirerServer_Disconnected);
                    bancsServer.DataReceived += new SocketClientDataReceivedEventHandler(AcquirerServer_DataReceived);
                    bancsServer.SendComplete += new SocketClientEventHandler(AcquirerServer_SendComplete);
                    bancsServer.SendFailed += new SocketClientDataSendFailedEventHandler(AcquirerServer_SendFailed);
                    bancsServer.SendHandShakeMsg += new SocketClientEventHandler(AcquirerServer_SendHandShakeMsg);
                }

                return bancsServer;
            }
            private set { bancsServer = value; }
        }

        void AcquirerServer_SendHandShakeMsg(object sender, SocketClientEventArgs e)
        {
            //if (this.SendHandShakeMsg != null)
            //{
            //    this.SendHandShakeMsg(this, new TerminalEventArgs());
            //}
        }
        void AcquirerServer_Stopped(object sender, EventArgs e)
        {
            ////WriteLogEntry("Listener stopped");
        }
        void AcquirerServer_Started(object sender, EventArgs e)
        {
            //// WriteLogEntry("Listener started");
        }
        void AcquirerServer_IsValidIP(object sender, SocketServerIsValidIPEventArgs e)
        {

        }
        void AcquirerServer_Connected(object sender, SocketClientEventArgs e)
        {
            ////WriteLogEntry(e.ID + " connected");
            al.Add(e.ID.ToString());
            OnUpdateAcquirerClientList();
        }
        void AcquirerServer_Disconnected(object sender, SocketClientEventArgs e)
        {
            ////WriteLogEntry(e.ID + " disconnected");
            OnUpdateAcquirerClientList();
        }
        public string IDs;
        void AcquirerServer_DataReceived(object sender, SocketClientDataReceivedEventArgs e)
        {
            try
            {
                string RawMessage = string.Empty;
                string strMsg = string.Empty;
                string message = string.Empty;

                RawMessage = Utils.ByteArrayToASCII(e.Message).ToString();
                if (ConfigurationManager.AppSettings["ConfigHID"].ToString() == "2")
                {
                    //// WriteLogEntry(string.Format("Received << {0} : {1}", e.ID, RawMessage));
                }
                else
                {
                    ////WriteLogEntry(string.Format("Received << {0} : {1}", e.ID, new string('X', RawMessage.Length - 4) + RawMessage.Substring(RawMessage.Length - 4)));
                }
                string SS = Utils.ByteArrayToASCII(e.Message);
                string ID = e.ID;
                ID = ID.PadRight(46, '#');

                strMsg = SS.Substring(48);

                ID = ID + strMsg;
                int length = ID.Length;
                if (ConfigurationManager.AppSettings["ConfigHID"].ToString() == "2")
                {
                    ////WriteLogEntry("HSM Send to Cleint Manager : " + ID);
                    ////WriteLogEntry("HSM Sending Message Length : " + length);
                }
                else
                {
                    message = ID.Substring(46);
                    ////WriteLogEntry("HSM Send to Cleint Manager : " + ID.Substring(0, 46) + new string('X', message.Length - 4) + message.Substring(message.Length - 4));
                    ////WriteLogEntry("HSM Sending Message Length : " + length);
                }
                //WriteLogEntry(string.Format("Received << {0} : {1}", e.ID,strHeader));
                al.Add(e.ID);
                IDs = e.ID;
                ClientList.Add(client);
                byte[] msg;
                msg = Utils.ASCIIToByteArray(ID);
                ProcessData(msg, e.ID);
            }
            catch (Exception ex)
            {
                /////WriteLogEntry("Error Occured in the Acquirer DataRecieve : " + ex.Message.ToString() + ex.Source.ToString() + ex.StackTrace.ToString());
            }
        }
        void AcquirerServer_SendComplete(object sender, SocketClientEventArgs e)
        {
            ////WriteLogEntry("Data send completed to Application : " + e.ID);
            al.Remove(e.ID);
        }
        void AcquirerServer_SendFailed(object sender, SocketClientDataSendFailedEventArgs e)
        {
            ////WriteLogEntry("Data send failed to Application : " + e.ID);
        }
        public event EventHandler UpdateAcquirerClientList = null;
        private void OnUpdateAcquirerClientList()
        {

            //if (UpdateAcquirerClientList != null)
            //    UpdateAcquirerClientList.BeginInvoke(this.Acquirer, new EventArgs(), null, null);
        }
        ArrayList al = new ArrayList();
        #endregion

        #region Client
        private SocketClient client = null;
        private string SyncDataReceived = string.Empty;
        public SocketClient Client
        {
            get
            {
                if (client == null)
                {
                    client = new SocketClient(SocketClientLocationEnum.ClientSide);
                    client.Connected += new SocketClientEventHandler(client_Connected);
                    client.DataReceived += new SocketClientDataReceivedEventHandler(client_DataReceived);
                    client.Disconnected += new SocketClientEventHandler(client_Disconnected);
                    client.SendComplete += new SocketClientEventHandler(client_SendComplete);
                    client.SendFailed += new SocketClientDataSendFailedEventHandler(client_SendFailed);
                    client.SendHandShakeMsg += new SocketClientEventHandler(client_SendHandShakeMsg);
                    //client.IDType = SocketClient.IDTypeEnum.Local;
                }

                return client;
            }
            set { client = value; }
        }

        void client_SendHandShakeMsg(object sender, SocketClientEventArgs e)
        {

        }

        void client_SendFailed(object sender, SocketClientDataSendFailedEventArgs e)
        {
            HsmLogger.WriteTransLog(null, "Message Send Failed....");
            SyncDataReceived = string.Empty;
        }

        void client_SendComplete(object sender, SocketClientEventArgs e)
        {
            HsmLogger.WriteTransLog(null, "Message Send Completed....");
        }

        void client_Disconnected(object sender, SocketClientEventArgs e)
        {
            HsmLogger.WriteTransLog(null, "Client Is Disconnected....");
            SyncDataReceived = string.Empty;
        }

        void client_DataReceived(object sender, SocketClientDataReceivedEventArgs e)
        {
            HsmLogger.WriteTransLog(null, "Client DataReceived....");
            try
            {
                ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(delegate(object state)
                {
                    CommonConfiguration.HSMType = HSMType.Safenet;
                    if (CommonConfiguration.HSMType == HSMType.Thales)
                    {
                        SyncDataReceived = Utils.ByteArrayToASCII(e.Message);
                    }
                    else if (CommonConfiguration.HSMType == HSMType.Safenet)
                    {
                        SyncDataReceived = Utils.ByteArrayToHex(e.Message);
                    }
                }));
            }
            catch (Exception ex)
            { HsmLogger.WriteTransLog(null, "Error Occured On DataReceive : " + ex.Message.ToString() + ex.StackTrace.ToString() + ex.Source.ToString()); }
        }

        void client_Connected(object sender, SocketClientEventArgs e)
        {
            HsmLogger.WriteTransLog(null, "Client Is Connected....");
        }

        #endregion

        public void ServerStartSTOP()
        {
            if (BancsServer.IsListening)
                BancsServer.StopListening();
            else
            {
                BancsServer.StartListening();
            }
        }
        bool IsRunning = false;

        public void ClientStartStop()
        {
            if (IsRunning) //(Client.IsRunning)
            {
                Client.StopClient();
                IsRunning = false;

            }
            else
            {
                Client.RemoteHostIP = ConfigurationManager.AppSettings["HSMIP"].ToString();
                Client.RemoteHostPort = Convert.ToInt32(ConfigurationManager.AppSettings["HSMPort"].ToString());
                Client.StartClient();////*******************Commented due to Connection Created with every new instance of Class**
                IsRunning = true;
            }
        }

        public void SecondaryClientStartStop()
        {
            if (IsRunning) //(Client.IsRunning)
            {
                Client.StopClient();
                IsRunning = false;
            }
            else
            {
                Client.RemoteHostIP = ConfigurationManager.AppSettings["SecondaryHSMIP"].ToString();
                Client.RemoteHostPort = Convert.ToInt32(ConfigurationManager.AppSettings["SecondaryHSMPort"].ToString());
                ResponseTime = Convert.ToInt32(ConfigurationManager.AppSettings["HSMResponseTime"].ToString());
                Client.StartClient();////*******************Commented due to Connection Created with every new instance of Class**                
                IsRunning = true;
            }
        }

        private List<SocketClient> clientList = null;
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

        //******************Original On 090116*********************
        public string ProcessData(byte[] rawMsg, string ID)
        {
            ClientList.Add(client);
            try
            {
                HsmLogger.WriteTransLog(null, "Authentication Packed Data Sending (Primary) ");
                string Data = HSM.SendFunctionCommand(null, null, rawMsg, 0);

                if (!CommonConfiguration.ConfigHID)
                    HsmLogger.WriteTransLog(null, "Authentication Data Received (Primary) " + Data);
                else
                    HsmLogger.WriteTransLog(null, "Authentication Data Received (Primary) " + new string('X', Data.Length - 4) + Data.Substring(Data.Length - 4, 4));
                return Data;
            }
            catch (Exception ex)
            {
                HsmLogger.WriteTransLog(null, "Error Occured While Sending Message (Primary) " + ex.Message + ex.Source + ex.StackTrace);
                return "";
            }
        }

        #region "For Future Use"
        ////******************Modified On 090116*********************
        ////public string ProcessData(byte[] rawMsg, string ID)
        ////{
        ////    string Response = string.Empty;
        ////    try
        ////    {
        ////        if (!CommonConfiguration.SecondaryHSM)
        ////        {
        ////            HsmLogger.WriteTransLog(null, "Packed Data Sending (Primary)....");
        ////            Response = PrimarySend(rawMsg, ID);
        ////            HsmLogger.WriteTransLog(null, "Packed Data Received (Primary)....");
        ////        }
        ////        else
        ////        {
        ////            HsmLogger.WriteTransLog(null, "Packed Data Sending (Secondary)....");                    
        ////            Response = SecondarySend(rawMsg, ID);
        ////            HsmLogger.WriteTransLog(null, "Packed Data Received (Secondary)....");
        ////        }                
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        HsmLogger.WriteTransLog(null, "Error Occured While Sending Message" + ex.Message + ex.Source + ex.StackTrace);
        ////        Response = string.Empty;
        ////    }
        ////    return Response;
        ////}

        ////private string PrimarySend(byte[] rawMsg, string ID)
        ////{
        ////    ClientList.Add(client);
        ////    try
        ////    {
        ////        HsmLogger.WriteTransLog(null, "Authentication Packed Data Sending (Primary) ");
        ////        string Data = HSM.SendFunctionCommand(null, null, rawMsg, 0);

        ////        if (!CommonConfiguration.ConfigHID)
        ////            HsmLogger.WriteTransLog(null, "Authentication Data Received (Primary) " + Data);
        ////        else
        ////            HsmLogger.WriteTransLog(null, "Authentication Data Received (Primary) " + new string('X', Data.Length - 4) + Data.Substring(Data.Length - 4, 4));
        ////        return Data;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        HsmLogger.WriteTransLog(null, "Error Occured While Sending Message (Primary) " + ex.Message + ex.Source + ex.StackTrace);
        ////        return "";
        ////    }  
        ////}
        #endregion

        public string SecondarySend(byte[] rawMsg, string ID)
        {
            string DataReceived = string.Empty;
            try
            {
                SyncDataReceived = string.Empty;
                SecondaryClientStartStop();
                ClientList.Add(client);

                byte[] MsgToSend = null;
                CommonConfiguration.HSMType = HSMType.Safenet;
                if (CommonConfiguration.HSMType == HSMType.Thales)
                {
                    MsgToSend = new byte[rawMsg.GetLength(0) + 2];
                    MsgToSend[0] = Convert.ToByte(rawMsg.GetLength(0) / 256);
                    MsgToSend[1] = Convert.ToByte(rawMsg.GetLength(0) % 256);
                    Array.Copy(rawMsg, 0, MsgToSend, 2, rawMsg.GetLength(0));
                    rawMsg = null;
                }
                else
                { MsgToSend = rawMsg; }

                HsmLogger.WriteTransLog(null, "Authentication Packed Data Sending (Secondary) ");
                Client.Send(MsgToSend);

                if (!CommonConfiguration.ConfigHID)
                    HsmLogger.WriteTransLog(null, string.Format("Send (Secondary)  >> HSM : {0} ", Utils.ByteArrayToASCII(MsgToSend)));
                else
                    HsmLogger.WriteTransLog(null, string.Format("Send (Secondary)  >> HSM : {0} ", ""));

                MsgToSend = null;
                double Counter = 0;

                while (string.IsNullOrEmpty(SyncDataReceived) && Convert.ToInt64(ResponseTime) > Counter)
                {
                    System.Threading.Thread.Sleep(1);
                    Counter++;
                }

                if (!CommonConfiguration.ConfigHID)
                    HsmLogger.WriteTransLog(null, "Authentication Data Received (Secondary) " + SyncDataReceived);
                else
                    HsmLogger.WriteTransLog(null, "Authentication Data Received (Secondary) " + new string('X', SyncDataReceived.Length - 4) + SyncDataReceived.Substring(SyncDataReceived.Length - 4, 4));

                DataReceived = SyncDataReceived;
                ClientList.Remove(client);
                Client.StopClient();
            }
            catch (Exception ex)
            {
                HsmLogger.WriteTransLog(null, "Error Occured While Sending Message (Secondary) " + ex.Message + ex.Source + ex.StackTrace);
                ClientList.Remove(client);
                Client.StopClient();
                DataReceived = string.Empty;
            }
            return DataReceived;
        }

        ////Creating Instance For HSM Class

        private static MIIPLHSMConnMgr hsm = null;

        public static MIIPLHSMConnMgr HSM
        {
            get
            {
                if (hsm == null)
                {
                    try
                    {
                        hsm = new MIIPLHSMConnMgr();
                        HsmLogger.WriteTransLog(null, "New Hsm Connections are Created");
                    }
                    catch (Exception ex)
                    { HsmLogger.WriteTransLog(null, "Error at send msg to HSM" + ex.Message + ex.Source + ex.StackTrace); }
                }
                return hsm;
            }
            set { hsm = value; }
        }
    }

    public interface iMIIPLResourceItem : IDisposable
    {
        bool IsDirty { get; set; }
        event EventHandler Completed;
        event EventHandler RemoveItem;
        DateTime ItemRunningSince { get; set; }
        string ItemKey { get; set; }
    }

    public class MIIPLResourceEventArgs : EventArgs
    {
        public iMIIPLResourceItem Item { get; set; }
        public MIIPLResourceEventArgs() : this(null) { }
        public MIIPLResourceEventArgs(iMIIPLResourceItem item) { Item = item; }
    }

    public delegate void SocketClientHandler(string pID, string key, byte[] rawMsg);

    public class MIIPLResourceMgr
    {
        public MIIPLResourceMgr()
        {
            lockObj = new object();
            MinCount = Convert.ToInt32(ConfigurationManager.AppSettings["MinConnection"]);
            MaxCount = Convert.ToInt32(ConfigurationManager.AppSettings["MaxConnection"]);
            MaxWaitingTime = Convert.ToInt32(ConfigurationManager.AppSettings["MaxWaitingTime"]);
            RunningItemStatus = Convert.ToInt32(ConfigurationManager.AppSettings["MaxRuningResourceTime"]);
        }

        public int MinCount { get; set; }
        public int MaxCount { get; set; }
        public int MaxWaitingTime { get; set; }
        public int RunningItemStatus { get; set; }
        private Queue<iMIIPLResourceItem> idleResources;
        public Queue<iMIIPLResourceItem> IdleResources
        {
            get
            {
                if (idleResources == null)
                    idleResources = new Queue<iMIIPLResourceItem>();
                return idleResources;
            }
            set { idleResources = value; }
        }

        private List<iMIIPLResourceItem> runningResources;
        public List<iMIIPLResourceItem> RunningResources
        {
            get
            {
                if (runningResources == null)
                    runningResources = new List<iMIIPLResourceItem>();
                return runningResources;
            }
            set { runningResources = value; }
        }

        private object lockObj = null;

        public event EventHandler<MIIPLResourceEventArgs> CreateItem = null;

        private void OnCreateItem()
        {
            try
            {
                if (CreateItem != null)
                {
                    MIIPLResourceEventArgs arg = new MIIPLResourceEventArgs();
                    CreateItem(this, arg);
                    if (arg.Item != null)
                    {
                        arg.Item.RemoveItem += new EventHandler(Item_RemoveItem);
                        arg.Item.Completed += new EventHandler(Item_Completed);
                        lock (lockObj)
                        {
                            IdleResources.Enqueue(arg.Item);
                        }
                        HsmLogger.WriteTransLog(null, "Resource Manager : " + arg.Item.ToString() + " created and added to idle resource list");
                    }
                }
            }
            catch (Exception ex)
            {
                HsmLogger.WriteTransLog(null, "Resource Manager : Error occured while creating new item : " + ex.Message);
            }
        }

        void Item_RemoveItem(object sender, EventArgs e)
        {
            try
            {
                iMIIPLResourceItem item = (iMIIPLResourceItem)sender;
                if (item != null)
                {
                    lock (lockObj)
                    {
                        if (RunningResources.Contains(item))
                        {
                            if (RunningResources.Remove(item))
                            {
                                HsmLogger.WriteTransLog(this, "Resource Manager : " + item.ToString() + " removed from running resource list" + " Key Removed : " + item.ItemRunningSince);
                                item.Dispose();
                                item = null;
                            }
                            else
                                HsmLogger.WriteTransLog(this, "Resource Manager : unable to remove " + item.ToString() + " from running resource list" + " Key Removed : " + item.ItemRunningSince);
                        }
                        ////*************Start Modified On 100316 For Disconnected Event*******************
                        ////else if (MaxCount > IdleResources.Count + RunningResources.Count)
                        ////    CreateItems();
                        ////*************End Modified On 100316 For Disconnected Event*******************
                        else if (IdleResources.Contains(item))
                        {
                            item.IsDirty = true;
                            ////*************Start Modified On 100316 For Disconnected Event*******************
                            IdleResources.Clear();
                            RunningResources.Clear();
                            ////*************End Modified On 100316 For Disconnected Event*******************
                            item.Dispose();
                            item = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HsmLogger.WriteTransLog(null, "Resource Manager : Error occured while removing item : " + ex.Message);
            }
        }

        private void Item_Completed(object sender, EventArgs e)
        {
            try
            {
                iMIIPLResourceItem item = (iMIIPLResourceItem)sender;
                if (item == null) return;
                lock (lockObj)
                {
                    if (RunningResources.Contains(item))
                    {
                        if (RunningResources.Remove(item))
                            HsmLogger.WriteTransLog(this, "Resource Manager : " + item.ToString() + " removed from running resource list" + " Key Removed : " + item.ItemRunningSince);
                        else
                            HsmLogger.WriteTransLog(this, "Resource Manager : unable to remove " + item.ToString() + " from running resource list" + " Key Not removed : " + item.ItemRunningSince);
                    }
                    if (IdleResources.Count < MaxCount)
                        IdleResources.Enqueue(item);
                    else
                    {
                        item.Dispose();
                        item = null;
                    }
                }
            }
            catch (Exception ex)
            {
                HsmLogger.WriteTransLog(null, "Resource Manager : Error occured in Item_Completed : " + ex.Message);
            }
        }

        public iMIIPLResourceItem GetItem()
        {
            iMIIPLResourceItem item = null;
            try
            {
                lock (lockObj)
                {
                    if (IdleResources.Count + RunningResources.Count == 0)
                        OnCreateItem();
                    HsmLogger.WriteTransLog(null, "IdleResources : " + IdleResources.Count + " RunningResources : " + RunningResources.Count);

                    if (MaxCount > IdleResources.Count + RunningResources.Count)
                        CreateItems();

                    if (RunningResources.Count > 0)
                    {
                        for (int i = 0; i < RunningResources.Count; i++)
                        {
                            iMIIPLResourceItem RunninngItem = (iMIIPLResourceItem)RunningResources[i];
                            if ((DateTime.Now - RunninngItem.ItemRunningSince).Minutes >= RunningItemStatus)
                            {
                                RunningResources.Remove(RunninngItem);

                                HsmLogger.WriteTransLog(null, "RunningResources Item Removed Due To Not Used From Long Time " + " key : " + RunninngItem.ItemRunningSince);
                            }
                        }
                    }
                }

                if (IdleResources.Count == 0)
                {
                    int CurrentWaitingTime = 0;
                    while (IdleResources.Count == 0 && MaxWaitingTime > CurrentWaitingTime)
                    {
                        if (CurrentWaitingTime == 0)
                        {
                            HsmLogger.WriteTransLog(null, "Resource Manager : Waiting for IdleResource");
                        }
                        CurrentWaitingTime = CurrentWaitingTime + 100;
                        Thread.Sleep(100);
                    }
                }

                if (IdleResources.Count > 0)
                {
                    item = IdleResources.Dequeue();
                    item.ItemRunningSince = DateTime.Now;
                    RunningResources.Add(item);
                    HsmLogger.WriteTransLog(null, "After IdleResources : " + IdleResources.Count + " After RunningResources : " + RunningResources.Count + "             key     add  " + item.ItemRunningSince);
                    HsmLogger.WriteTransLog(null, "Resource Manager : Item Dequeued From IdleResource");
                    if (item.IsDirty)
                    {
                        item.Dispose();
                        item = null;
                        item = GetItem();
                        item.IsDirty = false;
                    }
                }

            }
            catch (Exception ex)
            {
                HsmLogger.WriteErrorLog(null, ex);
            }
            return item;
        }

        private delegate void CreateItemsHandler();

        private CreateItemsHandler handler = null;

        private bool createItemsRunning = false;

        public void CreateItems()
        {
            if (handler == null)
                handler = new CreateItemsHandler(InternalCreateItems);
            handler.BeginInvoke(null, null);
        }

        private void InternalCreateItems()
        {
            HsmLogger.WriteTransLog(null, "Before Items creation");
            if (IdleResources.Count <= MinCount && !createItemsRunning)
            {
                createItemsRunning = true;
                int IncrementalCount = IdleResources.Count + RunningResources.Count;
                for (int i = 0; i < MaxCount - IncrementalCount; i++)
                {
                    OnCreateItem();
                    HsmLogger.WriteTransLog(null, "New Resource Created Count : " + i);
                }
                createItemsRunning = false;
            }
        }
    }

    public class HSMSocketClient : SocketClient, iMIIPLResourceItem
    {
        int HSMResponseTime = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["HSMResponseTime"].ToString());

        public HSMSocketClient()
            : base()
        {
            Init();
        }

        public HSMSocketClient(SocketClientLocationEnum pLocation)
            : base(pLocation)
        {
            Init();
        }

        private void Init()
        {
            RetryToConnect = false;
            WireEvents();
        }
        public string CID { get; set; }
        public string Key { get; set; }
        public byte[] MsgToSend { get; set; }
        public decimal HSMResponseDelay { get; set; }

        #region iMIIPLResourceItem Members

        public bool IsDirty { get; set; }

        public DateTime ItemRunningSince { get; set; }

        public string ItemKey { get; set; }

        public event EventHandler Completed;

        public event EventHandler RemoveItem;

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            UnwireEvents();
        }

        #endregion

        SocketClientEventHandler _UnableToConnect = null;
        SocketClientDataReceivedEventHandler _DataReceived = null;
        SocketClientEventHandler _Disconnected = null;
        SocketClientDataSendFailedEventHandler _SendFailed = null;

        private void WireEvents()
        {
            if (_UnableToConnect == null) _UnableToConnect = new SocketClientEventHandler(IBTGSocketClientNew_UnableToConnect);
            if (_DataReceived == null) _DataReceived = new SocketClientDataReceivedEventHandler(IBTGSocketClientNew_DataReceived);
            if (_Disconnected == null) _Disconnected = new SocketClientEventHandler(IBTGSocketClientNew_Disconnected);
            if (_SendFailed == null) _SendFailed = new SocketClientDataSendFailedEventHandler(IBTGSocketClientNew_SendFailed);
            base.UnableToConnect += _UnableToConnect;
            base.DataReceived += _DataReceived;
            base.Disconnected += _Disconnected;
            base.SendFailed += _SendFailed;
        }

        private void UnwireEvents()
        {
            if (_UnableToConnect != null) base.UnableToConnect -= _UnableToConnect;
            if (_DataReceived != null) base.DataReceived -= _DataReceived;
            if (_Disconnected != null) base.Disconnected -= _Disconnected;
            if (_SendFailed != null) base.SendFailed -= _SendFailed;
        }

        void IBTGSocketClientNew_SendFailed(object sender, SocketClientDataSendFailedEventArgs e)
        {
            if (RemoveItem != null)
                RemoveItem(this, new EventArgs());
        }

        void IBTGSocketClientNew_Disconnected(object sender, SocketClientEventArgs e)
        {
            if (RemoveItem != null)
                RemoveItem(this, new EventArgs());
        }

        void IBTGSocketClientNew_DataReceived(object sender, SocketClientDataReceivedEventArgs e)
        {
            //////thalesData = IBTG.Utilities.Utils.ByteArrayToASCII(Utils.GetSubBytes(e.Message, 2, e.Message.Length - 1));
            ////thalesData = Utils.ByteArrayToASCII(e.Message);
            ////if (Completed != null)
            ////   Completed(this, new EventArgs());
            try
            {
                ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(delegate(object state)
                {
                    CommonConfiguration.HSMType = HSMType.Safenet;
                    if (CommonConfiguration.HSMType == HSMType.Thales)
                    {
                        thalesData = Utils.ByteArrayToASCII(e.Message);
                    }
                    else if (CommonConfiguration.HSMType == HSMType.Safenet)
                    {
                        thalesData = Utils.ByteArrayToHex(e.Message);
                    }

                    if (Completed != null)
                        Completed(this, new EventArgs());
                }));
            }
            catch (Exception ex)
            { HsmLogger.WriteTransLog(null, "Error Occured On DataReceive : " + ex.Message.ToString() + ex.StackTrace.ToString() + ex.Source.ToString()); }
        }

        void IBTGSocketClientNew_UnableToConnect(object sender, SocketClientEventArgs e)
        {
            if (RemoveItem != null)
                RemoveItem(this, new EventArgs());
        }

        string thalesData = "";

        public string SendFunctionCommand(byte[] buffer)
        {
            thalesData = "";
            try
            {
                byte[] MsgToSend = null;
                CommonConfiguration.HSMType = HSMType.Safenet;
                if (CommonConfiguration.HSMType == HSMType.Thales)
                {
                    MsgToSend = new byte[buffer.GetLength(0) + 2];
                    MsgToSend[0] = Convert.ToByte(buffer.GetLength(0) / 256);
                    MsgToSend[1] = Convert.ToByte(buffer.GetLength(0) % 256);
                    Array.Copy(buffer, 0, MsgToSend, 2, buffer.GetLength(0));
                    buffer = null;
                }
                else
                { MsgToSend = buffer; }

                Client.Send(MsgToSend);

                if (!CommonConfiguration.ConfigHID)
                    HsmLogger.WriteTransLog(null, string.Format("Send >> HSM : {0} ", Utils.ByteArrayToASCII(MsgToSend)));
                else
                    HsmLogger.WriteTransLog(null, string.Format("Send >> HSM : {0} ", ""));

                MsgToSend = null;
                double Counter = 0;

                while (string.IsNullOrEmpty(thalesData) && Client.Connected
                    && Convert.ToInt64(HSMResponseTime) > Counter)
                {
                    System.Threading.Thread.Sleep(1);
                    Counter++;
                }

                if (string.IsNullOrEmpty(thalesData) && Client.Connected)
                {
                    if (Completed != null)
                        Completed(this, new EventArgs());
                    HsmLogger.WriteTransLog(null, "Data IsNullOrEmpty " + thalesData);
                }
                HsmLogger.WriteTransLog(null, "Data IsNullOrEmpty " + thalesData + " Counter : " + Counter);
            }
            catch (Exception ex)
            {
                HsmLogger.WriteTransLog(null, "Error occurd while initializing HSM Client : " + ex.Message);
            }
            return thalesData;
        }
    }

    public interface iMIIPLHSMConnMgr
    {
        string HSMIP { get; set; }
        int HSMPort { get; set; }
        event SocketClientHandler DataReceived;
        event SocketClientHandler SendFailed;
        void Init();
        void Send(string pID, string key, byte[] rawMsg, decimal HSMResponseDelay);
    }

    #region IBTGSocketClient

    public class IBTGSocketClient : SocketClient, iMIIPLResourceItem
    {
        public IBTGSocketClient()
            : base()
        {
            Init();
        }
        public IBTGSocketClient(SocketClientLocationEnum pLocation)
            : base(pLocation)
        {
            Init();
        }
        private void Init()
        {
            RetryToConnect = true;
            WireEvents();
        }
        public string CID { get; set; }
        public string Key { get; set; }
        public byte[] MsgToSend { get; set; }
        public decimal HostResponseDelay { get; set; }

        #region iMIIPLResourceItem Members

        public bool IsDirty { get; set; }

        public DateTime ItemRunningSince { get; set; }

        public string ItemKey { get; set; }

        public event EventHandler Completed;

        public event EventHandler RemoveItem;

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            UnwireEvents();
        }

        #endregion

        SocketClientEventHandler _UnableToConnect = null;
        SocketClientDataReceivedEventHandler _DataReceived = null;
        SocketClientEventHandler _Disconnected = null;
        SocketClientDataSendFailedEventHandler _SendFailed = null;

        private void WireEvents()
        {
            if (_UnableToConnect == null) _UnableToConnect = new SocketClientEventHandler(IBTGSocketClientNew_UnableToConnect);
            if (_DataReceived == null) _DataReceived = new SocketClientDataReceivedEventHandler(IBTGSocketClientNew_DataReceived);
            if (_Disconnected == null) _Disconnected = new SocketClientEventHandler(IBTGSocketClientNew_Disconnected);
            if (_SendFailed == null) _SendFailed = new SocketClientDataSendFailedEventHandler(IBTGSocketClientNew_SendFailed);

            base.UnableToConnect += _UnableToConnect;
            base.DataReceived += _DataReceived;
            base.Disconnected += _Disconnected;
            base.SendFailed += _SendFailed;
        }
        private void UnwireEvents()
        {
            if (_UnableToConnect != null) base.UnableToConnect -= _UnableToConnect;
            if (_DataReceived != null) base.DataReceived -= _DataReceived;
            if (_Disconnected != null) base.Disconnected -= _Disconnected;
            if (_SendFailed != null) base.SendFailed -= _SendFailed;
        }

        void IBTGSocketClientNew_SendFailed(object sender, SocketClientDataSendFailedEventArgs e)
        {
            if (RemoveItem != null)
                RemoveItem(this, new EventArgs());
        }

        void IBTGSocketClientNew_Disconnected(object sender, SocketClientEventArgs e)
        {
            if (RemoveItem != null)
                RemoveItem(this, new EventArgs());
        }

        void IBTGSocketClientNew_DataReceived(object sender, SocketClientDataReceivedEventArgs e)
        {
            if (Completed != null)
                Completed(this, new EventArgs());
        }

        void IBTGSocketClientNew_UnableToConnect(object sender, SocketClientEventArgs e)
        {
            if (RemoveItem != null)
                RemoveItem(this, new EventArgs());
        }
    }

    #endregion

    public class MIIPLHSMConnMgr : iMIIPLHSMConnMgr
    {
        private MIIPLResourceMgr resourceMgr = null;

        public MIIPLResourceMgr ResourceMgr
        {
            get
            {
                if (resourceMgr == null)
                {
                    resourceMgr = new MIIPLResourceMgr();
                    resourceMgr.CreateItem += new EventHandler<MIIPLResourceEventArgs>(resourceMgr_CreateItem);
                }

                return resourceMgr;
            }
            set { resourceMgr = value; }
        }

        void resourceMgr_CreateItem(object sender, MIIPLResourceEventArgs e)
        {
            HSMSocketClient client = new HSMSocketClient(SocketClientLocationEnum.ClientSide);
            client.RemoteHostIP = HSMIP;
            client.RemoteHostPort = HSMPort;
            client.DataReceived += new SocketClientDataReceivedEventHandler(client_DataReceived);
            client.Disconnected += new SocketClientEventHandler(client_Disconnected);
            client.Connected += new SocketClientEventHandler(client_Connected);
            client.SendComplete += new SocketClientEventHandler(client_SendComplete);
            client.SendFailed += new SocketClientDataSendFailedEventHandler(client_SendFailed);
            client.SendHandShakeMsg += new SocketClientEventHandler(client_SendHandShakeMsg);
            client.UnableToConnect += new SocketClientEventHandler(client_UnableToConnect);
            client.StartClient();
            e.Item = client;
        }

        public MIIPLHSMConnMgr()
        {
            if (resourceMgr == null)
            {
                resourceMgr = new MIIPLResourceMgr();
                resourceMgr.CreateItem += new EventHandler<MIIPLResourceEventArgs>(resourceMgr_CreateItem);
                resourceMgr.CreateItems();
            }
        }

        public void Init()
        {
        }

        private SocketClient client = null;

        public SocketClient HSMSocketClient
        {
            get
            {
                if (client == null)
                {

                    client = new SocketClient(SocketClientLocationEnum.ClientSide);
                    client.RemoteHostIP = HSMIP;
                    client.RemoteHostPort = Convert.ToInt32(HSMPort);
                    client.Connected += new SocketClientEventHandler(client_Connected);
                    client.DataReceived += new SocketClientDataReceivedEventHandler(client_DataReceived);
                    client.Disconnected += new SocketClientEventHandler(client_Disconnected);
                    client.SendComplete += new SocketClientEventHandler(client_SendComplete);
                    client.SendFailed += new SocketClientDataSendFailedEventHandler(client_SendFailed);
                    client.SendHandShakeMsg += new SocketClientEventHandler(client_SendHandShakeMsg);
                    ////client.Logger = this.Logger;
                    client.RetryToConnect = true;
                    client.MaxWaitingDelay = 15;
                    client.StartClient();
                }
                return client;
            }
            set { client = value; }
        }

        #region iMIIPLHSMConnectionManager Members

        public string HSMIP { get; set; }

        public int HSMPort { get; set; }

        public event SocketClientHandler DataReceived;
        public event SocketClientHandler SendFailed;

        public string SendFunctionCommand_old(string pID, string key, byte[] rawMsg, decimal HSMResponseDelay)
        {
            string result = string.Empty;
            try
            {
                HSMSocketClient client = (HSMSocketClient)ResourceMgr.GetItem();
                if (client == null)
                {
                    ////OnSendFailed(pID, key, rawMsg);
                    HsmLogger.WriteTransLog(null, "No Resource To Process The Request....");
                    return result;
                }

                ////client.Logger = this.Logger;
                client.RemoteHostIP = HSMIP;
                client.RemoteHostPort = HSMPort;
                client.CID = pID;
                client.Key = key;
                client.MsgToSend = rawMsg;
                client.HSMResponseDelay = HSMResponseDelay;

                HsmLogger.WriteTransLog(null, "Before Sending to HSM : Step 2 : Client Created for HSM");
                HsmLogger.WriteTransLog(null, string.Format("client.IsRunning  {0}", client.IsRunning.ToString()));

                if (!client.IsRunning)
                    Thread.Sleep(1000);

                if (client.IsRunning)
                    result = InternalSend(client);
                else
                    OnSendFailed(pID, key, rawMsg);
            }
            catch (Exception ex)
            {
                HsmLogger.WriteTransLog(null, "Error occurd while initializing HSM Client : " + ex.Message.ToString());
            }
            return result;
        }

        public string SendFunctionCommand(string pID, string key, byte[] rawMsg, decimal HSMResponseDelay)
        {
            string result = string.Empty;
            try
            {
                HSMSocketClient client = new HSMSocketClient(SocketClientLocationEnum.ClientSide);
                HsmLogger.WriteTransLog(null, "HSMIP...." + HSMIP);
                HsmLogger.WriteTransLog(null, "HSMPort...." + HSMPort);
                client.RemoteHostIP = HSMIP;
                client.RemoteHostPort = HSMPort;
                client.CID = pID;
                client.Key = key;
                client.MsgToSend = rawMsg;
                client.HSMResponseDelay = HSMResponseDelay;

                HsmLogger.WriteTransLog(null, "Before Sending to HSM : Step 2 : Client Created for HSM");
                HsmLogger.WriteTransLog(null, string.Format("client.IsRunning  {0}", client.IsRunning.ToString()));
                 result = InternalSend(client);
            }
            catch (Exception ex)
            {
                HsmLogger.WriteTransLog(null, "Error occurd while initializing HSM Client : " + ex.Message.ToString());
            }
            return result;
        }

        object LockThalesData = new object();

        object LockIsHSMSendingSuccessfull = new object();

        private string thalesData = string.Empty;

        public string ThalesData
        {
            get { lock (LockThalesData) { return thalesData; } }
            set { lock (LockThalesData) { thalesData = value; } }
        }

        private bool isHSMSendingSuccessfull = true;

        public bool IsHSMSendingSuccessfull
        {
            get { lock (LockIsHSMSendingSuccessfull) { return isHSMSendingSuccessfull; } }
            set { lock (LockIsHSMSendingSuccessfull) { isHSMSendingSuccessfull = value; } }
        }

        public string SendFunctionCommand(byte[] buffer)
        {
            ThalesData = "";
            IsHSMSendingSuccessfull = true;
            try
            {
                byte[] b = new byte[buffer.GetLength(0) + 2];
                b[0] = Convert.ToByte(buffer.GetLength(0) / 256);
                b[1] = Convert.ToByte(buffer.GetLength(0) % 256);
                Array.Copy(buffer, 0, b, 2, buffer.GetLength(0));
                buffer = null;
                HSMSocketClient.Send(b);
                b = null;
                double Counter = 0;
                while (string.IsNullOrEmpty(thalesData) && HSMSocketClient.Client.Connected
                    && Convert.ToInt64(HSMSocketClient.MaxWaitingDelay * 1000) > Counter && IsHSMSendingSuccessfull)
                {
                    System.Threading.Thread.Sleep(1);
                    Counter++;
                }
            }
            catch (Exception ex)
            {
                HsmLogger.WriteTransLog(null, "Error occurd while waiting for HSM data : " + ex.Message);
            }
            if ((!string.IsNullOrEmpty(ThalesData)))
                if (ThalesData.Length > 2)
                    return ThalesData.Substring(2);

            return ThalesData;
        }

        #endregion

        public void OnDataReceived(string pID, string key, byte[] rawMsg)
        {
            try
            {
                CommonConfiguration.HSMType = HSMType.Safenet;
                switch (CommonConfiguration.HSMType)
                {
                    case HSMType.Thales:
                        ThalesData = Utils.ByteArrayToASCII(rawMsg);
                        if (!CommonConfiguration.ConfigHID)
                            HsmLogger.WriteTransLog(null, string.Format("Received << HSM : {0} : {1}", HSMIP, ThalesData));
                        else
                        {
                            string Successful = ThalesData.Substring(thalesData.Length - 4);
                            int length = ThalesData.Length;
                            if (Successful == "EB02" || Successful == "EB00" || Successful == "CZ00" || Successful == "EB01" || Successful == "CZ01")
                            {
                                HsmLogger.WriteTransLog(null, string.Format("Received << HSM : {0} : {1}", HSMIP, ThalesData));
                            }
                            else if (length > 65)
                            {
                                string data = thalesData.Substring(47, 4);
                                string mask = thalesData.Substring(47);
                                data = data + new string('X', mask.Length - 4) + mask.Substring((mask.Length - 4));
                                HsmLogger.WriteTransLog(null, string.Format("Received << HSM : {0} : {1}", HSMIP, data));
                            }
                            else
                            {
                                string EncryptedValue = ThalesData.Substring(0, 2) + new string('X', ThalesData.Length - 4) + ThalesData.Substring((ThalesData.Length - 4));
                                HsmLogger.WriteTransLog(null, string.Format("Received << HSM : {0} : {1}", HSMIP, EncryptedValue));
                            }
                        }
                        break;

                    case HSMType.Safenet:
                        ThalesData = Utils.ByteArrayToHex(rawMsg);
                        if (!CommonConfiguration.ConfigHID)
                            HsmLogger.WriteTransLog(null, string.Format("Received << HSM : {0} : {1}", HSMIP, ThalesData));
                        else
                        {
                            if (ThalesData.Substring(CommonConfiguration.HsmWhiteSpace, CommonConfiguration.HSMHeaderLength).ToUpper() == SafenetHSMFunctionCode.PINTranslation
                                && ThalesData.Substring(CommonConfiguration.HsmWhiteSpace + CommonConfiguration.HSMHeaderLength, 2) == "00")
                                HsmLogger.WriteTransLog(null, "Received << HSM : " + new string('X', ThalesData.Length - 4) + ThalesData.Substring((ThalesData.Length - 4)));
                            else
                                HsmLogger.WriteTransLog(null, "Received << HSM : " + ThalesData);
                        }
                        break;
                }
            }
            catch (Exception ex)
            { HsmLogger.WriteTransLog(null, "Exception Occured at On-DataReceived " + ex.Source + ex.Message + ex.StackTrace); }

            if (this.DataReceived != null)
                this.DataReceived(pID, key, rawMsg);
            ////HSMSocketClient.StopClient();
        }

        public void OnSendFailed(string pID, string key, byte[] rawMsg)
        {
            if (!CommonConfiguration.ConfigHID)
                HsmLogger.WriteTransLog(null, string.Format("Send failed >> HSM : {0} : {1}", HSMIP, Utils.ByteArrayToASCII(rawMsg)));
            else
                HsmLogger.WriteTransLog(null, string.Format("Send failed >> HSM : {0} : {1}", HSMIP, ""));
            IsHSMSendingSuccessfull = false;
            if (this.SendFailed != null)
                this.SendFailed(pID, key, rawMsg);
        }

        private string InternalSend(object sender)
        {
            string result = string.Empty;
            try
            {

                HsmLogger.WriteTransLog(null, "Before Sending to HSM : Step 3 : Client connected to HSM");
                HSMSocketClient client = (HSMSocketClient)sender;
                if (client.MsgToSend != null && client.MsgToSend.Length > 0)// != "")
                {
                    result = client.SendFunctionCommand(client.MsgToSend);
                }
                else
                    HsmLogger.WriteTransLog(null, "Before Sending to HSM : Step 4 : MsgToSend is null or length is zero");
            }
            catch (Exception ex)
            {
                HsmLogger.WriteTransLog(null, "Error occurd sending message to HSM : " + ex.Message);
            }
            return result;
        }

        void client_UnableToConnect(object sender, SocketClientEventArgs e)
        {
            try
            {
                HsmLogger.WriteTransLog(null, "Unable to connect HSM  to HSM : " + HSMIP);
                HSMSocketClient client = (HSMSocketClient)sender;
                client.StopClient();
                HsmLogger.WriteTransLog(null, "Unable to connect HSM, from : " + client.CID + " to HSM : " + e.ID);
                if (client.MsgToSend != null)
                {
                    OnSendFailed(client.CID, client.Key, client.MsgToSend);
                }
                client = null;
            }
            catch (Exception ex)
            {
                HsmLogger.WriteTransLog(null, "Error occurd while initializing HSM Client : " + ex.Message);
            }
        }
        void client_DataReceived(object sender, SocketClientDataReceivedEventArgs e)
        {
            try
            {
                OnDataReceived(HSMIP.ToString(), HSMPort.ToString(), e.Message);
            }
            catch (Exception ex)
            {
                HsmLogger.WriteTransLog(null, "Error occurd while data received from HSM : " + ex.Message);
            }
        }
        void client_Connected(object sender, SocketClientEventArgs e)
        {
            try
            {
                HsmLogger.WriteTransLog(null, "HSM Connected to :IP  " + HSMIP + " Port : " + HSMPort);
            }
            catch (Exception ex)
            {
                HsmLogger.WriteTransLog(null, "Error occurd while initializing HSM Client : " + ex.Message);
            }
        }
        void client_Disconnected(object sender, SocketClientEventArgs e)
        {
            try
            {
                HSMSocketClient client = (HSMSocketClient)sender;
                HsmLogger.WriteTransLog(null, "HSM Client : " + e.ID + " disconnected");
                client.StopClient();
                client = null;
            }
            catch (Exception ex)
            {
                HsmLogger.WriteTransLog(null, "Error occurd while initializing HSM Client : " + ex.Message);
            }
        }
        void client_SendComplete(object sender, SocketClientEventArgs e)
        {
            try
            {
                HsmLogger.WriteTransLog(null, "Message send completed");
            }
            catch (Exception ex)
            {
                HsmLogger.WriteTransLog(null, "Error occurd while initializing HSM Client : " + ex.Message);
            }
        }
        void client_SendFailed(object sender, SocketClientDataSendFailedEventArgs e)
        {
            try
            {
                HsmLogger.WriteTransLog(null, "Message send failed , to : " + HSMIP);

            }
            catch (Exception ex)
            {
                HsmLogger.WriteTransLog(null, "Error occurd while initializing HSM Client : " + ex.Message);
            }


        }
        void client_SendHandShakeMsg(object sender, SocketClientEventArgs e)
        {
            try
            {
                //
            }
            catch (Exception ex)
            {
                HsmLogger.WriteTransLog(null, "Error occurd while initializing HSM Client : " + ex.Message);
            }
        }
        public void Send(string pID, string key, byte[] rawMsg, decimal HSMResponseDelay)
        {

        }
    }
}
