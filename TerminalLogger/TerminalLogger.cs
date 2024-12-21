using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIIPL.Common;
using MaxiSwitch.Common.cons;

namespace MaxiSwitch.Common.TerminalLogger
{
    public class SystemLogger
    {
        private iLogger _terminalTransactionLog = null;
        private iLogger _terminalTraceLog = null;
        private iLogger _terminalErrorLogger = null;

        public string DeviceID
        {
            get;
            set;
        }

        public SystemLogger()
        {

        }

        iLogger TerminalTransactionLog
        {
            get
            {
                if (_terminalTransactionLog == null)
                {
                    try
                    {
                        _terminalTransactionLog = new FileLogger(MIIPL.Common.LogFilePath.TransactionLogPath, "dd-MM-yyyy" + " _ " + "HH", "TransactionLog_", ".txt");
                    }
                    catch (Exception ex)
                    { }
                }
                return _terminalTransactionLog;
            }
            set { _terminalTransactionLog = value; }
        }

        iLogger TerminalTraceLog
        {
            get
            {
                if (_terminalTraceLog == null)
                {
                    try
                    {
                        _terminalTraceLog = new FileLogger(MaxiSwitch.Common.cons.LogFilePath.TraceLogPath, "dd-MM-yyyy" + " _ " + "HH", "TraceLog_", ".txt");
                    }
                    catch (Exception ex)
                    { }
                }
                return _terminalTraceLog;
            }
            set { _terminalTraceLog = value; }
        }

        iLogger TerminalErrorLogger
        {
            get
            {
                if (_terminalErrorLogger == null)
                {
                    try
                    {
                        _terminalErrorLogger = new FileLogger(MIIPL.Common.LogFilePath.ErrorLogPath, "dd-MM-yyyy" + " _ " + "HH", "ErrorLog_", ".txt");
                    }
                    catch (Exception ex)
                    { }
                }
                return _terminalErrorLogger;
            }
            set { _terminalErrorLogger = value; }
        }

        public void WriteTransLog(object sender, string message)
        {
            TerminalTransactionLog.WriteEntry(sender, message);
        }

        public void WriteTraceLog(object sender, string message)
        {
            TerminalTraceLog.WriteEntry(sender, message);
        }

        public void WriteErrorLog(object sender, Exception ex)
        {
            TerminalErrorLogger.WriteError(sender,
                                                "Source - : " + ex.Source.ToString() + Environment.NewLine +
                                                "StackTrace -  : " + ex.StackTrace.ToString() + Environment.NewLine +
                                                "Message - : " + ex.Message.ToString()
                                              );
        }

    }

    public class CommonLogger
    {
        private static iLogger _commonTransLog = null;

        private static iLogger _commonErrorLog = null;

        static iLogger CommonTransactionLog
        {
            get
            {
                if (_commonTransLog == null)
                {
                    try
                    {
                        _commonTransLog = new FileLogger(CommmonLogFilePath.CommanTransactionLogPath + "\\" + "CommonTransLog", "dd-MM-yyyy", "CommonTransLog_", ".txt");
                    }
                    catch (Exception ex)
                    { }
                }
                return _commonTransLog;
            }
            set { _commonTransLog = value; }
        }

        static iLogger CommonErrorLogger
        {
            get
            {
                if (_commonErrorLog == null)
                {
                    try
                    {
                        _commonErrorLog = new FileLogger(CommmonLogFilePath.ErrorLogPath + "\\" + "CommonErrorLog", "dd-MM-yyyy", "CommonErrorLog_", ".txt");
                    }
                    catch (Exception ex)
                    { }
                }
                return _commonErrorLog;
            }
            set { _commonErrorLog = value; }
        }

        public static void WriteTransLog(object sender, string message)
        {
            CommonTransactionLog.WriteEntry(sender, message);
        }

        public static void WriteErrorLog(object sender, Exception ex)
        {
            CommonErrorLogger.WriteError(sender,
                                                "Source - : " + ex.Source.ToString() + Environment.NewLine +
                                                "StackTrace -  : " + ex.StackTrace.ToString() + Environment.NewLine +
                                                "Message - : " + ex.Message.ToString()
                                              );
        }
    }

    public class AuditTrailLog
    {
        private static iLogger _auditTrailLog = null;

        static iLogger AuditTrailLogg
        {
            get
            {
                if (_auditTrailLog == null)
                {
                    try
                    {
                        _auditTrailLog = new FileLogger(CommmonLogFilePath.AuditTrailLogPath, "dd-MM-yyyy", "AuditTrail_", ".txt");
                    }
                    catch (Exception ex)
                    { }
                }
                return _auditTrailLog;
            }
            set { _auditTrailLog = value; }
        }

        public static void WriteAuditTrailLog(object sender, string UserName, string ActivityPerformed)
        {
            AuditTrailLogg.WriteEntry(sender,
                                     string.Format("UserName : {0}" + Environment.NewLine +
                                     "Activity Performed : {1}" + Environment.NewLine +
                                     "Activity Date Time : {2}" + Environment.NewLine + "-------------------------------------------------------"
                                     , UserName, ActivityPerformed, DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss tt")));
        }
    }

}
