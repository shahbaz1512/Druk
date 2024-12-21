/* Class used for connecting Host Interface */
using MaxiSwitch.API.Terminal;
using TransactionRouter.CommunicationManager.Host;
using System;
using MaxiSwitch.Common.TerminalLogger;
using MaxiSwitch.DALC.Configuration;
using System.Configuration;

namespace TransactionRouter.CommunicationChanel.Host
{
    public delegate SwitchConsumerRequestReqMsg SendTransactionRequest(ref SwitchConsumerRequestReqMsg SwitchTransRequest);
    public delegate SwitchConsumerRequestReqMsg SendReversalTransactionRequest(ref SwitchConsumerRequestReqMsg SwitchTransRequest);
    public delegate void SendAcknowedgmentRequest(ref SwitchConsumerRequestReqMsg SwitchTransRequest);

    public class HostCommChanel
    {
        public SystemLogger SystemLogger = null;
        public CommonLogger CommonLogger = null;
        public SendTransactionRequest SendTransRequest { get; set; }
        public SendReversalTransactionRequest SendReversalTransRequest { get; set; }
        public SendAcknowedgmentRequest SendAckRequest { get; set; }

        public static int CBSCounter = 0;

        public HostCommChanel()
        {
            SendTransRequest = new SendTransactionRequest(ref TransactionRequest);
            SendReversalTransRequest = new SendReversalTransactionRequest(ref ReversalTransactionRequest);
            SendAckRequest = new SendAcknowedgmentRequest(ref AcknowedgmentRequest);
            try
            {
                SystemLogger = new SystemLogger();
                CommonLogger = new CommonLogger();
            }
            catch { }
        }

        #region Host Connection Manager
        static HostConnectionManager _hostConnectionManager = null;
        public static HostConnectionManager HostConnectionManager
        {
            get
            {
                if (_hostConnectionManager == null)
                {
                    _hostConnectionManager = new HostConnectionManager();
                }
                return _hostConnectionManager;
            }
            set { _hostConnectionManager = value; }
        }
        #endregion

        #region Remort Commands

        //internal SwitchConsumerRequestReqMsg TransactionRequest(ref SwitchConsumerRequestReqMsg SwitchRequestMsg)
        //{
        //    CBSCounter = CBSCounter + 1;
        //    HostCommChanel _HostCommChanel = new HostCommChanel();
        //    if (CBSCounter % 2 == 0)
        //    {
        //            var Resource = (ITerminalRequest)Activator.GetObject
        //            (typeof(ITerminalRequest), CONFIGURATIONCONFIGDATA.HostInterfaceAddress);
        //           var TransactionRequest = Resource.TransactionRequest(SwitchRequestMsg);
        //           SystemLogger.WriteTransLog(null, "Request sent to HostInterface1");
        //            SwitchRequestMsg = MsgConvertor(SwitchRequestMsg, TransactionRequest);
        //    }
        //   else if (CBSCounter % 2 != 0)
        //    {
        //        var Resource = (ITerminalRequest)Activator.GetObject
        //            (typeof(ITerminalRequest), CONFIGURATIONCONFIGDATA.HostInterfaceAddress2);
        //          var  TransactionRequest = Resource.TransactionRequest(SwitchRequestMsg);
        //        SystemLogger.WriteTransLog(null, "Request sent to HostInterface2");
        //        SwitchRequestMsg = MsgConvertor(SwitchRequestMsg, TransactionRequest);
        //    }
        //    else
        //    {
        //        SystemLogger.WriteTraceLog(null, "Transaction Resource Not Found Or Busy " + SwitchRequestMsg.TransactionRefrenceNumber);
        //        ProcessUnsuccessfulTransaction(ref SwitchRequestMsg);
        //    }
        //    return SwitchRequestMsg;
        //}


        // new method for 3 interface

