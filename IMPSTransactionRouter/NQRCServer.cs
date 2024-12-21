using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading.Tasks;
using MaxiSwitch.API.Terminal;
using IMPSTransactionRouter.Models;
using MaxiSwitch.DALC.Configuration;
using System.Text.RegularExpressions;
using System.Data;
using MaxiSwitch.DALC.ConsumerTransactions;
using System.Configuration;
using System.Web.Http;

namespace IMPSTransactionRouter
{

    public class NQRCServer
    {
        public TcpChannel TerminalControllerChannel { get; set; }

        CommanDetails _CommanDetails = new CommanDetails();
       

        public NQRCServer()
        {
            SwitchInitialization();
        }

        internal void SwitchInitialization()
        {
            try
            {
                _CommanDetails.SystemLogger.WriteTransLog(this ,"Switch Server Initialised on Port Number : " + Convert.ToInt32(ConfigurationManager.AppSettings["SwitchPortNumber"]));
             
                int SwitchPort = Convert.ToInt32(ConfigurationManager.AppSettings["SwitchPortNumber"]);

                _CommanDetails.SystemLogger.WriteTransLog(this, "SwitchPort : " + SwitchPort);

                TerminalControllerChannel = new TcpChannel(SwitchPort);

                ChannelServices.RegisterChannel(TerminalControllerChannel);

                RemotingConfiguration.RegisterWellKnownServiceType(typeof(NQRCRequestServer), "NQRCRemoteObject", WellKnownObjectMode.SingleCall);

                _CommanDetails.SystemLogger.WriteTransLog(this, "Switch Service Start on port :" + SwitchPort + " on service Object NQRCRemoteObject");

               // Console.ReadLine(); //on live uncomment
                _CommanDetails.SystemLogger.WriteTransLog(this, "Service End");
            }
            catch (Exception ex)
            { _CommanDetails.SystemLogger.WriteErrorLog(null, ex); }
        }
    }

    public class NQRCRequestServer : MarshalByRefObject, ITerminalRequest
    {
        ////TerminalTransactionProcessor _transactionProcessor = null;
        ProcessMessage _processMessage = new ProcessMessage();
        CommanDetails _commonDetails = new CommanDetails();
        public SwitchConsumerRequestReqMsg TransactionRequest(SwitchConsumerRequestReqMsg RequestMsg)
        {
            try
            {
                _commonDetails.SystemLogger.WriteTransLog(this, "Inward Transaction Recieved for RefrenceNumber : " + RequestMsg.TransactionRefrenceNumber);
                //_processMessage.TransactioninwardFundTransfer((int)IMPSTransactionRouter.Models.enumCommandTypeEnum.AuthorizationRequestMessage, RequestMsg, 1);
                if (RequestMsg.ProcessingCode == ConfigurationManager.AppSettings["IsAccountVerification"])
                {
                    _processMessage.ProcessAccountVerification(ref RequestMsg);
                }
                else if (RequestMsg.ProcessingCode == ConfigurationManager.AppSettings["IsQRCodeVerification"])
                {
                    _processMessage.ProcessQRVerification(ref RequestMsg);
                }
                else
                {
                    _processMessage.ProcessInwardTransaction(ref RequestMsg);
                }
                

                //_processMessage.TransactioninwardFundTransfer((int)IMPSTransactionRouter.Models.enumCommandTypeEnum.AuthorizationRequestMessage, RequestMsg, 7);
            }
            catch (Exception ex)
            { _commonDetails.SystemLogger.WriteErrorLog(null, ex); }
            
            return RequestMsg;
        }

        [HttpPost]
        public SwitchConsumerRequestReqMsg TransactionRequestNew(SwitchConsumerRequestReqMsg RequestMsg)
        {
            try
            {
                _commonDetails.SystemLogger.WriteTransLog(this, "Inward Transaction Recieved for RefrenceNumber : " + RequestMsg.TransactionRefrenceNumber);
                //_processMessage.TransactioninwardFundTransfer((int)IMPSTransactionRouter.Models.enumCommandTypeEnum.AuthorizationRequestMessage, RequestMsg, 1);
                if (RequestMsg.ProcessingCode == ConfigurationManager.AppSettings["IsAccountVerification"])
                {
                    _processMessage.ProcessAccountVerification(ref RequestMsg);
                }
                else if (RequestMsg.ProcessingCode == ConfigurationManager.AppSettings["IsQRCodeVerification"])
                {
                    _processMessage.ProcessQRVerification(ref RequestMsg);
                }
                else
                {
                    _processMessage.ProcessInwardTransaction(ref RequestMsg);
                }


                //_processMessage.TransactioninwardFundTransfer((int)IMPSTransactionRouter.Models.enumCommandTypeEnum.AuthorizationRequestMessage, RequestMsg, 7);
            }
            catch (Exception ex)
            { _commonDetails.SystemLogger.WriteErrorLog(null, ex); }

            return RequestMsg;
        }


