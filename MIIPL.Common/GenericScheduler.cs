using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MIIPL.Common
{
    public class GenericScheduler 
    {
        #region Status
        private bool isRunning = false; 
        public bool IsRunning
        {
            get { return isRunning; }
            private set
            {
                isRunning = value;
                OnStatusChanged(value);
            }
        }
        public event EventHandler StatusChanged = null;
        public void OnStatusChanged(bool val)
        {
            if (this.StatusChanged != null)
                this.StatusChanged(this, new EventArgs());
        }
        #endregion

        #region Logger
        private iLogger logger = null;
        public iLogger Logger
        {
            get { return logger; }
            set { logger = value; }
        }
        public void WriteLogEntry(string msg)
        {
            if (this.Logger != null)
            {
                this.Logger.WriteEntry(this, msg);
            }
        }
        #endregion

        #region Member Variables
        int invokeCount = 0;
        System.Threading.Timer timer = null;
        //AutoResetEvent autoEvent = null;
        TimerCallback timerCallBack = null;
        public TimerCallback TimerCallBack
        {
            get { return timerCallBack; }
            set { timerCallBack = value; }
        }
        #endregion

        #region Public Properties
        public int Count { get; set; }
        public int Delay { get; set; }
        #endregion

        #region Constructor
        public GenericScheduler(int pCount, int pDelay) : this(null,pCount,pDelay)
        {

        }
        public GenericScheduler(TimerCallback pTimerCallBack, int pCount, int pDelay)
        {
            invokeCount = 0;
            this.Count = pCount;
            this.Delay = pDelay;
            this.timerCallBack = pTimerCallBack;
        }
        #endregion

        #region Destructor
        ~GenericScheduler()
        {
            if (this.timer != null)
            {
                this.timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                this.timer.Dispose();
                this.timer = null;
            }
        }
        #endregion

        #region Public Methods
        public void Start()
        {
            if (this.timerCallBack == null) return;
            if (this.timer == null)
                this.timer = new Timer(this.TimerCallBack, null, 0, this.Delay * 1000);
            else
                this.timer.Change(0, this.Delay * 1000);

            this.IsRunning = true;
        }
        public void Stop()
        {
            if (this.timer != null)
            {
                this.timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                this.timer.Dispose();
                this.timer = null;
            }
            IsRunning = false;
        }
        #endregion
    }
}
