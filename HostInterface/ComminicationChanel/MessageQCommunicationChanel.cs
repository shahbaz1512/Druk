/* Class used for connecting Message Queue */
using MaxiSwitch.API.Terminal;
using TransactionRouter.CommunicationManager.MessageQue;
using System;
using MaxiSwitch.Common.TerminalLogger;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using MaxiSwitch.DALC.Configuration;
//using TransactionRouter.Logger;
//using TransactionRouter.ResponseCodeDetails;

namespace TransactionRouter.CommunicationChanel.MessageQue
{
    public delegate SwitchConsumerRequestReqMsg SendTransactionRequest(ref SwitchConsumerRequestReqMsg SwitchTransRequest);
    public delegate SwitchConsumerRequestReqMsg SendReversalTransactionRequest(ref SwitchConsumerRequestReqMsg SwitchTransRequest);
    public delegate void SendAcknowedgmentRequest(ref SwitchConsumerRequestReqMsg SwitchTransRequest);


    public class MessageQCommunicationChanel
    {   
        public SendTransactionRequest SendTransRequest { get; set; }
        public SendReversalTransactionRequest SendReversalTransRequest { get; set; }
        public SendAcknowedgmentRequest SendAckRequest { get; set; }
        public SystemLogger SystemLogger = null;
        public CommonLogger CommonLogger = null;
    

        public MessageQCommunicationChanel()
        {
            SendTransRequest = new SendTransactionRequest(ref TransactionRequest);
            SendReversalTransRequest = new SendReversalTransactionRequest(ref ReversalTransactionRequest);
            SendAckRequest = new SendAcknowedgmentRequest(ref AcknowedgmentRequest);
            SystemLogger = new SystemLogger();
            CommonLogger = new CommonLogger();                                       

        }

        #region Switch Connection Manager
        static MessageQCommManager _mqueConnectionManager = null;

        public static MessageQCommManager MessageQConnectionManager
        {
            get
            {
                if (_mqueConnectionManager == null)
                {
                    _mqueConnectionManager = new MessageQCommManager();                    
                }
                return _mqueConnectionManager;
            }
            set { _mqueConnectionManager = value; }
        }
        #endregion

        #region Remort Commands

        internal SwitchConsumerRequestReqMsg TransactionRequest(ref SwitchConsumerRequestReqMsg SwitchRequestMsg)
        {
            ////try
            ////{
          
            MessageQCommunicationChanel _MessageQCommunicationChanel = new MessageQCommunicationChanel();
                var Resource = ((MessageQConnectionResource)MessageQConnectionManager.ResourceMgr.GetItem());
                ////var TransactionRequest = Resource.RemoteObject.TransactionRequest(MsgConvertor(SwitchRequestMsg));
                if (Resource != null)
                {
                    var TransactionRequest = Resource.RemoteObject.TransactionRequest(SwitchRequestMsg);
                    Resource.TaskComplited.Invoke(Resource, new EventArgs());
                    SwitchRequestMsg = MsgConvertor(SwitchRequestMsg, TransactionRequest);
                }
                else
                {
                    _MessageQCommunicationChanel.SystemLogger.WriteTraceLog(null, "Transaction Resource Not Found Or Busy " + SwitchRequestMsg.TransactionRefrenceNumber);
                    ProcessUnsuccessfulTransaction(ref SwitchRequestMsg);
                }

                return SwitchRequestMsg;
            ////}
            ////catch(Exception ex)
            ////{ SwitchLogger.WriteErrorLog(this, ex); ProcessUnsuccessfulTransaction(ref SwitchRequestMsg); return SwitchRequestMsg; }
        }

        internal SwitchConsumerRequestReqMsg TransactionRequestNew(ref SwitchConsumerRequestReqMsg SwitchRequestMsg)
        {
            MessageQCommunicationChanel _MessageQCommunicationChanel = new MessageQCommunicationChanel();
            var Resource = (ITerminalRequest)Activator.GetObject(typeof(ITerminalRequest), CONFIGURATIONCONFIGDATA.MessageQueAddress);
            _MessageQCommunicationChanel.SystemLogger.WriteTraceLog(null, "MQ address : " + CONFIGURATIONCONFIGDATA.MessageQueAddress + " Resources : " + Resource);
            if (Resource != null)
            {
                var TransactionRequest = Resource.TransactionRequest(SwitchRequestMsg);
                SwitchRequestMsg = MsgConvertor(SwitchRequestMsg, TransactionRequest);
            }
            else
            {
                _MessageQCommunicationChanel.SystemLogger.WriteTraceLog(null, "Transaction Resource Not Found Or Busy " + SwitchRequestMsg.TransactionRefrenceNumber);
                ProcessUnsuccessfulTransaction(ref SwitchRequestMsg);
            }
            return SwitchRequestMsg;
            ////}
            ////catch(Exception ex)
            ////{ SwitchLogger.WriteErrorLog(this, ex); ProcessUnsuccessfulTransaction(ref SwitchRequestMsg); return SwitchRequestMsg; }
        }