        public void FormatMessage(ref MOBILEBANKING_REQ _MOBILEBANKING_REQ, SwitchConsumerRequestReqMsg ReqMessage)
        {
            CommanDetails _CommanDetails = new CommanDetails();
            try
            {
                #region Old
                //SwitchConsumerRequestReqMsg ReqMessage = new SwitchConsumerRequestReqMsg();
                //ReqMessage.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.AccountingAuthorizationRequestMessage;
                //ReqMessage.TransSource = enumTransactionSource.Terminal;
                //ReqMessage.TransactionMode = MaxiSwitch.API.Terminal.enumModeOfTransaction.OnUs;
                //ReqMessage.TransactionType = MaxiSwitch.API.Terminal.enumTransactionType.Purchase;
                //ReqMessage.CardNumber = _MOBILEBANKING_REQ.Pointofinitiationmethod;//"1234567890123456";
                //ReqMessage.ProcessingCode = _MOBILEBANKING_REQ.ProcessingCode; //"280000";
                //ReqMessage.TransactionAmount = _MOBILEBANKING_REQ.TXNAMT.ToString();
                //ReqMessage.LocalTransactionDateTime = DateTime.Now;
                //ReqMessage.SystemsTraceAuditNumber = DateTime.Now.ToString("HHmmss");
                //ReqMessage.MerchantCategoryCode = _MOBILEBANKING_REQ.MerchantCategoryCode;
                //ReqMessage.TransactionCurrencyCode = _MOBILEBANKING_REQ.TransactionCurrencyCode;
                //ReqMessage.PanEntryMode = "081";// _MOBILEBANKING_REQ.CountryCode; //
                //ReqMessage.PosConditionCode = "00";
                //ReqMessage.TransactionRefrenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                ////DateTime.Now.Year.ToString().Substring(3, 1) + DateTime.Now.DayOfYear.ToString().PadLeft(3, '0')  + DateTime.Now.ToString("HH") + ReqMessage.SystemsTraceAuditNumber;
                //ReqMessage.TerminalID = _MOBILEBANKING_REQ.DeviceID;//"BNB00001";
                //ReqMessage.TerminalLocation = _MOBILEBANKING_REQ.Merchantcity;
                //ReqMessage.CardAccepterName = _MOBILEBANKING_REQ.MerchantName;
                //ReqMessage.FromAccountNumber = _MOBILEBANKING_REQ.REMITTERACC;
                //ReqMessage.ToAccountNumber = _MOBILEBANKING_REQ.BENIFICIARYACC;
                //ReqMessage.MiniStateMentData = "";//"000201010211021694004900123546205204581253030645803BTN5924VISA TEST MERCHANT THREE6007THIMPHU6222030600125607080000309063043617";
                //ReqMessage.DeliveryChannel = "NQRC";
                #endregion Old

               
                _CommanDetails.SystemLogger.WriteTransLog(this, "Request Recived at  NQRCServer FormatMessage()  for refernce Number :" + ReqMessage.TransactionRefrenceNumber);


                #region oldstatic
                //_MOBILEBANKING_REQ.Payloadformatindicator = Details[0].ToString() + Details[1].ToString() + Details[2].ToString();
                //_MOBILEBANKING_REQ.Pointofinitiationmethod = Details[3].ToString() + Details[4].ToString() + Details[5].ToString();
                //_MOBILEBANKING_REQ.Merchantidentifier = Details[8].ToString();//Details[6].ToString() + Details[7].ToString() + Details[8].ToString();
                //_MOBILEBANKING_REQ.MerchantCategoryCode = Details[11].ToString();// Details[9].ToString() + Details[10].ToString() + Details[11].ToString();
                //_MOBILEBANKING_REQ.TransactionCurrencyCode = Details[14].ToString(); //Details[12].ToString() + Details[13].ToString() + Details[14].ToString();
                //_MOBILEBANKING_REQ.CountryCode = Details[17].ToString();// Details[15].ToString() + Details[16].ToString() + Details[17].ToString();
                //_MOBILEBANKING_REQ.MerchantName = Details[20].ToString();// Details[18].ToString() + Details[19].ToString() + Details[20].ToString();
                //_MOBILEBANKING_REQ.NQRCcity = Details[23].ToString();// Details[21].ToString() + Details[22].ToString() + Details[23].ToString();
                #endregion oldstatic

                #region IncomingReq

                string[] Sp_CardAccepterName = ReqMessage.CardAccepterName.Split(' ');

                Array.Reverse(Sp_CardAccepterName);

                string Country = Sp_CardAccepterName[0].ToString();
                string City = Sp_CardAccepterName[1].ToString();
                string Name = ReqMessage.CardAccepterName.Substring(0, 25);

                _MOBILEBANKING_REQ.Payloadformatindicator = "";
                _MOBILEBANKING_REQ.Pointofinitiationmethod ="";
                _MOBILEBANKING_REQ.Merchantidentifier = ReqMessage.CardNumber;
                _MOBILEBANKING_REQ.MerchantCategoryCode = ReqMessage.MerchantCategoryCode;
                _MOBILEBANKING_REQ.TransactionCurrencyCode = ReqMessage.TransactionCurrencyCode; 
                _MOBILEBANKING_REQ.CountryCode = Country;
                _MOBILEBANKING_REQ.MerchantName = Name;
                _MOBILEBANKING_REQ.NQRCcity = City;
                #endregion IncomingReg


                #region Splitor
                string TagData = ReqMessage.MiniStateMentData;
                string[] TagDetails = new string[200];
                string TxnType = string.Empty;



                int getValus = 3;
                int indexof = 0;
                try
                {
                    for (int index = 0; index < TagData.Length; )
                    {
                        string Tag = TagData.Substring(index, getValus);
                        index = index + 3;
                        int length = Convert.ToInt32(TagData.Substring(index, getValus));
                        string Length = TagData.Substring(index, getValus);
                        index = index + 3;
                        TagDetails[indexof] += Tag + Length + TagData.Substring(index, length);
                        index += TagData.Substring(index, length).Length;
                        indexof++;
                    }
                }
                catch { }
                _MOBILEBANKING_REQ.Remark = TagDetails[4].Substring(6, TagDetails[4].Length - 6);

                #endregion Splitor

                _CommanDetails.SystemLogger.WriteTransLog(this, "Set  parameter NQRCServer FormatMessage() for refernce Number :" + ReqMessage.TransactionRefrenceNumber);
                 _MOBILEBANKING_REQ.QRTYPE = "OnUs";
                 _MOBILEBANKING_REQ.TXNAMT = Convert.ToDecimal(ReqMessage.TransactionAmount);
                 _MOBILEBANKING_REQ.mPIN = "";
                 _MOBILEBANKING_REQ.QRUniquePANNumber = _MOBILEBANKING_REQ.Merchantidentifier;
                 _MOBILEBANKING_REQ.ReferenceNumber = ReqMessage.TransactionRefrenceNumber;
                 _MOBILEBANKING_REQ.DeviceID = "NQRC_Inward_Transaction";
                 _MOBILEBANKING_REQ.AcquirerBankID = ReqMessage.AcquirerInstCode;

                 _CommanDetails.SystemLogger.WriteTransLog(this, "ReqMessage.AcquirerInstCode : " + ReqMessage.AcquirerInstCode + " for refernce number :" + ReqMessage.TransactionRefrenceNumber);

                 _CommanDetails.SystemLogger.WriteTransLog(this, "_MOBILEBANKING_REQ.AcquirerBankID : " + _MOBILEBANKING_REQ.AcquirerBankID + " for refernce number :" + ReqMessage.TransactionRefrenceNumber);
                     

            }
            catch (Exception Ex)

            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, Ex);
            
            }
        }

        public SwitchConsumerRequestReqMsg ReversalTransactionRequest(SwitchConsumerRequestReqMsg RequestMsg)
        { return null; }

        public SwitchConsumerRequestReqMsg AcknowedgmentRequest(SwitchConsumerRequestReqMsg RequestMsg)
        {
            _commonDetails.SystemLogger.WriteTransLog(this, "NetworkMessage Message Sent : 00" );
            return RequestMsg;
        }
        public SwitchConsumerRequestReqMsg AcknowedgmentRequestHost(SwitchConsumerRequestReqMsg RequestMsg)
        {
            _commonDetails.SystemLogger.WriteTransLog(this, "NetworkMessage Message Sent : 00");
            return RequestMsg;
        }

        public string GenerateMSGID()
        {
            DateTime date = DateTime.Now;
            int m = date.Month;
            int d = date.Day;
            int y = date.Year;
            int jjj = date.DayOfYear;
            string jd = y.ToString().Substring(y.ToString().Length - 1) + jjj.ToString().PadLeft(3, '0') +
                        DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
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

    }
}