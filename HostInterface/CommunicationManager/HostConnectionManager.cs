/* class connects Host Interface for transaction processing */
using MaxiSwitch.API.Terminal;
using System;
using System.Collections.Generic;
using System.Threading;
using MaxiSwitch.Common.TerminalLogger;
using MaxiSwitch.DALC.Configuration;
 
namespace TransactionRouter.CommunicationManager.Host
{
  
    public class HostConnectionManager
    {
        public SystemLogger SystemLogger = null;
        public CommonLogger CommonLogger = null;
        public HostConnectionManager()
        {
            try
            {
                SystemLogger = new SystemLogger();
                CommonLogger = new CommonLogger();
            }
            catch { }
        }

        private MIIPLResourceMgr resourceMgr = null;
        public MIIPLResourceMgr ResourceMgr
        {
            get
            {
                if (resourceMgr == null)
                {
                    try
                    {
                        resourceMgr = new MIIPLResourceMgr();
                        resourceMgr.MaxCount = Convert.ToInt32(CONFIGURATIONCONFIGDATA.MaxConnection);
                        resourceMgr.MinCount = Convert.ToInt32(CONFIGURATIONCONFIGDATA.MaxConnection);
                        resourceMgr.BatchCount = Convert.ToInt32(CONFIGURATIONCONFIGDATA.MaxConnection);
                        resourceMgr.CreateItem += new EventHandler<MIIPLResourceEventArgs>(resourceMgr_CreateItem);
                    }
                    catch(Exception ex)
                    {  }
                }                
                return resourceMgr;
            }
            set { resourceMgr = value; }
        }

        public MIIPLResourceMgr ResourceMgr2
        {
            get
            {
                if (resourceMgr == null)
                {
                    try
                    {
                        resourceMgr = new MIIPLResourceMgr();
                        resourceMgr.MaxCount = Convert.ToInt32(CONFIGURATIONCONFIGDATA.MaxConnection);
                        resourceMgr.MinCount = Convert.ToInt32(CONFIGURATIONCONFIGDATA.MaxConnection);
                        resourceMgr.BatchCount = Convert.ToInt32(CONFIGURATIONCONFIGDATA.MaxConnection);
                        resourceMgr.CreateItem2 += new EventHandler<MIIPLResourceEventArgs>(resourceMgr_CreateItem2);
                    }
                    catch (Exception ex)
                    { }
                }
                return resourceMgr;
            }
            set { resourceMgr = value; }
        }


        private void resourceMgr_CreateItem(object sender, MIIPLResourceEventArgs e)
        {
            try
            {
                HostConnectionResource _connectionResource = new HostConnectionResource();
                _connectionResource.RemoteObject = (ITerminalRequest)Activator.GetObject
                    (typeof(ITerminalRequest), CONFIGURATIONCONFIGDATA.HostInterfaceAddress);
                e.Item = _connectionResource;
            }
            catch(Exception ex)
            { SystemLogger.WriteErrorLog(this, ex); }
        }

        private void resourceMgr_CreateItem2(object sender, MIIPLResourceEventArgs e)
        {
            try
            {
                HostConnectionResource _connectionResource = new HostConnectionResource();
                _connectionResource.RemoteObject = (ITerminalRequest)Activator.GetObject
                    (typeof(ITerminalRequest), CONFIGURATIONCONFIGDATA.HostInterfaceAddress2);
                e.Item = _connectionResource;
            }
            catch (Exception ex)
            { SystemLogger.WriteErrorLog(this, ex); }
        }

    }

    public class HostConnectionResource : iMIIPLResourceItem
    {
        public ITerminalRequest RemoteObject = null;
        public bool IsDirty { get; set; }
        public ResourceStatusChanged TaskCompleted { get; set; }
        public ResourceStatusChanged RemoveItem { get; set; }
        public DateTime ItemRunningSince { get; set; }
        public string ItemKey { get; set; }
        public void Dispose()
        {

        }
    }

    public interface iMIIPLResourceItem : IDisposable
    {
        bool IsDirty { get; set; }
        ResourceStatusChanged TaskCompleted { get; set; }
        ResourceStatusChanged RemoveItem { get; set; }
        DateTime ItemRunningSince { get; set; }
        string ItemKey { get; set; }
    }

