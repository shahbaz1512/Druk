using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using MIIPL.Common;
using MaxiSwitch.Common.TerminalLogger;
using System.Net.Sockets;
using MaxiSwitch.DALC.Configuration;

namespace HSMManager
{
    public class HSMCommunication
    {
        private int ResponseTime = 0;
        public SystemLogger SystemLogger = null;
        public SystemLogger_HSM SystemLogger_HSM = null;
        Socket _socketClient = null;
        System.Net.IPEndPoint _hsmEndPoint = null;
        public HSMCommunication()
        {
            try
            {
                SystemLogger_HSM = new SystemLogger_HSM();
                SystemLogger = new SystemLogger();
                _socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                System.Net.IPAddress _hsmIpAddress = System.Net.IPAddress.Parse(CONFIGURATIONCONFIGDATA.HSMIP);
                _hsmEndPoint = new System.Net.IPEndPoint(_hsmIpAddress, CONFIGURATIONCONFIGDATA.HSPPORT);
                _socketClient.Connect(_hsmEndPoint);
                SystemLogger_HSM.WriteTransLog(this, "Connected to HSM at " + CONFIGURATIONCONFIGDATA.HSMIP + " : " + CONFIGURATIONCONFIGDATA.HSPPORT);
            }
            catch (Exception ex)
            {
                SystemLogger_HSM.WriteTransLog(this, "Unable to Connected with HSM at " + CONFIGURATIONCONFIGDATA.HSMIP + " : " + CONFIGURATIONCONFIGDATA.HSPPORT);
                SystemLogger.WriteErrorLog(this, ex);
            }
        }

        //******************Original On 090116*********************
        public string ProcessData(string ReqMessage)
        {
            try
            {
                SystemLogger_HSM.WriteTransLog(this, "Authentication Packed Data Sending (Primary) ");
                string Data = SendFunctionCommand(ReqMessage);
                SystemLogger_HSM.WriteTransLog(null, "Authentication Data Received (Primary) " + Data);
                if (!CommonConfiguration.ConfigHID)
                    SystemLogger_HSM.WriteTransLog(null, "Authentication Data Received (Primary) " + Data);
                else
                    SystemLogger_HSM.WriteTransLog(null, "Authentication Data Received (Primary) " + new string('X', Data.Length - 4) + Data.Substring(Data.Length - 4, 4));
                return Data;
            }
            catch (Exception ex)
            {
                SystemLogger_HSM.WriteTransLog(null, "Error Occured While Sending Message (Primary) " + ex.Message + ex.Source + ex.StackTrace);
                return "";
            }
        }
        public string SendFunctionCommand(string _Commad)
        {
            try
            {
                if (_socketClient.Connected)
                {
                    byte[] b = new byte[1024];
                    string _CommandToSend = Utils.HEX2ASCII(_Commad);
                    b = Utils.ASCIIToByteArray(_CommandToSend);
                    _socketClient.Send(b);
                    string rec = recivedForSafenet();
                    return rec;
                }
                else
                {

                    SystemLogger_HSM.WriteTransLog(null, " HSM is not connected, Trying to connect");
                    _socketClient.Close();
                    _socketClient.Connect(_hsmEndPoint);
                    SystemLogger_HSM.WriteTransLog(null, "NOW HSM IS CONNECTED");
                }
            }
            catch (Exception ex)
            {
                SystemLogger_HSM.WriteTransLog(null, "Data send failed to HSM");
                _socketClient.Close();
                HSMCommunication _HSMCommunication = new HSMCommunication();
                return ex.Message;
                goto End;
            }
            return recivedForSafenet();
        End:
            int i;
        }

        public string recivedForSafenet()
        {
            try
            {
                byte[] buffer = new byte[1024];
                int iRx = _socketClient.Receive(buffer);
                SystemLogger_HSM.WriteTransLog(this, Utils.ByteArrayToASCII(buffer));
                string _CommandResponse = Utils.ASCII2HEX(Utils.ByteArrayToASCII(buffer).Replace("\0\0\0", ""));
                return _CommandResponse;
            }
            catch (Exception ex)
            { return ex.Message; }
        }
        public event SocketClientHandler SendFailed;

    }
    public delegate void SocketClientHandler(string pID, string key, byte[] rawMsg);
    public interface iMIIPLResourceItem : IDisposable
    {
        bool IsDirty { get; set; }
        event EventHandler Completed;
        event EventHandler RemoveItem;
        DateTime ItemRunningSince { get; set; }
        string ItemKey { get; set; }
    }
}
