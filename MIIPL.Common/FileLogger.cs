using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace MIIPL.Common
{
    public class FileLogger : iLogger
    {
        private static object objLock = new object();
        private StreamWriter output; // Log file
        private string LogFileNamePattern;
        private string LogFilePath;
        private string strTransLogFileName;
        private string FilePrefix = "TransLog";
        private string Extenstion = ".txt";
        private string BankName = string.Empty;

        public FileLogger()
            : this(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\Log", "yyyyMMdd")
        {
        }

        public FileLogger(string logFilePath, string logFileNamePattern)
            : this(logFilePath, logFileNamePattern, "TransLog", ".txt")
        {
        }

        public FileLogger(string logFilePath, string logFileNamePattern, string filePrefix, string extenstion)
        {
            LogFilePath = logFilePath;
            LogFileNamePattern = logFileNamePattern;
            FilePrefix = filePrefix;
            Extenstion = extenstion;
        }

        public FileLogger(string logFilePath, string logFileNamePattern, string filePrefix, string extenstion, string pBankName)
        {
            LogFilePath = logFilePath;
            LogFileNamePattern = logFileNamePattern;
            FilePrefix = filePrefix;
            Extenstion = extenstion;
            BankName = pBankName;
        }

        //string LogFolderName = "TerminalControllerLogs";
        public void CreateTransLogFile()
        {
            string LogFolderNamePath = LogFilePath;
            string strTransLogFilePath = LogFilePath;
            string SubFolder = LogFolderNamePath;

            if (string.IsNullOrEmpty(BankName))
            {
                strTransLogFileName = LogFolderNamePath + "\\" + FilePrefix + DateTime.Now.ToString(LogFileNamePattern) + Extenstion;

            }
            else
            {
                strTransLogFileName = LogFolderNamePath + "\\" + BankName + "\\" + FilePrefix + DateTime.Now.ToString(LogFileNamePattern) + Extenstion;
                SubFolder = LogFolderNamePath + "\\" + BankName;
            }
            if (!System.IO.Directory.Exists(LogFolderNamePath))
                Directory.CreateDirectory(LogFolderNamePath);


            if (!System.IO.Directory.Exists(SubFolder))
                Directory.CreateDirectory(SubFolder);

            if (!System.IO.File.Exists(strTransLogFileName))
            {

                output = File.CreateText(strTransLogFileName);
                output.Close();
            }
        }

        public void WriteEntry(object sender, string msg)
        {
            lock (objLock)
            {
                CreateTransLogFile();
                File.AppendAllText(strTransLogFileName, Convert.ToString(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss:ffff")) + " : ");
                File.AppendAllText(strTransLogFileName, msg + Environment.NewLine);
            }
            if (this.OnWriteEntry != null)
            {
                this.OnWriteEntry(sender, new LoggerEventArgs(msg));
            }
        }

        public event WriteEntryDelegate OnWriteEntry;

        //public event WriteErrorDelegate OnWriteError;
        #region iLogger Members


        public void WriteError(object sender, string message)
        {
            //string m_methodName = "";
            //string m_className = "";
            //string m_fileName = "";
            //string m_lineNumber = "";
            ////var st = new StackTrace(); 
            //return LogManager.GetLogger(stackTrace.GetFrame(1).GetMethod().DeclaringType); 

            // now frameIndex is the first 'user' caller frame
            //StackFrame aFrame = st.GetFrame(0);
            StackFrame Callstack = new StackFrame(1, true);
            //if (Callstack != null)
            //{
            //    System.Reflection.MethodBase method = Callstack.GetMethod();
            //    if (method != null)
            //    {
            //        m_methodName = method.Name;
            //        if (method.DeclaringType != null)
            //        {
            //            m_className = method.DeclaringType.FullName;
            //        }
            //    }
            //    m_fileName = Callstack.GetFileName();
            //    m_lineNumber = Callstack.GetFileLineNumber().ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
            //}

            string logMessage = string.Format("MaxError:{0}File Name: {1}Line Number: {2}Method: {3}Description: {4}", Environment.NewLine,
                Callstack.GetFileName() + Environment.NewLine, Callstack.GetFileLineNumber() + Environment.NewLine,
                Callstack.GetMethod() + Environment.NewLine, message);
            WriteEntry(sender, logMessage);
        }

        #endregion
    }
}