    public class MIIPLResourceEventArgs : EventArgs
    {
        public iMIIPLResourceItem Item { get; set; }
        public MIIPLResourceEventArgs() : this(null) { }
        public MIIPLResourceEventArgs(iMIIPLResourceItem item) { Item = item; }
    }

    public delegate void ResourceStatusChanged(object sender, EventArgs e);

    public class MIIPLResourceMgr 
    {
        public SystemLogger SystemLogger = null;
        public CommonLogger CommonLogger = null;
        public MIIPLResourceMgr()
        {
            lockObj = new object();
            MinCount = Convert.ToInt32(CONFIGURATIONCONFIGDATA.MinConnection);
            BatchCount = CONFIGURATIONCONFIGDATA.BatchCount;
            MaxCount = Convert.ToInt32(CONFIGURATIONCONFIGDATA.MaxConnection);
            MaxWaitingTime = Convert.ToInt32(CONFIGURATIONCONFIGDATA.MaxWaitingTime);
            RunningItemStatus = Convert.ToInt32(CONFIGURATIONCONFIGDATA.RunningItemStatus);
            IsActive = true;
            SystemLogger = new SystemLogger();
            CommonLogger = new CommonLogger();
        }

        internal int MinCount { get; set; }
        internal int MaxCount { get; set; }
        internal int BatchCount { get; set; }
        internal int MaxWaitingTime { get; set; }
        internal int RunningItemStatus { get; set; }
        internal bool IsActive { get; set; }

        private Queue<iMIIPLResourceItem> idleResources;
        private Queue<iMIIPLResourceItem> IdleResources
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

        internal event EventHandler<MIIPLResourceEventArgs> CreateItem = null;

        internal event EventHandler<MIIPLResourceEventArgs> CreateItem2 = null;

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
                        arg.Item.RemoveItem += new ResourceStatusChanged(RemoveItem);
                        arg.Item.TaskCompleted += new ResourceStatusChanged(TaskCompleted);