        internal SwitchConsumerRequestReqMsg TransactionRequest(ref SwitchConsumerRequestReqMsg SwitchRequestMsg)
        {
            CBSCounter = CBSCounter + 1;
            HostCommChanel _HostCommChanel = new HostCommChanel();
           if( SwitchRequestMsg.TransactionType == MaxiSwitch.API.Terminal.enumTransactionType.FundTransfer || SwitchRequestMsg.TransactionType == MaxiSwitch.API.Terminal.enumTransactionType.Debit || SwitchRequestMsg.TransactionType == MaxiSwitch.API.Terminal.enumTransactionType.AccountVerification)
            {
                var Resource = (ITerminalRequest)Activator.GetObject
                                (typeof(ITerminalRequest), CONFIGURATIONCONFIGDATA.HostInterfaceAddress2);
                var TransactionRequest = Resource.TransactionRequest(SwitchRequestMsg);
                SystemLogger.WriteTransLog(null, "Request sent to HostInterface2");
                SwitchRequestMsg = MsgConvertor(SwitchRequestMsg, TransactionRequest);
            }
            else
            {
                var Resource = (ITerminalRequest)Activator.GetObject
                                (typeof(ITerminalRequest), CONFIGURATIONCONFIGDATA.HostInterfaceAddress);
                var TransactionRequest = Resource.TransactionRequest(SwitchRequestMsg);
                SystemLogger.WriteTransLog(null, "Request sent to HostInterface1");
                SwitchRequestMsg = MsgConvertor(SwitchRequestMsg, TransactionRequest);
            }
            //else
            //{
            //    SystemLogger.WriteTraceLog(null, "Transaction Resource Not Found Or Busy " + SwitchRequestMsg.TransactionRefrenceNumber);
            //    ProcessUnsuccessfulTransaction(ref SwitchRequestMsg);
            //}
            return SwitchRequestMsg;
        }


        internal SwitchConsumerRequestReqMsg ReversalTransactionRequest(ref SwitchConsumerRequestReqMsg SwitchRequestMsg)
        {
            ////try
            ////{
            var Resource = ((HostConnectionResource)HostConnectionManager.ResourceMgr.GetItem());
            if (Resource != null)
            {
                var ReversalResponse = Resource.RemoteObject.ReversalTransactionRequest(SwitchRequestMsg);
                Resource.TaskCompleted.Invoke(Resource, new EventArgs());
                SwitchRequestMsg = MsgConvertor(SwitchRequestMsg, ReversalResponse);
            }
            else
            {
                SystemLogger.WriteTraceLog(null, "Reversal Transaction Resource Not Found Or Busy " + SwitchRequestMsg.TransactionRefrenceNumber);
                ProcessUnsuccessfulTransaction(ref SwitchRequestMsg);
            }

            return SwitchRequestMsg;
            ////}
            ////catch(Exception ex)
            ////{ SwitchLogger.WriteErrorLog(this, ex); return SwitchRequestMsg; }
        }

        internal void AcknowedgmentRequest(ref SwitchConsumerRequestReqMsg SwitchRequestMsg)
        {
            ////try
            ////{
            var Resource = ((HostConnectionResource)HostConnectionManager.ResourceMgr.GetItem());
            if (Resource != null)
            {
                var AckRequest = Resource.RemoteObject.AcknowedgmentRequest(SwitchRequestMsg);
                Resource.TaskCompleted.Invoke(Resource, new EventArgs());
                SwitchRequestMsg = MsgConvertor(SwitchRequestMsg, AckRequest);
            }
            else
            {
                SystemLogger.WriteTraceLog(null, "Accounting Transaction Resource Not Found Or Busy " + SwitchRequestMsg.TransactionRefrenceNumber);
                ProcessUnsuccessfulTransaction(ref SwitchRequestMsg);
            }
            ////}
            ////catch(Exception ex)
            ////{ SwitchLogger.WriteErrorLog(this, ex); }
            ////return SwitchRequestMsg;           
        }

