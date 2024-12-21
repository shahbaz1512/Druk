using BIPS.Communication;
using DALC;
using HSMCommunicationChanel;
using IMPSTransactionRouter.Controllers;
using MaxiSwitch.DALC.Configuration;
using MaxiSwitch.DALC.ConsumerTransactions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using TransactionRouter.CommunicationChanel.Host;
using TransactionRouter.CommunicationChanel.MessageQue;

namespace IMPSTransactionRouter.Models
{
    public class ProcessHost
    {
        CommanDetails _CommanDetails = new CommanDetails();
        ProcessMessage _ProcessMessage = new ProcessMessage();
        HSMCommunicationChanel.Authentication _Authentication = new HSMCommunicationChanel.Authentication();
        HTTPCommunicationChanel _HTTPCommunicationChanel = new HTTPCommunicationChanel();
        SSM _SSM = new SSM();

        private HostCommChanel hostCommChanel;
        public HostCommChanel HostCommChanel
        {
            get
            {
                if (hostCommChanel == null)
                    hostCommChanel = new HostCommChanel();
                return hostCommChanel;
            }
            set { hostCommChanel = value; }
        }

        private MessageQCommunicationChanel messageQCommunicationChanel;
        public MessageQCommunicationChanel MessageQCommunicationChanel
        {
            get
            {
                if (messageQCommunicationChanel == null)
                    messageQCommunicationChanel = new MessageQCommunicationChanel();
                return messageQCommunicationChanel;
            }
            set { messageQCommunicationChanel = value; }
        }

        public void ProcessBalanceinquiryToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSAccService.QUERYACCBAL_IOFS_REQ _QUERYACCBAL_IOFS_REQ,
                                                                   ref FCUBSAccService.QUERYACCBAL_IOFS_RES _QUERYACCBAL_IOFS_RES, FCUBSAccService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                                   FCUBSAccService.FCUBSAccServiceSEIClient _FCUBSAccService, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(IMPSTransactionRouter.FCUBSAccService.QUERYACCBAL_IOFS_REQ));
                        _serelized.Serialize(xmlWriter, _QUERYACCBAL_IOFS_REQ);
                    }
                    string HostRequestData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostRequestData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Balance inquiry Transaction Request send to HOST for reference number : " + _QUERYACCBAL_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                #region RequestToHost
                try
                {
                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();

                    _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;

                    #region stop to verifing account
                    //else
                    //{
                    //    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                    //    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    //    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    //    _MOBILEBANKING_RESP.ResponseData = null;

                    //    _ProcessMessage.ProcessUnsuccessfullTransactionBLQ(ref _MOBILEBANKING_RESP, _QUERYACCBAL_IOFS_REQ, _QUERYACCBAL_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
                    //    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Account Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                    //                                                , _MOBILEBANKING_REQ.ReferenceNumber, status, _MOBILEBANKING_REQ.ResponseCode));
                    //    return;
                    //}
                    #endregion stop to verifing account

                    // _ProcessMessage.TransactionBalanceinquiry((int)enumCommandTypeEnum.AuthorizationRequestMessage, _QUERYACCBAL_IOFS_REQ, _QUERYACCBAL_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 3);
                    _QUERYACCBAL_IOFS_RES = _FCUBSAccService.QueryAccBalIO(_QUERYACCBAL_IOFS_REQ);
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _ProcessMessage.ProcessUnsuccessfullTransactionBLQ(ref _MOBILEBANKING_RESP, _QUERYACCBAL_IOFS_REQ, _QUERYACCBAL_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(IMPSTransactionRouter.FCUBSAccService.QUERYACCBAL_IOFS_RES));
                        _serelized.Serialize(xmlWriter, _QUERYACCBAL_IOFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Balance inquiry Response recieved from HOST for reference number : " + _QUERYACCBAL_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessBalanceinquiry(ref _MOBILEBANKING_RESP, _QUERYACCBAL_IOFS_REQ, _QUERYACCBAL_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _QUERYACCBAL_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                _ProcessMessage.ProcessUnsuccessfullTransactionBLQ(ref _MOBILEBANKING_RESP, _QUERYACCBAL_IOFS_REQ, _QUERYACCBAL_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
            }
        }

        public void ProcessGetCCOutstandingAmtToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSAccService.QUERYACCBAL_IOFS_REQ _QUERYACCBAL_IOFS_REQ,
                                                                   ref FCUBSAccService.QUERYACCBAL_IOFS_RES _QUERYACCBAL_IOFS_RES, FCUBSAccService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                                   FCUBSAccService.FCUBSAccServiceSEIClient _FCUBSAccService, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(IMPSTransactionRouter.FCUBSAccService.QUERYACCBAL_IOFS_REQ));
                        _serelized.Serialize(xmlWriter, _QUERYACCBAL_IOFS_REQ);
                    }
                    string HostRequestData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostRequestData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Get Credit Card  Outstanding Amount Transaction Request send to HOST for reference number : " + _QUERYACCBAL_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                #region RequestToHost
                try
                {
                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();

                    #region stop to verifing account
                    //_ImpsTransaction.VERIFYIMPSACCOUNT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                    //                                  ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                    //                                  ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                    //                                  ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status);
                    //if (status == 0)
                    //{
                    #endregion stop to verifing account

                    #region FormatedLogWriting
                    _CommanDetails.SystemLogger.WriteTransLog(this,
                     string.Format("CardStatus    : {0}" + "\t" +
                                  "TransType      : {1}" + "\t" +
                                  "PinOffset      : {2}" + "\t" +
                                  "AmountAvailable: {3}" + "\t" +
                                  "FTLimit       : {4}" + "\t" +
                                  "AccountUseLimit   : {5}" + "\t" +
                                  "AccountUseCount   : {6}" + "\t" +
                                  "UsedLastDate   : {7}" + "\t" +
                                  "UsedLastTime   : {8}" + "\t" +
                                  "MaxPinCount    : {9}" + "\t" +
                                  "MaxPinUseCount : {10}" + "\t" +
                                  "AccountNumber  : {11}"
                                   , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                   , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                    #endregion FormatedLogWriting

                    _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;

                    #region stop to verifing account
                    //else
                    //{
                    //    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                    //    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    //    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    //    _MOBILEBANKING_RESP.ResponseData = null;

                    //    _ProcessMessage.ProcessUnsuccessfullTransactionBLQ(ref _MOBILEBANKING_RESP, _QUERYACCBAL_IOFS_REQ, _QUERYACCBAL_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
                    //    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Account Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                    //                                                , _MOBILEBANKING_REQ.ReferenceNumber, status, _MOBILEBANKING_REQ.ResponseCode));
                    //    return;
                    //}
                    #endregion stop to verifing account

                    //_ProcessMessage.TransactionBalanceinquiry((int)enumCommandTypeEnum.AuthorizationRequestMessage, _QUERYACCBAL_IOFS_REQ, _QUERYACCBAL_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 3);
                    _QUERYACCBAL_IOFS_RES = _FCUBSAccService.QueryAccBalIO(_QUERYACCBAL_IOFS_REQ);
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _ProcessMessage.ProcessUnsuccessfullTransactionBLQ(ref _MOBILEBANKING_RESP, _QUERYACCBAL_IOFS_REQ, _QUERYACCBAL_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(IMPSTransactionRouter.FCUBSAccService.QUERYACCBAL_IOFS_RES));
                        _serelized.Serialize(xmlWriter, _QUERYACCBAL_IOFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Get Credit Card  Outstanding Amount Response recieved from HOST for reference number : " + _QUERYACCBAL_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessGetOutstandingAmt(ref _MOBILEBANKING_RESP, _QUERYACCBAL_IOFS_REQ, _QUERYACCBAL_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _QUERYACCBAL_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                _ProcessMessage.ProcessUnsuccessfullTransactionBLQ(ref _MOBILEBANKING_RESP, _QUERYACCBAL_IOFS_REQ, _QUERYACCBAL_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
            }
        }

        public void ProcessMinistatementToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSAccFinService.REQUESTACCSTMT_IOFS_REQ _REQUESTACCSTMT_IOFS_REQ,
                                                                ref FCUBSAccFinService.REQUESTACCSTMT_IOFS_RES _REQUESTACCSTMT_IOFS_RES, FCUBSAccFinService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                                 FCUBSAccFinService.FCUBSAccFinServiceSEIClient _FCUBSAccFinService, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSAccFinService.REQUESTACCSTMT_IOFS_REQ));
                        _serelized.Serialize(xmlWriter, _REQUESTACCSTMT_IOFS_REQ);
                    }
                    string HostRequestData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostRequestData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Ministatement Generate Transaction Request send to HOST for reference number : " + _REQUESTACCSTMT_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                #region RequestToHost
                try
                {
                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();

                    #region stop to verifing account
                    //_ImpsTransaction.VERIFYIMPSACCOUNT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                    //                                  ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                    //                                  ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                    //                                  ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status);
                    //if (status == 0)
                    //{
                    #endregion stop to verifing account

                    #region FormatedLogWriting
                    _CommanDetails.SystemLogger.WriteTransLog(this,
                     string.Format("CardStatus    : {0}" + "\t" +
                                  "TransType      : {1}" + "\t" +
                                  "PinOffset      : {2}" + "\t" +
                                  "AmountAvailable: {3}" + "\t" +
                                  "FTLimit       : {4}" + "\t" +
                                  "AccountUseLimit   : {5}" + "\t" +
                                  "AccountUseCount   : {6}" + "\t" +
                                  "UsedLastDate   : {7}" + "\t" +
                                  "UsedLastTime   : {8}" + "\t" +
                                  "MaxPinCount    : {9}" + "\t" +
                                  "MaxPinUseCount : {10}" + "\t" +
                                  "AccountNumber  : {11}"
                                   , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                   , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                    #endregion FormatedLogWriting

                    _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;

                    #region stop to verifing account
                    //else
                    //{
                    //    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                    //    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    //    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    //    _MOBILEBANKING_RESP.ResponseData = null;

                    //    _ProcessMessage.ProcessUnsuccessfullTransactionBLQ(ref _MOBILEBANKING_RESP, _QUERYACCBAL_IOFS_REQ, _QUERYACCBAL_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
                    //    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Account Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                    //                                                , _MOBILEBANKING_REQ.ReferenceNumber, status, _MOBILEBANKING_REQ.ResponseCode));
                    //    return;
                    //}
                    #endregion stop to verifing account

                    //_ProcessMessage.TransactionGenerateMiniStatement((int)enumCommandTypeEnum.AuthorizationRequestMessage, _REQUESTACCSTMT_IOFS_REQ, _REQUESTACCSTMT_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 0);

                    _REQUESTACCSTMT_IOFS_RES = _FCUBSAccFinService.RequestAccStmtIO(_REQUESTACCSTMT_IOFS_REQ);
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _ProcessMessage.ProcessUnsuccessfullTransactionMINI(ref _MOBILEBANKING_RESP, _REQUESTACCSTMT_IOFS_REQ, _REQUESTACCSTMT_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(IMPSTransactionRouter.FCUBSAccFinService.REQUESTACCSTMT_IOFS_RES));
                        _serelized.Serialize(xmlWriter, _REQUESTACCSTMT_IOFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Ministatement Generate Response recieved from HOST for reference number : " + _REQUESTACCSTMT_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion
                _MOBILEBANKING_RESP.MSGSTAT = _REQUESTACCSTMT_IOFS_RES.FCUBS_HEADER.MSGSTAT.ToString();

                //_ProcessMessage.TransactionGenerateMiniStatement((int)enumCommandTypeEnum.AuthorizationRequestMessage, _REQUESTACCSTMT_IOFS_REQ, _REQUESTACCSTMT_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 0);

                if (_REQUESTACCSTMT_IOFS_RES.FCUBS_HEADER.MSGSTAT.ToString().Contains("SUCCESS"))
                {
                    FCUBSMessagingService.FCUBSMessagingServiceSEIClient _FCUBSMessagingServiceSEIClient = new FCUBSMessagingService.FCUBSMessagingServiceSEIClient("FCUBSMessagingServiceSEI");
                    FCUBSMessagingService.GETCONTRACTMSGS_IOFS_REQ _GETCONTRACTMSGS_IOFS_REQ = new FCUBSMessagingService.GETCONTRACTMSGS_IOFS_REQ();
                    FCUBSMessagingService.FCUBS_HEADERType _FCUBS_HEADERType1 = new FCUBSMessagingService.FCUBS_HEADERType();
                    FCUBSMessagingService.GETCONTRACTMSGS_IOFS_REQFCUBS_BODY _GETCONTRACTMSGS_IOFS_REQFCUBS_BODY = new FCUBSMessagingService.GETCONTRACTMSGS_IOFS_REQFCUBS_BODY();
                    FCUBSMessagingService.ContractMsgsPKType _ContractMessagesIO = new FCUBSMessagingService.ContractMsgsPKType();
                    FCUBSMessagingService.GETCONTRACTMSGS_IOFS_RES _GETCONTRACTMSGS_IOFS_RES = new FCUBSMessagingService.GETCONTRACTMSGS_IOFS_RES();

                    _ContractMessagesIO.FCCREF = _REQUESTACCSTMT_IOFS_RES.FCUBS_BODY.CustAccStmtAdhocRequest.DCN;



                    _GETCONTRACTMSGS_IOFS_REQFCUBS_BODY.ContractMessagesIO = _ContractMessagesIO;
                    _FCUBS_HEADERType1.SOURCE = "MBANKING";
                    _FCUBS_HEADERType1.UBSCOMP = FCUBSMessagingService.UBSCOMPType.FCUBS;
                    _FCUBS_HEADERType1.CORRELID = _REQUESTACCSTMT_IOFS_REQ.FCUBS_HEADER.CORRELID;
                    _FCUBS_HEADERType1.USERID = "FLEXSWITCH";
                    _FCUBS_HEADERType1.BRANCH = "100";
                    _FCUBS_HEADERType1.SERVICE = "FCUBSMessagingService";
                    _FCUBS_HEADERType1.OPERATION = "GetContractMsgs";
                    _GETCONTRACTMSGS_IOFS_REQ.FCUBS_HEADER = _FCUBS_HEADERType1;
                    _GETCONTRACTMSGS_IOFS_REQ.FCUBS_BODY = _GETCONTRACTMSGS_IOFS_REQFCUBS_BODY;

                    _CommanDetails.SystemLogger.WriteTransLog(this, "FCCRFF : " + _ContractMessagesIO.FCCREF + " CORRELID" + _FCUBS_HEADERType1.CORRELID);

                    ProcessGetMiniStatement(ref _MOBILEBANKING_RESP, _REQUESTACCSTMT_IOFS_REQ, _GETCONTRACTMSGS_IOFS_REQ, ref _GETCONTRACTMSGS_IOFS_RES, _FCUBSMessagingServiceSEIClient,
                                                                     _FCUBS_HEADERType1, _ContractMessagesIO, _MOBILEBANKING_REQ);
                }
                else
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidBenificiary);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.HOSTResponseCODE = _REQUESTACCSTMT_IOFS_RES.FCUBS_BODY.FCUBS_ERROR_RESP[0].ECODE;
                    _MOBILEBANKING_RESP.HOSTResponseDesc = _REQUESTACCSTMT_IOFS_RES.FCUBS_BODY.FCUBS_ERROR_RESP[0].EDESC;
                    _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_REQ.MobileNumber;
                    _MOBILEBANKING_RESP.DeviceLocation = _MOBILEBANKING_REQ.DeviceLocation;
                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.TransactionRefrenceNumber = _MOBILEBANKING_REQ.TransactionRefrenceNumber;
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host on Ministatement For Reference Number : " + _REQUESTACCSTMT_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                _ProcessMessage.ProcessUnsuccessfullTransactionMINI(ref _MOBILEBANKING_RESP, _REQUESTACCSTMT_IOFS_REQ, _REQUESTACCSTMT_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
            }
        }

        public void ProcessGetMiniStatement(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSAccFinService.REQUESTACCSTMT_IOFS_REQ _REQUESTACCSTMT_IOFS_REQ, FCUBSMessagingService.GETCONTRACTMSGS_IOFS_REQ _GETCONTRACTMSGS_IOFS_REQ,
                                                                     ref FCUBSMessagingService.GETCONTRACTMSGS_IOFS_RES _GETCONTRACTMSGS_IOFS_RES, FCUBSMessagingService.FCUBSMessagingServiceSEIClient _FCUBSMessagingService,
                                                                     FCUBSMessagingService.FCUBS_HEADERType _FCUBS_HEADERType1, FCUBSMessagingService.ContractMsgsPKType _ContractMsgsPKType, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSMessagingService.GETCONTRACTMSGS_IOFS_REQ));
                        _serelized.Serialize(xmlWriter, _GETCONTRACTMSGS_IOFS_REQ);
                    }
                    string HostRequestData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostRequestData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** GET Ministatement Transaction Request send to HOST for reference number : " + _REQUESTACCSTMT_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                #region RequestToHost
                try
                {
                    _ProcessMessage.TransactionGetMiniStatement((int)enumCommandTypeEnum.AuthorizationRequestMessage, _REQUESTACCSTMT_IOFS_REQ, _GETCONTRACTMSGS_IOFS_REQ, _GETCONTRACTMSGS_IOFS_RES, _FCUBS_HEADERType1, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 0);

                    _GETCONTRACTMSGS_IOFS_RES = _FCUBSMessagingService.GetContractMsgsIO(_GETCONTRACTMSGS_IOFS_REQ);
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _ProcessMessage.ProcessUnsuccessfullTransactionMINI(ref _MOBILEBANKING_RESP, _GETCONTRACTMSGS_IOFS_REQ, _GETCONTRACTMSGS_IOFS_RES, _FCUBS_HEADERType1, _MOBILEBANKING_REQ, _REQUESTACCSTMT_IOFS_REQ);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }
                #endregion

                #region Loger
                try
                {
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSMessagingService.GETCONTRACTMSGS_IOFS_RES));
                            _serelized.Serialize(xmlWriter, _GETCONTRACTMSGS_IOFS_RES);
                        }
                        string HostResponseData = stringWriter.ToString();
                        XDocument FormattedXML = XDocument.Parse(HostResponseData);
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** GET Ministatement Response recieved from HOST for reference number : " + _REQUESTACCSTMT_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }
                }
                catch (Exception ex) { _CommanDetails.SystemLogger.WriteErrorLog(this, ex); }
                #endregion

                _ProcessMessage.ProcessMiniStatement(ref _MOBILEBANKING_RESP, _GETCONTRACTMSGS_IOFS_REQ, _GETCONTRACTMSGS_IOFS_RES, _FCUBS_HEADERType1, _MOBILEBANKING_REQ, _REQUESTACCSTMT_IOFS_REQ);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host on GET Ministatement For Reference Number : " + _REQUESTACCSTMT_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                _ProcessMessage.ProcessUnsuccessfullTransactionMINI(ref _MOBILEBANKING_RESP, _GETCONTRACTMSGS_IOFS_REQ,
                                                                     _GETCONTRACTMSGS_IOFS_RES, _FCUBS_HEADERType1, _MOBILEBANKING_REQ, _REQUESTACCSTMT_IOFS_REQ);
            }
        }

        public void ProcessIntraFundTransferToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSRTService.CREATETRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                                  ref FCUBSRTService.CREATETRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSRTService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                                  FCUBSRTService.FCUBSRTServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    int Accstatus = -1;
                    IMPSTransactions.GETCUSTOMERDETAILS_ACC(_MOBILEBANKING_REQ.BENIFICIARYACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out Accstatus);
                    if (Accstatus == 74)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidCurrencyCode);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidCurrencyCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        return;
                    }
                    try
                    {
                        string CCY = "BTN";
                        IMPSTransactions.GETCUSTOMERCURRENCY_ACC(_MOBILEBANKING_REQ.REMITTERACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out CCY);
                        _CREATETRANSACTION_FSFS_REQ.FCUBS_BODY.TransactionDetails.TXNCCY = CCY;
                    }
                    catch { }

                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);



                    if (status == 0)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        if (_MOBILEBANKING_REQ.IsAccountFT)
                        {
                            _ProcessMessage.TransactionIntraFundTransforACC((int)enumCommandTypeEnum.AuthorizationResponseMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        }
                        else if (_MOBILEBANKING_REQ.IsMobileFT)
                        {
                            _ProcessMessage.TransactionIntraFundTransforMobile((int)enumCommandTypeEnum.AuthorizationResponseMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        }

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }

                    if (_MOBILEBANKING_REQ.IsAccountFT)
                    {
                        _ProcessMessage.TransactionIntraFundTransforACC((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    }
                    else if (_MOBILEBANKING_REQ.IsMobileFT)
                    {
                        _ProcessMessage.TransactionIntraFundTransforMobile((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    }
                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Intra Fund Transfer Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Intra Fund Transfer Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    if (_MOBILEBANKING_REQ.IsAccountFT)
                    {
                        _ProcessMessage.TransactionIntraFundTransforACC((int)enumCommandTypeEnum.AuthorizationResponseMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    }
                    else if (_MOBILEBANKING_REQ.IsMobileFT)
                    {
                        _ProcessMessage.TransactionIntraFundTransforMobile((int)enumCommandTypeEnum.AuthorizationResponseMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    }
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Intra Fund Transfer Response recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessIntraFundTransfer(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _ProcessMessage.ProcessUnsuccessfullTransactionFD(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }

        public void ProcessOutwardFundTransforToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSRTService.CREATETRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                                 ref FCUBSRTService.CREATETRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSRTService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                                 FCUBSRTService.FCUBSRTServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();

                    try
                    {
                        string CCY = "BTN";
                        IMPSTransactions.GETCUSTOMERCURRENCY_ACC(_MOBILEBANKING_REQ.REMITTERACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out CCY);
                        _CREATETRANSACTION_FSFS_REQ.FCUBS_BODY.TransactionDetails.TXNCCY = CCY;
                    }
                    catch { }
                    //old commented by archana on dated 25062019 for ACQ balance 

                    //_ImpsTransaction.VERIFYIMPSACCOUNT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                    //                                     ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                    //                                     ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                    //                                     ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset,
                    //                                     out status);


                    _ImpsTransaction.VERIFYIMPSACCOUNTACQ(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                         ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                         ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                         ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset,
                                                         out status, ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);





                    if (status == 0)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        if (_MOBILEBANKING_REQ.IsAccountFT)
                        {
                            _ProcessMessage.TransactionIntraFundTransforACC((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        }
                        else if (_MOBILEBANKING_REQ.IsMobileFT)
                        {
                            _ProcessMessage.TransactionIntraFundTransforMobile((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        }

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }

                    _ProcessMessage.TransactionOutwardFundTransfer((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Outward Fund Transfer Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Outward Fund Transfer Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }

                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.ProcessUnsuccessfullTransactionFD(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    _ProcessMessage.TransactionOutwardFundTransfer((int)enumCommandTypeEnum.AuthorizationResponseMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Outward Fund Transfer Response recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessOutwardFundTransfer(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _ProcessMessage.ProcessUnsuccessfullTransactionFD(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }

        public void ProcessRechargeToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                               ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                               FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    //int Accstatus = -1;
                    //IMPSTransactions.GETCUSTOMERDETAILS_ACC(_MOBILEBANKING_REQ.BENIFICIARYACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out Accstatus);
                    //if (Accstatus == 74)
                    //{
                    //    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidCurrencyCode);
                    //    _MOBILEBANKING_RESP.ResponseDesc = "This Currency is not be allowed for fund transfer.";
                    //    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    //    return;
                    //}

                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT_PAYMENT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0 || status == 47 || status == 48)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }

                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                    _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                    if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Intra Fund Transfer Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                        _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                        _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                        return;
                    }
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Intra Fund Transfer Response recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessBillPayment(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _ProcessMessage.ProcessUnsuccessfullTransactionUP(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }

        public void ProcessBTRechargeToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                               ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                               FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    //int Accstatus = -1;
                    //IMPSTransactions.GETCUSTOMERDETAILS_ACC(_MOBILEBANKING_REQ.BENIFICIARYACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out Accstatus);
                    //if (Accstatus == 74)
                    //{
                    //    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidCurrencyCode);
                    //    _MOBILEBANKING_RESP.ResponseDesc = "This Currency is not be allowed for fund transfer.";
                    //    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    //    return;
                    //}

                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();

                    _ImpsTransaction.VERIFYIMPSACCOUNT_PAYMENT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset,
                                                      out status, ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0 || status == 47 || status == 48)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.BHUTANTELECOM, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    #region Send Request to BT
                    if (!CONFIGURATIONCONFIGDATA.BTACTIVE)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BTIsUnderMaintenance);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BTIsUnderMaintenance);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        return;
                    }
                    //else if (!_HTTPCommunicationChanel.CheckBTConnection())
                    //{
                    //    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectBT);
                    //    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectBT);
                    //    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    //    _MOBILEBANKING_RESP.ResponseData = null;
                    //    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    //    return;
                    //}
                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Recharge Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Recharge Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }

                    #endregion Send Request to BT
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Recharge Response recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessBillPayment(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                //_ProcessMessage.ProcessUnsuccessfullTransactionFD(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }

        public void ProcessBpcRechargeToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                             ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                             FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    //int Accstatus = -1;
                    //IMPSTransactions.GETCUSTOMERDETAILS_ACC(_MOBILEBANKING_REQ.BENIFICIARYACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out Accstatus);
                    //if (Accstatus == 74)
                    //{
                    //    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidCurrencyCode);
                    //    _MOBILEBANKING_RESP.ResponseDesc = "This Currency is not be allowed for fund transfer.";
                    //    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    //    return;
                    //}

                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT_PAYMENT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset,
                                                      out status, ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0 || status == 47 || status == 48)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.BHUTANTELECOM, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    #region Send Request to BT
                    if (!CONFIGURATIONCONFIGDATA.BPCACTIVE)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCIsUnderMaintenance);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCIsUnderMaintenance);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        return;
                    }
                    else if (!_HTTPCommunicationChanel.CheckBPCConnection())
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectBPC);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectBPC);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        return;
                    }
                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BPC Recharge Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BPC Recharge Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }

                    #endregion Send Request to BT
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BPC Recharge Response recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessBillPayment(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _ProcessMessage.ProcessUnsuccessfullTransactionUP(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }

        public void ProcessTcellPrepaidRechargeToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                             ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                             FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    //int Accstatus = -1;
                    //IMPSTransactions.GETCUSTOMERDETAILS_ACC(_MOBILEBANKING_REQ.BENIFICIARYACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out Accstatus);
                    //if (Accstatus == 74)
                    //{
                    //    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidCurrencyCode);
                    //    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidCurrencyCode);
                    //    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    //    return;
                    //}

                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT_PAYMENT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0 || status == 47 || status == 48)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.TCRECHARGE, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    #region Send Request to BT
                    if (!CONFIGURATIONCONFIGDATA.TCELLACTIVE)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.TCELLIsUnderMaintenance);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.TCELLIsUnderMaintenance);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        return;
                    }
                    else if (!_HTTPCommunicationChanel.CheckTCellConnection())
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectTcell);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectTcell);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        return;
                    }
                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Tcell Prepaid Recharge Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Tcell Prepaid Recharge Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                    #endregion Send Request to BT
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Tcell Prepaid Recharge Response recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessBillPayment(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _ProcessMessage.ProcessUnsuccessfullTransactionUP(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }

        public void ProcessTcellPostpaidRechargeToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                            ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                            FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    //int Accstatus = -1;
                    //IMPSTransactions.GETCUSTOMERDETAILS_ACC(_MOBILEBANKING_REQ.BENIFICIARYACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out Accstatus);
                    //if (Accstatus == 74)
                    //{
                    //    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidCurrencyCode);
                    //    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidCurrencyCode);
                    //    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    //    return;
                    //}

                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.BHUTANTELECOM, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    #region Send Request to BT
                    if (!CONFIGURATIONCONFIGDATA.TCELLACTIVE)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.TCELLIsUnderMaintenance);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.TCELLIsUnderMaintenance);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        return;
                    }
                    else if (!_HTTPCommunicationChanel.CheckTCellConnection())
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectTcell);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectTcell);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        return;
                    }
                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Tcell Postpaid Recharge Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Tcell Postpaid Recharge Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                    #endregion Send Request to BT
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Tcell Postpaid Recharge Response recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessBillPayment(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _ProcessMessage.ProcessUnsuccessfullTransactionUP(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }

        public void ProcessTcellLeaseLineRechargeToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                            ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                            FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    //int Accstatus = -1;
                    //IMPSTransactions.GETCUSTOMERDETAILS_ACC(_MOBILEBANKING_REQ.BENIFICIARYACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out Accstatus);
                    //if (Accstatus == 74)
                    //{
                    //    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidCurrencyCode);
                    //    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidCurrencyCode);
                    //    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    //    return;
                    //}

                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.BHUTANTELECOM, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    #region Send Request to BT
                    if (!CONFIGURATIONCONFIGDATA.TCELLACTIVE)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.TCELLIsUnderMaintenance);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.TCELLIsUnderMaintenance);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        return;
                    }
                    else if (!_HTTPCommunicationChanel.CheckTCellConnection())
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectTcell);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectTcell);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        return;
                    }

                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Tcell Lease Line Recharge Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Tcell Lease Line Recharge Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);

                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                    #endregion Send Request to BT
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Tcell Lease Line Recharge Response recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessBillPayment(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _ProcessMessage.ProcessUnsuccessfullTransactionUP(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }

        public void ProcessDrukComRechargeToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                           ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                           FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    //int Accstatus = -1;
                    //IMPSTransactions.GETCUSTOMERDETAILS_ACC(_MOBILEBANKING_REQ.BENIFICIARYACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out Accstatus);
                    //if (Accstatus == 74)
                    //{
                    //    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidCurrencyCode);
                    //    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidCurrencyCode);
                    //    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    //    return;
                    //}

                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT_PAYMENT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0 || status == 47 || status == 48)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.DRUKCOM, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);


                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** DrukCom Recharge Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** DrukCom Recharge Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }

                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }


                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** DrukCom Recharge Response recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessDrukComBillPayment(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

                #endregion Send Request to BT
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _ProcessMessage.ProcessUnsuccessfullTransactionUP(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }

        public void ProcessWaterBillPaymentToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                           ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                           FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.WATERBILLPAY, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);


                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Water Bill Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Water Bill Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                    #endregion Send Request to BT
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }


                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Water Bill Payment Response recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessBillPayment(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _ProcessMessage.ProcessUnsuccessfullTransactionUP(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }

        public void ProcessEthoMEthoRechargeToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                          ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                          FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    //int Accstatus = -1;
                    //IMPSTransactions.GETCUSTOMERDETAILS_ACC(_MOBILEBANKING_REQ.BENIFICIARYACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out Accstatus);
                    //if (Accstatus == 74)
                    //{
                    //    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidCurrencyCode);
                    //    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidCurrencyCode);
                    //    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    //    return;
                    //}

                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT_PAYMENT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset,
                                                      out status, ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0 || status == 47 || status == 48)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.ETHOMETHO, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);


                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** EthoMetho Recharge Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** EthoMetho Recharge Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                    #endregion Send Request to BT
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }


                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** EthoMetho Recharge Response recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessEthoMethoBillPayment(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _ProcessMessage.ProcessUnsuccessfullTransactionUP(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }

        public void ProcessNorlingRechargeToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                         ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                         FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    //int Accstatus = -1;
                    //IMPSTransactions.GETCUSTOMERDETAILS_ACC(_MOBILEBANKING_REQ.BENIFICIARYACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out Accstatus);
                    //if (Accstatus == 74)
                    //{
                    //    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidCurrencyCode);
                    //    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidCurrencyCode);
                    //    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    //    return;
                    //}

                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT_PAYMENT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0 || status == 47 || status == 48)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.ETHOMETHO, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);


                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Norling Recharge Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Norling Recharge Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                    #endregion Send Request to BT
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }


                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Norling Recharge Response recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessEthoMethoBillPayment(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _ProcessMessage.ProcessUnsuccessfullTransactionUP(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }


        public void ProcessCreditCardPaymentToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSRTService.CREATETRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                              ref FCUBSRTService.CREATETRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSRTService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                              FCUBSRTService.FCUBSRTServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    decimal ExchangeRate = 0;
                    int Accstatus = -1;
                    try
                    {
                        IMPSTransactions.GETCUSTOMERDETAILS_ACC_CC(_MOBILEBANKING_REQ.REMITTERACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out Accstatus);
                        if (Accstatus == 74)
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidCrossCurrencyCode);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidCrossCurrencyCode);
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                            return;
                        }
                        try
                        {
                            string CCY = "BTN";
                            IMPSTransactions.GETCUSTOMERCURRENCY_ACC(_MOBILEBANKING_REQ.REMITTERACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out CCY);
                            _CommanDetails.SystemLogger.WriteTransLog(this, "Remitter Account Currency Code Is: " + CCY);
                            _CREATETRANSACTION_FSFS_REQ.FCUBS_BODY.TransactionDetails.TXNCCY = CCY;
                        }
                        catch { }

                        IMPSTransactions.GET_CREDITCARD_CCY(_MOBILEBANKING_REQ.CREDITCARDACC, out Accstatus);
                        if (Accstatus == 0 && _CREATETRANSACTION_FSFS_REQ.FCUBS_BODY.TransactionDetails.TXNCCY == "USD")
                        {
                            _CommanDetails.SystemLogger.WriteTransLog(this, "Remitter account currency Code is USD and Credit Card Account currency code is USD for Reference Number " + _MOBILEBANKING_REQ.MSGID);
                            _CREATETRANSACTION_FSFS_REQ.FCUBS_BODY.TransactionDetails.OFFSETCCY = "USD";
                        }
                        else if (Accstatus == 0 && _CREATETRANSACTION_FSFS_REQ.FCUBS_BODY.TransactionDetails.TXNCCY == "BTN")
                        {
                            _CommanDetails.SystemLogger.WriteTransLog(this, "Remitter account currency Code is BTN and Credit Card Account currency code is USD " + _MOBILEBANKING_REQ.MSGID);
                            _CREATETRANSACTION_FSFS_REQ.FCUBS_BODY.TransactionDetails.OFFSETCCY = "USD";
                            ExchangeRate = IMPSTransactions.GET_EXCHANGERATE(out Accstatus);
                            if (Accstatus == 0)
                            {
                                _CREATETRANSACTION_FSFS_REQ.FCUBS_BODY.TransactionDetails.TXNAMT = _CREATETRANSACTION_FSFS_REQ.FCUBS_BODY.TransactionDetails.TXNAMT * ExchangeRate;
                                _CommanDetails.SystemLogger.WriteTransLog(this, "Converted Amount From USD/INR to BTN is " + _CREATETRANSACTION_FSFS_REQ.FCUBS_BODY.TransactionDetails.TXNAMT + " for Reference Number " + _MOBILEBANKING_REQ.MSGID + " and exchange rate is " + ExchangeRate);
                            }
                            else
                            {
                                _CommanDetails.SystemLogger.WriteTransLog(this, "Unable to get Exchange Rate from CBS BD for Reference Number " + _MOBILEBANKING_REQ.MSGID);
                                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                                _MOBILEBANKING_RESP.ResponseData = null;
                                _ProcessMessage.TransactionPayment_CC((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.CREDITCARDBILLPAYMENT, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                                return;
                            }
                        }
                        else if (Accstatus == 1 && _CREATETRANSACTION_FSFS_REQ.FCUBS_BODY.TransactionDetails.TXNCCY == "USD")
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidCrossCurrencyCode);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidCrossCurrencyCode);
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                            return;
                        }
                        else if (Accstatus == 1 && _CREATETRANSACTION_FSFS_REQ.FCUBS_BODY.TransactionDetails.TXNCCY == "BTN")
                        {
                            _CREATETRANSACTION_FSFS_REQ.FCUBS_BODY.TransactionDetails.OFFSETCCY = "INR";
                            _CommanDetails.SystemLogger.WriteTransLog(this, "Remitter account currency Code is BTN and Credit Card Account currency code is INR for Reference Number " + _MOBILEBANKING_REQ.MSGID);
                        }
                    }
                    catch { }
                    //int Accstatus = -1;
                    //IMPSTransactions.GETCUSTOMERDETAILS_ACC(_MOBILEBANKING_REQ.BENIFICIARYACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out Accstatus);
                    //if (Accstatus == 74)
                    //{
                    //    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidCurrencyCode);
                    //    _MOBILEBANKING_RESP.ResponseDesc = "This Currency is not be allowed for fund transfer.";
                    //    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    //    return;
                    //}

                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment_CC((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.CREDITCARDBILLPAYMENT, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }

                    _ProcessMessage.TransactionPayment_CC((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.CREDITCARDBILLPAYMENT, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Credit Card Bill Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Credit Card Bill Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment_CC((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.CREDITCARDBILLPAYMENT, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Credit Card Bill Payment Response recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessBillPayment_CC(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessNPPFLoanPaymentToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                               ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                               FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT_PAYMENT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset,
                                                      out status, ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0 || status == 47 || status == 48)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.BHUTANTELECOM, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    if (!CONFIGURATIONCONFIGDATA.NPPFACTIVE)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.NPPFIsUnderMaintenance);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.NPPFIsUnderMaintenance);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        return;
                    }
                    else if (!_HTTPCommunicationChanel.CheckNPPFConnection())
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectNPPF);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectNPPF);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        return;
                    }
                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** NPPF Loan Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion Loger

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** NPPF Loan Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);


                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion RequestToHost

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** NPPF Loan Payment Response recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessBillPayment(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                //_ProcessMessage.ProcessUnsuccessfullTransactionFD(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }

        public void ProcessNPPFRentPaymentToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                                ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                                FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT_PAYMENT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0 || status == 47 || status == 48)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.BHUTANTELECOM, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    if (!CONFIGURATIONCONFIGDATA.NPPFACTIVE)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.NPPFIsUnderMaintenance);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.NPPFIsUnderMaintenance);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        return;
                    }
                    else if (!_HTTPCommunicationChanel.CheckNPPFConnection())
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectNPPF);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectNPPF);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        return;
                    }
                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** NPPF Rent Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);


                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** NPPF Rent Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);

                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }

                    #endregion Send Request to BT
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** NPPF Rent Payment Response recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessBillPayment(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                //_ProcessMessage.ProcessUnsuccessfullTransactionFD(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }


        public void ProcessRequestMoneyToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSRTService.CREATETRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                               ref FCUBSRTService.CREATETRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSRTService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                               FCUBSRTService.FCUBSRTServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                int statusvalid = -1;
                DataTable DtRequestvalid = null;
                DtRequestvalid = IMPSTransactions.GetRequestMoneyDetails(_MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.ReferenceNumber, out statusvalid);

                if (statusvalid != 0)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.VailidityExpire);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                    _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                    _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILED";

                    int StatusNUll = 0;
                    StatusNUll = 0;
                    IMPSTransactions.InsertTokenDetails(_MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.ReferenceNumber, _MOBILEBANKING_REQ.MobileNumber, "", "", "", _MOBILEBANKING_RESP.MSGSTAT, "", 3, 0.00m, "", out StatusNUll, "", "", "", _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_RESP.ResponseCode, "", _MOBILEBANKING_REQ.RemarkfinalPayment);

                    return;
                }

                #region RequestToHost
                try
                {
                    int Accstatus = -1;
                    IMPSTransactions.GETCUSTOMERDETAILS_ACC(_MOBILEBANKING_REQ.BENIFICIARYACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out Accstatus);
                    if (Accstatus == 74)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidCurrencyCode);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidCurrencyCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        return;
                    }
                    try
                    {
                        string CCY = "BTN";
                        IMPSTransactions.GETCUSTOMERCURRENCY_ACC(_MOBILEBANKING_REQ.REMITTERACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out CCY);
                        _CREATETRANSACTION_FSFS_REQ.FCUBS_BODY.TransactionDetails.TXNCCY = CCY;
                    }
                    catch { }

                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset,
                                                      out status, ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.REMITTERACC));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        if (_MOBILEBANKING_REQ.IsAccountFT)
                        {
                            _ProcessMessage.TransactionRequestMoneyTranforACC((int)enumCommandTypeEnum.AuthorizationResponseMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        }
                        else if (_MOBILEBANKING_REQ.IsMobileFT)
                        {
                            _ProcessMessage.TransactionRequestMoneyTranforMobile((int)enumCommandTypeEnum.AuthorizationResponseMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        }

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }

                    if (_MOBILEBANKING_REQ.IsAccountFT)
                    {
                        _ProcessMessage.TransactionRequestMoneyTranforACC((int)enumCommandTypeEnum.AuthorizationRequestMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    }
                    else if (_MOBILEBANKING_REQ.IsMobileFT)
                    {
                        _ProcessMessage.TransactionRequestMoneyTranforMobile((int)enumCommandTypeEnum.AuthorizationRequestMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    }
                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Request Money Final Transfer Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Request Money Final Transfer Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    if (_MOBILEBANKING_REQ.IsAccountFT)
                    {
                        _ProcessMessage.TransactionRequestMoneyTranforACC((int)enumCommandTypeEnum.AuthorizationResponseMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    }
                    else if (_MOBILEBANKING_REQ.IsMobileFT)
                    {
                        _ProcessMessage.TransactionRequestMoneyTranforMobile((int)enumCommandTypeEnum.AuthorizationResponseMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    }
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    //int StatusNUll = 0;
                    //IMPSTransactions.InsertTokenDetails(_MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.ReferenceNumber, _MOBILEBANKING_REQ.MobileNumber, "", "", "", "", "", 3, "", "", out StatusNUll, "", "", "", _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_RESP.ResponseCode,"");

                    //return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Request Money Final  Transfer Response recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessRequestMoneyTransfer(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _ProcessMessage.ProcessUnsuccessfullTransactionFD(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);

            }
        }

        #region Reversal Transactions

        //added by sk
        public void ProcessReversalToHost(MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg _SwitchConsumerRequestReqMsg, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg = new MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg();

                #region RequestToHost
                try
                {
                    #region Loger
                    using (var stringWriter = new StringWriter())
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** " + _MOBILEBANKING_REQ.TransType + " Reversal Transaction Request send to HOST for reference number : " + _MOBILEBANKING_REQ.ReferenceNumber));
                    }
                    #endregion

                    RequestMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.ReversalAdviceRequestMessage;
                    RequestMsg.TransactionType = MaxiSwitch.API.Terminal.enumTransactionType.Debit;
                    _ProcessMessage.ReversalTransactionPayment((int)RequestMsg.CommandType, _MOBILEBANKING_REQ.TransType, RequestMsg, _MOBILEBANKING_REQ, 1);
                    _CommanDetails.SystemLogger.WriteTransLog(this, "Reversal Transaction Request convert to MessageConvertor for Reference  number :" + _MOBILEBANKING_REQ.ReferenceNumber);
                    ReversalMessageConvertor(ref _MOBILEBANKING_REQ, ref RequestMsg);
                    _CommanDetails.SystemLogger.WriteTransLog(this, "Reversal Transaction Request send to Host Interface for Reference  number :" + _MOBILEBANKING_REQ.ReferenceNumber);
                    var task = Task.Factory.StartNew(() =>
                    {
                        HostCommChanel.SendReversalTransRequest.Invoke(ref RequestMsg);
                    });
                    task.Wait(Convert.ToInt32(TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut).TotalMilliseconds));
                    if (!task.IsCompleted)
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format(" Reversal Trasaction TimeOut at Host Interface for Reference Number : " + RequestMsg.ReferenceNumber));
                        return;
                    }
                    _CommanDetails.SystemLogger.WriteTransLog(this, "Reversal Transaction Response received from Host Interface for Reference  number :" + _MOBILEBANKING_REQ.ReferenceNumber);
                    _CommanDetails.SystemLogger.WriteTransLog(this, "Reversal Transaction Response Code :" + _SwitchConsumerRequestReqMsg.ResponseCode + " for Reference  number :" + _SwitchConsumerRequestReqMsg.ReferenceNumber);
                }
                catch (Exception ex)
                {
                    _ProcessMessage.ReversalTransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, _MOBILEBANKING_REQ.TransType, _SwitchConsumerRequestReqMsg, _MOBILEBANKING_REQ, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                _ProcessMessage.ReversalTransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, _MOBILEBANKING_REQ.TransType, _SwitchConsumerRequestReqMsg, _MOBILEBANKING_REQ, 2);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _MOBILEBANKING_REQ.ReferenceNumber);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                //_ProcessMessage.ProcessUnsuccessfullTransactionUP(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }


        public void ProcessReversalToHost(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                             FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                             FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region assign value from Transaction Request to Reversal Request

                FCUBSUPService.REVERSEUPTRANSACTION_FSFS_REQ _REVERSEUPTRANSACTION_FSFS_REQ = new FCUBSUPService.REVERSEUPTRANSACTION_FSFS_REQ();
                FCUBSUPService.REVERSEUPTRANSACTION_FSFS_REQFCUBS_BODY _REVERSEUPTRANSACTION_FSFS_REQFCUBS_BODY = new FCUBSUPService.REVERSEUPTRANSACTION_FSFS_REQFCUBS_BODY();
                FCUBSUPService.UtilPaymentTxnPKType _UtilPaymentTxnFullType = new FCUBSUPService.UtilPaymentTxnPKType();
                FCUBSUPService.REVERSEUPTRANSACTION_FSFS_RES _REVERSEUPTRANSACTION_FSFS_RES = new FCUBSUPService.REVERSEUPTRANSACTION_FSFS_RES();
                FCUBSUPService.FCUBS_HEADERType _FCUBSREV_HEADERType = new FCUBSUPService.FCUBS_HEADERType();

                _FCUBSREV_HEADERType.MSGID = "91" + GenerateMSGID();
                _FCUBSREV_HEADERType.MODULEID = "UP";
                _FCUBSREV_HEADERType.SOURCE = "MBANKING";
                _FCUBSREV_HEADERType.UBSCOMP = FCUBSUPService.UBSCOMPType.FCUBS;
                _FCUBSREV_HEADERType.CORRELID = GenerateReferenceNumber();
                _FCUBSREV_HEADERType.USERID = "MBANKING";
                _FCUBSREV_HEADERType.BRANCH = "000";
                _FCUBSREV_HEADERType.MODULEID = "UP";
                _FCUBSREV_HEADERType.SERVICE = "FCUBSUPService";
                _FCUBSREV_HEADERType.OPERATION = "ReverseUPTransaction";
                _REVERSEUPTRANSACTION_FSFS_REQ.FCUBS_HEADER = _FCUBSREV_HEADERType;
                _UtilPaymentTxnFullType.XREF = _CREATETRANSACTION_FSFS_REQ.FCUBS_BODY.TransactionDetails.XREF;
                _REVERSEUPTRANSACTION_FSFS_REQFCUBS_BODY.TransactionDetails = _UtilPaymentTxnFullType;
                _REVERSEUPTRANSACTION_FSFS_REQ.FCUBS_BODY = _REVERSEUPTRANSACTION_FSFS_REQFCUBS_BODY;

                #endregion assign value from Transaction Request to Reversal Request

                #region RequestToHost
                try
                {

                    #region Loger
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.REVERSEUPTRANSACTION_FSFS_REQ));
                            _serelized.Serialize(xmlWriter, _REVERSEUPTRANSACTION_FSFS_REQ);
                        }
                        string HostRequestData = stringWriter.ToString();
                        XDocument FormattedXML = XDocument.Parse(HostRequestData);
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** " + _MOBILEBANKING_REQ.TransType + " Reversal Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_BODY.TransactionDetails.XREF + Environment.NewLine + Environment.NewLine +
                                                                                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }

                    #endregion
                    _ProcessMessage.ReversalTransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ.TransType, _REVERSEUPTRANSACTION_FSFS_REQ, _REVERSEUPTRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, 1);

                    _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                    _REVERSEUPTRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.ReverseUPTransactionFS(_REVERSEUPTRANSACTION_FSFS_REQ);

                }
                catch (Exception ex)
                {
                    _ProcessMessage.ReversalTransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, _MOBILEBANKING_REQ.TransType, _REVERSEUPTRANSACTION_FSFS_REQ, _REVERSEUPTRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.REVERSEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _REVERSEUPTRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** " + _MOBILEBANKING_REQ.TransType + " Reversal Response recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_BODY.TransactionDetails.XREF + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion
                _ProcessMessage.ReversalTransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, _MOBILEBANKING_REQ.TransType, _REVERSEUPTRANSACTION_FSFS_REQ, _REVERSEUPTRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, 2);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_BODY.TransactionDetails.XREF + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                //_ProcessMessage.ProcessUnsuccessfullTransactionUP(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }

        #endregion Reversal Transactions

        #region Process Loans & Deposit

        public void ProcessGetListofLoanAccounts(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSAccService.QUERYACCSUMM_IOFS_REQ _QUERYACCSUMM_IOFS_REQ,
                                                                   ref FCUBSAccService.QUERYACCSUMM_IOFS_RES _QUERYACCSUMM_IOFS_RES, FCUBSAccService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                                   FCUBSAccService.FCUBSAccServiceSEIClient _FCUBSAccService, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(IMPSTransactionRouter.FCUBSAccService.QUERYACCSUMM_IOFS_REQ));
                        _serelized.Serialize(xmlWriter, _QUERYACCSUMM_IOFS_REQ);
                    }
                    string HostRequestData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostRequestData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Get List of loan accounts Transaction Request send to HOST for reference number : " + _QUERYACCSUMM_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                #region RequestToHost
                try
                {
                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();

                    _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                    _FCUBSAccService.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                    _QUERYACCSUMM_IOFS_RES = _FCUBSAccService.QueryAccSummIO(_QUERYACCSUMM_IOFS_REQ);
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(IMPSTransactionRouter.FCUBSAccService.QUERYACCSUMM_IOFS_RES));
                        _serelized.Serialize(xmlWriter, _QUERYACCSUMM_IOFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Get List of loan accounts Response recieved from HOST for reference number : " + _QUERYACCSUMM_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessGetListofloanaccount(ref _MOBILEBANKING_RESP, _QUERYACCSUMM_IOFS_REQ, _QUERYACCSUMM_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _QUERYACCSUMM_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
            }
        }

        public void ProcessViewLoanDetailsToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSCLService.QUERYACCOUNT_IOFS_REQ _QUERYACCOUNT_IOFS_REQ,
                                                              ref FCUBSCLService.QUERYACCOUNT_IOFS_RES _QUERYACCOUNT_IOFS_RES, FCUBSCLService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                              FCUBSCLService.FCUBSCLServiceSEIClient _FCUBSCLServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {

                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.ViewLoansTransaction((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.VIEWLOANDETAILS, _QUERYACCOUNT_IOFS_REQ, _QUERYACCOUNT_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);


                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }


                    _ProcessMessage.ViewLoansTransaction((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.VIEWLOANDETAILS, _QUERYACCOUNT_IOFS_REQ, _QUERYACCOUNT_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    #region Loger
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSCLService.QUERYACCOUNT_IOFS_REQ));
                            _serelized.Serialize(xmlWriter, _QUERYACCOUNT_IOFS_REQ);
                        }
                        string HostRequestData = stringWriter.ToString();
                        XDocument FormattedXML = XDocument.Parse(HostRequestData);
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** View Loan Details Transaction Request send to HOST for reference number : " + _QUERYACCOUNT_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }
                    #endregion

                    _FCUBSCLServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                    _QUERYACCOUNT_IOFS_RES = _FCUBSCLServiceSEIClient.QueryAccountIO(_QUERYACCOUNT_IOFS_REQ);
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.ViewLoansTransaction((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.VIEWLOANDETAILS, _QUERYACCOUNT_IOFS_REQ, _QUERYACCOUNT_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion
                if (CONFIGURATIONCONFIGDATA.SKIPVIEWLOANLOG)
                {
                    #region Loger
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSCLService.QUERYACCOUNT_IOFS_RES));
                            _serelized.Serialize(xmlWriter, _QUERYACCOUNT_IOFS_RES);
                        }
                        string HostResponseData = stringWriter.ToString();
                        XDocument FormattedXML = XDocument.Parse(HostResponseData);
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** View Loan Details Response recieved from HOST for reference number : " + _QUERYACCOUNT_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine));
                    }
                    #endregion
                }
                else
                {
                    #region Loger
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSCLService.QUERYACCOUNT_IOFS_RES));
                            _serelized.Serialize(xmlWriter, _QUERYACCOUNT_IOFS_RES);
                        }
                        string HostResponseData = stringWriter.ToString();
                        XDocument FormattedXML = XDocument.Parse(HostResponseData);
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** View Loan Details Response recieved from HOST for reference number : " + _QUERYACCOUNT_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }
                    #endregion
                }

                _ProcessMessage.ProcessViewLoanDetails(ref _MOBILEBANKING_RESP, _QUERYACCOUNT_IOFS_REQ, _QUERYACCOUNT_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _QUERYACCOUNT_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessViewRecurringDetails(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSCLService.QUERYACCOUNT_IOFS_REQ _QUERYACCOUNT_IOFS_REQ,
                                                             ref FCUBSCLService.QUERYACCOUNT_IOFS_RES _QUERYACCOUNT_IOFS_RES, FCUBSCLService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                             FCUBSCLService.FCUBSCLServiceSEIClient _FCUBSCLServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {

                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.ViewLoansTransaction((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.VIEWLOANDETAILS, _QUERYACCOUNT_IOFS_REQ, _QUERYACCOUNT_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);


                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }


                    _ProcessMessage.ViewLoansTransaction((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.VIEWLOANDETAILS, _QUERYACCOUNT_IOFS_REQ, _QUERYACCOUNT_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    #region Loger
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEBANKING_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEBANKING_REQ);
                        }
                        string HostRequestData = stringWriter.ToString();
                        XDocument FormattedXML = XDocument.Parse(HostRequestData);
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** View Recurring Detail Transaction Request send Oracle Database for reference number : " + _QUERYACCOUNT_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }
                    #endregion

                    _MOBILEBANKING_RESP.RecurringTermDetails = MobilePortalProcess.RecurringAccountDetails(_MOBILEBANKING_REQ.CUSTOMERID, _MOBILEBANKING_REQ.LoanAccountNumber, out status);
                    _MOBILEBANKING_RESP.RecurringTermDetails.TableName = "RecurringTermDetails";

                    if (status == 0)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "SUCCESS";
                        _MOBILEBANKING_RESP.ResponseData = null;
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidAccount);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.ViewLoansTransaction((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.VIEWLOANDETAILS, _QUERYACCOUNT_IOFS_REQ, _QUERYACCOUNT_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion
                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEBANKING_RESP));
                        _serelized.Serialize(xmlWriter, _MOBILEBANKING_RESP);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** View Recurring Detail Response recieved from HOST for reference number : " + _QUERYACCOUNT_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine
                                                                                      + FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessViewRecurringTermDetails(ref _MOBILEBANKING_RESP, _QUERYACCOUNT_IOFS_REQ, _QUERYACCOUNT_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _QUERYACCOUNT_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessViewTermDetails(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSCLService.QUERYACCOUNT_IOFS_REQ _QUERYACCOUNT_IOFS_REQ,
                                                           ref FCUBSCLService.QUERYACCOUNT_IOFS_RES _QUERYACCOUNT_IOFS_RES, FCUBSCLService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                           FCUBSCLService.FCUBSCLServiceSEIClient _FCUBSCLServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {

                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.ViewLoansTransaction((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.VIEWLOANDETAILS, _QUERYACCOUNT_IOFS_REQ, _QUERYACCOUNT_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);


                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }


                    _ProcessMessage.ViewLoansTransaction((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.VIEWLOANDETAILS, _QUERYACCOUNT_IOFS_REQ, _QUERYACCOUNT_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    #region Loger
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEBANKING_REQ));
                            _serelized.Serialize(xmlWriter, _MOBILEBANKING_REQ);
                        }
                        string HostRequestData = stringWriter.ToString();
                        XDocument FormattedXML = XDocument.Parse(HostRequestData);
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** View Term Detail Transaction Request send Oracle Database for reference number : " + _QUERYACCOUNT_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }
                    #endregion

                    _MOBILEBANKING_RESP.RecurringTermDetails = MobilePortalProcess.TermAccountDetails(_MOBILEBANKING_REQ.CUSTOMERID, _MOBILEBANKING_REQ.LoanAccountNumber, out status);
                    _MOBILEBANKING_RESP.RecurringTermDetails.TableName = "RecurringTermDetails";

                    if (status == 0)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "SUCCESS";
                        _MOBILEBANKING_RESP.ResponseData = null;
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidAccount);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.ViewLoansTransaction((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.VIEWLOANDETAILS, _QUERYACCOUNT_IOFS_REQ, _QUERYACCOUNT_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion
                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEBANKING_RESP));
                        _serelized.Serialize(xmlWriter, _MOBILEBANKING_RESP);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** View Term Detail Response recieved from HOST for reference number : " + _QUERYACCOUNT_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine
                                                                                      + FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessViewRecurringTermDetails(ref _MOBILEBANKING_RESP, _QUERYACCOUNT_IOFS_REQ, _QUERYACCOUNT_IOFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _QUERYACCOUNT_IOFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessLoanPaymentToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSCLService.CREATEPAYMENT_FSFS_REQ _CREATEPAYMENT_FSFS_REQ,
                                                              ref FCUBSCLService.CREATEPAYMENT_FSFS_RES _CREATEPAYMENT_FSFS_RES, FCUBSCLService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                              FCUBSCLService.FCUBSCLServiceSEIClient _FCUBSCLServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    int Accstatus = -1;
                    try
                    {
                        string CCY = "BTN";
                        IMPSTransactions.GETCUSTOMERCURRENCY_ACC(_MOBILEBANKING_REQ.REMITTERACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out CCY);
                        _CREATEPAYMENT_FSFS_REQ.FCUBS_BODY.LiqFull.Settelments[0].STLCCY = CCY;
                    }
                    catch { }

                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT_PAYMENT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0 || status == 47 || status == 48)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.LoansPaymentTransaction((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.LOANPAYMENT, _CREATEPAYMENT_FSFS_REQ, _CREATEPAYMENT_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);


                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }


                    _ProcessMessage.LoansPaymentTransaction((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.LOANPAYMENT, _CREATEPAYMENT_FSFS_REQ, _CREATEPAYMENT_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSCLService.CREATEPAYMENT_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATEPAYMENT_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Loan Payment Transaction Request send to HOST for reference number : " + _CREATEPAYMENT_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSCLServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATEPAYMENT_FSFS_RES = _FCUBSCLServiceSEIClient.CreatePaymentFS(_CREATEPAYMENT_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATEPAYMENT_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSCLService.CREATEPAYMENT_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATEPAYMENT_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Loan Payment Transaction Request send to HOST for reference number : " + _CREATEPAYMENT_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSCLServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATEPAYMENT_FSFS_RES = _FCUBSCLServiceSEIClient.CreatePaymentFS(_CREATEPAYMENT_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.LoansPaymentTransaction((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.LOANPAYMENT, _CREATEPAYMENT_FSFS_REQ, _CREATEPAYMENT_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSCLService.CREATEPAYMENT_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATEPAYMENT_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Loan Payment Response recieved from HOST for reference number : " + _CREATEPAYMENT_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessLoanPayment(ref _MOBILEBANKING_RESP, _CREATEPAYMENT_FSFS_REQ, _CREATEPAYMENT_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATEPAYMENT_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        //public void ProcessRecurringViewToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
        //                                                       ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
        //                                                       FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        //{
        //    try
        //    {
        //        #region RequestToHost
        //        try
        //        {
        //            int Accstatus = -1;
        //            IMPSTransactions.GETCUSTOMERDETAILS_ACC(_MOBILEBANKING_REQ.BENIFICIARYACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out Accstatus);
        //            if (Accstatus == 74)
        //            {
        //                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidCurrencyCode);
        //                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidCurrencyCode);
        //                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
        //                return;
        //            }
        //            try
        //            {
        //                string CCY = "BTN";
        //                IMPSTransactions.GETCUSTOMERCURRENCY_ACC(_MOBILEBANKING_REQ.REMITTERACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out CCY);
        //                _CREATETRANSACTION_FSFS_REQ.FCUBS_BODY.TransactionDetails.TXNCCY = CCY;
        //            }
        //            catch { }

        //            int status = -1;
        //            IMPSTransactions _ImpsTransaction = new IMPSTransactions();
        //            _ImpsTransaction.VERIFYIMPSACCOUNT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
        //                                              ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
        //                                              ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
        //                                              ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status);
        //            if (status == 0)
        //            {
        //                #region FormatedLogWriting
        //                _CommanDetails.SystemLogger.WriteTransLog(this,
        //                 string.Format("CardStatus    : {0}" + "\t" +
        //                              "TransType      : {1}" + "\t" +
        //                              "PinOffset      : {2}" + "\t" +
        //                              "AmountAvailable: {3}" + "\t" +
        //                              "FTLimit       : {4}" + "\t" +
        //                              "AccountUseLimit   : {5}" + "\t" +
        //                              "AccountUseCount   : {6}" + "\t" +
        //                              "UsedLastDate   : {7}" + "\t" +
        //                              "UsedLastTime   : {8}" + "\t" +
        //                              "MaxPinCount    : {9}" + "\t" +
        //                              "MaxPinUseCount : {10}" + "\t" +
        //                              "AccountNumber  : {11}"
        //                               , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
        //                               , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
        //                #endregion FormatedLogWriting

        //                if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
        //                    _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
        //                else
        //                    _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
        //            }
        //            else
        //            {
        //                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
        //                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
        //                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
        //                _MOBILEBANKING_RESP.ResponseData = null;

        //                _ProcessMessage.LoansAndPaymentTransaction((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.VIEWRECURRINGLOAN, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);


        //                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
        //                                                            , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
        //                return;
        //            }


        //            _ProcessMessage.LoansAndPaymentTransaction((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.VIEWRECURRINGLOAN, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

        //            if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
        //            {
        //                #region Loger
        //                using (var stringWriter = new StringWriter())
        //                {
        //                    using (var xmlWriter = XmlWriter.Create(stringWriter))
        //                    {
        //                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_REQ));
        //                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
        //                    }
        //                    string HostRequestData = stringWriter.ToString();
        //                    XDocument FormattedXML = XDocument.Parse(HostRequestData);
        //                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** View Recurring Loan Details Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
        //                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
        //                }
        //                #endregion

        //                _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
        //                _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
        //            }
        //            else
        //            {
        //                _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
        //                _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
        //                if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
        //                {
        //                    #region Loger
        //                    using (var stringWriter = new StringWriter())
        //                    {
        //                        using (var xmlWriter = XmlWriter.Create(stringWriter))
        //                        {
        //                            XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_REQ));
        //                            _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
        //                        }
        //                        string HostRequestData = stringWriter.ToString();
        //                        XDocument FormattedXML = XDocument.Parse(HostRequestData);
        //                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** View Recurring Loan Details Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
        //                                                                                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
        //                    }
        //                    #endregion

        //                    _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
        //                    _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
        //                }
        //                else
        //                {
        //                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
        //                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
        //                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
        //                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
        //                    _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
        //                    _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
        //                    _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
        //                    return;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
        //            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
        //            _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
        //            _MOBILEBANKING_RESP.ResponseData = null;
        //            _ProcessMessage.LoansAndPaymentTransaction((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.VIEWRECURRINGLOAN, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
        //            _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
        //            return;
        //        }

        //        #endregion

        //        #region Loger
        //        using (var stringWriter = new StringWriter())
        //        {
        //            using (var xmlWriter = XmlWriter.Create(stringWriter))
        //            {
        //                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_RES));
        //                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
        //            }
        //            string HostResponseData = stringWriter.ToString();
        //            XDocument FormattedXML = XDocument.Parse(HostResponseData);
        //            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** View Recurring Loan Details Response recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
        //                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
        //        }
        //        #endregion

        //        _ProcessMessage.ProcessViewRecurringLoanDetails(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
        //    }
        //    catch (Exception ex)
        //    {
        //        _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
        //        _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
        //        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
        //        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
        //        _MOBILEBANKING_RESP.ResponseData = null;
        //    }
        //}

        public void ProcessCreateRecurringAccountToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSAccService.CREATETDCUSTACC_FSFS_REQ _CREATETDCUSTACC_FSFS_REQ,
                                                                ref FCUBSAccService.CREATETDCUSTACC_FSFS_RES _CREATETDCUSTACC_FSFS_RES, FCUBSAccService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                                FCUBSAccService.FCUBSAccServiceSEIClient _FCUBSAccServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    int Accstatus = -1;
                    IMPSTransactions.GETCUSTOMERDETAILS_ACC(_MOBILEBANKING_REQ.BENIFICIARYACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out Accstatus);
                    if (Accstatus == 74)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidCurrencyCode);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidCurrencyCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        return;
                    }
                    try
                    {
                        string CCY = "BTN";
                        IMPSTransactions.GETCUSTOMERCURRENCY_ACC(_MOBILEBANKING_REQ.REMITTERACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out CCY);
                        //_CREATEACCOUNT_FSFS_REQ.FCUBS_BODY.AccountMasterFull.cur.TXNCCY = CCY;
                    }
                    catch { }

                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT_PAYMENT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status,
                                                      ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0 || status == 47 || status == 48)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.CreateRDandTDTransaction((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.CREATERECURRING, enumTransactionType.CreateRecurring, _CREATETDCUSTACC_FSFS_REQ, _CREATETDCUSTACC_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);


                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }

                    _ProcessMessage.CreateRDandTDTransaction((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.CREATERECURRING, enumTransactionType.CreateRecurring, _CREATETDCUSTACC_FSFS_REQ, _CREATETDCUSTACC_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    DataTable DT_CustDetails = new DataTable();
                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSAccService.CREATETDCUSTACC_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETDCUSTACC_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Create Recurring Deposit Account Transaction Request send to HOST for reference number : " + _CREATETDCUSTACC_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        DT_CustDetails = MobilePortalProcess.RecurringAccountDetails(_MOBILEBANKING_REQ.CUSTOMERID, _MOBILEBANKING_REQ.REMITTERACC, out status);
                        if (DT_CustDetails != null && DT_CustDetails.Rows.Count > 0 && Convert.ToDecimal(DT_CustDetails.Rows[0]["BALANCE"].ToString()) >= _MOBILEBANKING_REQ.RDTD_Amount)
                        {
                            _FCUBSAccServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETDCUSTACC_FSFS_RES = _FCUBSAccServiceSEIClient.CreateTDCustAccFS(_CREATETDCUSTACC_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InsufficiantBalance);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETDCUSTACC_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSAccService.CREATETDCUSTACC_FSFS_RES));
                                    _serelized.Serialize(xmlWriter, _CREATETDCUSTACC_FSFS_RES);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Create Term Loan Account Transaction Request send to HOST for reference number : " + _CREATETDCUSTACC_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            DT_CustDetails = MobilePortalProcess.RecurringAccountDetails(_MOBILEBANKING_REQ.CUSTOMERID, _MOBILEBANKING_REQ.REMITTERACC, out status);
                            if (DT_CustDetails != null && DT_CustDetails.Rows.Count > 0 && Convert.ToDecimal(DT_CustDetails.Rows[0][9].ToString()) > _MOBILEBANKING_REQ.RDTD_Amount)
                            {
                                _FCUBSAccServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                                _CREATETDCUSTACC_FSFS_RES = _FCUBSAccServiceSEIClient.CreateTDCustAccFS(_CREATETDCUSTACC_FSFS_REQ);
                            }
                            else
                            {
                                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InsufficiantBalance);
                                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                                _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                                _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                                _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                                _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                                _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                                return;

                            }
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.CreateRDandTDTransaction((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.CREATERECURRING, enumTransactionType.CreateRecurring, _CREATETDCUSTACC_FSFS_REQ, _CREATETDCUSTACC_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSAccService.CREATETDCUSTACC_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETDCUSTACC_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Create Recurring Deposit Account Response recieved from HOST for reference number : " + _CREATETDCUSTACC_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessCreateRecurringLoanAccount(ref _MOBILEBANKING_RESP, _CREATETDCUSTACC_FSFS_REQ, _CREATETDCUSTACC_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETDCUSTACC_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        //public void ProcessTermViewToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
        //                                                       ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
        //                                                       FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        //{
        //    try
        //    {
        //        #region RequestToHost
        //        try
        //        {
        //            int Accstatus = -1;
        //            IMPSTransactions.GETCUSTOMERDETAILS_ACC(_MOBILEBANKING_REQ.BENIFICIARYACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out Accstatus);
        //            if (Accstatus == 74)
        //            {
        //                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidCurrencyCode);
        //                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidCurrencyCode);
        //                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
        //                return;
        //            }
        //            try
        //            {
        //                string CCY = "BTN";
        //                IMPSTransactions.GETCUSTOMERCURRENCY_ACC(_MOBILEBANKING_REQ.REMITTERACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out CCY);
        //                _CREATETRANSACTION_FSFS_REQ.FCUBS_BODY.TransactionDetails.TXNCCY = CCY;
        //            }
        //            catch { }

        //            int status = -1;
        //            IMPSTransactions _ImpsTransaction = new IMPSTransactions();
        //            _ImpsTransaction.VERIFYIMPSACCOUNT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
        //                                              ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
        //                                              ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
        //                                              ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status);
        //            if (status == 0)
        //            {
        //                #region FormatedLogWriting
        //                _CommanDetails.SystemLogger.WriteTransLog(this,
        //                 string.Format("CardStatus    : {0}" + "\t" +
        //                              "TransType      : {1}" + "\t" +
        //                              "PinOffset      : {2}" + "\t" +
        //                              "AmountAvailable: {3}" + "\t" +
        //                              "FTLimit       : {4}" + "\t" +
        //                              "AccountUseLimit   : {5}" + "\t" +
        //                              "AccountUseCount   : {6}" + "\t" +
        //                              "UsedLastDate   : {7}" + "\t" +
        //                              "UsedLastTime   : {8}" + "\t" +
        //                              "MaxPinCount    : {9}" + "\t" +
        //                              "MaxPinUseCount : {10}" + "\t" +
        //                              "AccountNumber  : {11}"
        //                               , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
        //                               , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
        //                #endregion FormatedLogWriting

        //                if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
        //                    _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
        //                else
        //                    _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
        //            }
        //            else
        //            {
        //                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
        //                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
        //                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
        //                _MOBILEBANKING_RESP.ResponseData = null;

        //                _ProcessMessage.LoansAndPaymentTransaction((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.VIEWTERMLOAN, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);


        //                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
        //                                                            , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
        //                return;
        //            }


        //            _ProcessMessage.LoansAndPaymentTransaction((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.VIEWTERMLOAN, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

        //            if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
        //            {
        //                #region Loger
        //                using (var stringWriter = new StringWriter())
        //                {
        //                    using (var xmlWriter = XmlWriter.Create(stringWriter))
        //                    {
        //                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_REQ));
        //                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
        //                    }
        //                    string HostRequestData = stringWriter.ToString();
        //                    XDocument FormattedXML = XDocument.Parse(HostRequestData);
        //                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** View Term Loan Details Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
        //                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
        //                }
        //                #endregion

        //                _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
        //                _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
        //            }
        //            else
        //            {
        //                _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
        //                _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
        //                if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
        //                {
        //                    #region Loger
        //                    using (var stringWriter = new StringWriter())
        //                    {
        //                        using (var xmlWriter = XmlWriter.Create(stringWriter))
        //                        {
        //                            XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_REQ));
        //                            _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
        //                        }
        //                        string HostRequestData = stringWriter.ToString();
        //                        XDocument FormattedXML = XDocument.Parse(HostRequestData);
        //                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** View Term Loan Details Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
        //                                                                                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
        //                    }
        //                    #endregion

        //                    _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
        //                    _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
        //                }
        //                else
        //                {
        //                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
        //                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
        //                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
        //                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
        //                    _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
        //                    _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
        //                    _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
        //                    return;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
        //            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
        //            _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
        //            _MOBILEBANKING_RESP.ResponseData = null;
        //            _ProcessMessage.LoansAndPaymentTransaction((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.VIEWTERMLOAN, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
        //            _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
        //            return;
        //        }

        //        #endregion

        //        #region Loger
        //        using (var stringWriter = new StringWriter())
        //        {
        //            using (var xmlWriter = XmlWriter.Create(stringWriter))
        //            {
        //                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_RES));
        //                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
        //            }
        //            string HostResponseData = stringWriter.ToString();
        //            XDocument FormattedXML = XDocument.Parse(HostResponseData);
        //            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** View Term Loan Details Response recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
        //                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
        //        }
        //        #endregion

        //        //_ProcessMessage.ProcessViewTermLoanDetails(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
        //    }
        //    catch (Exception ex)
        //    {
        //        _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
        //        _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
        //        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
        //        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
        //        _MOBILEBANKING_RESP.ResponseData = null;
        //    }
        //}

        public void ProcessCreateTermLoanAccountToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSAccService.CREATETDCUSTACC_FSFS_REQ _CREATETDCUSTACC_FSFS_REQ,
                                                               ref FCUBSAccService.CREATETDCUSTACC_FSFS_RES _CREATETDCUSTACC_FSFS_RES, FCUBSAccService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                               FCUBSAccService.FCUBSAccServiceSEIClient _FCUBSAccServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    int Accstatus = -1;
                    IMPSTransactions.GETCUSTOMERDETAILS_ACC(_MOBILEBANKING_REQ.BENIFICIARYACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out Accstatus);
                    if (Accstatus == 74)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidCurrencyCode);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidCurrencyCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        return;
                    }
                    try
                    {
                        string CCY = "BTN";
                        IMPSTransactions.GETCUSTOMERCURRENCY_ACC(_MOBILEBANKING_REQ.REMITTERACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out CCY);
                        //_CREATEACCOUNT_FSFS_REQ.FCUBS_BODY.AccountMasterFull.cur.TXNCCY = CCY;
                    }
                    catch { }

                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT_PAYMENT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0 || status == 47 || status == 48)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.CreateRDandTDTransaction((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.CREATETERM, enumTransactionType.CreateTerm, _CREATETDCUSTACC_FSFS_REQ, _CREATETDCUSTACC_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);


                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }


                    _ProcessMessage.CreateRDandTDTransaction((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.CREATETERM, enumTransactionType.CreateTerm, _CREATETDCUSTACC_FSFS_REQ, _CREATETDCUSTACC_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSAccService.CREATETDCUSTACC_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETDCUSTACC_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Create Term Deposit Account Transaction Request send to HOST for reference number : " + _CREATETDCUSTACC_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSAccServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETDCUSTACC_FSFS_RES = _FCUBSAccServiceSEIClient.CreateTDCustAccFS(_CREATETDCUSTACC_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETDCUSTACC_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSAccService.CREATETDCUSTACC_FSFS_RES));
                                    _serelized.Serialize(xmlWriter, _CREATETDCUSTACC_FSFS_RES);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Create Term Deposit Account Transaction Request send to HOST for reference number : " + _CREATETDCUSTACC_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSAccServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETDCUSTACC_FSFS_RES = _FCUBSAccServiceSEIClient.CreateTDCustAccFS(_CREATETDCUSTACC_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.CreateRDandTDTransaction((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.CREATETERM, enumTransactionType.CreateTerm, _CREATETDCUSTACC_FSFS_REQ, _CREATETDCUSTACC_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSAccService.CREATETDCUSTACC_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETDCUSTACC_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Create Term Loan Account Response recieved from HOST for reference number : " + _CREATETDCUSTACC_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessCreateTermLoanAccount(ref _MOBILEBANKING_RESP, _CREATETDCUSTACC_FSFS_REQ, _CREATETDCUSTACC_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETDCUSTACC_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessDonationPaymentToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                                ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                                FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.DONATION, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Donation Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        #endregion Loger
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Donation Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Donation Payment Response recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                //_ProcessMessage.ProcessBillPayment(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
                _ProcessMessage.ProcessBillPayment_Donation(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                //_ProcessMessage.ProcessUnsuccessfullTransactionFD(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }




        public void ProcessVotingPaymentToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                        ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                        FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.SHOWVOTING, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Voting Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        #endregion Loger
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Voting Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Voting Payment Response recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessVotingBillPayment(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                //_ProcessMessage.ProcessUnsuccessfullTransactionFD(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }


        public void ProcessTaxPaymentToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                            ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                            FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    //int Accstatus = -1;
                    //IMPSTransactions.GETCUSTOMERDETAILS_ACC(_MOBILEBANKING_REQ.BENIFICIARYACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out Accstatus);
                    //if (Accstatus == 74)
                    //{
                    //    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidCurrencyCode);
                    //    _MOBILEBANKING_RESP.ResponseDesc = "This Currency is not be allowed for fund transfer.";
                    //    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    //    return;
                    //}

                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT_PAYMENT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0 || status == 47 || status == 48)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.BHUTANTELECOM, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    #region Send Request to RRCO

                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Tax Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Tax Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }

                    #endregion Send Request to BT
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Tax Payment Transaction Response recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessBillPayment(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _ProcessMessage.ProcessUnsuccessfullTransactionUP(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }


        #endregion Process Loans & Deposit

        #region BT PostPaid, LandLine ,BroadBand ,LeaseLine

        public void ProcessBTPostPaidPaymentToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                       ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                       FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT_PAYMENT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0 || status == 47 || status == 48)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Account Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.BHUTANTELECOM, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    #region Send Request to BT
                    if (!CONFIGURATIONCONFIGDATA.BTPOSTPAID)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BTIsUnderMaintenance);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BTIsUnderMaintenance);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        return;
                    }
                    //else if (!_HTTPCommunicationChanel.CheckBTPostPaidConnection())
                    //{
                    //    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectBT);
                    //    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectBT);
                    //    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    //    _MOBILEBANKING_RESP.ResponseData = null;
                    //    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    //    return;
                    //}

                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT PostPaid Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT PostPaid Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                    #endregion Send Request to BT
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT PostPaid Payment recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessBillPayment(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _ProcessMessage.ProcessUnsuccessfullTransactionUP(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }

        public void ProcessBTBroadBandPostpaidPaymentToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                       ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                       FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT_PAYMENT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0 || status == 47 || status == 48)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Account Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.BHUTANTELECOM, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    #region Send Request to BT
                    if (!CONFIGURATIONCONFIGDATA.BTPOSTPAID)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BTIsUnderMaintenance);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BTIsUnderMaintenance);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        return;
                    }
                    //else if (!_HTTPCommunicationChanel.CheckBTPostPaidConnection())
                    //{
                    //    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectBT);
                    //    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectBT);
                    //    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    //    _MOBILEBANKING_RESP.ResponseData = null;
                    //    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    //    return;
                    //}

                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Broad Band PostPaid Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Broad Band PostPaid Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                    #endregion Send Request to BT
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Broad Band PostPaid Payment recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessBillPayment(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _ProcessMessage.ProcessUnsuccessfullTransactionUP(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }

        public void ProcessBTBroadBandPrePaidPaymentToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                      ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                      FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT_PAYMENT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0 || status == 47 || status == 48)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Account Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.BHUTANTELECOM, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    #region Send Request to BT
                    if (!CONFIGURATIONCONFIGDATA.BTPOSTPAID)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BTIsUnderMaintenance);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BTIsUnderMaintenance);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        return;
                    }
                    //else if (!_HTTPCommunicationChanel.CheckBTPostPaidConnection())
                    //{
                    //    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectBT);
                    //    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectBT);
                    //    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    //    _MOBILEBANKING_RESP.ResponseData = null;
                    //    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    //    return;
                    //}

                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Broad Band PrePaid Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Broad Band PrePaid Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                    #endregion Send Request to BT
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Broad Band PrePaid Payment recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessBillPayment(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _ProcessMessage.ProcessUnsuccessfullTransactionUP(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }

        public void ProcessBTLandLinePaymentToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                      ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                      FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT_PAYMENT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0 || status == 47 || status == 48)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Account Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.BHUTANTELECOM, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    #region Send Request to BT
                    if (!CONFIGURATIONCONFIGDATA.BTPOSTPAID)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BTIsUnderMaintenance);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BTIsUnderMaintenance);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        return;
                    }
                    //else if (!_HTTPCommunicationChanel.CheckBTPostPaidConnection())
                    //{
                    //    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectBT);
                    //    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectBT);
                    //    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    //    _MOBILEBANKING_RESP.ResponseData = null;
                    //    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    //    return;
                    //}

                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Land Line Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Land Line Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                    #endregion Send Request to BT
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Land Line Payment recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessBillPayment(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _ProcessMessage.ProcessUnsuccessfullTransactionUP(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }



        public void ProcessBTLeaseLinePaymentToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                     ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                     FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT_PAYMENT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0 || status == 47 || status == 48)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Account Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.BTLEASELINEPAYMENT, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    #region Send Request to BT
                    if (!CONFIGURATIONCONFIGDATA.BTPOSTPAID)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BTIsUnderMaintenance);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BTIsUnderMaintenance);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        return;
                    }

                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT LeaseLine Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT LeaseLine Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;

                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                    #endregion Send Request to BT
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT LeaseLine Payment recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessBillPayment(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _ProcessMessage.ProcessUnsuccessfullTransactionUP(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }


        public void ProcessBngulFundTransforToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                       ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                       FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    //_ImpsTransaction.VERIFYIMPSACCOUNT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                    //                                  ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                    //                                  ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                    //                                  ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                    //                                  , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit);



                    _ImpsTransaction.VERIFYIMPSACCOUNTBngul(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                     ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                     ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                     ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                     , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                     ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);


                    if (status == 0)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Account Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.BNGULPAYMENT, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    #region Send Request to BT
                    if (!CONFIGURATIONCONFIGDATA.BTPOSTPAID)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BTIsUnderMaintenance);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BTIsUnderMaintenance);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        return;
                    }

                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BNgul Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BNgul Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                    #endregion Send Request to BT
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BNgul Payment recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessBNgulBillPayment(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _ProcessMessage.ProcessUnsuccessfullTransactionUP(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }




        #endregion BT PostPaid, LandLine , BroadBand ,LeaseLine

        #region BT PostPaid, LandLine & BroadBand

        public void ProcessRICBLifeInsurancePaymentToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                       ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                       FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT_PAYMENT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0 || status == 47 || status == 48)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Account Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.BHUTANTELECOM, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    #region Send Request to BT
                    if (!CONFIGURATIONCONFIGDATA.RICBACTIVE)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.RICBIsUnderMaintenance);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.RICBIsUnderMaintenance);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        return;
                    }
                    //else if (!_HTTPCommunicationChanel.CheckBTPostPaidConnection())
                    //{
                    //    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectBT);
                    //    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectBT);
                    //    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    //    _MOBILEBANKING_RESP.ResponseData = null;
                    //    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    //    return;
                    //}

                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** RICB Life Insurance Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** RICB Life Insurance Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                    #endregion Send Request to BT
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** RICB Life Insurance Payment recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessBillPayment(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _ProcessMessage.ProcessUnsuccessfullTransactionUP(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }

        public void ProcessRICBCreditPaymentToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                      ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                      FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT_PAYMENT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0 || status == 47 || status == 48)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Account Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.BHUTANTELECOM, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    #region Send Request to BT
                    if (!CONFIGURATIONCONFIGDATA.RICBACTIVE)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.RICBIsUnderMaintenance);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.RICBIsUnderMaintenance);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        return;
                    }
                    //else if (!_HTTPCommunicationChanel.CheckBTPostPaidConnection())
                    //{
                    //    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectBT);
                    //    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectBT);
                    //    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    //    _MOBILEBANKING_RESP.ResponseData = null;
                    //    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    //    return;
                    //}

                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** RICB Credit Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** RICB Credit Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                    #endregion Send Request to BT
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** RICB Credit Payment recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessBillPayment(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _ProcessMessage.ProcessUnsuccessfullTransactionUP(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }

        public void ProcessRICBAnnuityPaymentToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                      ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                      FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {
                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT_PAYMENT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0 || status == 47 || status == 48)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Account Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.BHUTANTELECOM, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    #region Send Request to BT
                    if (!CONFIGURATIONCONFIGDATA.RICBACTIVE)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.RICBIsUnderMaintenance);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.RICBIsUnderMaintenance);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        return;
                    }
                    //else if (!_HTTPCommunicationChanel.CheckBTPostPaidConnection())
                    //{
                    //    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectBT);
                    //    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectBT);
                    //    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    //    _MOBILEBANKING_RESP.ResponseData = null;
                    //    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    //    return;
                    //}

                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Life Annuity Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Life Annuity Payment Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                    #endregion Send Request to BT
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion

                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** RICB Annuity Payment recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion

                _ProcessMessage.ProcessBillPayment(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _ProcessMessage.ProcessUnsuccessfullTransactionUP(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }

        #endregion BT PostPaid, LandLine & BroadBand

        #region cheque deposit
        public void ProcessChequeDepositToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                          ref FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                          FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                #region RequestToHost
                try
                {

                    int status = -1;
                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    _ImpsTransaction.VERIFYIMPSACCOUNT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    if (status == 0)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        // _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Account Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                    // _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.DRUKCOM, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);


                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {
                        #region Loger
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                            }
                            string HostRequestData = stringWriter.ToString();
                            XDocument FormattedXML = XDocument.Parse(HostRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Cheque Deposit Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                            FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                        }
                        #endregion

                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                    }
                    else
                    {
                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Cheque Deposit Transaction Request send to HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateUPTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }

                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    // _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }


                #region Loger
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSUPService.CREATEUPTRANSACTION_FSFS_RES));
                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    }
                    string HostResponseData = stringWriter.ToString();
                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("*****Cheque Deposit Response recieved from HOST for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                }
                #endregion
                #endregion

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                //  _ProcessMessage.ProcessUnsuccessfullTransactionUP(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }


        #endregion

        public void ProcessAccountQueryFromHostPortal(ref MOBILEPORTAL_RES _REGISTRATION_RESP, MOBILEPORTAL_REQ _REGISTRATION_REQ)
        {
            MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg _SwitchConsumerRequestReqMsg;
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Account Query(portal) Transaction Request Sent To HostInterface For Reference Number : " + _REGISTRATION_REQ.ReferenceNumber + Environment.NewLine));
                }

                try
                {
                    _SwitchConsumerRequestReqMsg = new MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg();
                    _SwitchConsumerRequestReqMsg.ResponseCode = "-1";
                    _SwitchConsumerRequestReqMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage;
                    _SwitchConsumerRequestReqMsg.TransactionMode = MaxiSwitch.API.Terminal.enumModeOfTransaction.OnUs;
                    _SwitchConsumerRequestReqMsg.TransactionType = MaxiSwitch.API.Terminal.enumTransactionType.AccountVerification;
                    _SwitchConsumerRequestReqMsg.ProcessingCode = "820000";
                    MessageConvertor(ref _REGISTRATION_REQ, ref _SwitchConsumerRequestReqMsg);
                    var task = Task.Factory.StartNew(() =>
                    {
                        HostCommChanel.SendTransRequest.Invoke(ref _SwitchConsumerRequestReqMsg);
                    });
                    task.Wait(Convert.ToInt32(TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut).TotalMilliseconds));
                    if (!task.IsCompleted)
                    {
                        if (_SwitchConsumerRequestReqMsg.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage)
                            _SwitchConsumerRequestReqMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationResponseMessage;
                        _REGISTRATION_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                        _REGISTRATION_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_REGISTRATION_RESP.ResponseCode);
                        _REGISTRATION_RESP.ReferenceNumber = _SwitchConsumerRequestReqMsg.ReferenceNumber;
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Trasaction Timeout at Host Interface for Reference Number : " + _SwitchConsumerRequestReqMsg.ReferenceNumber));
                        return;
                    }
                    MessageConvertor(ref _REGISTRATION_RESP, ref _SwitchConsumerRequestReqMsg);
                }
                catch (Exception ex)
                {
                    _REGISTRATION_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _REGISTRATION_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_REGISTRATION_RESP.ResponseCode);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }

                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Account Query Response Recieved From HostInterface For Reference Number : " + _REGISTRATION_RESP.ReferenceNumber + Environment.NewLine));
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "(Account Query) Error Occured In Send Request To HostInterface For Reference Number : " + _REGISTRATION_RESP.ReferenceNumber + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _REGISTRATION_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _REGISTRATION_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_REGISTRATION_RESP.ResponseCode);
            }
        }


        public void MessageConvertor(ref MOBILEPORTAL_REQ _REGISTRATION_REQ, ref MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg)
        {
            try
            {

                RequestMsg.CardNumber = Convert.ToString(CONFIGURATIONCONFIGDATA.BankBIN + _REGISTRATION_REQ.ACCOUNTNUMBER).PadRight(16, '0');
                RequestMsg.AcquirerInstCode = CONFIGURATIONCONFIGDATA.BankBIN;
                RequestMsg.TransactionAmount = string.IsNullOrEmpty(_REGISTRATION_REQ.Amount) ? "0" : _REGISTRATION_REQ.Amount;
                RequestMsg.LocalTransactionDateTime = DateTime.Now;
                RequestMsg.CardScheme = MaxiSwitch.API.Terminal.enumCardScheme.BFS;
                RequestMsg.CardType = MaxiSwitch.API.Terminal.enumCardType.Debit;
                RequestMsg.SystemsTraceAuditNumber = _REGISTRATION_REQ.ReferenceNumber;
                RequestMsg.ReferenceNumber = _REGISTRATION_REQ.ReferenceNumber;
                RequestMsg.TransactionRefrenceNumber = _REGISTRATION_REQ.ReferenceNumber;
                //RequestMsg.TerminalID = _REGISTRATION_REQ.DeviceID.Substring(0, 16);
                RequestMsg.TerminalID = "1234567890321456";
                RequestMsg.TerminalLocation = "";
                RequestMsg.FromAccountNumber = _REGISTRATION_REQ.ACCOUNTNUMBER;
                RequestMsg.ToAccountNumber = _REGISTRATION_REQ.ACCOUNTNUMBER;
                RequestMsg.DeliveryChannel = "BWY";
                RequestMsg.CardAccepterName = "THIMPHU                THIMPHU      THBT";
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
        }

        public void MessageConvertor(ref MOBILEPORTAL_RES _REGISTRATION_RESP, ref MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg)
        {
            try
            {
                _REGISTRATION_RESP.ResponseCode = "";
                _REGISTRATION_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(RequestMsg.ResponseCode);
                _REGISTRATION_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescriptionHost(_REGISTRATION_RESP.ResponseCode);
                ////_REGISTRATION_RESP.DeviceID = RequestMsg.TerminalID;
                _REGISTRATION_RESP.ReferenceNumber = RequestMsg.ReferenceNumber;
                // _REGISTRATION_RESP= RequestMsg.FromAccountNumber;
                //_REGISTRATION_RESP.CustomerID = "1234";
                //_REGISTRATION_RESP.CustomerName = "ABC";
                //_REGISTRATION_RESP.MobileNumber = "17851854";
                //_REGISTRATION_RESP.EmailID = "archana.hadawale@maximusinfoware.in";
                if (!string.IsNullOrEmpty(RequestMsg.MiniStateMentData) && RequestMsg.TransactionType == MaxiSwitch.API.Terminal.enumTransactionType.AccountVerification)
                {
                    string[] ExtendedData = RequestMsg.MiniStateMentData.Split('|');
                    _REGISTRATION_RESP.CustomerID = RequestMsg.MiniStateMentData.Substring(0, 9);
                    _REGISTRATION_RESP.CustomerName = RequestMsg.MiniStateMentData.Substring(89, 80).Trim();
                    _REGISTRATION_RESP.MobileNumber = RequestMsg.MobileNumber.Substring(7, 8);
                    //_REGISTRATION_RESP.AccountType = "SAVING";// (RequestMsg.MiniStateMentData.Substring(182, 3).Contains('S') ? "SAVING" : RequestMsg.MiniStateMentData.Substring(182, 3).Contains('C') ? "CURRENT" : RequestMsg.MiniStateMentData.Substring(128, 3).Contains('O') ? "OVERDRAFT" : "SAVING");
                    //if (ExtendedData.Length == 1)
                    //{
                    //    _REGISTRATION_RESP.MobileNumber = "17858685";
                    //    _REGISTRATION_RESP.EmailID = "krgurung@drukpnbbank.bt";
                    //if (RequestMsg.FromAccountNumber == "110020001137")
                    //{
                    //    _REGISTRATION_RESP.MobileNumber = "17851854";
                    //    _REGISTRATION_RESP.EmailID = "sanjeev.atharga@maximusinfoware.in";
                    //}
                    //else if (RequestMsg.FromAccountNumber == "110020001155")
                    //{
                    //    _REGISTRATION_RESP.MobileNumber = "17605864";
                    //    _REGISTRATION_RESP.EmailID = "vaibhav.gawde@maximusinfoware.in";
                    //}
                    //else if (RequestMsg.FromAccountNumber == "110020001146")
                    //{
                    //    _REGISTRATION_RESP.MobileNumber = "17426261";
                    //    _REGISTRATION_RESP.EmailID = "ibs.mbs@drukpnbbank.bt";
                    //}
                    //else if (RequestMsg.FromAccountNumber == "110020000323")
                    //{
                    //    _REGISTRATION_RESP.MobileNumber = "17858685";
                    //    _REGISTRATION_RESP.EmailID = "krgurung@drukpnbbank.bt";
                    //}
                    //    }

                    _CommanDetails.SystemLogger.WriteTransLog(null, "Portal");
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
        }

        public string GenerateMSGID()
        {
            DateTime date = DateTime.Now;
            int m = date.Month;
            int d = date.Day;
            int y = date.Year;
            int jjj = date.DayOfYear;
            string jd = y.ToString().Substring(y.ToString().Length - 1) + jjj.ToString().PadLeft(3, '0') +
                        DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0');
            return jd.PadLeft(14, '0');

        }

        public string GenerateReferenceNumber()
        {
            DateTime date = DateTime.Now;
            int m = date.Month;
            int d = date.Day;
            int y = date.Year;
            int jjj = date.DayOfYear;
            string jd = y.ToString().Substring(y.ToString().Length - 1) + jjj.ToString().PadLeft(3, '0') +
                        DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0');
            return jd.PadLeft(12, '0');

        }

        public void ProcessQRInwardFundTransferToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSRTService.CREATETRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                           ref FCUBSRTService.CREATETRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSRTService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                           FCUBSRTService.FCUBSRTServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {

                MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg ReqMsg = new MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg();
                _MOBILEBANKING_REQ.CUST_AC_NO = _MOBILEBANKING_REQ.BENIFICIARYACC;
                _ProcessMessage.TransactionInwardIntraFundTrans((int)enumCommandTypeEnum.AuthorizationRequestMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 1);

                #region RequestToHost
                try
                {
                    //DataTable DTNQRCAccountNumber = null;
                    //DTNQRCAccountNumber = IMPSTransactions.GetNQRCPrimaryAccount(_MOBILEBANKING_REQ.QRUniquePANNumber);
                    //_CommanDetails.SystemLogger.WriteTransLog(this, "3");
                    //if (DTNQRCAccountNumber != null && DTNQRCAccountNumber.Rows.Count >0)
                    //{
                    //    _CommanDetails.SystemLogger.WriteTransLog(this, "4");
                    //    _MOBILEBANKING_REQ.BENIFICIARYACC = DTNQRCAccountNumber.Rows[0][0].ToString();
                    //    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidAccount);
                    //    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidAccount);
                    //    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    //    return;
                    //}

                    int Accstatus = -1;
                    if (_MOBILEBANKING_REQ.QRTYPE == "OnUs")
                    {


                        IMPSTransactions.GETCUSTOMERDETAILS_ACC(_MOBILEBANKING_REQ.BENIFICIARYACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out Accstatus);
                        if (Accstatus == 74)
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidCurrencyCode);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidCurrencyCode);
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                            return;
                        }
                    }
                    try
                    {
                        string CCY = "BTN";
                        IMPSTransactions.GETCUSTOMERCURRENCY_ACC(_MOBILEBANKING_REQ.REMITTERACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out CCY);
                        _CREATETRANSACTION_FSFS_REQ.FCUBS_BODY.TransactionDetails.TXNCCY = CCY;
                    }
                    catch { }


                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();


                    #region VERIFYIMPSACCOUNT
                    /*
                    int status = -1;
                    _ImpsTransaction.VERIFYIMPSACCOUNT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);



                    if (status == 0)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;


                        _ProcessMessage.TransactionIntraFundTransforMobile((int)enumCommandTypeEnum.AuthorizationResponseMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);


                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _MOBILEBANKING_REQ.TransactionRefrenceNumber, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;
                    }
                     *  */
                    #endregion VERIFYIMPSACCOUNT



                    _ProcessMessage.TransactionInwardIntraFundTrans((int)enumCommandTypeEnum.AuthorizationRequestMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);


                    #region Loger
                    try
                    {
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEBANKING_REQ));
                                _serelized.Serialize(xmlWriter, _MOBILEBANKING_REQ);
                            }
                            string MobileRequestData = stringWriter.ToString();

                            XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("*****NQRC Inward Intra Fund Transaction Request recived from middle ware Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                            FormattedXML.ToString() + Environment.NewLine));
                        }

                    }
                    catch (Exception ex)
                    {
                        _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    }
                    #endregion

                    if (_MOBILEBANKING_REQ.QRTYPE == "OnUs")
                    {

                        #region Loger
                        try
                        {
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string MobileRequestData = stringWriter.ToString();

                                XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("*****NQRC Inward Intra Fund request send to HOST for Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                FormattedXML.ToString() + Environment.NewLine));
                            }

                        }
                        catch (Exception ex)
                        {
                            _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        }
                        #endregion
                        _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                        _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                        #region Loger
                        try
                        {
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_RES));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                                }
                                string MobileRequestData = stringWriter.ToString();

                                XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("*****NQRC Inward Intra Fund response recived from HOST for Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                FormattedXML.ToString() + Environment.NewLine));
                            }

                        }
                        catch (Exception ex)
                        {
                            _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        }
                        #endregion

                        _ProcessMessage.ProcessInwardIntraFundTransfer(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

                        // _ProcessMessage.TransactionInwardIntraFundTrans((int)enumCommandTypeEnum.AuthorizationRequestMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    }

                    #region Loger
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEBANKING_RESP));
                            _serelized.Serialize(xmlWriter, _MOBILEBANKING_RESP);
                        }
                        string HostRequestData = stringWriter.ToString();
                        XDocument FormattedXML = XDocument.Parse(HostRequestData);
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** NQRC Inward Fund Transfer Transaction Response send to middlware for reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                        FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                    }
                    #endregion


                    #region Old
                    //                    else
                    //                    {
                    //                        if (_MOBILEBANKING_REQ.QRTYPE == "OnUs")
                    //                        {
                    //                            #region Loger
                    //                            try
                    //                            {
                    //                                using (var stringWriter = new StringWriter())
                    //                                {
                    //                                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    //                                    {
                    //                                        XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEBANKING_REQ));
                    //                                        _serelized.Serialize(xmlWriter, _MOBILEBANKING_REQ);
                    //                                    }
                    //                                    string MobileRequestData = stringWriter.ToString();

                    //                                    XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                    //                                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("*****NQRC inward Intra Fund Transaction Request recived from middle ware Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                    //                                                                                    FormattedXML.ToString() + Environment.NewLine));
                    //                                }

                    //                            }
                    //                            catch (Exception ex)
                    //                            {
                    //                                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    //                            }
                    //                            #endregion

                    //                            _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                    //                            _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);

                    //                            if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                    //                            {
                    //                                #region Loger
                    //                                try
                    //                                {
                    //                                    using (var stringWriter = new StringWriter())
                    //                                    {
                    //                                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                    //                                        {
                    //                                            XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_REQ));
                    //                                            _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                    //                                        }
                    //                                        string MobileRequestData = stringWriter.ToString();

                    //                                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                    //                                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("*****NQRC inward Intra Fund request send to HOST for Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                    //                                                                                        FormattedXML.ToString() + Environment.NewLine));
                    //                                    }

                    //                                }
                    //                                catch (Exception ex)
                    //                                {
                    //                                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    //                                }
                    //                                #endregion

                    //                                _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                    //                                _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateTransactionFS(_CREATETRANSACTION_FSFS_REQ);

                    //                                #region Loger
                    //                                try
                    //                                {
                    //                                    using (var stringWriter = new StringWriter())
                    //                                    {
                    //                                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                    //                                        {
                    //                                            XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_RES));
                    //                                            _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                    //                                        }
                    //                                        string MobileRequestData = stringWriter.ToString();

                    //                                        XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                    //                                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("*****NQRC inward Intra Fund response recived from HOST for Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                    //                                                                                        FormattedXML.ToString() + Environment.NewLine));
                    //                                    }

                    //                                }
                    //                                catch (Exception ex)
                    //                                {
                    //                                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    //                                }
                    //                                #endregion



                    //                                _ProcessMessage.ProcessInwardIntraFundTransfer(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

                    //                            }
                    //                            else
                    //                            {
                    //                                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                    //                                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    //                                _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                    //                                _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                    //                                _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                    //                                _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                    //                                _MOBILEBANKING_RESP.MSGSTAT = "FAILED";

                    //                                _ProcessMessage.TransactionInwardIntraFundTrans((int)enumCommandTypeEnum.AuthorizationResponseMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    //                                return;
                    //                            }

                    //                            #region Loger
                    //                            try
                    //                            {
                    //                                using (var stringWriter = new StringWriter())
                    //                                {
                    //                                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    //                                    {
                    //                                        XmlSerializer _serelized = new XmlSerializer(typeof(MOBILEBANKING_RESP));
                    //                                        _serelized.Serialize(xmlWriter, _MOBILEBANKING_RESP);
                    //                                    }
                    //                                    string MobileRequestData = stringWriter.ToString();

                    //                                    XDocument FormattedXML = XDocument.Parse(MobileRequestData);
                    //                                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("*****NQRC inward Intra Fund Response send to middleware for Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                    //                                                                                    FormattedXML.ToString() + Environment.NewLine));
                    //                                }

                    //                            }
                    //                            catch (Exception ex)
                    //                            {
                    //                                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    //                            }
                    //                            #endregion
                    //                        }


                    //                    }

                    #endregion Old
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionInwardIntraFundTrans((int)enumCommandTypeEnum.AuthorizationResponseMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion




            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                // _ProcessMessage.ProcessUnsuccessfullTransactionFD(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
                _ProcessMessage.TransactionInwardIntraFundTrans((int)enumCommandTypeEnum.AuthorizationResponseMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

            }
        }

        public void ProcessQROutwardFundTransferToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSRTService.CREATETRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                               ref FCUBSRTService.CREATETRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSRTService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                               FCUBSRTService.FCUBSRTServiceSEIClient _FCUBSRTServiceSEIClient, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg ReqMsg = new MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg();

                #region RequestToHost
                try
                {

                    int status = -1;

                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    if (_MOBILEBANKING_REQ.QRTYPE == "Acquire")
                    {
                        _ImpsTransaction.VERIFYIMPSACCOUNTACQ(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                     ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                     ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                     ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset,
                                                     out status, ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                  ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    }

                    if (status == 0)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _ProcessMessage.TransactionAcquireOutwardFundTransfer((int)enumCommandTypeEnum.AuthorizationRequestMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;

                    }

                    _ProcessMessage.TransactionAcquireOutwardFundTransfer((int)enumCommandTypeEnum.AuthorizationRequestMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {


                        if (_MOBILEBANKING_REQ.QRTYPE == "Acquire")
                        {
                            #region Request Send To MQ NQRC HOST

                            _MOBILEBANKING_REQ.Payloadformatindicator = NQRCConfiguration.PAYLOAD_FORMAT_INDICATOR;
                            _MOBILEBANKING_REQ.Pointofinitiationmethod = NQRCConfiguration.POINT_OF_INITIATION_METHOD;

                            _ProcessMessage.TransactionAcquireOutwardFundTransfer((int)enumCommandTypeEnum.AuthorizationResponseMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);


                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** NQRC outward Fund Transfer Transaction Request send to HOST for debit for  reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateTransactionFS(_CREATETRANSACTION_FSFS_REQ);

                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_RES));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** NQRC outward Fund Transfer Transaction response recived from HOST for debit for  reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion

                            if (_CREATETRANSACTION_FSFS_RES.FCUBS_HEADER.MSGSTAT.ToString().Contains("SUCCESS"))
                            {


                                _CommanDetails.SystemLogger.WriteTransLog(this, "NQRC outward Transaction Request send to message convert for Reference  number :" + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID);

                                _CommanDetails.SystemLogger.WriteTransLog(this, "NQRC outward Transaction Request send to message convert for Reference  number Send to middleware :" + _MOBILEBANKING_REQ.QRMSGID);

                                MsgConvertorAcquirer(ref ReqMsg, _MOBILEBANKING_REQ);

                                _CommanDetails.SystemLogger.WriteTransLog(this, " NQRC outward Transaction Request send to middleware for Reference  number :" + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID);



                                MessageQCommunicationChanel.SendTransRequest.Invoke(ref ReqMsg);

                                _CommanDetails.SystemLogger.WriteTransLog(this, "*****NQRC outward Transfer Intra Fund Transfer transaction response recived from  Middleware Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID);

                                // ResponseConvertor(ref ReqMsg, _MOBILEBANKING_RESP);
                                ResponseConvertor(ReqMsg, ref _MOBILEBANKING_RESP);

                                _CommanDetails.SystemLogger.WriteTransLog(this, "*****NQRCF Outward Fund Transfer  response convert successful for Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID);

                                _CommanDetails.SystemLogger.WriteTransLog(this, "*****NQRC Ouward Fund Transfer Transaction ResponseCode :  " + ReqMsg.ResponseCode + "  for Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID);

                                #region old 
                                /*
                                if (_MOBILEBANKING_RESP.ResponseCode == "00")
                                {
                                    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.ReferenceNumber;
                                    //_MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                                    //_MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescriptionHost(_MOBILEBANKING_RESP.ResponseCode);
                                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                                    _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                                    _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                                    _MOBILEBANKING_RESP.MSGSTAT = "SUCCESS";
                                    _ProcessMessage.ProcessNQRCOutwardFundTransfer(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
                                }
                                else
                                {
                                    

                                    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.ReferenceNumber;
                                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ReqMsg.ResponseCode);
                                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescriptionHost(_MOBILEBANKING_RESP.ResponseCode);
                                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                                    _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                                    _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                                    _MOBILEBANKING_RESP.MSGSTAT = "FAILD";

                                    _MOBILEBANKING_RESP.ResponseData = null;
                                    _CommanDetails.SystemLogger.WriteTransLog(this, "*****NQRCFundTransfer Error at RMA :  " + Convert.ToInt16(ReqMsg.ResponseCode) + "for Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID);
                                    _ProcessMessage.TransactionAcquireOutwardFundTransfer((int)enumCommandTypeEnum.AuthorizationResponseMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                                   return;
                                }
*/
                                #endregion old

                                _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.ReferenceNumber;
                                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescriptionHost(_MOBILEBANKING_RESP.ResponseCode);
                                _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                                _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                                _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                                _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                                _MOBILEBANKING_RESP.MSGSTAT = "SUCCESS";
                                _ProcessMessage.ProcessNQRCOutwardFundTransfer(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

                            }
                            else
                            {
                                _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.ReferenceNumber;
                                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescriptionHost(_MOBILEBANKING_RESP.ResponseCode);
                                _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                                _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                                _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                                _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                                _MOBILEBANKING_RESP.ResponseData = null;
                                _CommanDetails.SystemLogger.WriteTransLog(this, "*****NQRCFundTransfer Error at CBS :  " + Convert.ToInt16(ReqMsg.ResponseCode) + "for Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID);
                                _ProcessMessage.TransactionAcquireOutwardFundTransfer((int)enumCommandTypeEnum.AuthorizationResponseMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                                return;

                            }

                            #endregion Request Send To MQ NQRC HOST

                        }
                    }
                    else
                    {

                        _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                        _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                        if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                        {
                            if (_MOBILEBANKING_REQ.QRTYPE == "Acquire")
                            {
                                #region Request Send To MQ NQRC HOST

                                _MOBILEBANKING_REQ.Payloadformatindicator = NQRCConfiguration.PAYLOAD_FORMAT_INDICATOR;
                                _MOBILEBANKING_REQ.Pointofinitiationmethod = NQRCConfiguration.POINT_OF_INITIATION_METHOD;

                                _ProcessMessage.TransactionAcquireOutwardFundTransfer((int)enumCommandTypeEnum.AuthorizationResponseMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);


                                #region Loger
                                using (var stringWriter = new StringWriter())
                                {
                                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                                    {
                                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_REQ));
                                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                    }
                                    string HostRequestData = stringWriter.ToString();
                                    XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** NQRC outward Fund Transfer Transaction Request send to HOST for debit for  reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                                }
                                #endregion

                                _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                                _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateTransactionFS(_CREATETRANSACTION_FSFS_REQ);

                                #region Loger
                                using (var stringWriter = new StringWriter())
                                {
                                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                                    {
                                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_RES));
                                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                                    }
                                    string HostRequestData = stringWriter.ToString();
                                    XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** NQRC outward Fund Transfer Transaction Request send to HOST for debit for  reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                                }
                                #endregion

                                if (_CREATETRANSACTION_FSFS_RES.FCUBS_HEADER.MSGSTAT.ToString().Contains("SUCCESS"))
                                {

                                    _CommanDetails.SystemLogger.WriteTransLog(this, " NQRC outward Transaction Request send to Message Converter for Reference  number :" + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID);
                                    MsgConvertorAcquirer(ref ReqMsg, _MOBILEBANKING_REQ);
                                    _CommanDetails.SystemLogger.WriteTransLog(this, " NQRC outward Transaction Request send to Middleware for Reference  number :" + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID);
                                    MessageQCommunicationChanel.SendTransRequest.Invoke(ref ReqMsg);
                                    _CommanDetails.SystemLogger.WriteTransLog(this, "*****NQRC outward  Transaction response recived from  Middleware Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID);

                                    // ResponseConvertor(ref ReqMsg, _MOBILEBANKING_RESP);
                                    ResponseConvertor(ReqMsg, ref _MOBILEBANKING_RESP);

                                    _CommanDetails.SystemLogger.WriteTransLog(this, "*****NQRCF Outward Fund Transfer  response Converstion Successful for Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID);
                                    _CommanDetails.SystemLogger.WriteTransLog(this, "*****NQRC Ouward Fund Transfer Transaction ResponseCode :  " + Convert.ToInt16(ReqMsg.ResponseCode) + "  for Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID);

                                    //if (Convert.ToInt16(ReqMsg.ResponseCode) == (int)MaxiSwitch.API.Terminal.enumTransactionStatus.Successful)
                                    #region old
                                    /*             if (_MOBILEBANKING_RESP.ResponseCode == "00")
                                                 {
                                                     _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.ReferenceNumber;
                                                     _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                                                     _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescriptionHost(_MOBILEBANKING_RESP.ResponseCode);
                                                     _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                                                     _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                                                     _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                                                     _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                                                     _MOBILEBANKING_RESP.MSGSTAT = "SUCCESS";
                                                     _ProcessMessage.ProcessNQRCOutwardFundTransfer(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
                                                     _CommanDetails.SystemLogger.WriteTransLog(this, "*****NQRCFundTransfer Sucess from RMA :  " + Convert.ToInt16(ReqMsg.ResponseCode) + "  for Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID);

                                                 }
                                                 else
                                                 {
                                                     _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.ReferenceNumber;
                                                     _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ReqMsg.ResponseCode);
                                                     _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescriptionHost(_MOBILEBANKING_RESP.ResponseCode);
                                                     _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                                                     _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                                                     _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                                                     _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                                                     _MOBILEBANKING_RESP.MSGSTAT = "FAILD";

                                                     _MOBILEBANKING_RESP.ResponseData = null;
                                                     _ProcessMessage.TransactionAcquireOutwardFundTransfer((int)enumCommandTypeEnum.AuthorizationResponseMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                                                     _CommanDetails.SystemLogger.WriteTransLog(this, "*****NQRCFundTransfer Error at RMA :  " + Convert.ToInt16(ReqMsg.ResponseCode) + "for Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID);

                                                     //_ProcessMessage.TransactionIntraFundTransforMobile((int)enumCommandTypeEnum.AuthorizationResponseMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                                                     return;
                                                 }

                                                 */
                                    #endregion old

                                    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.ReferenceNumber;
                                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescriptionHost(_MOBILEBANKING_RESP.ResponseCode);
                                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                                    _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                                    _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                                    _MOBILEBANKING_RESP.MSGSTAT = "SUCCESS";
                                    _ProcessMessage.ProcessNQRCOutwardFundTransfer(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
                                    _CommanDetails.SystemLogger.WriteTransLog(this, "*****NQRCFundTransfer  from RMA :  " + Convert.ToInt16(ReqMsg.ResponseCode) + "  for Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID);

                                }
                                else
                                {
                                    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.ReferenceNumber;
                                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescriptionHost(_MOBILEBANKING_RESP.ResponseCode);
                                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                                    _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                                    _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                                    _MOBILEBANKING_RESP.MSGSTAT = "FAILD";
                                    _CommanDetails.SystemLogger.WriteTransLog(this, "*****NQRCFundTransfer Error at CBS :  " + Convert.ToInt16(ReqMsg.ResponseCode) + "for Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID);

                                    _MOBILEBANKING_RESP.ResponseData = null;
                                    _ProcessMessage.TransactionAcquireOutwardFundTransfer((int)enumCommandTypeEnum.AuthorizationResponseMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                                    return;

                                }
                                #endregion Request Send To MQ NQRC HOST
                            }
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                            _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                            _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                            _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionAcquireOutwardFundTransfer((int)enumCommandTypeEnum.AuthorizationResponseMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion RequestToHost
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _ProcessMessage.ProcessUnsuccessfullTransactionFD(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }

        public void ProcessQROnusFundTransferToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, FCUBSRTService.CREATETRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ,
                                                             ref FCUBSRTService.CREATETRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, FCUBSRTService.FCUBS_HEADERType _FCUBS_HEADERType,
                                                             FCUBSRTService.FCUBSRTServiceSEIClient _FCUBSRTServiceSEIClient, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg ReqMsg = new MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg();

                #region RequestToHost
                try
                {
                    int Accstatus = -1;




                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("NQRC OnUs Scan  For Reference Number : {0} \t Remitter  Account Status :{1} \t Benificiary Account :{2} "
                                                           , _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID, _MOBILEBANKING_REQ.REMITTERACC, _MOBILEBANKING_REQ.BENIFICIARYACC));

                    IMPSTransactions.GETCUSTOMERDETAILS_ACC(_MOBILEBANKING_REQ.BENIFICIARYACC, "", out Accstatus);
                    if (Accstatus == 74)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidCurrencyCode);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidCurrencyCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        return;
                    }

                    try
                    {
                        string CCY = "BTN";
                        IMPSTransactions.GETCUSTOMERCURRENCY_ACC(_MOBILEBANKING_REQ.REMITTERACC, _MOBILEBANKING_REQ.BENIFICIARYMOBILE, out CCY);
                        _CREATETRANSACTION_FSFS_REQ.FCUBS_BODY.TransactionDetails.TXNCCY = CCY;
                    }
                    catch { }



                    int status = -1;

                    IMPSTransactions _ImpsTransaction = new IMPSTransactions();
                    if (_MOBILEBANKING_REQ.QRTYPE == "OnUs")
                    {

                        _ImpsTransaction.VERIFYIMPSACCOUNT(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.TXNAMT,
                                                      ref _MOBILEBANKING_REQ.AmountAvailable, ref _MOBILEBANKING_REQ.FtLimit, ref _MOBILEBANKING_REQ.AccountUseLimit,
                                                      ref _MOBILEBANKING_REQ.AccountUseCount, ref _MOBILEBANKING_REQ.LastDate, ref _MOBILEBANKING_REQ.LastTime,
                                                      ref _MOBILEBANKING_REQ.MaxPinCount, ref _MOBILEBANKING_REQ.MaxPinUseCount, ref _MOBILEBANKING_REQ.PinOffset, out status
                                                      , ref _MOBILEBANKING_REQ.ACQAmountAvailable, ref _MOBILEBANKING_REQ.ACQFtLimit,
                                                      ref _MOBILEBANKING_REQ.BNgulAmountAvailable, ref _MOBILEBANKING_REQ.BNgulFtLimit);
                    }


                    if (status == 0)
                    {
                        #region FormatedLogWriting
                        _CommanDetails.SystemLogger.WriteTransLog(this,
                         string.Format("CardStatus    : {0}" + "\t" +
                                      "TransType      : {1}" + "\t" +
                                      "PinOffset      : {2}" + "\t" +
                                      "AmountAvailable: {3}" + "\t" +
                                      "FTLimit       : {4}" + "\t" +
                                      "AccountUseLimit   : {5}" + "\t" +
                                      "AccountUseCount   : {6}" + "\t" +
                                      "UsedLastDate   : {7}" + "\t" +
                                      "UsedLastTime   : {8}" + "\t" +
                                      "MaxPinCount    : {9}" + "\t" +
                                      "MaxPinUseCount : {10}" + "\t" +
                                      "AccountNumber  : {11}"
                                       , enumResponseCode.Approved, null, _MOBILEBANKING_REQ.PinOffset, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.FtLimit, _MOBILEBANKING_REQ.AccountUseLimit
                                       , _MOBILEBANKING_REQ.AccountUseCount, _MOBILEBANKING_REQ.LastDate, _MOBILEBANKING_REQ.LastTime, _MOBILEBANKING_REQ.MaxPinCount, _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.CUST_AC_NO));
                        #endregion FormatedLogWriting

                        if (_MOBILEBANKING_REQ.LastDate == "000000" && _MOBILEBANKING_REQ.LastTime == "000000")
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                        else
                            _MOBILEBANKING_REQ.LastDateTime = DateTime.ParseExact(_MOBILEBANKING_REQ.LastDate + _MOBILEBANKING_REQ.LastTime, "ddMMyyHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(status.ToString());
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _MOBILEBANKING_RESP.ResponseData = null;

                        _ProcessMessage.TransactionOnusIntraFundTrans((int)enumCommandTypeEnum.AuthorizationResponseMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Card Verification Failed For Reference Number : {0} \t Account Verification Status :{1} \t Response Code :{2} "
                                                                    , _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID, _MOBILEBANKING_RESP.ResponseDesc, _MOBILEBANKING_REQ.ResponseCode));
                        return;

                    }



                    _ProcessMessage.TransactionOnusIntraFundTrans((int)enumCommandTypeEnum.AuthorizationRequestMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                    if (CONFIGURATIONCONFIGDATA.SKIPMPIN)
                    {



                        if (_MOBILEBANKING_REQ.QRTYPE == "OnUs")
                        {
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_REQ));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                }
                                string HostRequestData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** NQRC Fund Transfer Transaction Request send to HOST (SKIP) for  reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion
                            _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                            _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateTransactionFS(_CREATETRANSACTION_FSFS_REQ);
                            _ProcessMessage.ProcessNQRCIntraFundTransfer(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);
                            #region Loger
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_RES));
                                    _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                                }
                                string HostResponseData = stringWriter.ToString();
                                XDocument FormattedXML = XDocument.Parse(HostResponseData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("NQRC Intra Fund Transfer Transaction Response Recived from Host(Onus NCQRC) Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                            }
                            #endregion
                        }

                    }
                    else
                    {
                        if (_MOBILEBANKING_REQ.QRTYPE == "OnUs")
                        {
                            _Authentication.TransactionRefrenceNumber = _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID;
                            _SSM.VerifyPin(ref _Authentication, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.mPIN, _MOBILEBANKING_REQ.DeviceID);
                            if (_Authentication.TransactionStatus == enumTransactionStatus.Successful)
                            {
                                #region Loger
                                using (var stringWriter = new StringWriter())
                                {
                                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                                    {
                                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_REQ));
                                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_REQ);
                                    }
                                    string HostRequestData = stringWriter.ToString();
                                    XDocument FormattedXML = XDocument.Parse(HostRequestData);
                                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** NQRC Fund Transfer Transaction Request send to HOST (SKIP) for  reference number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                                }
                                #endregion


                                _FCUBSRTServiceSEIClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut);
                                _CREATETRANSACTION_FSFS_RES = _FCUBSRTServiceSEIClient.CreateTransactionFS(_CREATETRANSACTION_FSFS_REQ);

                                #region Loger
                                using (var stringWriter = new StringWriter())
                                {
                                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                                    {
                                        XmlSerializer _serelized = new XmlSerializer(typeof(FCUBSRTService.CREATETRANSACTION_FSFS_RES));
                                        _serelized.Serialize(xmlWriter, _CREATETRANSACTION_FSFS_RES);
                                    }
                                    string HostResponseData = stringWriter.ToString();
                                    XDocument FormattedXML = XDocument.Parse(HostResponseData);
                                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("NQRC Intra Fund Transfer Transaction Response Recived from Host(Onus NCQRC) Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                                                                                    FormattedXML.ToString() + Environment.NewLine + Environment.NewLine));
                                }
                                #endregion

                                _ProcessMessage.ProcessNQRCIntraFundTransfer(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ);

                            }
                            else
                            {
                                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IncorrectMPIN);
                                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                                _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_RESP.DeviceID;
                                _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_RESP.ReferenceNumber;
                                _MOBILEBANKING_RESP.CUST_AC_NO = _MOBILEBANKING_RESP.CUST_AC_NO;
                                _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_RESP.MobileNumber;
                                _MOBILEBANKING_RESP.MSGSTAT = "FAILED";
                                _MOBILEBANKING_RESP.ResponseData = null;
                                _ProcessMessage.TransactionOnusIntraFundTrans((int)enumCommandTypeEnum.AuthorizationResponseMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                                return;
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionOnusIntraFundTrans((int)enumCommandTypeEnum.AuthorizationResponseMessage, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                #endregion


            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To Host For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _ProcessMessage.ProcessUnsuccessfullTransactionFD(ref _MOBILEBANKING_RESP, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType);
            }
        }

        public void MsgConvertorAcquirer(ref MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg ReqMessage, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Request Recived in method  MsgConvertorAcquirer() to set ReqMessage parameters for Reference  Number :" + _MOBILEBANKING_REQ.QRMSGID);
                //SwitchConsumerRequestReqMsg ReqMessage = new SwitchConsumerRequestReqMsg();
                ReqMessage.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AccountingAuthorizationRequestMessage;
                ReqMessage.TransSource = MaxiSwitch.API.Terminal.enumTransactionSource.Terminal;
                ReqMessage.TransactionMode = MaxiSwitch.API.Terminal.enumModeOfTransaction.OnUs;
                ReqMessage.TransactionType = MaxiSwitch.API.Terminal.enumTransactionType.Purchase;
                ReqMessage.CardNumber = _MOBILEBANKING_REQ.Merchantidentifier;
                ReqMessage.ProcessingCode = "280000";
                ReqMessage.TransactionAmount = _MOBILEBANKING_REQ.TXNAMT.ToString();
                ReqMessage.LocalTransactionDateTime = DateTime.Now;
                ReqMessage.GmtTransactionDateTime = ReqMessage.LocalTransactionDateTime.AddHours(-6);
                ReqMessage.SystemsTraceAuditNumber = DateTime.Now.ToString("HHmmss");
                ReqMessage.MerchantCategoryCode = _MOBILEBANKING_REQ.MerchantCategoryCode;
                ReqMessage.TransactionCurrencyCode = _MOBILEBANKING_REQ.TransactionCurrencyCode;
                ReqMessage.PanEntryMode = "081";
                ReqMessage.PosConditionCode = "00";
                ReqMessage.TransactionRefrenceNumber = _MOBILEBANKING_REQ.QRMSGID;
                ReqMessage.TerminalID = "BNBQR001";
                ReqMessage.TerminalLocation = _MOBILEBANKING_REQ.NQRCcity.PadLeft(15, ' ');
                //ReqMessage.CardAccepterName = _MOBILEBANKING_REQ.DeviceLocation.PadLeft(23, ' ') + _MOBILEBANKING_REQ.NQRCcity.PadLeft(13, ' ')
                //                            + _MOBILEBANKING_REQ.NQRCcity.Substring(0, 2) + "BT";

                ReqMessage.CardAccepterName = _MOBILEBANKING_REQ.MerchantName.PadLeft(25, ' ') + _MOBILEBANKING_REQ.NQRCcity.PadLeft(13, ' ') + "BT";


                ReqMessage.FromAccountNumber = _MOBILEBANKING_REQ.REMITTERACC;
                ReqMessage.ToAccountNumber = _MOBILEBANKING_REQ.BENIFICIARYACC;
                //   ReqMessage.MiniStateMentData = _MOBILEBANKING_REQ.QRValue;//"000201010211021694004900123546205204581253030645803BTN5924VISA TEST MERCHANT THREE6007THIMPHU6222030600125607080000309063043617";
                ReqMessage.DeliveryChannel = "NQRC";
                ReqMessage.AcquirerInstCode = "639545";

                //  ReqMessage.MiniStateMentData = _MOBILEBANKING_REQ.QRValue;

                string DE120 = string.Empty;
                string Remitter = (_MOBILEBANKING_REQ.REMITTERNAME.Length > 20 ? _MOBILEBANKING_REQ.REMITTERNAME.Substring(0, 20) : _MOBILEBANKING_REQ.REMITTERNAME);
                string Benificiary = (_MOBILEBANKING_REQ.BENIFICIARYNAME.Length > 20 ? _MOBILEBANKING_REQ.BENIFICIARYNAME.Substring(0, 20) : _MOBILEBANKING_REQ.BENIFICIARYNAME);
                string Remark = (_MOBILEBANKING_REQ.Remark.Length > 20 ? _MOBILEBANKING_REQ.Remark.Substring(0, 20) : _MOBILEBANKING_REQ.Remark);

                DE120 = "001003004" +
                       "002002QR" +
                       "003" + (Remitter.Length).ToString().PadLeft(3, '0') + Remitter +
                       "004" + (Benificiary.Length).ToString().PadLeft(3, '0') + Benificiary +
                       "005" + (Remark.Length).ToString().PadLeft(3, '0') + Remark +
                       "006" + (ReqMessage.TransactionRefrenceNumber.Length).ToString().PadLeft(3, '0') + ReqMessage.TransactionRefrenceNumber +
                       "007" + (ReqMessage.AcquirerInstCode.Length).ToString().PadLeft(3, '0') + ReqMessage.AcquirerInstCode +
                       "008" + (CONFIGURATIONCONFIGDATA.BankCode.Length).ToString().PadLeft(3, '0') + CONFIGURATIONCONFIGDATA.BankCode +
                       "009008" + _MOBILEBANKING_REQ.Merchantidentifier.Substring(0, 8);

                ReqMessage.MiniStateMentData = DE120;


                // DE120;// DE120.Length.ToString().PadLeft(3,'0')+//_MOBILEBANKING_REQ.QRValue;//"000201010211021694004900123546205204581253030645803BTN5924VISA TEST MERCHANT THREE6007THIMPHU6222030600125607080000309063043617";




                _CommanDetails.SystemLogger.WriteTransLog(this, "Response Send to process host message convert successfully for Reference  Number :  :" + ReqMessage.TransactionRefrenceNumber);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "MsgConvertorAcquirer : " + ex.StackTrace.ToString() + "\n" + ex.Message.ToString() + "\n" + ReqMessage.TransactionRefrenceNumber);
            }
        }

        public MOBILEBANKING_RESP ResponseConvertor(MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg SwitchReqMsg, ref MOBILEBANKING_RESP _MOBILEBANKING_RESP)
        {
            try
            {
                _MOBILEBANKING_RESP.ResponseCode = "";

                if (SwitchReqMsg.ResponseCode == "")
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, "SwitchReqMsg.ResponseCode Not recived: " + SwitchReqMsg.ResponseCode + "for  Reference  number " + SwitchReqMsg.TransactionRefrenceNumber);
                    _MOBILEBANKING_RESP.ResponseCode = "-1";
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescriptionHost(SwitchReqMsg.ResponseCode);
                }


                if (SwitchReqMsg.ResponseCode != "")
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, "SwitchReqMsg.ResponseCode Else: " + SwitchReqMsg.ResponseCode + "for  Reference  number " + SwitchReqMsg.TransactionRefrenceNumber);
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(SwitchReqMsg.ResponseCode);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescriptionHost(SwitchReqMsg.ResponseCode);
                }
                _MOBILEBANKING_RESP.MSGID = SwitchReqMsg.TransactionRefrenceNumber;

                _MOBILEBANKING_RESP.DeviceID = SwitchReqMsg.TerminalID;
                _MOBILEBANKING_RESP.ReferenceNumber = SwitchReqMsg.TransactionRefrenceNumber;
                _MOBILEBANKING_RESP.CUST_AC_NO = SwitchReqMsg.FromAccountNumber;
                _MOBILEBANKING_RESP.MobileNumber = SwitchReqMsg.MobileNumber;
                _MOBILEBANKING_RESP.TRNDT = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                _MOBILEBANKING_RESP.DateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                _MOBILEBANKING_RESP.MSGSTAT = "SUCCESS";
                return _MOBILEBANKING_RESP;
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "ResponseConvertor : " + ex.StackTrace.ToString() + "\n" + ex.Message.ToString() + "\n" + _MOBILEBANKING_RESP.ReferenceNumber);
                return _MOBILEBANKING_RESP;
            }

        }

        #region Host Processing Using ISO Message Format

        public void ProcessAccountQueryFromHost(ref REGISTRATION_RES _REGISTRATION_RESP, REGISTRATION_REQ _REGISTRATION_REQ)
        {
            MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg _SwitchConsumerRequestReqMsg;
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Account Query Transaction Request Sent To HostInterface For Reference Number : " + _REGISTRATION_REQ.ReferenceNumber + Environment.NewLine));
                }

                try
                {
                    _SwitchConsumerRequestReqMsg = new MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg();
                    _SwitchConsumerRequestReqMsg.ResponseCode = "-1";
                    _SwitchConsumerRequestReqMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage;
                    _SwitchConsumerRequestReqMsg.TransactionMode = MaxiSwitch.API.Terminal.enumModeOfTransaction.OnUs;
                    _SwitchConsumerRequestReqMsg.TransactionType = MaxiSwitch.API.Terminal.enumTransactionType.AccountVerification;
                    _SwitchConsumerRequestReqMsg.ProcessingCode = "820000";
                    MessageConvertor(ref _REGISTRATION_REQ, ref _SwitchConsumerRequestReqMsg);
                    var task = Task.Factory.StartNew(() =>
                    {
                        HostCommChanel.SendTransRequest.Invoke(ref _SwitchConsumerRequestReqMsg);
                    });
                    task.Wait(Convert.ToInt32(TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut).TotalMilliseconds));
                    if (!task.IsCompleted)
                    {
                        if (_SwitchConsumerRequestReqMsg.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage)
                            _SwitchConsumerRequestReqMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationResponseMessage;
                        _REGISTRATION_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                        _REGISTRATION_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_REGISTRATION_RESP.ResponseCode);
                        _REGISTRATION_RESP.ReferenceNumber = _SwitchConsumerRequestReqMsg.ReferenceNumber;
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Trasaction Timeout at Host Interface for Reference Number : " + _SwitchConsumerRequestReqMsg.ReferenceNumber));
                        return;
                    }
                    MessageConvertor(ref _REGISTRATION_RESP, ref _SwitchConsumerRequestReqMsg);
                }
                catch (Exception ex)
                {
                    _REGISTRATION_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _REGISTRATION_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_REGISTRATION_RESP.ResponseCode);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }

                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Account Query Response Recieved From HostInterface For Reference Number : " + _REGISTRATION_RESP.ReferenceNumber + Environment.NewLine));
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "(Account Query) Error Occured In Send Request To HostInterface For Reference Number : " + _REGISTRATION_RESP.ReferenceNumber + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _REGISTRATION_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _REGISTRATION_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_REGISTRATION_RESP.ResponseCode);
            }
        }
 

        public void ProcessCustomerDataValidationFromHost(ref REGISTRATION_RES _REGISTRATION_RESP, REGISTRATION_REQ _REGISTRATION_REQ)
        {
            MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg _SwitchConsumerRequestReqMsg;
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Customer Data Validation Transaction Request Sent To HostInterface For Reference Number : " + _REGISTRATION_REQ.ReferenceNumber + Environment.NewLine));
                }

                try
                {
                    _SwitchConsumerRequestReqMsg = new MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg();
                    _SwitchConsumerRequestReqMsg.ResponseCode = "-1";
                    _SwitchConsumerRequestReqMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage;
                    _SwitchConsumerRequestReqMsg.TransactionMode = MaxiSwitch.API.Terminal.enumModeOfTransaction.OnUs;
                    _SwitchConsumerRequestReqMsg.TransactionType = MaxiSwitch.API.Terminal.enumTransactionType.AccountVerification;
                    _SwitchConsumerRequestReqMsg.ProcessingCode = "910000";
                    MessageConvertor(ref _REGISTRATION_REQ, ref _SwitchConsumerRequestReqMsg);
                    var task = Task.Factory.StartNew(() =>
                    {
                        HostCommChanel.SendTransRequest.Invoke(ref _SwitchConsumerRequestReqMsg);
                    });
                    task.Wait(Convert.ToInt32(TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut).TotalMilliseconds));
                    if (!task.IsCompleted)
                    {
                        if (_SwitchConsumerRequestReqMsg.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage)
                            _SwitchConsumerRequestReqMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationResponseMessage;
                        _REGISTRATION_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                        _REGISTRATION_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_REGISTRATION_RESP.ResponseCode);
                        _REGISTRATION_RESP.ReferenceNumber = _SwitchConsumerRequestReqMsg.ReferenceNumber;
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("(Customer Data Validation) Trasaction Timeout at Host Interface for Reference Number : " + _SwitchConsumerRequestReqMsg.ReferenceNumber));
                        return;
                    }
                    MessageConvertor(ref _REGISTRATION_RESP, ref _SwitchConsumerRequestReqMsg);
                }
                catch (Exception ex)
                {
                    _REGISTRATION_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _REGISTRATION_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_REGISTRATION_RESP.ResponseCode);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }

                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Customer Data Validation Response Recieved From HostInterface For Reference Number : " + _REGISTRATION_RESP.ReferenceNumber + Environment.NewLine));
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "(Customer Data Validation) Error Occured In Send Request To HostInterface For Reference Number : " + _REGISTRATION_RESP.ReferenceNumber + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _REGISTRATION_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _REGISTRATION_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_REGISTRATION_RESP.ResponseCode);
            }
        }

        public void ProcessBalanceinquiryToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Balance Enquiry Transaction Request Sent to HostInterface For Reference Number : " + _MOBILEBANKING_REQ.ReferenceNumber + Environment.NewLine));
                }
                _ProcessMessage.TransactionBalanceinquiry((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                try
                {
                    MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg = new MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg();
                    _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                    RequestMsg.TransactionType = MaxiSwitch.API.Terminal.enumTransactionType.BalanceEnquiry;
                    MessageConvertor(ref _MOBILEBANKING_REQ, ref RequestMsg);
                    var task = Task.Factory.StartNew(() =>
                    {
                        HostCommChanel.SendTransRequest.Invoke(ref RequestMsg);
                    });
                    task.Wait(Convert.ToInt32(TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut).TotalMilliseconds));
                    if (!task.IsCompleted)
                    {
                        if (RequestMsg.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage)
                            RequestMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationResponseMessage;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.ReferenceNumber = RequestMsg.ReferenceNumber;
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Trasaction TimeOut at Host Interface for Reference Number : " + RequestMsg.ReferenceNumber));
                        return;
                    }
                    MessageConvertor(ref _MOBILEBANKING_RESP, ref RequestMsg);
                    _ProcessMessage.TransactionBalanceinquiry((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _ProcessMessage.ProcessUnsuccessfullTransactionBLQ(ref _MOBILEBANKING_RESP, null, null, null, _MOBILEBANKING_REQ);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    _ProcessMessage.TransactionBalanceinquiry((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    return;
                }

                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Balance Enquiry Response Received From HostInterface For Reference Number : " + _MOBILEBANKING_RESP.ReferenceNumber + Environment.NewLine));
                }
                //_ProcessMessage.ProcessBalanceinquiry(ref _MOBILEBANKING_RESP, _SwitchConsumerRequestReqMsg, _MOBILEBANKING_REQ);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To HostInterface For Reference Number : " + _MOBILEBANKING_RESP.ReferenceNumber + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                _ProcessMessage.ProcessUnsuccessfullTransactionBLQ(ref _MOBILEBANKING_RESP, null, null, null, _MOBILEBANKING_REQ);
            }
        }

        public void ProcessMinistatementToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Ministatement Transaction Request Sent To HostInterface For Reference Number : " + _MOBILEBANKING_REQ.ReferenceNumber + Environment.NewLine));
                }
                _ProcessMessage.TransactionGenerateMiniStatement((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                try
                {
                    //_ProcessMessage.TransactionGenerateMiniStatement((int)enumCommandTypeEnum.AuthorizationRequestMessage,_MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 3);
                    MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg = new MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg();
                    _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                    RequestMsg.TransactionType = MaxiSwitch.API.Terminal.enumTransactionType.MiniStatement;
                    MessageConvertor(ref _MOBILEBANKING_REQ, ref RequestMsg);
                    var task = Task.Factory.StartNew(() =>
                    {
                        HostCommChanel.SendTransRequest.Invoke(ref RequestMsg);
                    });
                    task.Wait(Convert.ToInt32(TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut).TotalMilliseconds));
                    if (!task.IsCompleted)
                    {
                        if (RequestMsg.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage)
                            RequestMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationResponseMessage;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.ReferenceNumber = RequestMsg.ReferenceNumber;
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Trasaction TimeOut at Host Interface for Reference Number : " + RequestMsg.ReferenceNumber));
                        return;
                    }
                    MessageConvertor(ref _MOBILEBANKING_RESP, ref RequestMsg);
                    _ProcessMessage.TransactionGenerateMiniStatement((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Ministatement Response Recieved From HostInterface For Reference Number : " + _MOBILEBANKING_RESP.ReferenceNumber + Environment.NewLine));
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To HostInterface For Reference Number : " + _MOBILEBANKING_RESP.ReferenceNumber + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
            }
        }

        public void ProcessFundTransferToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                if (_MOBILEBANKING_REQ.IsAccountFT)
                {
                    _ProcessMessage.TransactionIntraFundTransforACC((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 2);
                }
                else if (_MOBILEBANKING_REQ.IsMobileFT)
                {
                    _ProcessMessage.TransactionIntraFundTransforMobile((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 2);
                }
                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Fund Transfer Transaction Request Sent To HostInterface For Reference Number : " + _MOBILEBANKING_REQ.ReferenceNumber + Environment.NewLine));
                }
                try
                {
                    MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg = new MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg();
                    _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                    RequestMsg.TransactionType = MaxiSwitch.API.Terminal.enumTransactionType.FundTransfer;
                    MessageConvertor(ref _MOBILEBANKING_REQ, ref RequestMsg);
                    var task = Task.Factory.StartNew(() =>
                    {
                        HostCommChanel.SendTransRequest.Invoke(ref RequestMsg);
                    });
                    task.Wait(Convert.ToInt32(TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut).TotalMilliseconds));
                    if (!task.IsCompleted)
                    {
                        if (RequestMsg.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage)
                            RequestMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationResponseMessage;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.ReferenceNumber = RequestMsg.ReferenceNumber;
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Trasaction TimeOut at Host Interface For Reference Number : " + RequestMsg.ReferenceNumber));
                        return;
                    }
                    MessageConvertor(ref _MOBILEBANKING_RESP, ref RequestMsg);
                    if (_MOBILEBANKING_REQ.IsAccountFT)
                    {
                        _ProcessMessage.TransactionIntraFundTransforACC((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 3);
                    }
                    else if (_MOBILEBANKING_REQ.IsMobileFT)
                    {
                        _ProcessMessage.TransactionIntraFundTransforMobile((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 3);
                    }
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Fund Transfer Response Recieved From HostInterface For Reference Number : " + _MOBILEBANKING_RESP.ReferenceNumber + Environment.NewLine));
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To HostInterface For Reference Number : " + _MOBILEBANKING_RESP.ReferenceNumber + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
            }
        }




        #region BillProcessing 
        public void BillProcessFundTransferToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                if (_MOBILEBANKING_REQ.IsAccountFT)
                {
                    _ProcessMessage.TransactionIntraFundTransforACC((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                }
                else if (_MOBILEBANKING_REQ.IsMobileFT)
                {
                    _ProcessMessage.TransactionIntraFundTransforMobile((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                }
                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Bill Processing Fund Transfer Transaction Request Sent To HostInterface For Reference Number : " + _MOBILEBANKING_REQ.ReferenceNumber + Environment.NewLine));
                }
                try
                {
                    MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg = new MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg();
                    _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;

                    if (_MOBILEBANKING_REQ.TransType == enumTransactionType.BTRecharge.ToString())
                    {
                        RequestMsg.TransactionType = MaxiSwitch.API.Terminal.enumTransactionType.MobileTopUp; //abhishek
                    }

                    MessageConvertor(ref _MOBILEBANKING_REQ, ref RequestMsg);
                    var task = Task.Factory.StartNew(() =>
                    {
                        HostCommChanel.SendTransRequest.Invoke(ref RequestMsg);
                    });
                    task.Wait(Convert.ToInt32(TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut).TotalMilliseconds));
                    if (!task.IsCompleted)
                    {
                        if (RequestMsg.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage)
                            RequestMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationResponseMessage;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.ReferenceNumber = RequestMsg.ReferenceNumber;
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Trasaction TimeOut at Host Interface For Reference Number : " + RequestMsg.ReferenceNumber));
                        if (_MOBILEBANKING_REQ.IsAccountFT)
                        {
                            _ProcessMessage.TransactionIntraFundTransforACC((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        }
                        else if (_MOBILEBANKING_REQ.IsMobileFT)
                        {
                            _ProcessMessage.TransactionIntraFundTransforMobile((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        }
                        return;
                    }
                    MessageConvertor(ref _MOBILEBANKING_RESP, ref RequestMsg);

                    if (_MOBILEBANKING_REQ.IsAccountFT)
                    {
                        _ProcessMessage.TransactionIntraFundTransforACC((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    }
                    else if (_MOBILEBANKING_REQ.IsMobileFT)
                    {
                        _ProcessMessage.TransactionIntraFundTransforMobile((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    }
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    if (_MOBILEBANKING_REQ.IsAccountFT)
                    {
                        _ProcessMessage.TransactionIntraFundTransforACC((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    }
                    else if (_MOBILEBANKING_REQ.IsMobileFT)
                    {
                        _ProcessMessage.TransactionIntraFundTransforMobile((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    }
                    return;
                }

                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Fund Transfer Response Recieved From HostInterface For Reference Number : " + _MOBILEBANKING_RESP.ReferenceNumber + Environment.NewLine));
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To HostInterface For Reference Number : " + _MOBILEBANKING_RESP.ReferenceNumber + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                if (_MOBILEBANKING_REQ.IsAccountFT)
                {
                    _ProcessMessage.TransactionIntraFundTransforACC((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                }
                else if (_MOBILEBANKING_REQ.IsMobileFT)
                {
                    _ProcessMessage.TransactionIntraFundTransforMobile((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                }
            }
        }
        #endregion BillProcessing

        // Added by Krn on 21-11-22..
        public void ProcessAccountVerificationTransactionToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsgRMA = new MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg();
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Account Verification Transaction Request Sent To Message Queue For Reference Number : " + _MOBILEBANKING_REQ.ReferenceNumber + Environment.NewLine));
                }
                try
                {
                    _ProcessMessage.TransactionAccountVerification((int)enumCommandTypeEnum.AuthorizationResponseMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 3);
                    
                    MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg = new MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg();
                    _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                    RequestMsg.TransactionType = MaxiSwitch.API.Terminal.enumTransactionType.PosAccountVerification; //for account verification(PosAccountVerification) 
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** before RMA Connection" ));
                    var taskMQ = Task.Factory.StartNew(() =>
                    {
                        RequestMsgRMA = MessageConvertor(ref _MOBILEBANKING_REQ);
                        MessageQCommunicationChanel.SendTransRequest.Invoke(ref RequestMsgRMA);
                        if (RequestMsgRMA.ResponseCode != CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved))
                            RequestMsg.ResponseCode = RequestMsgRMA.ResponseCode;
                        else
                        {
                            RequestMsg.ResponseCode = RequestMsgRMA.ResponseCode;
                            RequestMsg.AuthorizationCode = (string.IsNullOrEmpty(RequestMsg.AuthorizationCode) ? RequestMsg.AuthorizationCode : RequestMsgRMA.AuthorizationCode);
                        }
                    });
                    taskMQ.Wait(Convert.ToInt32(TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut).TotalMilliseconds));
                    if (!taskMQ.IsCompleted)
                    {
                        if (RequestMsg.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage)
                            RequestMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationResponseMessage;

                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.ReferenceNumber = RequestMsg.ReferenceNumber;
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _MOBILEBANKING_RESP.MSGSTAT = "Transaction UnSuccessful.";
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("(MQ) Trasaction TimeOut  For Reference Number : " + RequestMsg.ReferenceNumber));
                        // return;
                    }
                    //}
                    MessageConvertor(ref _MOBILEBANKING_RESP, ref RequestMsgRMA);
                    _ProcessMessage.TransactionAccountVerification((int)enumCommandTypeEnum.AuthorizationResponseMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 5);
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("no rma connection"));
                    return;
                }

                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Account Verificaion Response Recieved From MQ For Reference Number : " + _MOBILEBANKING_RESP.ReferenceNumber + Environment.NewLine));
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To MQ For Reference Number : " + _MOBILEBANKING_RESP.ReferenceNumber + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                _ProcessMessage.TransactionAccountVerification((int)enumCommandTypeEnum.AuthorizationResponseMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 4);
            }
        }

        // Added by Krn on 21-11-22
        public void ProcessQRCodeVerificationTransactionToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsgRMA = new MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg();
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** QR Code Verification Transaction Request Sent To Message Queue For Reference Number : " + _MOBILEBANKING_REQ.ReferenceNumber + Environment.NewLine));
                }
                try
                {
                    _ProcessMessage.TransactionORCodeVerification((int)enumCommandTypeEnum.AuthorizationResponseMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 3);
                    MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg = new MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg();
                    _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                    //RequestMsg.TransactionType = MaxiSwitch.API.Terminal.enumTransactionType.PosAccountVerification; //for account verification(PosAccountVerification) 
                    var taskMQ = Task.Factory.StartNew(() =>
                    {
                        RequestMsgRMA = MessageConvertor(ref _MOBILEBANKING_REQ);
                        MessageQCommunicationChanel.SendTransRequest.Invoke(ref RequestMsgRMA);
                        if (RequestMsgRMA.ResponseCode != CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved))
                            RequestMsg.ResponseCode = RequestMsgRMA.ResponseCode;
                        else
                        {
                            RequestMsg.ResponseCode = RequestMsgRMA.ResponseCode;
                            RequestMsg.AuthorizationCode = (string.IsNullOrEmpty(RequestMsg.AuthorizationCode) ? RequestMsg.AuthorizationCode : RequestMsgRMA.AuthorizationCode);
                        }
                    });
                    taskMQ.Wait(Convert.ToInt32(TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut).TotalMilliseconds));
                    if (!taskMQ.IsCompleted)
                    {
                        if (RequestMsg.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage)
                            RequestMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationResponseMessage;

                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.ReferenceNumber = RequestMsg.ReferenceNumber;
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _MOBILEBANKING_RESP.MSGSTAT = "Transaction UnSuccessful.";
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("(MQ) Trasaction TimeOut at Host Interface For Reference Number : " + RequestMsg.ReferenceNumber));
                        // return;
                    }
                    //}
                    MessageConvertor(ref _MOBILEBANKING_RESP, ref RequestMsgRMA);
                    _ProcessMessage.TransactionORCodeVerification((int)enumCommandTypeEnum.AuthorizationResponseMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 5);
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("no rma connection"));
                    return;
                }

                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** QR Code Verificaion Response Recieved From HostInterface For Reference Number : " + _MOBILEBANKING_RESP.ReferenceNumber + Environment.NewLine));
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To HostInterface For Reference Number : " + _MOBILEBANKING_RESP.ReferenceNumber + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                _ProcessMessage.TransactionORCodeVerification((int)enumCommandTypeEnum.AuthorizationResponseMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 4);
            }
        }


        public void ProcessOutwardTransactionToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsgRMA;
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Outward Fund Transfer Transaction Request Sent To HostInterface For Reference Number : " + _MOBILEBANKING_REQ.ReferenceNumber + Environment.NewLine));
                }
                try
                {
                    _ProcessMessage.TransactionOutwardFundTransfer((int)enumCommandTypeEnum.AuthorizationResponseMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 2);
                    MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg = new MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg();
                    _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                    RequestMsg.TransactionType = MaxiSwitch.API.Terminal.enumTransactionType.Debit;
                    MessageConvertor(ref _MOBILEBANKING_REQ, ref RequestMsg);
                    var task = Task.Factory.StartNew(() =>
                    {
                        HostCommChanel.SendTransRequest.Invoke(ref RequestMsg);
                    });
                    task.Wait(Convert.ToInt32(TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut).TotalMilliseconds));
                    if (!task.IsCompleted)
                    {
                        if (RequestMsg.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage)
                            RequestMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationResponseMessage;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.ReferenceNumber = RequestMsg.ReferenceNumber;
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Trasaction TimeOut at Host Interface For Reference Number : " + RequestMsg.ReferenceNumber));
                        return;
                    }
                    _ProcessMessage.TransactionOutwardFundTransfer((int)enumCommandTypeEnum.AuthorizationResponseMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 3);
                    if (Convert.ToInt32(RequestMsg.ResponseCode) == (int)MaxiSwitch.API.Terminal.enumResponseCode.Approved && RequestMsg.TransStatus == MaxiSwitch.API.Terminal.enumTransactionStatus.Successful &&
                            RequestMsg.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationResponseMessage)
                    {
                        _ProcessMessage.TransactionOutwardFundTransfer((int)enumCommandTypeEnum.AuthorizationResponseMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 4);
                        var taskMQ = Task.Factory.StartNew(() =>
                        {
                            RequestMsgRMA = MessageConvertor(ref _MOBILEBANKING_REQ);
                            MessageQCommunicationChanel.SendTransRequest.Invoke(ref RequestMsgRMA);
                            if (RequestMsgRMA.ResponseCode != CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved))
                                RequestMsg.ResponseCode = RequestMsgRMA.ResponseCode;
                            else
                            {
                                RequestMsg.ResponseCode = RequestMsgRMA.ResponseCode;
                                RequestMsg.AuthorizationCode = (string.IsNullOrEmpty(RequestMsg.AuthorizationCode) ? RequestMsg.AuthorizationCode : RequestMsgRMA.AuthorizationCode);
                            }
                        });
                        taskMQ.Wait(Convert.ToInt32(TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut).TotalMilliseconds));
                        if (!taskMQ.IsCompleted)
                        {
                            if (RequestMsg.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage)
                                RequestMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationResponseMessage;
                            // commented for force credit as per rma circular
                            // _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);                           
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.ReferenceNumber = RequestMsg.ReferenceNumber;
                            _MOBILEBANKING_RESP.ResponseData = null;
                            _MOBILEBANKING_RESP.MSGSTAT = "Transaction Successful.";
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("(MQ) Trasaction TimeOut at Host Interface For Reference Number : " + RequestMsg.ReferenceNumber));
                            // return;
                        }
                    }
                    MessageConvertor(ref _MOBILEBANKING_RESP, ref RequestMsg);
                    _ProcessMessage.TransactionOutwardFundTransfer((int)enumCommandTypeEnum.AuthorizationResponseMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 5);
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("no rma connection"));
                    return;
                }

                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Outward Fund Transfer Response Recieved From HostInterface For Reference Number : " + _MOBILEBANKING_RESP.ReferenceNumber + Environment.NewLine));
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To HostInterface For Reference Number : " + _MOBILEBANKING_RESP.ReferenceNumber + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                _ProcessMessage.TransactionOutwardFundTransfer((int)enumCommandTypeEnum.AuthorizationResponseMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
            }
        }

        public void ProcessRechargeTransactionToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsgRMA;
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Recharge Debit Transaction Request Sent To HostInterface For Reference Number : " + _MOBILEBANKING_REQ.ReferenceNumber + Environment.NewLine));
                }
                try
                {
                    MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg = new MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg();
                    _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                    RequestMsg.TransactionType = MaxiSwitch.API.Terminal.enumTransactionType.Debit;
                    MessageConvertor(ref _MOBILEBANKING_REQ, ref RequestMsg);
                    var task = Task.Factory.StartNew(() =>
                    {
                        HostCommChanel.SendTransRequest.Invoke(ref RequestMsg);
                    });
                    task.Wait(Convert.ToInt32(TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut).TotalMilliseconds));
                    if (!task.IsCompleted)
                    {
                        if (RequestMsg.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage)
                            RequestMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationResponseMessage;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.ReferenceNumber = RequestMsg.ReferenceNumber;
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Trasaction TimeOut at Host Interface For Reference Number : " + RequestMsg.ReferenceNumber));
                        _ProcessMessage.TransactionOutwardFundTransfer((int)enumCommandTypeEnum.AuthorizationResponseMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                        return;
                    }
                    MessageConvertor(ref _MOBILEBANKING_RESP, ref RequestMsg);
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _ProcessMessage.TransactionOutwardFundTransfer((int)enumCommandTypeEnum.AuthorizationResponseMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Recharge Debit Transaction Response Recieved From HostInterface For Reference Number : " + _MOBILEBANKING_RESP.ReferenceNumber + Environment.NewLine));
                }
                _ProcessMessage.TransactionOutwardFundTransfer((int)enumCommandTypeEnum.AuthorizationResponseMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To HostInterface For Reference Number : " + _MOBILEBANKING_RESP.ReferenceNumber + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                _ProcessMessage.TransactionOutwardFundTransfer((int)enumCommandTypeEnum.AuthorizationResponseMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
            }
        }

        public void ProcessNQRCOutwardTransactionToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsgRMA;
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** NQRC Outward Fund Transfer Transaction Request Sent To HostInterface For Reference Number : " + _MOBILEBANKING_REQ.ReferenceNumber + Environment.NewLine));
                }
                try
                {
                    _ProcessMessage.TransactionOutwardFundTransfer((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 2);
                    MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg = new MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg();
                    _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                    RequestMsg.TransactionType = MaxiSwitch.API.Terminal.enumTransactionType.Debit;
                    MessageConvertor(ref _MOBILEBANKING_REQ, ref RequestMsg);
                    var task = Task.Factory.StartNew(() =>
                    {
                        HostCommChanel.SendTransRequest.Invoke(ref RequestMsg);
                    });
                    task.Wait(Convert.ToInt32(TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut).TotalMilliseconds));
                    if (!task.IsCompleted)
                    {
                        if (RequestMsg.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage)
                            RequestMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationResponseMessage;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.ReferenceNumber = RequestMsg.ReferenceNumber;
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("(HI - NQRC) Trasaction TimeOut at Host Interface For Reference Number : " + RequestMsg.ReferenceNumber));
                        return;
                    }
                    _ProcessMessage.TransactionOutwardFundTransfer((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 3);
                    _CommanDetails.SystemLogger.WriteTransLog(this, "(Host) Response Code : " + Convert.ToInt32(RequestMsg.ResponseCode) + " Trans. Status : " + Convert.ToString(RequestMsg.TransStatus) + " CommadType : " + Convert.ToString(RequestMsg.CommandType));
                    if (Convert.ToInt32(RequestMsg.ResponseCode) == (int)MaxiSwitch.API.Terminal.enumResponseCode.Approved && RequestMsg.TransStatus == MaxiSwitch.API.Terminal.enumTransactionStatus.Successful &&
                                RequestMsg.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationResponseMessage)
                    {
                        _ProcessMessage.TransactionOutwardFundTransfer((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 4);
                        var taskMQ = Task.Factory.StartNew(() =>
                            {

                                RequestMsgRMA = MsgConvertorAcquirerNQRC(ref _MOBILEBANKING_REQ);
                                MessageQCommunicationChanel.SendTransRequest.Invoke(ref RequestMsgRMA);
                                _CommanDetails.SystemLogger.WriteTransLog(this, "(RMA) Response Code : " + RequestMsgRMA.ResponseCode);
                                if (RequestMsgRMA.ResponseCode != CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved))
                                    RequestMsg.ResponseCode = RequestMsgRMA.ResponseCode;
                                else
                                {
                                    RequestMsg.ResponseCode = RequestMsgRMA.ResponseCode;
                                    RequestMsg.AuthorizationCode = (string.IsNullOrEmpty(RequestMsg.AuthorizationCode) ? RequestMsg.AuthorizationCode : RequestMsgRMA.AuthorizationCode);
                                }
                            });
                        taskMQ.Wait(Convert.ToInt32(TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut).TotalMilliseconds));
                        if (!taskMQ.IsCompleted)
                        {
                            if (RequestMsg.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage)
                                RequestMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationResponseMessage;

                            // commented as per rma circular
                            // _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                            _MOBILEBANKING_RESP.ResponseData = null;
                            _MOBILEBANKING_RESP.MSGSTAT = "Transaction Successful.";
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("(MQ - NQRC) Trasaction TimeOut at Host Interface For Reference Number : " + RequestMsg.ReferenceNumber));
                            //return;
                        }
                    }
                    MessageConvertor(ref _MOBILEBANKING_RESP, ref RequestMsg);
                    _ProcessMessage.TransactionOutwardFundTransfer((int)enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 5);
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    ////_ProcessMessage.ProcessUnsuccessfullTransactionBLQ(ref _MOBILEBANKING_RESP, null, null, null, _MOBILEBANKING_REQ);
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** NQRC Outward Fund Transfer Response Recieved From HostInterface For Reference Number : " + _MOBILEBANKING_RESP.ReferenceNumber + Environment.NewLine));
                }
                //_ProcessMessage.ProcessBalanceinquiry(ref _MOBILEBANKING_RESP, _SwitchConsumerRequestReqMsg, _MOBILEBANKING_REQ);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error Occured In Send Request To HostInterface For Reference Number : " + _MOBILEBANKING_RESP.ReferenceNumber + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                ////_ProcessMessage.ProcessUnsuccessfullTransactionBLQ(ref _MOBILEBANKING_RESP, null, null, null, _MOBILEBANKING_REQ);
            }
        }

        public void ProcessReversalTransactionToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Reversal Transaction Request Sent To HostInterface For Reference Number : " + _MOBILEBANKING_REQ.ReferenceNumber + Environment.NewLine));
                }
                try
                {
                    MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg = new MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg();
                    _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                    RequestMsg.TransactionType = MaxiSwitch.API.Terminal.enumTransactionType.Debit;
                    MessageConvertor(ref _MOBILEBANKING_REQ, ref RequestMsg);
                    RequestMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.ReversalAdviceRequestMessage;
                    var task = Task.Factory.StartNew(() =>
                    {
                        HostCommChanel.SendTransRequest.Invoke(ref RequestMsg);
                    });
                    task.Wait(Convert.ToInt32(TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut).TotalMilliseconds));
                    if (!task.IsCompleted)
                    {
                        if (RequestMsg.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.ReversalAdviceRequestMessage)
                            RequestMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.ReversalAdviceResponseMessage;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.ReferenceNumber = RequestMsg.ReferenceNumber;
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("(Reversal) Trasaction TimeOut at Host Interface For Reference Number : " + RequestMsg.ReferenceNumber));
                        return;
                    }
                    MessageConvertor(ref _MOBILEBANKING_RESP, ref RequestMsg);
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    return;
                }

                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Reversal Response Recieved From HostInterface For Reference Number : " + _MOBILEBANKING_RESP.ReferenceNumber + Environment.NewLine));
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "(Reversal) Error Occured In Send Request To HostInterface For Reference Number : " + _MOBILEBANKING_RESP.ReferenceNumber + Environment.NewLine);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.SystemError);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
            }
        }

        public MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg ProcessInwardTransactionToHost(MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMessage)
        {
            try
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Inward Transaction Request Sent To HostInterface For Reference Number : " + RequestMessage.TransactionRefrenceNumber + Environment.NewLine));
                //_ProcessMessage.TransactioninwardFundTransfer((int)IMPSTransactionRouter.Models.enumCommandTypeEnum.AuthorizationRequestMessage, RequestMessage, 7);
                var task = Task.Factory.StartNew(() =>
                {
                    HostCommChanel.SendTransRequest.Invoke(ref RequestMessage);
                });
                task.Wait(Convert.ToInt32(TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut).TotalMilliseconds));
                if (!task.IsCompleted)
                {
                    if (RequestMessage.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage)
                        RequestMessage.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationResponseMessage;
                    if (RequestMessage.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AccountingAuthorizationRequestMessage)
                        RequestMessage.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AccountingAuthorizationResponseMessage;
                    RequestMessage.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Trasaction TimeOut at Host Interface For Reference Number : " + RequestMessage.TransactionRefrenceNumber));
                    return RequestMessage;
                }
                // _ProcessMessage.TransactioninwardFundTransfer((int)IMPSTransactionRouter.Models.enumCommandTypeEnum.AuthorizationRequestMessage, RequestMessage, 7);
                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Inward Transaction Response Recieved From HostInterface For Reference Number : " + RequestMessage.TransactionRefrenceNumber + Environment.NewLine));

            }
            catch (Exception ex)
            {
                if (RequestMessage.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage)
                    RequestMessage.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationResponseMessage;
                if (RequestMessage.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AccountingAuthorizationRequestMessage)
                    RequestMessage.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AccountingAuthorizationResponseMessage;
                RequestMessage.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                _CommanDetails.SystemLogger.WriteTransLog(null, "Error Occured In ProcessInwardTransactionToHost For Reference Number : " + RequestMessage.TransactionRefrenceNumber);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            return RequestMessage;
        }

        // added by krn on 24-11-2022.
        public MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg ProcessForAccountVerificationTransactionToHost(MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMessage)
        {
            REGISTRATION_RES _REGISTRATION_RES = new REGISTRATION_RES();
            REGISTRATION_REQ _REGISTRATION_REQ = new REGISTRATION_REQ();
            MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg _SwitchConsumerRequestReqMsg = new MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg();
            try
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Account Verification Transaction Request Sent To HostInterface For Reference Number : " + RequestMessage.TransactionRefrenceNumber + Environment.NewLine));
                //_ProcessMessage.TransactioninwardFundTransfer((int)IMPSTransactionRouter.Models.enumCommandTypeEnum.AuthorizationRequestMessage, RequestMessage, 7);
                //RequestMessage.ResponseCode = "-1";
                RequestMessage.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage;
                RequestMessage.TransactionMode = MaxiSwitch.API.Terminal.enumModeOfTransaction.Issuer;
                RequestMessage.TransactionType = MaxiSwitch.API.Terminal.enumTransactionType.AccountVerification;
                RequestMessage.ProcessingCode = "820000";
                RequestMessage.CardType = MaxiSwitch.API.Terminal.enumCardType.Debit;
                // added by sk
                RequestMessage.DeliveryChannel = "BWY";
                RequestMessage.FromAccountNumber = RequestMessage.ToAccountNumber;
                RequestMessage.MiniStateMentData = string.Empty;

                //MessageConvertor(ref RequestMessage, ref _SwitchConsumerRequestReqMsg);
                var task = Task.Factory.StartNew(() => 
                {
                    HostCommChanel.SendTransRequest.Invoke(ref RequestMessage);
                });

                task.Wait(Convert.ToInt32(TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut).TotalMilliseconds));
                if (!task.IsCompleted)
                {
                    if (RequestMessage.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage)
                        RequestMessage.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationResponseMessage;
                    if (RequestMessage.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AccountingAuthorizationRequestMessage)
                        RequestMessage.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AccountingAuthorizationResponseMessage;
                    RequestMessage.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Trasaction TimeOut at Host Interface For Reference Number : " + RequestMessage.TransactionRefrenceNumber));
                    return RequestMessage;
                }
                //MessageConvertor(ref _REGISTRATION_RES, ref _SwitchConsumerRequestReqMsg);
                AccountValidationMessageConvertor(ref RequestMessage);
               
                using (var stringWriter = new StringWriter())
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Account Verification Transaction Response Recieved From HostInterface For Reference Number : " + RequestMessage.TransactionRefrenceNumber + Environment.NewLine));
                }
                // _ProcessMessage.TransactioninwardFundTransfer((int)IMPSTransactionRouter.Models.enumCommandTypeEnum.AuthorizationRequestMessage, RequestMessage, 7);


            }
            catch (Exception ex)
            {
                if (RequestMessage.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage)
                    RequestMessage.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationResponseMessage;
                if (RequestMessage.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AccountingAuthorizationRequestMessage)
                    RequestMessage.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AccountingAuthorizationResponseMessage;
                RequestMessage.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                _CommanDetails.SystemLogger.WriteTransLog(null, "Error Occured In ProcessAccountVerificationTransactionToHost For Reference Number : " + RequestMessage.TransactionRefrenceNumber);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            return RequestMessage;
        }

        public MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg ProcessForQRVerificationTransactionToHost(MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMessage)
        {
            REGISTRATION_RES _REGISTRATION_RES = new REGISTRATION_RES();
            REGISTRATION_REQ _REGISTRATION_REQ = new REGISTRATION_REQ();
            MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg _SwitchConsumerRequestReqMsg = new MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg();
            try
            {
                //_CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** QR Verification Transaction Request Sent To HostInterface For Reference Number : " + RequestMessage.TransactionRefrenceNumber + Environment.NewLine));
                //_ProcessMessage.TransactioninwardFundTransfer((int)IMPSTransactionRouter.Models.enumCommandTypeEnum.AuthorizationRequestMessage, RequestMessage, 7);
                //RequestMessage.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationResponseMessage;
                RequestMessage.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AccountingAuthorizationResponseMessage;
                RequestMessage.TransactionMode = MaxiSwitch.API.Terminal.enumModeOfTransaction.Issuer;
                RequestMessage.TransactionType = MaxiSwitch.API.Terminal.enumTransactionType.AccountVerification;
                RequestMessage.CardType = MaxiSwitch.API.Terminal.enumCardType.Debit;
                //DB chck
                DataTable DTNQRCAccountNumber = new DataTable();
                DTNQRCAccountNumber = IMPSTransactions.GetNQRCPrimaryAccount(RequestMessage.ToAccountNumber);
                if (DTNQRCAccountNumber.Rows.Count > 0)
                {
                    RequestMessage.ResponseCode = "00";
                    RequestMessage.ToAccountNumber = DTNQRCAccountNumber.Rows[0][0].ToString();
                }
                else
                {
                    RequestMessage.ResponseCode = "14";
                }
              
            }
            catch (Exception ex)
            {
                if (RequestMessage.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage)
                    RequestMessage.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationResponseMessage;
                if (RequestMessage.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AccountingAuthorizationRequestMessage)
                    RequestMessage.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AccountingAuthorizationResponseMessage;
                RequestMessage.ResponseCode = "14"; //CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                _CommanDetails.SystemLogger.WriteTransLog(null, "Error Occured In ProcessQRVerificationTransactionToHost For Reference Number : " + RequestMessage.TransactionRefrenceNumber);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            return RequestMessage;
        }

         
        public void ProcessAccountStatementToHost(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** AccountStatement Transaction Request Sent To HostInterface For Reference Number : " + _MOBILEBANKING_REQ.ReferenceNumber + Environment.NewLine));
                string Transactiondata;
                System.Text.StringBuilder finaldata = new System.Text.StringBuilder();
                System.Text.StringBuilder finaldata1 = new System.Text.StringBuilder();
                int count = 0;
                do
                {
                    MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg = new MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg();
                    _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                    RequestMsg.TransactionType = MaxiSwitch.API.Terminal.enumTransactionType.StatementRequest;
                    MessageConvertor(ref _MOBILEBANKING_REQ, ref RequestMsg);
                    var task = Task.Factory.StartNew(() =>
                    {
                        HostCommChanel.SendTransRequest.Invoke(ref RequestMsg);
                    });
                    task.Wait(Convert.ToInt32(TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut).TotalMilliseconds));
                    if (!task.IsCompleted)
                    {
                        if (RequestMsg.CommandType == MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage)
                            RequestMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationResponseMessage;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _MOBILEBANKING_RESP.ReferenceNumber = RequestMsg.ReferenceNumber;
                        _MOBILEBANKING_RESP.ResponseData = null;
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("Trasaction TimeOut at Host Interface for Reference Number : " + RequestMsg.ReferenceNumber));
                        return;
                    }
                    MessageConvertor(ref _MOBILEBANKING_RESP, ref RequestMsg);
                    _CommanDetails.SystemLogger.WriteTransLog(this, "Response recieved from CBS");
                    try
                    {
                        finaldata = finaldata.Clear();

                        if (_MOBILEBANKING_RESP.MinistatementData.Length >= 147)
                        {
                            _MOBILEBANKING_RESP.MinistatementData = _MOBILEBANKING_RESP.MinistatementData.Substring(3);
                            _CommanDetails.SystemLogger.WriteTransLog(this, "_TRANSACTION_RESP.MinistatementData.Length >= 147");
                            int PrintedDataLength = _MOBILEBANKING_RESP.MinistatementData.Length;
                            for (int index = 0; index < PrintedDataLength; index = index + 147)
                            {
                                _CommanDetails.SystemLogger.WriteTransLog(this, "inside for loop");
                                Transactiondata = _MOBILEBANKING_RESP.MinistatementData.Substring(index, 147);
                                _CommanDetails.SystemLogger.WriteTransLog(this, "Transactiondata : " + Transactiondata);
                                finaldata.Append("|" + Transactiondata);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("splitted response " + finaldata + " count " + count));
                            }
                            _CommanDetails.SystemLogger.WriteTransLog(this, "data splitted correctly");
                            finaldata1.Append(finaldata.ToString());
                            _MOBILEBANKING_RESP.MinistatementData = finaldata1.ToString();
                            count++;
                            string lastline = string.Empty;
                            lastline = Convert.ToString(_MOBILEBANKING_RESP.MinistatementData).Substring(_MOBILEBANKING_RESP.MinistatementData.Length - 147);
                            lastline = lastline.Substring(0, 8) + lastline.Substring(8, 9) + lastline.Substring(17, 4) + lastline.Substring(100, 14) + lastline.Substring(130, 17);
                            _MOBILEBANKING_REQ.TransType = _MOBILEBANKING_REQ.TransType.Substring(0, 86);
                            _MOBILEBANKING_REQ.TransType = _MOBILEBANKING_REQ.TransType + lastline;
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("final splitted response " + _MOBILEBANKING_RESP.MinistatementData.Length.ToString() + " count " + count));
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("last line : " + _MOBILEBANKING_REQ.TransType));
                        }
                        //_TRANSACTION_RESP.MinistatementData = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        _CommanDetails.SystemLogger.WriteErrorLog("error in reading statement data", ex);
                    }
                }
                while (_MOBILEBANKING_RESP.Indicator.Contains("Y"));
            }
            catch (Exception ex)
            {
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseData = null;
                //_ProcessMessage.TransactionNonFinance((int)ProcessMessage.enumRequestTypeEnum.Ministatement, _TRANSACTION_REQ, _TRANSACTION_RESP, 99);
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error in ProcessAccountStatementToHost");
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                return;
            }
        }

        public void MessageConvertor(ref REGISTRATION_REQ _REGISTRATION_REQ, ref MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg)
        {
            try
            {
                RequestMsg.CardNumber = Convert.ToString(CONFIGURATIONCONFIGDATA.BankBIN + _REGISTRATION_REQ.AccountNumber).PadRight(16, '0');
                RequestMsg.AcquirerInstCode = CONFIGURATIONCONFIGDATA.BankBIN;
                RequestMsg.TransactionAmount = string.IsNullOrEmpty(_REGISTRATION_REQ.Amount) ? "0" : _REGISTRATION_REQ.Amount;
                RequestMsg.LocalTransactionDateTime = DateTime.Now;
                RequestMsg.CardScheme = MaxiSwitch.API.Terminal.enumCardScheme.BFS;
                RequestMsg.CardType = MaxiSwitch.API.Terminal.enumCardType.Debit;
                RequestMsg.SystemsTraceAuditNumber = _REGISTRATION_REQ.ReferenceNumber;
                RequestMsg.ReferenceNumber = _REGISTRATION_REQ.ReferenceNumber;
                RequestMsg.TransactionRefrenceNumber = _REGISTRATION_REQ.ReferenceNumber;
                //RequestMsg.TerminalID = _REGISTRATION_REQ.DeviceID.Substring(0, 16);
                RequestMsg.TerminalID = (_REGISTRATION_REQ.DeviceID.Length >= 16 ? _REGISTRATION_REQ.DeviceID.Substring(0, 16) : _REGISTRATION_REQ.DeviceID.PadRight(16, ' '));
                RequestMsg.TerminalLocation = "";
                RequestMsg.FromAccountNumber = _REGISTRATION_REQ.AccountNumber;
                RequestMsg.ToAccountNumber = _REGISTRATION_REQ.BenificiaryAccountNumber;
                RequestMsg.DeliveryChannel = "BWY";
                RequestMsg.CardAccepterName = "THIMPHU                THIMPHU      THBT";
                RequestMsg.MiniStateMentData = RequestMsg.ProcessingCode.Substring(0, 2) == "91" ? "HIF" : "";
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
        }

        public void MessageConvertor(ref REGISTRATION_RES _REGISTRATION_RESP, ref MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg)
        {
            try
            {
                _REGISTRATION_RESP.ResponseCode = "";
                _REGISTRATION_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(RequestMsg.ResponseCode);
                _REGISTRATION_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescriptionHost(_REGISTRATION_RESP.ResponseCode);
                ////_REGISTRATION_RESP.DeviceID = RequestMsg.TerminalID;
                _REGISTRATION_RESP.ReferenceNumber = RequestMsg.ReferenceNumber;
                _CommanDetails.SystemLogger.WriteTransLog(null, string.Format("From Account Number: " + RequestMsg.FromAccountNumber));
                _REGISTRATION_RESP.AccountNumber = RequestMsg.FromAccountNumber;
                _CommanDetails.SystemLogger.WriteTransLog(null, string.Format("From Account Number: " + _REGISTRATION_RESP.AccountNumber));
                if (!string.IsNullOrEmpty(RequestMsg.MiniStateMentData) && RequestMsg.TransactionType == MaxiSwitch.API.Terminal.enumTransactionType.AccountVerification
                    && RequestMsg.ProcessingCode.Substring(0, 2) == "82" && _REGISTRATION_RESP.ResponseCode =="00")
                {
                    _CommanDetails.SystemLogger.WriteTransLog(null, string.Format("Before Spliting"));
                    string[] ExtendedData = RequestMsg.MiniStateMentData.Split('|');
                    string[] ReserveFeild3 = RequestMsg.MobileNumber.Split('|');
                    //_REGISTRATION_RESP.CustomerID = RequestMsg.MiniStateMentData.Substring(0, 9);
                    _REGISTRATION_RESP.CustomerID = ReserveFeild3[2];
                    //_REGISTRATION_RESP.CustomerName = RequestMsg.MiniStateMentData.Substring(89, 80).Trim();//commented on 01082021 for testing
                    _REGISTRATION_RESP.CustomerName = RequestMsg.MiniStateMentData.Substring(9, 80).Trim();
                    if(string.IsNullOrEmpty( _REGISTRATION_RESP.CustomerName))
                    {
                        _REGISTRATION_RESP.CustomerName = RequestMsg.MiniStateMentData.Substring(89, 80).Trim();
                    }
                    _REGISTRATION_RESP.AccountType = (RequestMsg.MiniStateMentData.Substring(182, 3).Contains('S') ? "SAVING" : RequestMsg.MiniStateMentData.Substring(182, 3).Contains('C') ? "CURRENT" : RequestMsg.MiniStateMentData.Substring(182, 3).Contains('O') ? "OVERDRAFT" : RequestMsg.MiniStateMentData.Substring(182, 3).Contains('L') ? "LOAN" : RequestMsg.MiniStateMentData.Substring(182, 3).Contains('T') ? "TERMDEPOSIT" : "UNKNOWN");
                    _REGISTRATION_RESP.AccountStatus = (RequestMsg.MiniStateMentData.Substring(185, 1).Contains('A') ? "Active" : RequestMsg.MiniStateMentData.Substring(185, 1).Contains('I') ? "Inactive" : RequestMsg.MiniStateMentData.Substring(185, 1).Contains('O') ? "Open" :
                                                         RequestMsg.MiniStateMentData.Substring(185, 1).Contains('F') ? "Frozen" : RequestMsg.MiniStateMentData.Substring(185, 1).Contains('C') ? "Closed" : RequestMsg.MiniStateMentData.Substring(185, 1).Contains('R') ? "Credit Frozen" :
                                                          RequestMsg.MiniStateMentData.Substring(185, 1).Contains('D') ? "Debit Frozen" : RequestMsg.MiniStateMentData.Substring(185, 1).Contains('M') ? "Dormant" : "InvalidAccount");//RequestMsg.MiniStateMentData.Substring(186, 1).Contains('I') ? "InActive");
                    _REGISTRATION_RESP.BranchCode = RequestMsg.FromAccountNumber.Substring(0, 3);
                    
                    _CommanDetails.SystemLogger.WriteTransLog(null, string.Format("mobile number recieved from cbs : " + RequestMsg.MobileNumber));
                    if (ExtendedData.Length == 1)
                    {
                        _REGISTRATION_RESP.MobileNumber = ReserveFeild3[1]; //RequestMsg.MobileNumber.Substring(7,8);
                        _REGISTRATION_RESP.EmailID = "demo@drukpnb.in";
                        ////*********Modified On 30032021***********
                        ////_REGISTRATION_RESP.BranchCode = RequestMsg.FromAccountNumber.Substring(0,3);
                    }
                    else
                    {
                        _REGISTRATION_RESP.MobileNumber = (ExtendedData.Length > 0 ? ExtendedData[1].Substring(3) : string.Empty);
                        ////*********Modified On 30032021***********
                        ////_REGISTRATION_RESP.EmailID = (ExtendedData.Length > 1 ? ExtendedData[2] : string.Empty);
                        _REGISTRATION_RESP.EmailID = "demo@drukpnb.in";
                        _REGISTRATION_RESP.CustomerID = (ExtendedData.Length > 1 && string.IsNullOrEmpty(_REGISTRATION_RESP.CustomerID) ? Convert.ToString(ExtendedData[2]).Trim() : _REGISTRATION_RESP.CustomerID);
                    }
                    _CommanDetails.SystemLogger.WriteTransLog(null,
                    string.Format("CustomerID : {0} \t CustomerName : {1} \t AccountType : {2} \t MobileNo.: {3} \t EmailID : {4}"
                                 , _REGISTRATION_RESP.CustomerID, _REGISTRATION_RESP.CustomerName, _REGISTRATION_RESP.AccountType, _REGISTRATION_RESP.MobileNumber, _REGISTRATION_RESP.EmailID));
                }
                else if (!string.IsNullOrEmpty(RequestMsg.MiniStateMentData) && RequestMsg.TransactionType == MaxiSwitch.API.Terminal.enumTransactionType.AccountVerification
                    && RequestMsg.ProcessingCode.Substring(0, 2) == "91" && _REGISTRATION_RESP.ResponseCode == "00")
                {
                    _REGISTRATION_RESP.DateOfBirth = RequestMsg.MiniStateMentData.Length >= 268 ? RequestMsg.MiniStateMentData.Substring(260, 8) : "";
                    _REGISTRATION_RESP.DateOfBirth = _REGISTRATION_RESP.DateOfBirth.Substring(6, 2) + _REGISTRATION_RESP.DateOfBirth.Substring(4, 2) + _REGISTRATION_RESP.DateOfBirth.Substring(0, 4);
                    _REGISTRATION_RESP.CustomerGovermentID = RequestMsg.MiniStateMentData.Length >= 392 ? RequestMsg.MiniStateMentData.Substring(362, 30) : "";
                    _CommanDetails.SystemLogger.WriteTransLog(null,
                    string.Format("CustomerID : {0} \t CustomerName : {1} \t AccountType : {2} \t MobileNo.: {3} \t EmailID : {4} \t DateOfBirth : {5} \t CustomerGovermentID : {6}"
                                 , _REGISTRATION_RESP.CustomerID, _REGISTRATION_RESP.CustomerName, _REGISTRATION_RESP.AccountType, _REGISTRATION_RESP.MobileNumber, _REGISTRATION_RESP.EmailID, _REGISTRATION_RESP.DateOfBirth, _REGISTRATION_RESP.CustomerGovermentID));

                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
        }

        public void MessageConvertor(ref MOBILEBANKING_REQ _MOBILEBANKING_REQ, ref MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg)
        {
            try
            {
                RequestMsg.TransactionMode = MaxiSwitch.API.Terminal.enumModeOfTransaction.OnUs;
                RequestMsg.CardScheme = MaxiSwitch.API.Terminal.enumCardScheme.BFS;
                RequestMsg.CardType = MaxiSwitch.API.Terminal.enumCardType.Debit;
                RequestMsg.TransSource = MaxiSwitch.API.Terminal.enumTransactionSource.Terminal;
                RequestMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AuthorizationRequestMessage;
                RequestMsg.ProcessingCode = CommanDetails.GetProcessingCode(RequestMsg.AccountType, (int)RequestMsg.TransactionType, (int)RequestMsg.TransactionMode, (int)RequestMsg.CardScheme);
                RequestMsg.TransactionAmount = string.IsNullOrEmpty(Convert.ToString(_MOBILEBANKING_REQ.TXNAMT)) ? "0" : Convert.ToString(_MOBILEBANKING_REQ.TXNAMT);
                RequestMsg.CardNumber = _MOBILEBANKING_REQ.Merchantidentifier;
                RequestMsg.AcquirerInstCode = CONFIGURATIONCONFIGDATA.BankBIN;
                RequestMsg.LocalTransactionDateTime = _MOBILEBANKING_REQ.LastDateTime;
                RequestMsg.SystemsTraceAuditNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                RequestMsg.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                RequestMsg.TransactionRefrenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                //RequestMsg.TerminalID = _MOBILEBANKING_REQ.DeviceID.Substring(0, 16);
                RequestMsg.TerminalID = (_MOBILEBANKING_REQ.DeviceID.Length >= 16 ? _MOBILEBANKING_REQ.DeviceID.Substring(0, 16) : _MOBILEBANKING_REQ.DeviceID.PadRight(16, ' '));
                RequestMsg.TerminalLocation = "";
                RequestMsg.FromAccountNumber = _MOBILEBANKING_REQ.REMITTERACC;
                RequestMsg.ToAccountNumber = _MOBILEBANKING_REQ.BENIFICIARYACC;
                RequestMsg.DeliveryChannel = _MOBILEBANKING_REQ.DeliveryChannel; 
                RequestMsg.CardAccepterName = "THIMPHU                THIMPHU      THBT";
                RequestMsg.MiniStateMentData = _MOBILEBANKING_REQ.FundTransferType;//added by sk
                _CommanDetails.SystemLogger.WriteTransLog(null,
                string.Format("Transacttion amount : " + RequestMsg.TransactionAmount));
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
        }

        public void MessageConvertor(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, ref MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg)
        {
            try
            {

                _MOBILEBANKING_RESP.ResponseCode = "";
                ////_REGISTRATION_RESP.DeviceID = RequestMsg.TerminalID;
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(RequestMsg.ResponseCode);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.MSGSTAT = _MOBILEBANKING_RESP.ResponseDesc;
                _MOBILEBANKING_RESP.ReferenceNumber = RequestMsg.ReferenceNumber;
                _MOBILEBANKING_RESP.REMITTERACC = RequestMsg.FromAccountNumber;
                _MOBILEBANKING_RESP.AvailableBalance = RequestMsg.BalanceAmount;
                _MOBILEBANKING_RESP.MinistatementData = RequestMsg.MiniStateMentData;
                _CommanDetails.SystemLogger.WriteTransLog(null, "amount in case of decimal : " + RequestMsg.TransactionAmount);
                // _MOBILEBANKING_RESP.TXNAMT = Convert.ToInt32(RequestMsg.TransactionAmount);
                _MOBILEBANKING_RESP.TXNAMT = Convert.ToDecimal(RequestMsg.TransactionAmount);
                _MOBILEBANKING_RESP.LastTime = Convert.ToDateTime(RequestMsg.LocalTransactionDateTime).ToString();
                _CommanDetails.SystemLogger.WriteTransLog(null,
                    string.Format("RequestMsg.Transacttion amount : " + RequestMsg.TransactionAmount + " _MOBILEBANKING_RESP.TXNAMT :  " + _MOBILEBANKING_RESP.TXNAMT));
                if (!string.IsNullOrEmpty(RequestMsg.MiniStateMentData) && RequestMsg.TransactionType == MaxiSwitch.API.Terminal.enumTransactionType.MiniStatement)
                {
                    int NoOfRecords = RequestMsg.MiniStateMentData.Length / Convert.ToInt32(ConfigurationManager.AppSettings["MinistatementRecordLength"]);
                    _CommanDetails.SystemLogger.WriteTransLog(null, "No. Of Records : " + NoOfRecords);
                    string MinistatementProcessed = string.Empty;
                    int startIndex = 0, endIndex = Convert.ToInt32(ConfigurationManager.AppSettings["MinistatementRecordLength"]);
                    for (int j = 1; j <= NoOfRecords; j++)
                    {
                        string str = RequestMsg.MiniStateMentData.Substring(startIndex, endIndex);
                        if (string.IsNullOrEmpty(MinistatementProcessed))
                            MinistatementProcessed = str.Substring(0, 8) + " " + str.Substring(29, 40) + " " + str.Substring(69, 1) + " " + str.Substring(70, 17).Trim();
                        else
                            MinistatementProcessed = MinistatementProcessed + "|" + str.Substring(0, 8) + " " + str.Substring(29, 40) + " " + str.Substring(69, 1) + " " + str.Substring(70, 17).Trim();
                        startIndex += endIndex;
                    }
                    _MOBILEBANKING_RESP.MinistatementData = MinistatementProcessed;
                }
                _CommanDetails.SystemLogger.WriteTransLog(null, "customername : " + RequestMsg.AdditionalPrivateData);
                if (!string.IsNullOrEmpty(RequestMsg.ProcessingCode) && RequestMsg.ProcessingCode == "350000" && (!string.IsNullOrEmpty(RequestMsg.AdditionalPrivateData)))//added by sk
                {
                    //_MOBILEBANKING_RESP.BENFNAME = string.IsNullOrEmpty(RequestMsg.AdditionalPrivateData) ? "NA" : RequestMsg.AdditionalPrivateData.ToString();
                    _MOBILEBANKING_RESP.BENFNAME = RequestMsg.AdditionalPrivateData;
                   // _MOBILEBANKING_RESP.BENFNAME = _MOBILEBANKING_RESP.BENFNAME.Substring(0, _MOBILEBANKING_RESP.BENFNAME.Length-17);
                   // _MOBILEBANKING_RESP.BENFNAME = _MOBILEBANKING_RESP.BENFNAME.Substring(6);
                    string Message = _MOBILEBANKING_RESP.BENFNAME;
                    int getValus = 3;
                    int indexof = 0;
                    string[] Details = new string[50];
                    for (int index = 0; index < Message.Length;)
                    {
                        string length = Message.Substring(index, getValus);
                        Details[indexof] += length;
                        indexof++;
                        string length1 = Message.Substring(index + length.Length, getValus);
                        Details[indexof] += length1;
                        indexof++;
                        string length3 = Message.Substring(index + length.Length + length1.Length, Convert.ToInt16(length1));
                        Details[indexof] += length3;
                        indexof++;
                        index = index + (length.Length + length1.Length + length3.Length);
                    }
                    for (int i = 0; i < Details.Length; i = i + 3)
                    {
                        string s = Details[i];
                        switch (s)
                        {
                            case "001":
                                _MOBILEBANKING_RESP.BENFNAME = Details[i + 2];
                                break;
                            case "002":
                                _MOBILEBANKING_RESP.AccountType = Details[i + 2];
                                break;
                            case "003":
                                _MOBILEBANKING_RESP.AccountStatus = Details[i + 2];
                                break;
                            default:
                                //Console.WriteLine("Other");
                                break;
                        }
                    }
                    if ((_MOBILEBANKING_RESP.AccountType == "SA" || _MOBILEBANKING_RESP.AccountType == "CD" || _MOBILEBANKING_RESP.AccountType == "OD") && _MOBILEBANKING_RESP.AccountStatus == "ACT")
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(null, "Response for success account : " + RequestMsg.ResponseCode);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = "57";
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        _CommanDetails.SystemLogger.WriteTransLog(null, "Response for not permitable account : " + RequestMsg.ResponseCode);
                    }
                }

                if (!string.IsNullOrEmpty(RequestMsg.MiniStateMentData) && RequestMsg.TransactionType == MaxiSwitch.API.Terminal.enumTransactionType.StatementRequest
                    && RequestMsg.ProcessingCode.Substring(0, 2) == "93")
                {
                    _MOBILEBANKING_RESP.Indicator = RequestMsg.MiniStateMentData.Substring(0, 1).ToString();
                    _MOBILEBANKING_RESP.MinistatementData = RequestMsg.MiniStateMentData.ToString();
                    _CommanDetails.SystemLogger.WriteTransLog(null, string.Format("Indicator : " + _MOBILEBANKING_RESP.Indicator + "Data : " + _MOBILEBANKING_RESP.MinistatementData));
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.IssuerDown);
                _MOBILEBANKING_RESP.ResponseDesc = _MOBILEBANKING_RESP.ResponseCode;
            }
        }
        //added by sk
        public void ReversalMessageConvertor(ref MOBILEBANKING_REQ _MOBILEBANKING_REQ, ref MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg)
        {
            try
            {
                RequestMsg.TransactionMode = MaxiSwitch.API.Terminal.enumModeOfTransaction.OnUs;
                RequestMsg.CardScheme = MaxiSwitch.API.Terminal.enumCardScheme.BFS;
                RequestMsg.CardType = MaxiSwitch.API.Terminal.enumCardType.Debit;
                RequestMsg.TransSource = MaxiSwitch.API.Terminal.enumTransactionSource.Terminal;
                RequestMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.ReversalAdviceRequestMessage;
                RequestMsg.ProcessingCode = "400000";//CommanDetails.GetProcessingCode(RequestMsg.AccountType, (int)RequestMsg.TransactionType, (int)RequestMsg.TransactionMode, (int)RequestMsg.CardScheme);
                RequestMsg.TransactionAmount = string.IsNullOrEmpty(Convert.ToString(_MOBILEBANKING_REQ.TXNAMT)) ? "0" : Convert.ToString(_MOBILEBANKING_REQ.TXNAMT);
                RequestMsg.CardNumber = _MOBILEBANKING_REQ.Merchantidentifier;
                RequestMsg.AcquirerInstCode = CONFIGURATIONCONFIGDATA.BankBIN;
                RequestMsg.LocalTransactionDateTime = _MOBILEBANKING_REQ.LastDateTime;
                RequestMsg.SystemsTraceAuditNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                RequestMsg.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                RequestMsg.TransactionRefrenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                //RequestMsg.TerminalID = _MOBILEBANKING_REQ.DeviceID.Substring(0, 16);
                RequestMsg.TerminalID = (_MOBILEBANKING_REQ.DeviceID.Length >= 16 ? _MOBILEBANKING_REQ.DeviceID.Substring(0, 16) : _MOBILEBANKING_REQ.DeviceID.PadRight(16, ' '));
                RequestMsg.TerminalLocation = "";
                RequestMsg.FromAccountNumber = _MOBILEBANKING_REQ.BENIFICIARYACC;
                RequestMsg.ToAccountNumber = _MOBILEBANKING_REQ.REMITTERACC;
                RequestMsg.DeliveryChannel = "BWY";
                RequestMsg.CardAccepterName = "THIMPHU                THIMPHU      THBT";
                RequestMsg.MiniStateMentData = _MOBILEBANKING_REQ.FundTransferType;//added by sk
                RequestMsg.AdditionalPrivateData = RequestMsg.MiniStateMentData;
                RequestMsg.MiniStateMentData = "43";
                RequestMsg.OriginalDataElement = "1200"
                                               + RequestMsg.SystemsTraceAuditNumber.ToString().PadLeft(6, '0')
                                               + RequestMsg.LocalTransactionDateTime.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture).PadRight(10, '0')
                                               + RequestMsg.AcquirerInstCode.Length.ToString()
                                               + RequestMsg.AcquirerInstCode.ToString();
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
        }

        public MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg MessageConvertor(ref MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg;
            try
            {
                RequestMsg = new MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg();
                RequestMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AccountingAuthorizationRequestMessage;
                RequestMsg.TransSource = MaxiSwitch.API.Terminal.enumTransactionSource.Terminal;
                RequestMsg.TransactionMode = MaxiSwitch.API.Terminal.enumModeOfTransaction.Acquirer;
                RequestMsg.TransactionType = MaxiSwitch.API.Terminal.enumTransactionType.Purchase;
                RequestMsg.CardScheme = MaxiSwitch.API.Terminal.enumCardScheme.BFS;
                RequestMsg.CardType = MaxiSwitch.API.Terminal.enumCardType.Debit;
                RequestMsg.CardNumber = _MOBILEBANKING_REQ.Merchantidentifier;
                //changed on 21-11-22 by krn for account verification
                if (_MOBILEBANKING_REQ.IsAccountVerification)
                {
                    //RequestMsg.ProcessingCode = Convert.ToString(350000);
                    RequestMsg.ProcessingCode = ConfigurationManager.AppSettings["IsAccountVerification"].ToString();
                    RequestMsg.MiniStateMentData = _MOBILEBANKING_REQ.ResponseData;
                   
                    //RequestMsg.MiniStateMentData = Convert.ToString(_MOBILEBANKING_REQ.ResponseData.Length) + _MOBILEBANKING_REQ.ResponseData;
                }
                else if (_MOBILEBANKING_REQ.IsQRCodeVerification)
                {
                    //RequestMsg.ProcessingCode = Convert.ToString(360000);
                    RequestMsg.ProcessingCode = ConfigurationManager.AppSettings["IsQRCodeVerification"].ToString();
                    RequestMsg.MiniStateMentData = _MOBILEBANKING_REQ.ResponseData;
                    //RequestMsg.MiniStateMentData = Convert.ToString(_MOBILEBANKING_REQ.ResponseData.Length) + _MOBILEBANKING_REQ.ResponseData;
                }
                else
                {
                    RequestMsg.ProcessingCode = (_MOBILEBANKING_REQ.Merchantidentifier.Substring(0, 6) == Convert.ToString(ConfigurationManager.AppSettings["BankQRBIN"]) ? "280000" : "260000");
                    //RequestMsg.MiniStateMentData = _MOBILEBANKING_REQ.ResponseData;
                    //changes made  for old rrn (30-11-22)
                    RequestMsg.MiniStateMentData = _MOBILEBANKING_REQ.ResponseData.Replace(_MOBILEBANKING_REQ.ReferenceNumber,_MOBILEBANKING_REQ.LastTransactionReferenceNumber);

                }
                RequestMsg.TransactionAmount = string.IsNullOrEmpty(Convert.ToString(_MOBILEBANKING_REQ.TXNAMT)) ? "0" : Convert.ToString(_MOBILEBANKING_REQ.TXNAMT);
                RequestMsg.SystemsTraceAuditNumber = _MOBILEBANKING_REQ.ReferenceNumber.Substring(_MOBILEBANKING_REQ.ReferenceNumber.Length - 5, 5);
               // _MOBILEBANKING_REQ.ReferenceNumber.Substring(6);
                RequestMsg.LocalTransactionDateTime = _MOBILEBANKING_REQ.LastDateTime;
                RequestMsg.GmtTransactionDateTime = RequestMsg.LocalTransactionDateTime.AddHours(-6);
                RequestMsg.MerchantCategoryCode = CONFIGURATIONCONFIGDATA.MerchantCategoryCode;
                RequestMsg.TransactionCurrencyCode = CONFIGURATIONCONFIGDATA.TransactionCurrencyCode;
                RequestMsg.PanEntryMode = (_MOBILEBANKING_REQ.Merchantidentifier.Substring(0, 6) == Convert.ToString(ConfigurationManager.AppSettings["BankQRBIN"]) ? "081" : "900");
                RequestMsg.PosConditionCode = (_MOBILEBANKING_REQ.Merchantidentifier.Substring(0, 6) == Convert.ToString(ConfigurationManager.AppSettings["BankQRBIN"]) ? "01" : "02");
                RequestMsg.AcquirerInstCode = CONFIGURATIONCONFIGDATA.BankBIN;
                RequestMsg.TransactionRefrenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                RequestMsg.ResponseCode = string.Empty;
                RequestMsg.TransStatus = MaxiSwitch.API.Terminal.enumTransactionStatus.Unknown;
                RequestMsg.TerminalID = (_MOBILEBANKING_REQ.NQRCBankName == "Bank of Bhutan Ltd.") ? "10018430" : _MOBILEBANKING_REQ.DeviceID.Substring(0, 8);
                RequestMsg.CardAccepterName = CONFIGURATIONCONFIGDATA.CardAccepterName;
                RequestMsg.FromAccountNumber = _MOBILEBANKING_REQ.REMITTERACC;
                RequestMsg.ToAccountNumber = _MOBILEBANKING_REQ.BENIFICIARYACC;
                //RequestMsg.MiniStateMentData = _MOBILEBANKING_REQ.ResponseData;
                RequestMsg.MobileOperator = _MOBILEBANKING_REQ.FundTransferType;
            }
            catch (Exception ex)
            { _CommanDetails.SystemLogger.WriteErrorLog(null, ex); RequestMsg = null; }
            return RequestMsg;
        }

        public MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg MsgConvertorAcquirerNQRC(ref MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg;
            try
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Request Recived in method  MsgConvertorAcquirer() to set ReqMessage parameters for Reference  Number :" + _MOBILEBANKING_REQ.QRMSGID);
                RequestMsg = new MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg();
                RequestMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AccountingAuthorizationRequestMessage;
                RequestMsg.TransSource = MaxiSwitch.API.Terminal.enumTransactionSource.Terminal;
                RequestMsg.TransactionMode = MaxiSwitch.API.Terminal.enumModeOfTransaction.Acquirer;
                RequestMsg.TransactionType = MaxiSwitch.API.Terminal.enumTransactionType.Purchase;
                RequestMsg.CardScheme = MaxiSwitch.API.Terminal.enumCardScheme.BFS;
                RequestMsg.CardType = MaxiSwitch.API.Terminal.enumCardType.Debit;
                RequestMsg.CardNumber = _MOBILEBANKING_REQ.Merchantidentifier;
                RequestMsg.ProcessingCode = "280000";
                RequestMsg.TransactionAmount = string.IsNullOrEmpty(Convert.ToString(_MOBILEBANKING_REQ.TXNAMT)) ? "0" : Convert.ToString(_MOBILEBANKING_REQ.TXNAMT);
                RequestMsg.LocalTransactionDateTime = DateTime.Now;
                RequestMsg.GmtTransactionDateTime = RequestMsg.LocalTransactionDateTime.AddHours(-6);
                RequestMsg.MerchantCategoryCode = "4812";
                RequestMsg.TransactionCurrencyCode = "064";
                RequestMsg.SystemsTraceAuditNumber = _MOBILEBANKING_REQ.ReferenceNumber.Substring(_MOBILEBANKING_REQ.ReferenceNumber.Length - 5, 5); //DateTime.Now.ToString("HHmmss");
                RequestMsg.PanEntryMode = "081";
                RequestMsg.PosConditionCode = "00";
                RequestMsg.TransactionRefrenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                RequestMsg.TerminalID = (_MOBILEBANKING_REQ.NQRCBankName == "Bank of Bhutan Ltd.") ? "10018430" : _MOBILEBANKING_REQ.DeviceID.Substring(0, 8); //_MOBILEBANKING_REQ.DeviceID.Substring(0, 8);
                                                                                                                                                               //RequestMsg.TerminalID = _MOBILEBANKING_REQ.DeviceID.Substring(0, 8);
                                                                                                                                                               //RequstMsg.TerminalLocation = _MOBILEBANKING_REQ.NQRCcity.PadLeft(15, ' ');
                                                                                                                                                               //ReqMessage.CardAccepterName = _MOBILEBANKING_REQ.DeviceLocation.PadLeft(23, ' ') + _MOBILEBANKING_REQ.NQRCcity.PadLeft(13, ' ')
                                                                                                                                                               //                            + _MOBILEBANKING_REQ.NQRCcity.Substring(0, 2) + "BT";

                // RequstMsg.CardAccepterName = _MOBILEBANKING_REQ.MerchantName.PadLeft(25, ' ') + _MOBILEBANKING_REQ.NQRCcity.PadLeft(13, ' ') + "BT";
                RequestMsg.CardAccepterName = "THIMPHU                THIMPHU      THBT";

                RequestMsg.FromAccountNumber = _MOBILEBANKING_REQ.REMITTERACC;
                RequestMsg.ToAccountNumber = _MOBILEBANKING_REQ.BENIFICIARYACC;
                //   RequestMsg.MiniStateMentData = _MOBILEBANKING_REQ.QRValue;//"000201010211021694004900123546205204581253030645803BTN5924VISA TEST MERCHANT THREE6007THIMPHU6222030600125607080000309063043617";
                RequestMsg.DeliveryChannel = "NQR";
                RequestMsg.AcquirerInstCode = CONFIGURATIONCONFIGDATA.BankBIN;//"639545";

                //  ReqMessage.MiniStateMentData = _MOBILEBANKING_REQ.QRValue;

                string DE120 = string.Empty;
                string Remitter = (_MOBILEBANKING_REQ.REMITTERNAME.Length > 20 ? _MOBILEBANKING_REQ.REMITTERNAME.Substring(0, 20) : _MOBILEBANKING_REQ.REMITTERNAME);
                string Benificiary = (_MOBILEBANKING_REQ.BENIFICIARYNAME.Length > 20 ? _MOBILEBANKING_REQ.BENIFICIARYNAME.Substring(0, 20) : _MOBILEBANKING_REQ.BENIFICIARYNAME);
                string Remark = (_MOBILEBANKING_REQ.Remark.Length > 20 ? _MOBILEBANKING_REQ.Remark.Substring(0, 20) : _MOBILEBANKING_REQ.Remark);
                string Remarks = (_MOBILEBANKING_REQ.Remark.Length > 20 ? _MOBILEBANKING_REQ.Remark.Substring(0, 20) : _MOBILEBANKING_REQ.Remark);
                DE120 = "001003004" +
                       "002002QR" +
                       "003" + (Remitter.Length).ToString().PadLeft(3, '0') + Remitter +
                       "004" + (Benificiary.Length).ToString().PadLeft(3, '0') + Benificiary +
                       "005" + Convert.ToString(Remarks.Length).PadLeft(3, '0') + Remarks +
                       // "006" + (RequestMsg.TransactionRefrenceNumber.Length).ToString().PadLeft(3, '0') + RequestMsg.TransactionRefrenceNumber +
                       //Changes done for Last Transaction RRN 
                       "006" + (_MOBILEBANKING_REQ.LastTransactionReferenceNumber.Length).ToString().PadLeft(3, '0') + _MOBILEBANKING_REQ.LastTransactionReferenceNumber +
                       "007" + (RequestMsg.AcquirerInstCode.Length).ToString().PadLeft(3, '0') + RequestMsg.AcquirerInstCode +
                       "008" + (CONFIGURATIONCONFIGDATA.BankCode.Length).ToString().PadLeft(3, '0') + CONFIGURATIONCONFIGDATA.BankCode +
                       "009008" + _MOBILEBANKING_REQ.Merchantidentifier.Substring(0, 8);

                RequestMsg.MiniStateMentData = DE120;
                // DE120;// DE120.Length.ToString().PadLeft(3,'0')+//_MOBILEBANKING_REQ.QRValue;//"000201010211021694004900123546205204581253030645803BTN5924VISA TEST MERCHANT THREE6007THIMPHU6222030600125607080000309063043617";
                //_CommanDetails.SystemLogger.WriteTransLog(this, "Response Send to process host message convert successfully for Reference  Number :  :" + RequstMsg.TransactionRefrenceNumber);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex); RequestMsg = null;
            }
            return RequestMsg;
        }

        public bool UpdateTransactionDetails(ref MOBILEBANKING_REQ _MOBILEBANKING_REQ)
        {
            try
            {
                _MOBILEBANKING_REQ.LastDateTime = DateTime.Now;
                if (_MOBILEBANKING_REQ.TransType == enumTransactionType.FT.ToString() || _MOBILEBANKING_REQ.TransType == enumTransactionType.BNgul.ToString())
                {
                    string ActualAmount = "";
                    string ACQActualAmount = "";
                    string BNgulActualAmount = "";
                    _CommanDetails.SystemLogger.WriteTransLog(this, "Available Amount : " + _MOBILEBANKING_REQ.AmountAvailable + "  TXN Amount : " + _MOBILEBANKING_REQ.TXNAMT);
                    if (_MOBILEBANKING_REQ.FundTransferType == FundTransferType.Ouward.ToString())
                    {
                        ACQActualAmount = Convert.ToString(Convert.ToInt32(_MOBILEBANKING_REQ.ACQAmountAvailable) - Convert.ToInt32(_MOBILEBANKING_REQ.TXNAMT));
                        ACQActualAmount = ACQActualAmount.PadLeft(9, '0');
                        ActualAmount = _MOBILEBANKING_REQ.AmountAvailable;
                        ActualAmount = ActualAmount.PadLeft(9, '0');
                        BNgulActualAmount = _MOBILEBANKING_REQ.BNgulAmountAvailable;
                        BNgulActualAmount = BNgulActualAmount.PadLeft(9, '0');
                    }
                    else if (_MOBILEBANKING_REQ.FundTransferType == FundTransferType.BNgulCashIn.ToString())
                    {
                        ACQActualAmount = _MOBILEBANKING_REQ.ACQAmountAvailable;
                        ACQActualAmount = ACQActualAmount.PadLeft(9, '0');
                        ActualAmount = _MOBILEBANKING_REQ.AmountAvailable;
                        ActualAmount = ActualAmount.PadLeft(9, '0');
                        BNgulActualAmount = Convert.ToString(Convert.ToInt32(_MOBILEBANKING_REQ.BNgulAmountAvailable) - Convert.ToInt32(_MOBILEBANKING_REQ.TXNAMT));
                        BNgulActualAmount = BNgulActualAmount.PadLeft(9, '0');
                    }
                    else
                    {

                        ActualAmount = Convert.ToString(Convert.ToInt32(_MOBILEBANKING_REQ.AmountAvailable) - Convert.ToInt32(_MOBILEBANKING_REQ.TXNAMT));
                        ActualAmount = ActualAmount.PadLeft(9, '0');
                        ACQActualAmount = _MOBILEBANKING_REQ.ACQAmountAvailable;
                        ACQActualAmount = ACQActualAmount.PadLeft(9, '0');
                        BNgulActualAmount = _MOBILEBANKING_REQ.BNgulAmountAvailable;
                        BNgulActualAmount = BNgulActualAmount.PadLeft(9, '0');
                    }

                    IMPSTransactions.UpdateCardTxnDetails(_MOBILEBANKING_REQ.REMITTERACC, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, ActualAmount, _MOBILEBANKING_REQ.AccountUseCount.PadLeft(2, '0'), _MOBILEBANKING_REQ.LastDateTime.ToString("ddMMyy"), _MOBILEBANKING_REQ.LastDateTime.ToString("HHmmss")
                                             , _MOBILEBANKING_REQ.MaxPinUseCount, ACQActualAmount, BNgulActualAmount);
                }
                else
                {
                    IMPSTransactions.UpdateCardTxnDetails(_MOBILEBANKING_REQ.REMITTERACC, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, _MOBILEBANKING_REQ.AmountAvailable, _MOBILEBANKING_REQ.AccountUseCount.PadLeft(2, '0'), _MOBILEBANKING_REQ.LastDateTime.ToString("ddMMyy"), _MOBILEBANKING_REQ.LastDateTime.ToString("HHmmss")
                                             , _MOBILEBANKING_REQ.MaxPinUseCount, _MOBILEBANKING_REQ.ACQAmountAvailable, _MOBILEBANKING_REQ.BNgulAmountAvailable);
                }
                return true;
            }
            catch (Exception ex)
            { _CommanDetails.SystemLogger.WriteErrorLog(null, ex); return false; }
        }

        public void AccountValidationMessageConvertor(ref MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg)
        {
            try
            {
                RequestMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AccountingAuthorizationResponseMessage;
                RequestMsg.ProcessingCode = Convert.ToString(ConfigurationManager.AppSettings["IsAccountVerification"]);
                if(RequestMsg.ResponseCode =="00")
                {
                    RequestMsg.CustomerName = RequestMsg.MiniStateMentData.Substring(9, 80).Trim();
                    _CommanDetails.SystemLogger.WriteTransLog(null, "RequestMsg.CustomerName : " + RequestMsg.CustomerName);
                    //RequestMsg.NCommandType = (RequestMsg.MiniStateMentData.Substring(182, 3).Contains("S") ? "SA"
                    //                             : RequestMsg.MiniStateMentData.Substring(182, 3).Contains("C") ? "CD"
                    //                             : RequestMsg.MiniStateMentData.Substring(182, 3).Contains("O") ? "OD"
                    //                             : RequestMsg.MiniStateMentData.Substring(182, 3).Contains("L") ? "LO"
                    //                             : "NP");
                    RequestMsg.NCommandType = (RequestMsg.MiniStateMentData.Substring(182, 3)=="SBA" ? "SA"
                                             : RequestMsg.MiniStateMentData.Substring(182, 3)=="CAA" ? "CD"
                                             : RequestMsg.MiniStateMentData.Substring(182, 3)=="ODA" ? "OD"
                                             : RequestMsg.MiniStateMentData.Substring(182, 3)=="CCA" ? "OD"
                                             : "NP");
                    _CommanDetails.SystemLogger.WriteTransLog(null, "RequestMsg.NCommandType : " + RequestMsg.NCommandType);

                    RequestMsg.PrivateField62 = (RequestMsg.MiniStateMentData.Substring(185, 1).Contains("A") ? "ACT"
                                                 : RequestMsg.MiniStateMentData.Substring(185, 1).Contains("C") ? "CLS"
                                                 : RequestMsg.MiniStateMentData.Substring(185, 1).Contains("M") ? "DMT" 
                                                 : RequestMsg.MiniStateMentData.Substring(185, 1).Contains("F") ? "FRZ" : "InvalidAccount");
                    _CommanDetails.SystemLogger.WriteTransLog(null, "RequestMsg.PrivateField62 : " + RequestMsg.PrivateField62);

                    RequestMsg.ToAccountNumber = RequestMsg.FromAccountNumber + "|" + RequestMsg.CustomerName + "|" + RequestMsg.NCommandType +
                                                        "|" + RequestMsg.PrivateField62;
                    _CommanDetails.SystemLogger.WriteTransLog(null, "RequestMsg.ToAccountNumber : " + RequestMsg.ToAccountNumber);
                    // added by sk on 27012023
                    if ((RequestMsg.NCommandType == "SA" || RequestMsg.NCommandType == "CD" || RequestMsg.NCommandType == "OD") && RequestMsg.PrivateField62 == "ACT")
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(null, "Response for success account : " + RequestMsg.ResponseCode);
                    }
                    else
                    {
                        RequestMsg.ResponseCode = "57";
                        _CommanDetails.SystemLogger.WriteTransLog(null, "Response for not permitable account : " + RequestMsg.ResponseCode);
                    }
                }
                else
                {
                    RequestMsg.ResponseCode = "14";
                    _CommanDetails.SystemLogger.WriteTransLog(null, "RequestMsg.ResponseCode from host : " + RequestMsg.ResponseCode);
                }
                RequestMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AccountingAuthorizationResponseMessage;
            }
            catch (Exception ex)
            {
                RequestMsg.ResponseCode = "14";
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
            RequestMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AccountingAuthorizationResponseMessage;
        }

        #endregion Host Processing Using ISO Message Format
    }
}