                        lock (lockObj)
                        {
                            IdleResources.Enqueue(arg.Item);
                        }
                        SystemLogger.WriteTransLog(this, "Resource Manager : " + arg.Item.ToString() + " created and added to idle resource list");
                       
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLogger.WriteErrorLog(this, ex);
            }
        }

        private void OnCreateItem2()
        {
            try
            {
                if (CreateItem2 != null)
                {
                    MIIPLResourceEventArgs arg = new MIIPLResourceEventArgs();
                    CreateItem2(this, arg);

                    if (arg.Item != null)
                    {
                        arg.Item.RemoveItem += new ResourceStatusChanged(RemoveItem);
                        arg.Item.TaskCompleted += new ResourceStatusChanged(TaskCompleted);

                        lock (lockObj)
                        {
                            IdleResources.Enqueue(arg.Item);
                        }
                        SystemLogger.WriteTransLog(this, "Resource Manager : " + arg.Item.ToString() + " created and added to idle resource list");

                    }
                }
            }
            catch (Exception ex)
            {
                SystemLogger.WriteErrorLog(this, ex);
            }
        }


        private void RemoveItem(object sender, EventArgs e)
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
                                SystemLogger.WriteTraceLog(this, "Resource Manager : " + item.ToString() + " removed from running resource list");
                                item.Dispose();
                                item = null;
                            }
                            else
                            {
                                SystemLogger.WriteTraceLog(this, "Resource Manager : unable to remove " + item.ToString() + " from running resource list");
                            }
                        }
                        else if (MaxCount > IdleResources.Count + RunningResources.Count)
                            CreateItems();
                        else if (IdleResources.Contains(item))
                        {
                            item.IsDirty = true;
                            item.Dispose();
                            item = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLogger.WriteErrorLog(this, ex);
            }
        }

        private void TaskCompleted(object sender, EventArgs e)
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
                            SystemLogger.WriteTraceLog(this, "Resource Manager : " + item.ToString() + " removed from running resource list");
                        else
                            SystemLogger.WriteTraceLog(this, "Resource Manager : unable to remove " + item.ToString() + " from running resource list");
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
                SystemLogger.WriteErrorLog(this, ex);
            }
        }

        private void RemoveItem2(object sender, EventArgs e)
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
                                SystemLogger.WriteTraceLog(this, "Resource Manager : " + item.ToString() + " removed from running resource list");
                                item.Dispose();
                                item = null;
                            }
                            else
                            {
                                SystemLogger.WriteTraceLog(this, "Resource Manager : unable to remove " + item.ToString() + " from running resource list");
                            }
                        }
                        else if (MaxCount > IdleResources.Count + RunningResources.Count)
                            CreateItems2();
                        else if (IdleResources.Contains(item))
                        {
                            item.IsDirty = true;
                            item.Dispose();
                            item = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLogger.WriteErrorLog(this, ex);
            }
        }

        private void TaskCompleted2(object sender, EventArgs e)
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
                            SystemLogger.WriteTraceLog(this, "Resource Manager : " + item.ToString() + " removed from running resource list");
                        else
                            SystemLogger.WriteTraceLog(this, "Resource Manager : unable to remove " + item.ToString() + " from running resource list");
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
                SystemLogger.WriteErrorLog(this, ex);
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

                    SystemLogger.WriteTraceLog(this, "IdleResources : " + IdleResources.Count + " RunningResources : " + RunningResources.Count);

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
                                SystemLogger.WriteTransLog(this, "RunningResources Item Removed Due To Not Used From Long Time " + " key : " + RunninngItem.ItemRunningSince);
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
                            SystemLogger.WriteTransLog(this, "Resource Manager : Waiting for IdleResource");
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
                    SystemLogger.WriteTransLog(this, "After IdleResources : " + IdleResources.Count + " After RunningResources : " + RunningResources.Count + "             key     add  " + item.ItemRunningSince);
                    SystemLogger.WriteTransLog(this, "Resource Manager : Item Dequeued From IdleResource");
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
                SystemLogger.WriteErrorLog(this, ex);
            }
            return item;
        }

        public iMIIPLResourceItem GetItem2()
        {
            iMIIPLResourceItem item = null;
            try
            {
                lock (lockObj)
                {
                    if (IdleResources.Count + RunningResources.Count == 0)
                        OnCreateItem2();

                    SystemLogger.WriteTraceLog(this, "IdleResources : " + IdleResources.Count + " RunningResources : " + RunningResources.Count);

                    if (MaxCount > IdleResources.Count + RunningResources.Count)
                        CreateItems2();

                    if (RunningResources.Count > 0)
                    {
                        for (int i = 0; i < RunningResources.Count; i++)
                        {
                            iMIIPLResourceItem RunninngItem = (iMIIPLResourceItem)RunningResources[i];
                            if ((DateTime.Now - RunninngItem.ItemRunningSince).Minutes >= RunningItemStatus)
                            {
                                RunningResources.Remove(RunninngItem);
                                SystemLogger.WriteTransLog(this, "RunningResources Item Removed Due To Not Used From Long Time " + " key : " + RunninngItem.ItemRunningSince);
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
                            SystemLogger.WriteTransLog(this, "Resource Manager : Waiting for IdleResource");
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
                    SystemLogger.WriteTransLog(this, "After IdleResources : " + IdleResources.Count + " After RunningResources : " + RunningResources.Count + "             key     add  " + item.ItemRunningSince);
                    SystemLogger.WriteTransLog(this, "Resource Manager : Item Dequeued From IdleResource");
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
                SystemLogger.WriteErrorLog(this, ex);
            }
            return item;
        }


        #region Create Iteam
        private delegate void CreateItemsHandler();
        private CreateItemsHandler handler = null;
        private bool createItemsRunning = false;
        protected void CreateItems()
        {

            if (this.IsActive)
            {
                if (handler == null)
                    handler = new CreateItemsHandler(InternalCreateItems);
                handler.BeginInvoke(null, null);
            }
        }

        protected void CreateItems2()
        {

            if (this.IsActive)
            {
                if (handler == null)
                    handler = new CreateItemsHandler(InternalCreateItems);
                handler.BeginInvoke(null, null);
            }
        }

        private void InternalCreateItems()
        {
            SystemLogger.WriteTransLog(this, "Before Items creation");
            if (IdleResources.Count <= MinCount && !createItemsRunning)
            {
                createItemsRunning = true;

                int CurrentResource = (IdleResources.Count + RunningResources.Count);
                int NewResource = ((MaxCount - CurrentResource) > BatchCount) ? BatchCount : (MaxCount - CurrentResource);
                for (int i = 0; i < NewResource; i++)
                {
                    OnCreateItem();
                    CurrentResource = (IdleResources.Count + RunningResources.Count);
                    SystemLogger.WriteTransLog(this, "New Item Created : " + i);
                }
                createItemsRunning = false;
            }
        }
        #endregion
    }
}
