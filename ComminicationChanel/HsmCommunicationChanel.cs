using MaxiSwitch.Common.TerminalLogger;
using MaxiSwitch.DALC.Configuration;
using MaxiSwitch.HSMManager.HsmInterface;
using MaxiSwitch.Logger;
using MIIPL.Common;
using MIIPL.Sockets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HSMCommunicationChanel
{
    public delegate Authentication SendAuthenticationRequest(ref Authentication RequestMsg, enumSecurityModuleType SecurityModuleType);

    public class HsmCommunicationChanel
    {
        ProcessAuthonticationRequest _ProcessAuthonticationRequest = new ProcessAuthonticationRequest();

        private static ConcurrentDictionary<string, Authentication> _dictMessagingHsm = null;

        public static ConcurrentDictionary<string, Authentication> DictMessagingHsm
        {
            get
            {
                if (_dictMessagingHsm == null)
                {
                    try
                    {
                        _dictMessagingHsm = new ConcurrentDictionary<string, Authentication>();
                    }
                    catch (Exception ex)
                    { HsmLogger.WriteErrorLog(null, ex); }
                }
                return _dictMessagingHsm;
            }
            set { _dictMessagingHsm = value; }
        }

        public SendAuthenticationRequest SendAuthRequest { get; set; }
        public SystemLogger SystemLogger = null;
        //public SystemLogger_HSM SystemLogger_HSM = null;
        public string HsmResponse = string.Empty;
        public HSMCommunication _hsmCommander = null;

        #region NEW HSM CODE

        public static InterBankClientConnMngr _InterBankClientConnManager = null;
        public InterBankClientConnMngr InterBankClientConnManager
        {
            get
            {
                if (_InterBankClientConnManager == null)
                {
                    HsmLogger.WriteTransLog(null, "Object Creation Started for interBankClientConnMngr...");
                    try
                    {
                        _InterBankClientConnManager = new InterBankClientConnMngr();
                        _InterBankClientConnManager.RemoteHostIP = CONFIGURATIONCONFIGDATA.HSMIP.ToString();
                        _InterBankClientConnManager.RemoteHostPort = CONFIGURATIONCONFIGDATA.HSMPORT;
                        _InterBankClientConnManager.MaxConnections = 1;
                        _InterBankClientConnManager.RetryToConnect = true;
                        _InterBankClientConnManager.DataReceived += new SocketClientDataReceivedEventHandler(Client_DataReceived);
                        _InterBankClientConnManager.Connected += new SocketClientEventHandler(Client_Connected);
                        _InterBankClientConnManager.Disconnected += new SocketClientEventHandler(Client_Disconnected);
                        _InterBankClientConnManager.SendComplete += new SocketClientEventHandler(Client_SendComplete);
                        _InterBankClientConnManager.SendFailed += new SocketClientDataSendFailedEventHandler(Client_SendFailed);
                        _InterBankClientConnManager.UnableToConnect += new SocketClientEventHandler(Client_UnableToConnect);
                        _InterBankClientConnManager.SendHandShakeMsg += new SocketClientEventHandler(Client_SendHandShakeMsg);
                        _InterBankClientConnManager.Init();
                    }
                    catch (Exception ex)
                    { HsmLogger.WriteErrorLog("", ex); }
                }
                return _InterBankClientConnManager;
            }
            set { _InterBankClientConnManager = value; }
        }
        public static int NetWorkMsgCount = 0;
        public string ReceivedMessage = string.Empty;
        void Client_UnableToConnect(object sender, SocketClientEventArgs e)
        {
            this.InterBankClientConnManager.RetryToConnect = true;
        }

        async void Client_DataReceived(object sender, SocketClientDataReceivedEventArgs e)
        {
            await Task.Run(() =>
            {
                ReceivedMessage = Utils.ASCII2HEX(Utils.ByteArrayToASCII(e.Message));

                try
                {
                    string str = Utils.ASCII2HEX(Utils.ByteArrayToASCII(e.Message));
                    string[] Processmsg = Process_MultiRequest.MultiRequest(str);

                    HsmLogger.WriteTransLog(this, string.Format("Array Length of Message Recieved : - " + Processmsg.Length));

                    foreach (string DataToProcess in Processmsg)
                    {
                        byte[] message = null;
                        message = Utils.ASCIIToByteArray(Utils.HEX2ASCII(DataToProcess));

                        if (!CommonConfiguration.ConfigHID)
                            HsmLogger.WriteTransLog(this, string.Format("Message Received From HSM : {0} : {1}", e.ID, DataToProcess.ToString()));
                        else
                            HsmLogger.WriteTransLog(this, string.Format("Message Received From HSM : {0} : {1}", e.ID, ""));

                        if (DataToProcess.Length > 1)
                        {
                            InitialiseDataProcessor(message);
                        }
                    }
                }
                catch (Exception ex)
                { HsmLogger.WriteErrorLog(this, ex); }
            });
        }

        void Client_SendComplete(object sender, SocketClientEventArgs e)
        {
            HsmLogger.WriteTransLog(this, "Data Send Completed To Client : " + e.ID.ToString());
        }

        void Client_SendFailed(object sender, SocketClientDataSendFailedEventArgs e)
        {
            try
            {
                HsmLogger.WriteTransLog(this, "Data Sending Failed To Client : " + e.ID);
                ////ReceivedMessage = string.Empty;
                ////_nfsDataProcessor.ProcessSendingFailToClientResponseMessage(e.Message);
                ////*********Need To Implement Code For Removal of All key from the Dictionary (As New Connection is Getting Created)***************
                InterBankClientConnManager.CloseAllConnections();
                NetWorkMsgCount = 0;
                System.Threading.Thread.Sleep(1000);
                InterBankClientConnManager.Init();
            }
            catch (Exception ex)
            { HsmLogger.WriteErrorLog(this, ex); }
        }

        void Client_Disconnected(object sender, SocketClientEventArgs e)
        {
            try
            {
                HsmLogger.WriteTransLog(this, "Client Disconnected : " + e.ID.ToString());
                ReceivedMessage = string.Empty;
                ////*********Need To Implement Code For Removal of All key from the Dictionary (As New Connection is Getting Created)***************
                InterBankClientConnManager.CloseAllConnections();
                NetWorkMsgCount = 0;
                System.Threading.Thread.Sleep(1000);
                InterBankClientConnManager.Init();
            }
            catch (Exception ex)
            { HsmLogger.WriteErrorLog(this, ex); }
        }

        void Client_Connected(object sender, SocketClientEventArgs e)
        {
            try
            {
                HsmLogger.WriteTransLog(this, "Client connected : " + e.ID.ToString());
                //GetNetworkMessage(NetworkMsgType.Login);
            }
            catch (Exception ex)
            { HsmLogger.WriteErrorLog(this, ex); }
        }

        void Client_SendHandShakeMsg(object sender, SocketClientEventArgs e)
        {
        }

        public void SendMessage(byte[] SendingMessage)
        {
            _InterBankClientConnManager.Send(SendingMessage);
        }

        void InitialiseDataProcessor(byte[] RawMessage)
        {
            Authentication ReqMessage = new Authentication();
            try
            {
                string str = Utils.ASCII2HEX(Utils.ByteArrayToASCII(RawMessage));
                string ResponseKey = str.Substring(0, 8);
                HsmLogger.WriteTransLog(this, "Response Key : " + ResponseKey.ToUpper());

                if (DictMessagingHsm.TryGetValue(ResponseKey.ToUpper(), out ReqMessage))
                {
                    try
                    {
                        ReqMessage.ResponseKey = ResponseKey;
                        ReqMessage.HsmResponseData = str;
                        ReqMessage.ResponseFlag = true;

                        ////**********OverWriting the Existing Value of Object********
                        DictMessagingHsm[ReqMessage.ResponseKey.ToUpper().ToString()] = ReqMessage;

                        HsmLogger.WriteTransLog(this, "Updated Object Added For Key : " + ReqMessage.ResponseKey.ToUpper().ToString());
                    }
                    catch (Exception ex)
                    {
                        HsmLogger.WriteTransLog(this, "Error In InitialiseDataProcessor > Update Object...");
                        HsmLogger.WriteErrorLog(this, ex);
                    }
                }
                else
                {
                    HsmLogger.WriteTransLog(this, "Key Does not Exists : " + ResponseKey.ToUpper());
                }
            }
            catch (Exception ex)
            {
                HsmLogger.WriteTransLog(this, "Error In InitialiseDataProcessor...");
                HsmLogger.WriteErrorLog(this, ex);
            }
        }

        public class InterBankClientConnMngr
        {
            public InterBankClientConnMngr()
            {
                this.RetryToConnect = true;
            }

            public void Init()
            {
                for (int i = 0; i < MaxConnections; i++)
                    CreateItem();
            }

            #region Client Events

            public event SocketClientEventHandler Connected = null;
            public event SocketClientEventHandler Disconnected = null;
            public event SocketClientDataReceivedEventHandler DataReceived = null;
            public event SocketClientEventHandler SendComplete = null;
            public event SocketClientDataSendFailedEventHandler SendFailed = null;
            public event SocketClientEventHandler SendHandShakeMsg = null;
            public event SocketClientEventHandler UnableToConnect = null;
            #endregion

            public string RemoteHostIP { get; set; }
            public int RemoteHostPort { get; set; }
            public int MaxConnections { get; set; }
            public bool RetryToConnect { get; set; }

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

            private List<SocketClient> clientList = null;
            private readonly object connectedSocketsSyncHandle = new object();
            private delegate void SendDelegate(byte[] data);
            private SocketClientEventHandler client_disconnected = null;

            private void WireEvents(SocketClient state)
            {

                //state.Logger = this.Logger;
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

            public bool RemoveClient(string pID)
            {
                try
                {
                    SocketClient state = ClientList.Find(m => m.ID == pID);
                    if (state != null)
                    {
                        UnwireEvents(state);
                        state.StopClient();
                        lock (connectedSocketsSyncHandle)
                        {
                            HsmLogger.WriteTransLog(this, string.Format("Existing Client {0} ({1}), disconnected", pID, state.Location));
                            ClientList.Remove(state);
                        }
                        state = null;
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    HsmLogger.WriteTransLog(this, string.Format("Exception occured in RemoveClient : {0}", ex.Message));
                    return false;
                }
            }

            public void CloseAllConnections()
            {
                try
                {
                    int i = 0;
                    string ClientID = string.Empty;
                    while (i < this.ClientList.Count)
                    {
                        ClientID = this.ClientList[i].ID;
                        if (!RemoveClient(ClientID))
                            i++;
                        HsmLogger.WriteTransLog(this, "Client disconnected ID:" + ClientID);
                    }
                }
                catch (Exception ex)
                {
                    HsmLogger.WriteErrorLog(this, ex);
                    throw ex;
                }
            }

            public void CreateItem()
            {
                SocketClient client = new SocketClient(SocketClientLocationEnum.ClientSide);
                client.RemoteHostIP = this.RemoteHostIP;
                client.RemoteHostPort = this.RemoteHostPort;
                client.RetryToConnect = this.RetryToConnect;
                WireEvents(client);
                //client.IDType = SocketClient.IDTypeEnum.Local;
                ////*******Modified 241115
                //UnwireEvents(client);
                ClientList.Add(client);
                client.StartClient();
            }

            public void Send_old(string pID, byte[] data)
            {
                //go 
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

            public void Send(byte[] data)
            {
                SocketClient state = null;
                lock (connectedSocketsSyncHandle)
                {
                    state = ClientList[0];
                    HsmLogger.WriteTransLog(null, "Inside Final Send Function (Getting Socket) " + state.ToString() + "\tSending ID : " + state.ID);
                }
                if (state != null)
                {
                    HsmLogger.WriteTransLog(null, "Sending Final Message (Length) :- " + data.Length + "\tSending ID : " + state.ID);
                    SendDelegate sd = new SendDelegate(state.Send);
                    sd.BeginInvoke(data, null, null);
                }
            }

            public bool IsConnected()
            {
                return ((ClientList.Count > 0) ? true : false);
            }

            public string GetResponse(string key)
            {
                Authentication _Authentication = new Authentication();
                string Response = string.Empty;
                try
                {
                    int count = 0;
                    do
                    {
                        System.Threading.Thread.Sleep(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SleepCount"]));
                        _Authentication = DictMessagingHsm[key.ToUpper()];
                        count++;
                    }
                    while (count <= Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["WaitLoopCount"]) && _Authentication.ResponseFlag != true);
                    ////*****************Re-asigning CommandData value To Object*****************
                    try
                    {
                        DictMessagingHsm.TryRemove(key.ToUpper(), out _Authentication);
                    }
                    catch (Exception ex)
                    {
                        HsmLogger.WriteErrorLog(null, ex);
                    }

                    if (_Authentication.ResponseFlag != true)
                    {
                        //_reqCmdData.ResponseCode = "504";
                        //_reqCmdData.NResponseCode = "504";
                        //_reqCmdData.ResponseKey = key;
                        Response = string.Empty;
                    }
                    else
                    {
                        Response = _Authentication.HsmResponseData;
                    }

                    return Response;
                }
                catch (Exception ex)
                {
                    HsmLogger.WriteErrorLog(this, ex);
                    return Response;
                }
            }

        }

        public static class Process_MultiRequest
        {
            public static string[] MultiRequest(string Message)
            {
                string[] TagValueWithAscii1 = new string[100];
                //string[] TagValueWithAscii = new string[100];
                List<int> TagIndex = GetTagPosition(Message.ToUpper(), "EE0701");
                List<int> TagIndex1 = GetTagPosition(Message.ToUpper(), "EE0702");
                List<int> TagIndex2 = GetTagPosition(Message.ToUpper(), "EE0602");
                List<int> TagIndex3 = GetTagPosition(Message.ToUpper(), "EE0801");

                TagIndex.AddRange(TagIndex1);
                TagIndex.AddRange(TagIndex2);
                TagIndex.AddRange(TagIndex3);

                TagIndex.Sort();
                string[] RequestProcessData = new string[100];
                string[] RequestProcessFinalData = new string[100];
                int L = 0;
                try
                {
                    for (int i = 0; i < TagIndex.Count; i++)
                    {
                        TagValueWithAscii1[i] = Message.Substring(TagIndex[i] - 12, 12);
                    }
                }
                catch { }
                //try
                //{
                //    for (int i = 0; i < TagIndex.Count; i++)
                //    {
                //        if (Regex.IsMatch(TagValueWithAscii1[i].Substring(0, 1), @"^[a-zA-Z]+$") || Regex.IsMatch(TagValueWithAscii1[i].Substring(0, 1), @"^-?\d+$"))
                //        { }
                //        else
                //        {
                //            TagValueWithAscii[L] = TagValueWithAscii1[i];
                //            L++;
                //        }
                //    }
                //}
                //catch { }
                RequestProcessData = Message.Split(TagValueWithAscii1, StringSplitOptions.None);

                try
                {
                    for (int j = 1; j < RequestProcessData.Length; j++)
                    {
                        if (!RequestProcessData[j].Contains(TagValueWithAscii1[0]))
                        {
                            RequestProcessData[j] = TagValueWithAscii1[j - 1] + RequestProcessData[j];
                        }
                    }
                }
                catch { }
                return RequestProcessData;
            }
            public static List<int> GetTagPosition(string Message, string TagValue)
            {
                List<int> ret = new List<int>();
                int len = TagValue.Length;
                int start = -len;
                while (true)
                {
                    start = Message.IndexOf(TagValue, start + len);
                    if (start == -1)
                    {
                        break;
                    }
                    else
                    {
                        ret.Add(start);
                    }
                }
                return ret;
            }
        }

        public string GetResponse(string key)
        {
            Authentication _Authentication = new Authentication();
            string Response = string.Empty;
            try
            {
                int count = 0;
                do
                {
                    System.Threading.Thread.Sleep(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SleepCount"]));
                    _Authentication = DictMessagingHsm[key.ToUpper()];
                    count++;
                }
                while (count <= Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["WaitLoopCount"]) && _Authentication.ResponseFlag != true);
                ////*****************Re-asigning CommandData value To Object*****************
                try
                {
                    DictMessagingHsm.TryRemove(key.ToUpper(), out _Authentication);
                }
                catch (Exception ex)
                {
                    HsmLogger.WriteErrorLog(null, ex);
                }

                if (_Authentication.ResponseFlag != true)
                {
                    //_reqCmdData.ResponseCode = "504";
                    //_reqCmdData.NResponseCode = "504";
                    //_reqCmdData.ResponseKey = key;
                    Response = string.Empty;
                }
                else
                {
                    Response = _Authentication.HsmResponseData;
                }

                return Response;
            }
            catch (Exception ex)
            {
                HsmLogger.WriteErrorLog(this, ex);
                return Response;
            }
        }




        #endregion NEW HSM CODE

        public HsmCommunicationChanel()
        {
            try
            {
                //SystemLogger_HSM = new SystemLogger_HSM();
                SystemLogger = new SystemLogger();
                _hsmCommander = new HSMCommunication(CONFIGURATIONCONFIGDATA.HSMIP.ToString(), CONFIGURATIONCONFIGDATA.HSMPORT);
            }
            catch { }
        }

        public void GenerateGreenPinFormPAY(ref Authentication ReqMessage)
        {
            try
            {
                SystemLogger.WriteTransLog(this, "ATM Green Pin Generation Started for reference number : " + ReqMessage.TransactionRefrenceNumber);
                HsmResponse = string.Empty;
                string PackData = string.Empty;
                string PinBlock = string.Empty;
                string NewOffset = string.Empty;
                PackData = string.Empty;



                #region CVV

                ////*******************************Card CVV check************************************************


                //PackData = _ProcessAuthonticationRequest.SafenetPackCommand(SafenetHSMFunctionCode.CVVVerification + SafenetHSMFunctionModifier.FM + ReqMessage.HsmPvk
                //    + ReqMessage.DecryptedCardNo + ReqMessage.CardExp + "000" + "000000000" + ReqMessage.CardCVV + "F");

                //SystemLogger.WriteTransLog(this, "PackData :" + PackData);

                //if (string.IsNullOrEmpty(PackData))
                //{
                //    ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                //    ReqMessage.ATMGreenPinResponse = "Transaction UnSucessful.";
                //    HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                  
                //}  
                //else if (HsmResponse.Substring(CommonConfiguration.HsmWhiteSpace + CommonConfiguration.HSMHeaderLength, 2)  != "00")
                //{

                //    ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                //    ReqMessage.ATMGreenPinResponse = "Do Not Honor, Transaction Unsuccessful.";
                //    HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                //}
                //else if (HsmResponse.Substring(CommonConfiguration.HsmWhiteSpace + CommonConfiguration.HSMHeaderLength, 2) == "00")
                //{
                #endregion CVV

                string AccountNumber = ReqMessage.DecryptedCardNo.Substring(ReqMessage.DecryptedCardNo.Length - 12 - 1, 12);

                    PackData = _ProcessAuthonticationRequest.SafenetPackCommand((SafenetHSMFunctionCode.PINEncryption + SafenetHSMFunctionModifier.FM
                                 + "0402" + Convert.ToString(ReqMessage.NewAtmPin).Trim() + AccountNumber + ReqMessage.HsmZpk));

                    SystemLogger.WriteTransLog(this, "PackData (PINEncryption) Command:" + PackData);

                    HsmResponse = _hsmCommander.ProcessData(Utils.ASCIIToByteArray(Utils.HEX2ASCII(PackData)), "");

                    HsmLogger.WriteTransLog(this, string.Format("Hsm Response Received : {0}", HsmResponse));

                    SystemLogger.WriteTransLog(this, "PackData (PINEncryption) Command Response:" + HsmResponse);

                    ReqMessage.TransactionStatus = enumTransactionStatus.Successful;

                    if (string.IsNullOrEmpty(HsmResponse))
                    {
                        ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                        ReqMessage.ATMGreenPinResponse = "Transaction UnSucessful.";
                        HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));

                    }
                    else if (HsmResponse.Substring(CommonConfiguration.HsmWhiteSpace + CommonConfiguration.HSMHeaderLength, 2) == "00")
                    {
                        HsmLogger.WriteTransLog(this, "PINEncryption command sucessful");
                        HsmLogger.WriteTransLog(this, string.Format("Sucess {0}", HsmResponse.Substring(CommonConfiguration.HsmWhiteSpace + CommonConfiguration.HSMHeaderLength, 2)));

                        PinBlock = HsmResponse.Substring(Convert.ToInt32(CommonConfiguration.HsmWhiteSpace) + Convert.ToInt32(CommonConfiguration.HSMHeaderLength) + 2);

                        HsmLogger.WriteTransLog(this, string.Format("PinBlock After Removing Padding : {0}", "XXXXXXXXXXXX" + PinBlock.Substring(12)));
                        ReqMessage.TransactionStatus = enumTransactionStatus.Successful;
                        ReqMessage.ATMGreenPinResponse = "00";
                        PackData = string.Empty;


                        if (ReqMessage.DecryptedCardNo.Length == 19)
                           
                            PackData = _ProcessAuthonticationRequest.SafenetPackCommand(SafenetHSMFunctionCode.PINOffsetGeneration + SafenetHSMFunctionModifier.FM +
                            PinBlock.ToUpper() + ReqMessage.HsmZpk + SafenetHSMFunctionCode.ANSI + AccountNumber + ReqMessage.HsmPvk + ReqMessage.DecryptedCardNo.Substring(2,16));


                        else if (ReqMessage.DecryptedCardNo.Length == 16)
                        {
                             PackData = _ProcessAuthonticationRequest.SafenetPackCommand(SafenetHSMFunctionCode.PINOffsetGeneration + SafenetHSMFunctionModifier.FM +
                            PinBlock.ToUpper() + ReqMessage.HsmZpk + SafenetHSMFunctionCode.ANSI + AccountNumber + ReqMessage.HsmPvk + ReqMessage.DecryptedCardNo);

                        }

                        //PackData = _ProcessAuthonticationRequest.SafenetPackCommand(SafenetHSMFunctionCode.PINOffsetGeneration + SafenetHSMFunctionModifier.FM +
                        //  PinBlock.ToUpper() + ReqMessage.HsmZpk + SafenetHSMFunctionCode.ANSI + AccountNumber + ReqMessage.HsmPvk + ReqMessage.DecryptedCardNo);


                        SystemLogger.WriteTransLog(this, "PackData (PINOffsetGeneration) Command:" + PackData);

                        HsmResponse = _hsmCommander.ProcessData(Utils.ASCIIToByteArray(Utils.HEX2ASCII(PackData)), "");
                        HsmLogger.WriteTransLog(this, string.Format("Hsm Response Received : {0}", HsmResponse));


                        if (HsmResponse.Substring(Convert.ToInt32(CommonConfiguration.HsmWhiteSpace) + Convert.ToInt32(CommonConfiguration.HSMHeaderLength), 2) == "00")
                        {
                            HsmLogger.WriteTransLog(this, "PINOffsetGeneration command sucessful");
                            NewOffset = HsmResponse.Substring(Convert.ToInt32(CommonConfiguration.HsmWhiteSpace) + Convert.ToInt32(CommonConfiguration.HSMHeaderLength) + 2);
                            NewOffset = NewOffset.Substring(0, 4);
                            ReqMessage.NewOffset = NewOffset;
                            ReqMessage.TransactionStatus = enumTransactionStatus.Successful;
                            ReqMessage.ATMGreenPinResponse = "00";

                          //  HsmLogger.WriteTransLog(this, string.Format("New Offset After Removing Padding : {0}", NewOffset));


                        }
                        else
                        {
                            ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                            ReqMessage.ATMGreenPinResponse = "91";
                            HsmLogger.WriteTransLog(this, string.Format("UpdateOffset :Failed"));
                        }
                    }

                    else
                    {
                        ReqMessage.ATMGreenPinResponse = "91";
                        ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                        HsmLogger.WriteTransLog(this, string.Format("UpdateOffset :Failed"));
                    }

                //}
                //else
                //{
                //    PackData = string.Empty;
                //    ReqMessage.ATMGreenPinResponse = "91";
                //    ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                //    HsmLogger.WriteTransLog(this, string.Format("UpdateOffset :Failed"));
                //}
                
           }

           #region old
                /*

                string pinBlock = Utils.EBCDIC2HEX(ReqMessage.PinBlock);
                string PinChangepinBlock = Utils.EBCDIC2HEX(ReqMessage.PinChangeBlock);
                string pinValData = ReqMessage.Cardnumber.Substring(ReqMessage.Cardnumber.Length - 0x10);
                pinValData = pinValData.Substring(0, 10) + "N" + pinValData.Substring(pinValData.Length - 1);
                string offset = ReqMessage.PinOffset;
                offset = offset.PadRight(12, 'F');
                ////*******************HSM Command Format Packing***********************
                string AccountNumber = ReqMessage.Cardnumber.Substring(3, 12);
                string PackData = string.Empty;
                ////*******************HSM Command Format Packing***********************


                //PackData = _ProcessAuthonticationRequest.SafenetPackCommand(SafenetHSMFunctionCode.PINOffsetGeneration, SafenetHSMFunctionModifier.FM, Utils.EBCDIC2HEX(ReqMessage.PinChangeBlock)
                //               , ReqMessage.HsmComkey, SafenetMessageConst.DieboldPinBlockFormat, AccountNumber, ReqMessage.HsmPvk, ReqMessage.Cardnumber);

                // SystemLogger.WriteTransLog(this, "Request field   : " +"Pin block"+ReqMessage.PinBlock+"Pin chnage block"+ReqMessage.PinChangeBlock+"Hsm comm key"+ReqMessage.HsmComkey+"Account number"+AccountNumber+"HSMPVK"+ReqMessage.HsmPvk+"Card num"+ReqMessage.Cardnumber);


                if (ReqMessage.Cardnumber.Length == 19)
                    PackData = _ProcessAuthonticationRequest.SafenetPackCommand(SafenetHSMFunctionCode.PINOffsetGeneration, SafenetHSMFunctionModifier.FM, Utils.EBCDIC2HEX(ReqMessage.PinChangeBlock), ReqMessage.HsmComkey
                                        , SafenetMessageConst.DieboldPinBlockFormat, AccountNumber, ReqMessage.HsmPvk
                                        , ReqMessage.Cardnumber.Substring(2, 16));

                else if (ReqMessage.Cardnumber.Length == 16)
                {
                    PackData = _ProcessAuthonticationRequest.SafenetPackCommand(SafenetHSMFunctionCode.PINOffsetGeneration, SafenetHSMFunctionModifier.FM, Utils.EBCDIC2HEX(ReqMessage.PinChangeBlock), ReqMessage.HsmComkey
                                        , SafenetMessageConst.DieboldPinBlockFormat, AccountNumber, ReqMessage.HsmPvk
                                        , ReqMessage.Cardnumber);
                }


                if (!CommonConfiguration.ConfigHID)
                    HsmLogger.WriteTransLog(this, string.Format("Data Packed : {0}", PackData));
                else
                    HsmLogger.WriteTransLog(this, string.Format("Data Packed : {0}", new string('X', PackData.Length - 4) + PackData.Substring(PackData.Length - 4, 4)));

                try
                {
                    HsmResponse = _hsmCommander.ProcessData(Utils.ASCIIToByteArray(Utils.HEX2ASCII(PackData)), "");
                    HsmLogger.WriteTransLog(this, string.Format("Hsm Response Received : {0}", HsmResponse));
                }
                catch (Exception ex)
                {
                    HsmResponse = string.Empty;
                    HsmLogger.WriteTransLog(this, ex.Message.ToString() + ex.Source.ToString() + ex.StackTrace.ToString());
                }

                try
                {
                    if (string.IsNullOrEmpty(HsmResponse))
                    {
                        ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                        ReqMessage.VerifyPinChangeResponse = "";
                        HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                    }
                    else if (HsmResponse.Substring(CommonConfiguration.HsmWhiteSpace + CommonConfiguration.HSMHeaderLength, 2) == "00")
                    {
                        ReqMessage.TransactionStatus = enumTransactionStatus.Successful;
                        ReqMessage.VerifyPinChangeResponse = HsmResponse.Substring(CommonConfiguration.HsmWhiteSpace + CommonConfiguration.HSMHeaderLength);
                        HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                        HsmLogger.WriteTransLog(this, string.Format("DrukPay Pin Change Response(S) : {0}", ReqMessage.VerifyPinChangeResponse));
                        ReqMessage.NewOffset = HsmResponse.Substring(CommonConfiguration.HsmWhiteSpace + CommonConfiguration.HSMHeaderLength + 2);
                        HsmLogger.WriteTransLog(this, string.Format("DrukPay New Offset : {0}", ReqMessage.NewOffset));
                        ReqMessage.NewOffset = ReqMessage.NewOffset.Substring(0, 4);
                        HsmLogger.WriteTransLog(this, string.Format("DrukPay New Offset After Removing Padding : {0}", ReqMessage.NewOffset));
                    }
                    else
                    {
                        ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                        ReqMessage.VerifyPinChangeResponse = HsmResponse.Substring(CommonConfiguration.HsmWhiteSpace + CommonConfiguration.HSMHeaderLength, 2);
                        HsmLogger.WriteTransLog(this, string.Format("DrukPay Pin Change Response(U) : {0}", ReqMessage.VerifyPinChangeResponse));
                        HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                    ReqMessage.VerifyPinChangeResponse = string.Empty;
                    HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                    HsmLogger.WriteTransLog(this, ex.Message.ToString() + ex.Source.ToString() + ex.StackTrace.ToString());
                } 
                 */

                #endregion old
           
            catch (Exception ex)
            {
                ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                ReqMessage.ATMGreenPinResponse = "91";
                ReqMessage.VerifyPinChangeResponse = string.Empty;
                HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                HsmLogger.WriteTransLog(this, ex.Message.ToString() + ex.Source.ToString() + ex.StackTrace.ToString());
            }
            SystemLogger.WriteTransLog(this, "DrukPay ATM Pin Generation Completed : " + ReqMessage.TransactionRefrenceNumber + " and New Offset is Sucess. " );
        }

        public void GenerateOffsetFormPAY(ref Authentication ReqMessage)
        {
            try
            {
                SystemLogger.WriteTransLog(this, "mPIN Generation Started for reference number : " + ReqMessage.TransactionRefrenceNumber);
                HsmResponse = string.Empty;
                string pinBlock = Utils.EBCDIC2HEX(ReqMessage.PinBlock);
                string PinChangepinBlock = Utils.EBCDIC2HEX(ReqMessage.PinChangeBlock);
                string pinValData = ReqMessage.Cardnumber.Substring(ReqMessage.Cardnumber.Length - 0x10);
                pinValData = pinValData.Substring(0, 10) + "N" + pinValData.Substring(pinValData.Length - 1);
                string offset = ReqMessage.PinOffset;
                offset = offset.PadRight(12, 'F');
                ////*******************HSM Command Format Packing***********************
                string AccountNumber = ReqMessage.Cardnumber.Substring(3, 12);
                string PackData = string.Empty;
                if (ReqMessage.Cardnumber.Length == 19)
                    PackData = _ProcessAuthonticationRequest.SafenetPackCommand(SafenetHSMFunctionCode.PINOffsetGeneration, SafenetHSMFunctionModifier.FM, Utils.EBCDIC2HEX(ReqMessage.PinChangeBlock), ReqMessage.HsmComkey
                                        , SafenetMessageConst.DieboldPinBlockFormat, AccountNumber, ReqMessage.HsmPvk
                                        , ReqMessage.Cardnumber.Substring(2, 16));
                else if (ReqMessage.Cardnumber.Length == 16)
                {
                    PackData = _ProcessAuthonticationRequest.SafenetPackCommand(SafenetHSMFunctionCode.PINOffsetGeneration, SafenetHSMFunctionModifier.FM, Utils.EBCDIC2HEX(ReqMessage.PinChangeBlock), ReqMessage.HsmComkey
                                        , SafenetMessageConst.DieboldPinBlockFormat, AccountNumber, ReqMessage.HsmPvk
                                        , ReqMessage.Cardnumber);
                }

                if (!CommonConfiguration.ConfigHID)
                    HsmLogger.WriteTransLog(this, string.Format("Data Packed : {0}", PackData));

                try
                {
                    //HsmResponse = _hsmCommander.ProcessData(Utils.ASCIIToByteArray(Utils.HEX2ASCII(PackData)), "");
                    ReqMessage.RequestKey = PackData.Substring(0, 8);
                    DictMessagingHsm.TryAdd(ReqMessage.RequestKey.ToUpper(), ReqMessage);
                    InterBankClientConnManager.Send(Utils.ASCIIToByteArray(Utils.HEX2ASCII(PackData)));
                    HsmResponse = GetResponse(ReqMessage.RequestKey.ToUpper());
                    HsmLogger.WriteTransLog(this, string.Format("Hsm Response Received : {0}", HsmResponse));
                }
                catch (Exception ex)
                {
                    HsmResponse = string.Empty;
                    HsmLogger.WriteTransLog(this, ex.Message.ToString() + ex.Source.ToString() + ex.StackTrace.ToString());
                }

                try
                {
                    if (string.IsNullOrEmpty(HsmResponse))
                    {
                        ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                        ReqMessage.VerifyPinChangeResponse = "";
                        HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                    }
                    else if (HsmResponse.Substring(CommonConfiguration.HsmWhiteSpace + CommonConfiguration.HSMHeaderLength, 2) == "00")
                    {
                        ReqMessage.TransactionStatus = enumTransactionStatus.Successful;
                        ReqMessage.VerifyPinChangeResponse = HsmResponse.Substring(CommonConfiguration.HsmWhiteSpace + CommonConfiguration.HSMHeaderLength);
                        HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                        HsmLogger.WriteTransLog(this, string.Format("DrukPay Pin Change Response(S) : {0}", ReqMessage.VerifyPinChangeResponse));
                        ReqMessage.NewOffset = HsmResponse.Substring(CommonConfiguration.HsmWhiteSpace + CommonConfiguration.HSMHeaderLength + 2);
                        ReqMessage.NewOffset = ReqMessage.NewOffset.Substring(0, 4);
                    }
                    else
                    {
                        ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                        ReqMessage.VerifyPinChangeResponse = HsmResponse.Substring(CommonConfiguration.HsmWhiteSpace + CommonConfiguration.HSMHeaderLength, 2);
                        HsmLogger.WriteTransLog(this, string.Format("DrukPay Pin Change Response(U) : {0}", ReqMessage.VerifyPinChangeResponse));
                        HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                    ReqMessage.VerifyPinChangeResponse = string.Empty;
                    HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                    HsmLogger.WriteTransLog(this, ex.Message.ToString() + ex.Source.ToString() + ex.StackTrace.ToString());
                }

            }
            catch (Exception ex)
            {
                ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                ReqMessage.VerifyPinChangeResponse = string.Empty;
                HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                HsmLogger.WriteTransLog(this, ex.Message.ToString() + ex.Source.ToString() + ex.StackTrace.ToString());
            }
           // SystemLogger.WriteTransLog(this, "DrukPay PIN Generation Completed : " + ReqMessage.TransactionRefrenceNumber + " and New Offset is :  sucesss ");
        }

        public void Safenet_VerifymPAYPIN(ref Authentication ReqMessage)
        {
            try
            {
                SystemLogger.WriteTransLog(this, "mPIN Verification Started for reference number : " + ReqMessage.TransactionRefrenceNumber);
                HsmResponse = string.Empty;
                string PackData = string.Empty;
                ////string AccountNumber = ReqMessage.Cardnumber.Substring(ReqMessage.Cardnumber.Length - 12 - 1, 12);
                string AccountNumber = ReqMessage.Cardnumber.Substring(3, 12);
                ////*******************HSM Command Format Packing***********************
                PackData = _ProcessAuthonticationRequest.SafenetPackCommand(SafenetHSMFunctionCode.PINVerification, SafenetHSMFunctionModifier.FM, Utils.EBCDIC2HEX(ReqMessage.PinBlock), ReqMessage.HsmComkey
                                    , SafenetMessageConst.DieboldPinBlockFormat, AccountNumber, ReqMessage.HsmPvk, ReqMessage.Cardnumber, ReqMessage.PinOffset.PadRight(12, '0'), SafenetMessageConst.DefaultCheckLength);
                if (!CommonConfiguration.ConfigHID)
                    HsmLogger.WriteTransLog(this, string.Format("Data Packed : {0}", PackData));
                else
                    HsmLogger.WriteTransLog(this, string.Format("Data Packed : {0}", new string('X', PackData.Length - 4) + PackData.Substring(PackData.Length - 4, 4)));
                try
                {
                    //HsmResponse = _hsmCommander.ProcessData(Utils.ASCIIToByteArray(Utils.HEX2ASCII(PackData)), "");
                    ReqMessage.RequestKey = PackData.Substring(0, 8);
                    DictMessagingHsm.TryAdd(ReqMessage.RequestKey.ToUpper(), ReqMessage);
                    InterBankClientConnManager.Send(Utils.ASCIIToByteArray(Utils.HEX2ASCII(PackData)));
                    HsmResponse = GetResponse(ReqMessage.RequestKey.ToUpper());
                    HsmLogger.WriteTransLog(this, string.Format("Hsm Response Received : {0}", HsmResponse));
                }
                catch (Exception ex)
                {
                    HsmResponse = string.Empty;
                    HsmLogger.WriteTransLog(this, ex.Message.ToString() + ex.Source.ToString() + ex.StackTrace.ToString());
                }

                try
                {
                    if (string.IsNullOrEmpty(HsmResponse))
                    {
                        ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                        ReqMessage.VerifyPinResponse = "";
                        HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                    }
                    else if (HsmResponse.Substring(CommonConfiguration.HsmWhiteSpace + CommonConfiguration.HSMHeaderLength) == "00")
                    {
                        ReqMessage.TransactionStatus = enumTransactionStatus.Successful;
                        ReqMessage.VerifyPinResponse = HsmResponse.Substring(CommonConfiguration.HsmWhiteSpace + CommonConfiguration.HSMHeaderLength);
                        HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                        HsmLogger.WriteTransLog(this, string.Format("DrukPay Pin Response(S) : {0}", ReqMessage.VerifyPinResponse));
                    }
                    else
                    {
                        ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                        ReqMessage.VerifyPinResponse = HsmResponse.Substring(CommonConfiguration.HsmWhiteSpace + CommonConfiguration.HSMHeaderLength);
                        HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                        HsmLogger.WriteTransLog(this, string.Format("DrukPay Pin Response(U) : {0}", ReqMessage.VerifyPinResponse));
                    }
                }
                catch (Exception ex)
                {
                    ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                    ReqMessage.VerifyPinResponse = string.Empty;
                    HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                    HsmLogger.WriteTransLog(this, ex.Message.ToString() + ex.Source.ToString() + ex.StackTrace.ToString());
                }
            }
            catch (Exception ex)
            {
                ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                ReqMessage.VerifyPinResponse = string.Empty;
                HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                HsmLogger.WriteTransLog(this, ex.Message.ToString() + ex.Source.ToString() + ex.StackTrace.ToString());
            }
            SystemLogger.WriteTransLog(this, "DrukPay PIN Verification Completed : " + ReqMessage.TransactionRefrenceNumber);
        }

        public void ProcessVerifyAtmPin(ref Authentication ReqMessage, string[] Track2)
        {
            try
            {
                Safenet_VerifyPIN(ref ReqMessage);
            }
            catch (Exception ex)
            { HsmLogger.WriteErrorLog(null, ex); }
        }

        private void Safenet_VerifyCvv(ref Authentication ReqMessage, string ExpiryDate, string ServiceCode, string Delimiter, string CardCVV)
        {
            try
            {
                HsmResponse = string.Empty;
                HsmLogger.WriteTransLog(this, "Cvv Verification Started");

                string PackData = _ProcessAuthonticationRequest.SafenetPackCommand(SafenetHSMFunctionCode.CVVVerification, SafenetHSMFunctionModifier.FM, ReqMessage.HsmCvv1, ReqMessage.HsmCvv2, ReqMessage.Cardnumber
                                                    , ExpiryDate, ServiceCode, Delimiter, CardCVV);

                if (!CommonConfiguration.ConfigHID)
                    HsmLogger.WriteTransLog(this, string.Format("Data Packed : {0}", PackData));
                else
                    HsmLogger.WriteTransLog(this, string.Format("Data Packed : {0}", new string('X', PackData.Length - 4) + PackData.Substring(PackData.Length - 4, 4)));
                try
                {
                    HsmResponse = _hsmCommander.ProcessData(Utils.ASCIIToByteArray(Utils.HEX2ASCII(PackData)), "");
                    HsmLogger.WriteTransLog(this, string.Format("Hsm Response Received : {0}", HsmResponse));
                }
                catch (Exception ex)
                {
                    HsmResponse = string.Empty;
                    HsmLogger.WriteTransLog(this, ex.Message.ToString() + ex.Source.ToString() + ex.StackTrace.ToString());
                }

                try
                {
                    if (string.IsNullOrEmpty(HsmResponse))
                    {
                        ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                        ReqMessage.CCVResponse = "";
                        HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                    }
                    else if (HsmResponse.Substring(CommonConfiguration.HsmWhiteSpace + CommonConfiguration.HSMHeaderLength) == "00")
                    {
                        ReqMessage.TransactionStatus = enumTransactionStatus.Successful;
                        ReqMessage.CCVResponse = HsmResponse.Substring(CommonConfiguration.HsmWhiteSpace + CommonConfiguration.HSMHeaderLength);
                        HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                        HsmLogger.WriteTransLog(this, string.Format("Cvv Response(S) : {0}", ReqMessage.CCVResponse));
                    }
                    else
                    {
                        ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                        ReqMessage.CCVResponse = HsmResponse.Substring(CommonConfiguration.HsmWhiteSpace + CommonConfiguration.HSMHeaderLength);
                        HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                        HsmLogger.WriteTransLog(this, string.Format("Cvv Response(U) : {0}", ReqMessage.CCVResponse));
                    }
                }
                catch (Exception ex)
                {
                    ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                    ReqMessage.CCVResponse = string.Empty;
                    HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                    HsmLogger.WriteTransLog(this, ex.Message.ToString() + ex.Source.ToString() + ex.StackTrace.ToString());
                }
            }
            catch (Exception ex)
            {
                ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                ReqMessage.CCVResponse = string.Empty;
                HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                HsmLogger.WriteTransLog(this, ex.Message.ToString() + ex.Source.ToString() + ex.StackTrace.ToString());
            }
            HsmLogger.WriteTransLog(this, "Cvv Verification Completed");
        }

        private void Safenet_VerifyPIN(ref Authentication ReqMessage)
        {
            try
            {
                SystemLogger.WriteTransLog(this, "ReqMessage.PinOffset" + ReqMessage.PinOffset.ToString());
                SystemLogger.WriteTransLog(this, "PIN Verification Started for reference number : " + ReqMessage.TransactionRefrenceNumber);
                HsmResponse = string.Empty;
                string PackData = string.Empty;
                ////string AccountNumber = ReqMessage.Cardnumber.Substring(ReqMessage.Cardnumber.Length - 12 - 1, 12);
                string AccountNumber = ReqMessage.Cardnumber.Substring(3, 12);

                ////*******************HSM Command Format Packing***********************
                if (ReqMessage.Cardnumber.Length == 19)
                    PackData = _ProcessAuthonticationRequest.SafenetPackCommand(SafenetHSMFunctionCode.PINVerification, SafenetHSMFunctionModifier.FM, Utils.EBCDIC2HEX(ReqMessage.PinBlock), ReqMessage.HsmComkey
                                        , SafenetMessageConst.DieboldPinBlockFormat, AccountNumber, ReqMessage.HsmPvk
                                        , ReqMessage.Cardnumber.Substring(2, 16), ReqMessage.PinOffset.PadRight(12, '0'), SafenetMessageConst.DefaultCheckLength);

                else if (ReqMessage.Cardnumber.Length == 16)
                {
                    PackData = _ProcessAuthonticationRequest.SafenetPackCommand(SafenetHSMFunctionCode.PINVerification, SafenetHSMFunctionModifier.FM, Utils.EBCDIC2HEX(ReqMessage.PinBlock), ReqMessage.HsmComkey
                                        , SafenetMessageConst.DieboldPinBlockFormat, AccountNumber, ReqMessage.HsmPvk
                                        , ReqMessage.Cardnumber, ReqMessage.PinOffset.PadRight(12, '0'), SafenetMessageConst.DefaultCheckLength);
                }
               

                if (!CommonConfiguration.ConfigHID)
                    HsmLogger.WriteTransLog(this, string.Format("Data Packed : {0}", PackData));
                else
                    HsmLogger.WriteTransLog(this, string.Format("Data Packed : {0}", new string('X', PackData.Length - 4) + PackData.Substring(PackData.Length - 4, 4)));
                try
                {
                    //HsmResponse = _hsmCommander.ProcessData(Utils.ASCIIToByteArray(Utils.HEX2ASCII(PackData)), "");
                    //HsmLogger.WriteTransLog(this, string.Format("Hsm Response Received : {0}", HsmResponse));
                    ReqMessage.RequestKey = PackData.Substring(0, 8);
                    DictMessagingHsm.TryAdd(ReqMessage.RequestKey.ToUpper(), ReqMessage);
                    InterBankClientConnManager.Send(Utils.ASCIIToByteArray(Utils.HEX2ASCII(PackData)));
                    HsmResponse = GetResponse(ReqMessage.RequestKey.ToUpper());
                    HsmLogger.WriteTransLog(this, string.Format("Hsm Response Received : {0}", HsmResponse));

                }
                catch (Exception ex)
                {
                    HsmResponse = string.Empty;
                    HsmLogger.WriteTransLog(this, ex.Message.ToString() + ex.Source.ToString() + ex.StackTrace.ToString());
                }

                try
                {
                    if (string.IsNullOrEmpty(HsmResponse))
                    {
                        ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                        ReqMessage.VerifyPinResponse = "";
                        HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                    }
                    else if (HsmResponse.Substring(MIIPL.Common.CommonConfiguration.HsmWhiteSpace + MIIPL.Common.CommonConfiguration.HSMHeaderLength) == "00")
                    {
                        ReqMessage.TransactionStatus = enumTransactionStatus.Successful;
                        ReqMessage.VerifyPinResponse = HsmResponse.Substring(CommonConfiguration.HsmWhiteSpace + CommonConfiguration.HSMHeaderLength);
                        HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                        HsmLogger.WriteTransLog(this, string.Format("Pin Response(S) : {0}", ReqMessage.VerifyPinResponse));
                    }
                    else
                    {
                        ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                        ReqMessage.VerifyPinResponse = HsmResponse.Substring(CommonConfiguration.HsmWhiteSpace + CommonConfiguration.HSMHeaderLength);
                        HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                        HsmLogger.WriteTransLog(this, string.Format("Pin Response(U) : {0}", ReqMessage.VerifyPinResponse));
                    }
                }
                catch (Exception ex)
                {
                    ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                    ReqMessage.VerifyPinResponse = string.Empty;
                    HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                    HsmLogger.WriteTransLog(this, ex.Message.ToString() + ex.Source.ToString() + ex.StackTrace.ToString());
                }
            }
            catch (Exception ex)
            {
                ReqMessage.TransactionStatus = enumTransactionStatus.UnSuccessful;
                ReqMessage.VerifyPinResponse = string.Empty;
                HsmLogger.WriteTransLog(this, string.Format("Transaction Status : {0}", ReqMessage.TransactionStatus.ToString()));
                HsmLogger.WriteTransLog(this, ex.Message.ToString() + ex.Source.ToString() + ex.StackTrace.ToString());
            }
            SystemLogger.WriteTransLog(this, "PIN Verification Completed : " + ReqMessage.TransactionRefrenceNumber);
        }

        public Authentication MsgConvertor(Authentication ReqMsg, enumSecurityModuleType SecurityModuleType)
        {
            Authentication _authenticationRequest = new Authentication()
            {
                Cardnumber = ReqMsg.Cardnumber,
                CardTrack2 = ReqMsg.CardTrack2,
                SsmComkey = ReqMsg.SsmComkey,
                SsmMasterKey = ReqMsg.SsmMasterKey,
                SsmPvk = ReqMsg.SsmPvk,
                SsmCvv1 = ReqMsg.SsmCvv1,
                SsmCvv2 = ReqMsg.SsmCvv2,
                SsmCvkPair = ReqMsg.SsmCvkPair,
                TmkEncryptedKey = ReqMsg.TmkEncryptedKey,
                HsmZpk = ReqMsg.HsmZpk,
                HsmComkey = ReqMsg.HsmComkey,
                HsmPvk = ReqMsg.HsmPvk,
                HsmCvv1 = ReqMsg.HsmCvv1,
                HsmCvv2 = ReqMsg.HsmCvv2,
                HsmCvkPair = ReqMsg.HsmCvkPair,
                PinOffset = ReqMsg.PinOffset,
                PinBlock = ReqMsg.PinBlock,
                PinChangeBlock = ReqMsg.PinChangeBlock,
                SecurityModule = SecurityModuleType,
                TransactionReqMode = ReqMsg.TransactionReqMode,
                TransactionStatus = ReqMsg.TransactionStatus,
                TransactionType = ReqMsg.TransactionType,
            };
            return _authenticationRequest;
        }

        public Authentication MsgConvertor(Authentication ReqMsg, Authentication RespMsg)
        {
            try
            {
                ReqMsg.CCVResponse = RespMsg.CCVResponse;
                ReqMsg.VerifyPinResponse = RespMsg.VerifyPinResponse;
                ReqMsg.VerifyPinChangeResponse = RespMsg.VerifyPinChangeResponse;
                ReqMsg.NewOffset = RespMsg.NewOffset;
                ReqMsg.TransactionStatus = RespMsg.TransactionStatus;
                ReqMsg.SessionKeyResponse = RespMsg.SessionKeyResponse;
            }
            catch (Exception ex)
            { }
            return ReqMsg;
        }
    }

    public class Authentication
    {
        public enumPINCommandType CommandType = enumPINCommandType.Unknown;
        public string HsmHeader { get; set; }
        ////************SSM*********************
        public string SsmComkey = string.Empty;
        public string SsmMasterKey = string.Empty;
        public string SsmPvk = string.Empty;
        public string SsmCvv1 = string.Empty;
        public string SsmCvv2 = string.Empty;
        public string SsmCvkPair = string.Empty;
        ////*************HSM********************
        public string TmkEncryptedKey = string.Empty;
        public string HsmComkey = string.Empty;
        public string HsmPvk = string.Empty;
        public string HsmCvv1 = string.Empty;
        public string HsmCvv2 = string.Empty;
        public string HsmCvkPair = string.Empty;
        public string HsmZpk = string.Empty;
        public string Cardnumber = string.Empty;
        public string CardTrack2 = string.Empty;
        public string PinOffset = string.Empty;
        public string NewOffset = string.Empty;
        public string PinBlock = string.Empty;
        public string PinChangeBlock = string.Empty;
        public string SessionKeyResponse = string.Empty;
        public string CCVResponse = string.Empty;
        public string VerifyPinResponse = string.Empty;
        public string VerifyPinChangeResponse = string.Empty;
        public enumModeOfTransaction TransactionReqMode = enumModeOfTransaction.Unknown;
        public enumTransactionStatus TransactionStatus = enumTransactionStatus.Unknown;
        public string TransactionType = string.Empty;
        public enumSecurityModuleType SecurityModule;
        public string TransactionRefrenceNumber = string.Empty;
        public string ATMGreenPinResponse = string.Empty;
        public string HsmCvk = string.Empty;
        public string CardExp = string.Empty;
        public string DecryptedCardNo = string.Empty;
        public string CardCVV = string.Empty;
        public string NewAtmPin = string.Empty;
        //added by sk for new hsm code
        public string ResponseKey = string.Empty;
        public string HsmResponseData = string.Empty;
        public bool ResponseFlag;
        public string RequestKey = string.Empty;
    }

    public enum enumSecurityModuleType
    {
        HSM = 1,
        SSM = 2,
    }

    public enum enumPINCommandType
    {
        Unknown = 0,
        SetPin = 1,
        VerifyPin = 2,
        SetATMPin=3
    }

    public enum enumTransactionStatus
    {
        Unknown = -1,
        Successful = 0,
        UnSuccessful = 1,
    }

    public enum enumModeOfTransaction
    {
        Unknown = 0,
        Issuer = 1,
        Acquirer = 2,
        OnUs = 3,
    }

}
