using DALC;
using DbNetLink.Data;
using MaxiSwitch.DALC.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading;
using TransactionRouter.CommunicationChanel.HSM;

namespace TransactionRouter.CommunicationManager.HSM
{
    public class HsmConnectionManager 
    {
        public interface IAuthenticationRequest
        {
            Authentication AuthenticationRequest(Authentication RequestMsg);
        }
        public HsmConnectionManager()
        {   
           
        }
        HsmCommunicationChanel _CommanDetails = new HsmCommunicationChanel();
        public string HsmAddress { get; set; }

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
                        resourceMgr.MinCount = Convert.ToInt32(CONFIGURATIONCONFIGDATA.MinConnection);
                        resourceMgr.BatchCount = Convert.ToInt32(CONFIGURATIONCONFIGDATA.BatchCount);                        
                        resourceMgr.CreateItem += new EventHandler<MIIPLResourceEventArgs>(resourceMgr_CreateItem);
                    }
                    catch(Exception ex)
                    { _CommanDetails.SystemLogger.WriteErrorLog(this, ex); }
                }                
                return resourceMgr;
            }
            set { resourceMgr = value; }
        }

        private void resourceMgr_CreateItem(object sender, MIIPLResourceEventArgs e)
        {
            try
            {
                HsmConnectionResource _connectionResource = new HsmConnectionResource();
                _connectionResource.RemoteObject = (IAuthenticationRequest)Activator.GetObject
                    (typeof(IAuthenticationRequest), CONFIGURATIONCONFIGDATA.HSMIP);
                e.Item = _connectionResource;
            }
            catch(Exception ex)
            { _CommanDetails.SystemLogger.WriteErrorLog(this, ex); }
        }
    }

    public class HsmConnectionResource : iMIIPLResourceItem
    {
        public TransactionRouter.CommunicationManager.HSM.HsmConnectionManager.IAuthenticationRequest RemoteObject = null;
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
        HsmCommunicationChanel _CommanDetails = new HsmCommunicationChanel();
        public MIIPLResourceMgr()
        {
            lockObj = new object();
            MinCount = Convert.ToInt32(CONFIGURATIONCONFIGDATA.MinConnection);
            BatchCount = Convert.ToInt32(CONFIGURATIONCONFIGDATA.BatchCount);
            MaxCount = Convert.ToInt32(CONFIGURATIONCONFIGDATA.MaxConnection);
            MaxWaitingTime = Convert.ToInt32(CONFIGURATIONCONFIGDATA.MaxWaitingTime);
            RunningItemStatus = Convert.ToInt32(CONFIGURATIONCONFIGDATA.RunningItemStatus);
            IsActive = true;
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
                        _CommanDetails.SystemLogger.WriteTransLog(this, "Resource Manager : " + arg.Item.ToString() + " created and added to idle resource list");
                    }
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
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
                                _CommanDetails.SystemLogger.WriteTransLog(this, "Resource Manager : " + item.ToString() + " removed from running resource list" + " Key Removed : " + item.ItemRunningSince);
                                item.Dispose();
                                item = null;
                            }
                            else
                            {
                                _CommanDetails.SystemLogger.WriteTransLog(this, "Resource Manager : unable to remove " + item.ToString() + " from running resource list" + " Key Removed : " + item.ItemRunningSince);
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
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
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
                            _CommanDetails.SystemLogger.WriteTransLog(this, "Resource Manager : " + item.ToString() + " removed from running resource list" + " Key Removed : " + item.ItemRunningSince);
                        else
                            _CommanDetails.SystemLogger.WriteTransLog(this, "Resource Manager : unable to remove " + item.ToString() + " from running resource list" + " Key Not removed : " + item.ItemRunningSince);
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
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
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

                    _CommanDetails.SystemLogger.WriteTransLog(this, "IdleResources : " + IdleResources.Count + " RunningResources : " + RunningResources.Count);

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
                                _CommanDetails.SystemLogger.WriteTransLog(this, "RunningResources Item Removed Due To Not Used From Long Time " + " key : " + RunninngItem.ItemRunningSince);
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
                            _CommanDetails.SystemLogger.WriteTransLog(this, "Resource Manager : Waiting for IdleResource");
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
                    _CommanDetails.SystemLogger.WriteTransLog(this, "After IdleResources : " + IdleResources.Count + " After RunningResources : " + RunningResources.Count + "             key     add  " + item.ItemRunningSince);
                    _CommanDetails.SystemLogger.WriteTransLog(this, "Resource Manager : Item Dequeued From IdleResource");
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
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
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
        private void InternalCreateItems()
        {
            _CommanDetails.SystemLogger.WriteTransLog(this, "Before Items creation");
            if (IdleResources.Count <= MinCount && !createItemsRunning)
            {
                createItemsRunning = true;

                int CurrentResource = (IdleResources.Count + RunningResources.Count);
                int NewResource = ((MaxCount - CurrentResource) > BatchCount) ? BatchCount : (MaxCount - CurrentResource);
                for (int i = 0; i < NewResource; i++)
                {
                    OnCreateItem();
                    CurrentResource = (IdleResources.Count + RunningResources.Count);
                    _CommanDetails.SystemLogger.WriteTransLog(this, "New Item Created : " + i);
                }
                createItemsRunning = false;
            }
        }
        #endregion
    }
    public static class HSMConfigurationDetails
    {
        public static string HsmAddress = string.Empty;
        public static string ConnectionString = string.Empty;
        public static string Provider = string.Empty;
        public static bool ConfigHID { get; set; }
        public static bool LiquidityManager { get; set; }
        public static string MaxConnection = string.Empty;
        public static string MinConnection = string.Empty;
        public static string BatchCount = string.Empty;
        public static string SecurityModuleType { get; set; }
        public static int MaxWaitingTime { get; set; }
        public static int RunningItemStatus { get; set; }
    }
}