        internal SwitchConsumerRequestReqMsg ReversalTransactionRequest(ref SwitchConsumerRequestReqMsg SwitchRequestMsg)
        {
            ////try
            ////{
            MessageQCommunicationChanel _MessageQCommunicationChanel = new MessageQCommunicationChanel();
                var Resource = ((MessageQConnectionResource)MessageQConnectionManager.ResourceMgr.GetItem());
                ////var ReversalResponse = Resource.RemoteObject.TransactionRequest(MsgConvertor(SwitchRequestMsg));
                if (Resource != null)
                {
                    var ReversalResponse = Resource.RemoteObject.ReversalTransactionRequest(SwitchRequestMsg);
                    Resource.TaskComplited.Invoke(Resource, new EventArgs());
                    SwitchRequestMsg = MsgConvertor(SwitchRequestMsg, ReversalResponse);
                }
                else
                {
                    _MessageQCommunicationChanel.SystemLogger.WriteTraceLog(null, "Reversal Transaction Resource Not Found Or Busy " + SwitchRequestMsg.TransactionRefrenceNumber);
                    ProcessUnsuccessfulTransaction(ref SwitchRequestMsg);
                }
                return SwitchRequestMsg;
            ////}
            ////catch(Exception ex)
            ////{ SwitchLogger.WriteErrorLog(this, ex); ProcessUnsuccessfulTransaction(ref SwitchRequestMsg); return SwitchRequestMsg; }
        }

        internal void AcknowedgmentRequest(ref SwitchConsumerRequestReqMsg SwitchRequestMsg)
        {
            ////try
            ////{
            MessageQCommunicationChanel _MessageQCommunicationChanel = new MessageQCommunicationChanel();
                var Resource = ((MessageQConnectionResource)MessageQConnectionManager.ResourceMgr.GetItem());
                ////var AckRequest = Resource.RemoteObject.TransactionRequest(MsgConvertor(SwitchRequestMsg));
                if (Resource != null)
                {
                    var AckRequest = Resource.RemoteObject.TransactionRequest(SwitchRequestMsg);
                    Resource.TaskComplited.Invoke(Resource, new EventArgs());
                    SwitchRequestMsg = MsgConvertor(SwitchRequestMsg, AckRequest);
                }
                else
                {
                   _MessageQCommunicationChanel.SystemLogger.WriteTraceLog(null, "Accounting Transaction Resource Not Found Or Busy " + SwitchRequestMsg.TransactionRefrenceNumber);
                    ProcessUnsuccessfulTransaction(ref SwitchRequestMsg);
                }
                
            ////}
            ////catch(Exception ex)
            ////{ SwitchLogger.WriteErrorLog(this, ex); }
           ////// return SwitchRequestMsg;           
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

        #region Switch Message Formatting

        public SwitchConsumerRequestReqMsg MsgConvertor(SwitchConsumerRequestReqMsg ReqMsg)
        {
            SwitchConsumerRequestReqMsg _switchConsumerRequestReqMsg = new SwitchConsumerRequestReqMsg()
            {
                ////******************Switch to MessageQue (Member Variable Casting For Request)
            
            };
            return _switchConsumerRequestReqMsg;
        }

        public SwitchConsumerRequestReqMsg MsgConvertor(SwitchConsumerRequestReqMsg SwitchReqMsg, SwitchConsumerRequestReqMsg SwitchRespMsg)
        {
            MessageQCommunicationChanel _MessageQCommunicationChanel = new MessageQCommunicationChanel();
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
                SwitchReqMsg.CardAccepterName = SwitchRespMsg.CardAccepterName;
                SwitchReqMsg.BalanceAmount = SwitchRespMsg.BalanceAmount;
                SwitchReqMsg.FromAccountNumber = SwitchRespMsg.FromAccountNumber;
                SwitchReqMsg.MiniStateMentData = SwitchRespMsg.MiniStateMentData;
                SwitchReqMsg.CustomerName = SwitchRespMsg.CustomerName;
                SwitchReqMsg.AdditionalPrivateData = SwitchRespMsg.AdditionalPrivateData;
                _MessageQCommunicationChanel.SystemLogger.WriteTransLog(this, "SwitchRespMsg.CustomerName : "+ SwitchRespMsg.CustomerName);
                _MessageQCommunicationChanel.SystemLogger.WriteTransLog(this, "SwitchRespMsg.AdditionalPrivateData : " + SwitchRespMsg.AdditionalPrivateData);

            }
            catch (Exception ex)
            { _MessageQCommunicationChanel.SystemLogger.WriteErrorLog(this, ex); }
            return SwitchReqMsg;
        }

        #endregion Switch Message Formatting
    }
}
