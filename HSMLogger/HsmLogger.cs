using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIIPL.Common;

namespace MaxiSwitch.Logger
{
    public  class HsmLogger
    {
        private static iLogger _transactionLog = null;        
        private static iLogger _errorLogger = null;
        private static iLogger _auditTrailLogger = null;

        #region AuditTrailLog
        static iLogger AudiTrailLogg
        {
            get
            {
                if (_auditTrailLogger == null)
                {
                    try
                    {
                        _auditTrailLogger = new FileLogger(AuditLogFilePath.AuditTrailLogPath, "dd-MM-yyyy", "AuditTrailLog_", ".txt");
                    }
                    catch (Exception ex)
                    { }
                }
                return _auditTrailLogger;
            }
            set { _auditTrailLogger = value; }
        }
        public static void AuditTrailLog(object sender, string UserName, string ActivityPerformed)
        {
            AudiTrailLogg.WriteEntry(sender,
                                     string.Format("UserName : {0}" + Environment.NewLine +
                                     "Activity Performed : {1}" + Environment.NewLine +
                                     "Activity Date Time : {2}" + Environment.NewLine + "-------------------------------------------------------"
                                     , UserName, ActivityPerformed, DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss tt")));
        }
        #endregion AuditTrailLog

        static iLogger TransactionLog
        {
            get
            {
                if (_transactionLog == null)
                {
                    try
                    {
                        _transactionLog = new FileLogger(LogFilePath.TransactionLogPath, "dd-MM-yyyy", "HsmTransLog_", ".txt");                        
                    }
                    catch(Exception ex)
                    { }
                }
                return _transactionLog;
            }
            set { _transactionLog = value; }
        }

        static iLogger ErrorLog
        {
            get
            {
                if (_errorLogger == null)
                {
                    try
                    {
                        _errorLogger = new FileLogger(LogFilePath.ErrorLogPath, "dd-MM-yyyy", "HsmErrorLog_", ".txt");
                    }
                    catch (Exception ex)
                    { }
                }
                return _errorLogger;
            }
            set { _errorLogger = value; }
        }

        public static void WriteTransLog(object sender, string message)
        {
            TransactionLog.WriteEntry(sender, message);
        }

        public static void WriteErrorLog(object sender ,Exception ex)
        {
            ErrorLog.WriteError(sender,
                                                "Source - : " + ex.Source.ToString() + Environment.NewLine +
                                                "StackTrace -  : " + ex.StackTrace.ToString() + Environment.NewLine +
                                                "Message - : " + ex.Message.ToString()
                                              );

            
        }        
    }
}
