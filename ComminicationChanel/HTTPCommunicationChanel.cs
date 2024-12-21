using MaxiSwitch.Common.TerminalLogger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BIPS.Communication
{
    public class HTTPCommunicationChanel
    {
        public SystemLogger SystemLogger = null;
        public CommonLogger CommonLogger = null;
        public HTTPCommunicationChanel()
        {
            try
            {
                SystemLogger = new SystemLogger();
                CommonLogger = new CommonLogger();
            }
            catch { }
        } 

        public bool CheckBTConnection()
        {
            bool resturnVal = false;
            try
            {
               // System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
               //          System.Security.Cryptography.X509Certificates.X509Certificate certificate,
               //          System.Security.Cryptography.X509Certificates.X509Chain chain,
               //          System.Net.Security.SslPolicyErrors sslPolicyErrors)
               //{
               //    return true;
               //};
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                var result = socket.BeginConnect(ConfigurationManager.AppSettings["BTIP"].ToString(), 443, null, null);// uat 80
                bool success = result.AsyncWaitHandle.WaitOne(3000, false); // test the connection for 3 seconds
                resturnVal = socket.Connected;
                if (socket.Connected)
                    socket.Disconnect(true);
                socket.Dispose();
                return resturnVal;
            }
            catch(Exception ex)
            {
                SystemLogger.WriteErrorLog(this, ex);
                resturnVal = false;
                return resturnVal;
            }
        }

        public bool CheckTCellConnection()
        {
            bool resturnVal = false;
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                         System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                         System.Security.Cryptography.X509Certificates.X509Chain chain,
                         System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                var result = socket.BeginConnect(ConfigurationManager.AppSettings["TCELLIP"].ToString(), 8081, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(3000, false); // test the connection for 3 seconds
                resturnVal = socket.Connected;
                if (socket.Connected)
                    socket.Disconnect(true);
                socket.Dispose();
                return resturnVal;
            }
            catch (Exception ex)
            {
                SystemLogger.WriteErrorLog(this, ex);
                resturnVal = false;
                return resturnVal;
            }
        }

        public bool CheckBPCConnection()
        {
            bool resturnVal = false;
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                         System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                         System.Security.Cryptography.X509Certificates.X509Chain chain,
                         System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                var result = socket.BeginConnect(ConfigurationManager.AppSettings["BPCIP"].ToString(), 8000, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(3000, false); // test the connection for 3 seconds
                resturnVal = socket.Connected;
                if (socket.Connected)
                    socket.Disconnect(true);
                socket.Dispose();
                return resturnVal;
            }
            catch (Exception ex)
            {
                SystemLogger.WriteErrorLog(this, ex);
                resturnVal = false;
                return resturnVal;
            }
        }

        public bool CheckNPPFConnection()
        {
            bool resturnVal = false;
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                         System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                         System.Security.Cryptography.X509Certificates.X509Chain chain,
                         System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                var result = socket.BeginConnect(ConfigurationManager.AppSettings["NPPFIP"].ToString(), 80, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(3000, false); // test the connection for 3 seconds
                resturnVal = socket.Connected;
                if (socket.Connected)
                    socket.Disconnect(true);
                socket.Dispose();
                return resturnVal;
            }
            catch (Exception ex)
            {
                SystemLogger.WriteErrorLog(this, ex);
                resturnVal = false;
                return resturnVal;
            }
        }

        public bool CheckBTPostPaidConnection()
        {
            bool resturnVal = false;
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                         System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                         System.Security.Cryptography.X509Certificates.X509Chain chain,
                         System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                var result = socket.BeginConnect(ConfigurationManager.AppSettings["BTPOSTPAIDIP"].ToString(), 443, null, null);//8000 uat
                bool success = result.AsyncWaitHandle.WaitOne(3000, false); // test the connection for 3 seconds
                resturnVal = socket.Connected;
                if (socket.Connected)
                    socket.Disconnect(true);
                socket.Dispose();
                return resturnVal;
            }
            catch (Exception ex)
            {
                SystemLogger.WriteErrorLog(this, ex);
                resturnVal = false;
                return resturnVal;
            }
        }

        public bool CheckBTPrePaidConnection()
        {
            bool resturnVal = false;
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                         System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                         System.Security.Cryptography.X509Certificates.X509Chain chain,
                         System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                var result = socket.BeginConnect(ConfigurationManager.AppSettings["BTPREPAIDIP"].ToString(), 443, null, null);// uat 8000
                bool success = result.AsyncWaitHandle.WaitOne(3000, false); // test the connection for 3 seconds
                resturnVal = socket.Connected;
                if (socket.Connected)
                    socket.Disconnect(true);
                socket.Dispose();
                return resturnVal;
            }
            catch (Exception ex)
            {
                SystemLogger.WriteErrorLog(this, ex);
                resturnVal = false;
                return resturnVal;
            }
        }


    }
}