        void ProcessUnsuccessfulTransaction(ref SwitchConsumerRequestReqMsg RequestMsg)
        {
            if (RequestMsg.CommandType == enumCommandTypeEnum.AuthorizationRequestMessage)
                RequestMsg.CommandType = enumCommandTypeEnum.AuthorizationResponseMessage;
            else if (RequestMsg.CommandType == enumCommandTypeEnum.ReversalAdviceRequestMessage || RequestMsg.CommandType == enumCommandTypeEnum.ReversalAdviceRequestRepeatMessage)
                RequestMsg.CommandType = enumCommandTypeEnum.ReversalAdviceResponseMessage;
            else if (RequestMsg.CommandType == enumCommandTypeEnum.AccountingAuthorizationRequestMessage)
                RequestMsg.CommandType = enumCommandTypeEnum.AccountingAuthorizationResponseMessage;
            RequestMsg.TransStatus = enumTransactionStatus.UnSuccessful;
            // RequestMsg.ResponseCode = CommonResponseCodeDetails.GetSwitchResponseCode((int)enumResponseCode.IssuerDown);
        }

        #endregion

        #region Host Message Formatting

        //public SwitchConsumerRequestReqMsg MsgConvertor(SwitchConsumerRequestReqMsg ReqMsg)
        //{
        //    SwitchConsumerRequestReqMsg _switchConsumerRequestReqMsg = new SwitchConsumerRequestReqMsg()
        //    {
        //        ////******************Switch to MessageQue (Member Variable Casting For Request)

        //    };
        //    return _switchConsumerRequestReqMsg;
        //}

        public SwitchConsumerRequestReqMsg MsgConvertor(SwitchConsumerRequestReqMsg SwitchReqMsg, SwitchConsumerRequestReqMsg SwitchRespMsg)
        {
            try
            {
                ////******************Switch to MessageQue (Member Variable Casting of Response)
                SwitchReqMsg.CommandType = (enumCommandTypeEnum)SwitchRespMsg.CommandType;
                SwitchReqMsg.TransStatus = (enumTransactionStatus)SwitchRespMsg.TransStatus;
                SwitchReqMsg.CardNumber = SwitchRespMsg.CardNumber;
                SwitchReqMsg.ProcessingCode = SwitchRespMsg.ProcessingCode;
                ////SwitchReqMsg.TransactionAmount = SwitchRespMsg.TransactionAmount.ToString().PadLeft(8, '0');
                SwitchReqMsg.SystemsTraceAuditNumber = SwitchRespMsg.SystemsTraceAuditNumber.ToString();
                //RequestMsg.LocalTransactionDateTime =DateTime.ParseExact(cmdData.TransmissionDateTime);               
                //RequestMsg.Track2Data = cmdData.Track2Data;
                SwitchReqMsg.ReferenceNumber = SwitchRespMsg.ReferenceNumber;
                SwitchReqMsg.TransactionRefrenceNumber = SwitchRespMsg.TransactionRefrenceNumber;
                SwitchReqMsg.AuthorizationCode = SwitchRespMsg.AuthorizationCode;
                SwitchReqMsg.ResponseCode = (string.IsNullOrEmpty(SwitchRespMsg.ResponseCode) ? "91" : SwitchRespMsg.ResponseCode.ToString().PadLeft(2, '0'));
                SwitchReqMsg.TerminalID = SwitchRespMsg.TerminalID;
                SwitchReqMsg.TerminalLocation = SwitchRespMsg.TerminalLocation;
                ////SwitchReqMsg.CardAccepterName = SwitchRespMsg.CardAccepterName;
                SwitchReqMsg.BalanceAmount = SwitchRespMsg.BalanceAmount;
                SwitchReqMsg.FromAccountNumber = SwitchRespMsg.FromAccountNumber;
                SwitchReqMsg.MiniStateMentData = SwitchRespMsg.MiniStateMentData;
                SwitchReqMsg.MobileNumber = SwitchRespMsg.MobileNumber;
                SwitchReqMsg.MobileTopUpNumber = SwitchRespMsg.MobileTopUpNumber;
                SwitchReqMsg.CustomerName = SwitchRespMsg.CustomerName;

            }
            catch (Exception ex)
            { SystemLogger.WriteErrorLog(this, ex); }
            return SwitchReqMsg;
        }

        #endregion Host Message Formatting

    }
}
