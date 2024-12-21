using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace MIIPL.Common
{
    
    public class LoggerEventArgs : EventArgs
    {
        private string message; // private member variable

        // property
        public string Message { get { return message; } }

        // constructor
        public LoggerEventArgs(string message)
        {
            this.message = message;
        }
    }

    public delegate void WriteEntryDelegate(object sender, LoggerEventArgs e);
    //public delegate void WriteErrorDelegate(object sender, LoggerEventArgs e);
    public interface iLogger
    {
        //void WriteEntry(object sender, ArrayList entry);
        void WriteEntry(object sender, string message);
        void WriteError(object sender, string message);
        event WriteEntryDelegate OnWriteEntry;
        //event WriteErrorDelegate OnWriteError;
    }
}
