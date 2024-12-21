using DALC;
using HtmlAgilityPack;
using IMPSTransactionRouter.FCUBSRTService;
using IMPSTransactionRouter.FCUBSUPService;
using MaxiSwitch.DALC.Configuration;
using MaxiSwitch.DALC.ConsumerTransactions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace IMPSTransactionRouter.Models
{
    public class ProcessPayment
    {
        CommanDetails _CommanDetails = new CommanDetails();
        BTRESPONSE _BTRESPONSE = new BTRESPONSE();
        ProcessMessage _ProcessMessage = new ProcessMessage();
        ProcessHost _ProcessHost = new ProcessHost();
        WaterResponse _WaterResponse = new WaterResponse();
        BTPOSTPAIDRESPHEADER _BTPOSTPAIDRESP = new BTPOSTPAIDRESPHEADER();
        XDocument FormattedXML = null;
        XmlDocument doc = new XmlDocument();
        NPPF.Service _NppfClient = new NPPF.Service();
        RRCO.BobIntegrationServiceClient _RRCOSERVICE = new RRCO.BobIntegrationServiceClient();
        RRCO.DepositVoucherMaster _RRCORESP = new RRCO.DepositVoucherMaster();
        JavaScriptSerializer JS = new JavaScriptSerializer();
        BTPREPAIDRESPHEADER _BTPREPAIDRESPHEADER = new BTPREPAIDRESPHEADER();

        #region bt old method
/*
        public void ProcessBTRecharge(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient)
        {
            MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = new MOBILEBANKING_RESP();
            FCUBSUPService.REVERSEUPTRANSACTION_FSFS_REQ _REVERSEUPTRANSACTION_FSFS_REQ = new REVERSEUPTRANSACTION_FSFS_REQ();
            try
            {
                string ResponseCode = "-1";
                JavaScriptSerializer json = new JavaScriptSerializer();
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                //          System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //          System.Security.Cryptography.X509Certificates.X509Chain chain,
                //          System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{
                //    return true;
                //};

                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var task = Task.Factory.StartNew(() =>
                {
                    var BTURL = ConfigurationManager.AppSettings["BT"].ToString();
                    //var client = (HttpWebRequest)WebRequest.Create(BTURL + "fcgi-bin/reqMsg");

                    var client = (HttpWebRequest)WebRequest.Create(BTURL + "fcgi-bin/provReq");
                    //WebProxy myproxy = new WebProxy("172.30.10.222", 3128);
                    //myproxy.BypassProxyOnLocal = false;
                    //client.Proxy = myproxy;
                    client.Headers.Clear();
                    //client.Headers.Add("USERID", "Tayana");
                    //client.Headers.Add("PASSWD", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Tayana")));
                    client.Headers.Add("USERID", "dpnb");
                    client.Headers.Add("PASSWD", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("dpnb@10952021")));
                    client.Headers.Add("MSISDN-A", "975" + _MOBILEBANKING_REQ.MobileNumber);
                    client.Headers.Add("MSISDN-B", "975" + _MOBILEBANKING_REQ.RechargeMobileNumber);
                    client.Headers.Add("AMOUNT", _MOBILEBANKING_REQ.TXNAMT.ToString());
                    client.Headers.Add("REQTYPE", Convert.ToString((int)enumRechargeRequestType.BTREQMSG));
                    client.Headers.Add("TERMID", "PNB mPAY");
                    client.Headers.Add("REQID", _MOBILEBANKING_REQ.MSGID);
                    client.Headers.Add("DATE-TIME", DateTime.Now.ToString("ddMMyyyy"));
                    try
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(this, "****BT Recharge Request Send To BT For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + client.Headers.ToString());
                        //aa  _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
                        client.Timeout = Convert.ToInt32(TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut).TotalMilliseconds);
                        client.KeepAlive = false;
                        var postData = "";
                        var data = Encoding.ASCII.GetBytes(postData);
                        client.Method = "POST";
                        client.ContentLength = data.Length;
                        using (var stream = client.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }
                        _CommanDetails.SystemLogger.WriteTransLog(this, "****respon");
                        var response = (HttpWebResponse)client.GetResponse();
                        string EmpResponse = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        _CommanDetails.SystemLogger.WriteTransLog(this, "****BT Recharge Response Recieved from BT For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + EmpResponse);
                        string[] ResponseData = EmpResponse.Replace("\r", "").Split('\n');
                        string[] FinalResponseData = ResponseData[5].ToString().Split(':');
                        ResponseCode = FinalResponseData[1].Trim();
                        string[] FinalResponseDataRESP = ResponseData[2].ToString().Split(':');
                        _MOBILEBANKING_RESP1.BTFUNCTIONID = FinalResponseDataRESP[1].Trim();
                        _CommanDetails.SystemLogger.WriteTransLog(this, "response code" + ResponseCode);
                        _MOBILEBANKING_REQ.TXNID = _MOBILEBANKING_RESP1.BTFUNCTIONID;
                        _CommanDetails.SystemLogger.WriteTransLog(this, "RESPID : " + _MOBILEBANKING_REQ.TXNID + "functionid : " + _MOBILEBANKING_RESP1.BTFUNCTIONID);

                    }
                    catch (HttpException ex)
                    {
                        _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        System.Threading.Thread.Sleep(8000);
                        ProcessBTRechargeStatus(ref _MOBILEBANKING_RESP1, _MOBILEBANKING_REQ, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient);
                        return;
                    }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);

                });
                task.Wait(TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut));
                _MOBILEBANKING_RESP1.ResponseCode = CommanDetails.GetResponseCodeHost(ResponseCode);
                _MOBILEBANKING_RESP1.ResponseDesc = CommanDetails.GetResponseCodeDescription(ResponseCode).Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString()).Replace("@primaryaccount", _MOBILEBANKING_REQ.RechargeMobileNumber.ToString()).Replace("@MSGID", _CREATETRANSACTION_FSFS_RES.FCUBS_BODY.TransactionDetails.FCCREF);
                _MOBILEBANKING_RESP.ResponseCode = _MOBILEBANKING_RESP1.ResponseCode;
                _MOBILEBANKING_RESP.ResponseDesc = _MOBILEBANKING_RESP1.ResponseDesc;

             


                if (!task.IsCompleted)
                {
                    ProcessBTRechargeStatus(ref _MOBILEBANKING_RESP, _MOBILEBANKING_REQ, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient);
                }
                else if (ResponseCode == ConstResponseCode.BTApproved)
                {
                    System.Threading.Thread.Sleep(8000);
                    ProcessBTRechargeStatus(ref _MOBILEBANKING_RESP, _MOBILEBANKING_REQ, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient);
                }
                else if (ResponseCode != ConstResponseCode.BTApproved)
                {
                    var taskRev = Task.Factory.StartNew(() =>
                     {
                         _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For BT Recharge For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                         // _ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                         //_ProcessHost.ProcessReversalToHost(RequestMsg, _MOBILEBANKING_REQ);
                     });
                }
                _MOBILEBANKING_RESP.BTFUNCTIONID = _MOBILEBANKING_RESP1.BTFUNCTIONID;
                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
            }
            catch (Exception ex)
            {
                System.Threading.Thread.Sleep(8000);
                ProcessBTRechargeStatus(ref _MOBILEBANKING_RESP, _MOBILEBANKING_REQ, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
        }

        public void ProcessBTRechargeStatus(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient)
        {
            try
            {
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                //          System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //          System.Security.Cryptography.X509Certificates.X509Chain chain,
                //          System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{
                //    return true;
                //};


                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                var BTURL = ConfigurationManager.AppSettings["BT"].ToString();
                var client = (HttpWebRequest)WebRequest.Create(BTURL + "fcgi-bin/getStatus");
                //WebProxy myproxy = new WebProxy("172.30.10.222", 3128);
                //myproxy.BypassProxyOnLocal = false;
                //client.Proxy = myproxy;
                client.Headers.Clear();
                //client.Headers.Add("USERID", "Tayana");
                //client.Headers.Add("PASSWD", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Tayana")));
                client.Headers.Add("USERID", "dpnb");
                client.Headers.Add("PASSWD", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("dpnb@10952021")));//("pnb@2021")));dpnb@10952021
                client.Headers.Add("REQTYPE", Convert.ToString((int)enumRechargeRequestType.BTGETSTATUS));
                client.Headers.Add("REQID", _MOBILEBANKING_REQ.MSGID);
                client.Headers.Add("DATE-TIME", DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss"));

                _CommanDetails.SystemLogger.WriteTransLog(this, "****BT Recharge Get Status Request Send To BT For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + client.Headers.ToString());
                var postData = "";
                var data = Encoding.ASCII.GetBytes(postData);
                client.Method = "POST";
                client.KeepAlive = false;
                client.ContentLength = data.Length;
                using (var stream = client.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response = (HttpWebResponse)client.GetResponse();
                string EmpResponse = new StreamReader(response.GetResponseStream()).ReadToEnd();
                Dictionary<string, string> _HeaderValue = new System.Collections.Generic.Dictionary<string, string>();
                WebHeaderCollection responseHeadersCollection = response.Headers;
                //foreach (var value in responseHeadersCollection)
                //{
                //    _HeaderValue.Add(responseHeadersCollection.GetValues("Respcode");
                //}
                //_MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_HeaderValue["Respcode"]);
                //_MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_HeaderValue["Respcode"]);

                _CommanDetails.SystemLogger.WriteTransLog(this, "****BT Recharge Get Status Response Recieved from BT For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + " Response Code : " + responseHeadersCollection.GetValues("Respcode")[0].ToString() + Environment.NewLine
                                                               + " Action-code: " + responseHeadersCollection.GetValues("Action-code")[0].ToString()
                                                               + Environment.NewLine + " Recharge-status: " + responseHeadersCollection.GetValues("Recharge-status")[0].ToString());

                if (responseHeadersCollection.GetValues("Respcode")[0].ToString() == ConstResponseCode.BTApproved && responseHeadersCollection.GetValues("Action-code")[0].ToString() == ConstResponseCode.BTApproved && responseHeadersCollection.GetValues("Recharge-status")[0].ToString() == "0")
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, "if bt respcode is approved : ");
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost("0000");
                    _CommanDetails.SystemLogger.WriteTransLog(this, "_MOBILEBANKING_RESP.ResponseCode :=  " + _MOBILEBANKING_RESP.ResponseCode);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription("0000").Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString()).Replace("@primaryaccount", _MOBILEBANKING_REQ.RechargeMobileNumber.ToString()).Replace("@MSGID", _MOBILEBANKING_REQ.ReferenceNumber);//_CREATETRANSACTION_FSFS_RES.FCUBS_BODY.TransactionDetails.FCCREF);
                    _MOBILEBANKING_RESP.MSGSTAT = CommanDetails.GetResponseCodeHost("00");
                    _CommanDetails.SystemLogger.WriteTransLog(this, "_MOBILEBANKING_RESP.ResponseCodeDesc :=  " + _MOBILEBANKING_RESP.ResponseDesc);
                }
                else
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(responseHeadersCollection.GetValues("Action-code")[0].ToString());
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(responseHeadersCollection.GetValues("Action-code")[0].ToString());
                    _MOBILEBANKING_RESP.MSGSTAT = CommanDetails.GetResponseCodeDescription(responseHeadersCollection.GetValues("Action-code")[0].ToString());
                }

                //if (responseHeadersCollection.GetValues("Respcode")[0].ToString() == ConstResponseCode.BTApproved && responseHeadersCollection.GetValues("Action-code")[0].ToString() == ConstResponseCode.BTApproved && responseHeadersCollection.GetValues("Recharge-status")[0].ToString() == "0")
                //{ }

                if (responseHeadersCollection.GetValues("Recharge-status")[0].ToString() == "1" || responseHeadersCollection.GetValues("Recharge-status")[0].ToString() == "2" || responseHeadersCollection.GetValues("Recharge-status")[0].ToString() == "4")
                {
                    var task = Task.Factory.StartNew(() =>
                    {
                        //_CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For BT Recharge For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                        //_ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                    });
                }

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectBT);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectBT);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }
        */
        #endregion bt old method

        #region BT New Code

        public void ProcessBTRecharge_Eload(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg)
        {
            MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = new MOBILEBANKING_RESP();
            FCUBSUPService.REVERSEUPTRANSACTION_FSFS_REQ _REVERSEUPTRANSACTION_FSFS_REQ = new REVERSEUPTRANSACTION_FSFS_REQ();
            _MOBILEBANKING_REQ.LastDateTime = Convert.ToDateTime(_MOBILEBANKING_RESP.LastTime);
            //_ProcessHost.ProcessReversalToHost(RequestMsg, _MOBILEBANKING_REQ);//added for testing
           
            try
            {
                Dictionary<string, string> _HeaderValue = new System.Collections.Generic.Dictionary<string, string>();
                WebHeaderCollection responseHeadersCollection;
                string ResponseCode = "-1";
                string ResponseStatus = "-1";
                JavaScriptSerializer json = new JavaScriptSerializer();
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                          System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                          System.Security.Cryptography.X509Certificates.X509Chain chain,
                          System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
                var task = Task.Factory.StartNew(() =>
                {
                   
                    string mg = _MOBILEBANKING_REQ.MSGID;
                    string MsgID = mg + "-" + "975" + _MOBILEBANKING_REQ.RechargeMobileNumber;//"7113798151-97517690836";// "1234567890123456-17899871";// "91" + GenerateMSGID();//9100274214628568-17392651"; 
                    string Password = BTConfigurationData.BTEloadPassword;//"tayana@1021"; //"P455w06d";// txtPassword.Text;
                    string strSHA512 = SHA512(MsgID);//SHA512(MsgID.Substring(0,4)+"-"+txtmobileA.Text+"-"+txtAmt.Text);
                    string _token = MaximusAESEncryption.EncryptStringBT(Password, strSHA512.Substring(0, 16));//strSHA512.Substring(0, 16)  
                    var BTURL = ConfigurationManager.AppSettings["BT"].ToString();
                    var client = (HttpWebRequest)WebRequest.Create(BTURL + "fcgi-bin/provReq");
                    client.Headers.Clear();
                    client.Headers.Add("PASSWD", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Password)));//Base64Encode(txtPassword.Text));
                    client.Headers.Add("REQTYPE", Convert.ToString((int)enumRechargeRequestType.BTREQMSG));
                    client.Headers.Add("AMOUNT", _MOBILEBANKING_REQ.TXNAMT.ToString());
                    client.Headers.Add("USERID", BTConfigurationData.BTEloadUsername);
                    client.Headers.Add("DATE-TIME", DateTime.Now.ToString("dd-MMM-yyyy") + "-" + DateTime.Now.ToString("HH:mm").Substring(0, 2) + "-" + DateTime.Now.ToString("HH:mm").Substring(3, 2));
                    client.Headers.Add("MSISDN-A", "975" + _MOBILEBANKING_REQ.RechargeMobileNumber);
                    client.Headers.Add("REQID", mg);
                    client.Headers.Add("TOKENID", _token);
                    try
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(this, "****BT Recharge Request Send To BT For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + client.Headers.ToString());
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
                        client.Timeout = Convert.ToInt32(TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut).TotalMilliseconds);
                        client.KeepAlive = false;
                        var postData = "";
                        var data = Encoding.ASCII.GetBytes(postData);
                        client.Method = "POST";
                        client.ContentLength = data.Length;
                        using (var stream = client.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }
                        var response = (HttpWebResponse)client.GetResponse();
                        string EmpResponse = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        _CommanDetails.SystemLogger.WriteTransLog(this, " EmpResponse " + EmpResponse);
                        _CommanDetails.SystemLogger.WriteTransLog(this, "****BT Recharge Response Recieved from BT For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + EmpResponse);
                        string[] ResponseData = EmpResponse.Replace("\r", "").Split('\n');
                        _CommanDetails.SystemLogger.WriteTransLog(this, "ResponseData " + ResponseData);

                        //string[] FinalResponseData = ResponseData[5].ToString().Split(':');
                        //ResponseCode = FinalResponseData[3].Trim();

                        string[] FinalResponseData = ResponseData[7].ToString().Split(':');
                        ResponseCode = FinalResponseData[1].Trim();
                        _CommanDetails.SystemLogger.WriteTransLog(this, "ResponseCode:- " + ResponseCode);

                        string[] FinalResponseDataRESP = ResponseData[2].ToString().Split(':');
                        _MOBILEBANKING_RESP1.BTFUNCTIONID = FinalResponseDataRESP[1].Trim();
                        _CommanDetails.SystemLogger.WriteTransLog(this, "BTFUNCTIONID:- " + _MOBILEBANKING_RESP1.BTFUNCTIONID);

                        string[] FinalResponseDataStatus = ResponseData[4].ToString().Split(':');
                        ResponseStatus = FinalResponseDataStatus[1].Trim();
                        _CommanDetails.SystemLogger.WriteTransLog(this, "ResponseStatus:- " + ResponseStatus);

                        _MOBILEBANKING_REQ.TXNID = _MOBILEBANKING_RESP1.BTFUNCTIONID;
                        _CommanDetails.SystemLogger.WriteTransLog(this, "RESPID : " + _MOBILEBANKING_REQ.TXNID + "functionid : " + _MOBILEBANKING_RESP1.BTFUNCTIONID);

                     
                        string[] FinalStatus = ResponseData[0].ToString().Split(':');
                        string[] FinalDesc = ResponseData[3].ToString().Split(':');
                       


                        //string[] FinalResponseDataStatus = ResponseData[4].ToString().Split(':');
                        //ResponseStatus = FinalResponseDataStatus[1].Trim();
                        /*Changes For Report*/

                        _MOBILEBANKING_RESP1.BTStatusCode = FinalStatus[1].Trim();
                        _MOBILEBANKING_RESP1.BTTransStatus = ResponseStatus;
                        _MOBILEBANKING_RESP1.BTStatusDiscription = FinalDesc[1].Trim();

                        if (ResponseStatus != "0")
                        {
                            _CommanDetails.SystemLogger.WriteTransLog(this, "Response Code Recived In Case Of Failed :" + ResponseCode);
                            _CommanDetails.SystemLogger.WriteTransLog(this, "Response Code Recived In Case Of Failed ResponseStatus:" + ResponseStatus);
                            if (ResponseStatus == "1")
                            {
                                var taskRev = Task.Factory.StartNew(() =>
                                {
                                    _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For BT Recharge For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                                    //_ProcessHost.ProcessReversalToHost(RequestMsg, _MOBILEBANKING_REQ);
                                   // _ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                                });
                            }
                            if (ResponseStatus == "3")
                            {
                                var taskvr = Task.Factory.StartNew(() => { });
                                int VerificationRequestCount = 1;
                                do
                                {
                                    do
                                    {
                                        DateTime NewDateTime = DateTime.Now;
                                        System.Threading.Thread.Sleep(2000);
                                        var taskRev = Task.Factory.StartNew(() =>
                                        {
                                            ProcessBTRechargeStatus_Eload(ref _MOBILEBANKING_RESP1, _MOBILEBANKING_REQ, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient);
                                            _CommanDetails.SystemLogger.WriteTransLog(this, "Check Status count /Code:" + VerificationRequestCount + " _MOBILEBANKING_RESP1 code:" + _MOBILEBANKING_RESP1.ResponseCode);
                                            _CommanDetails.SystemLogger.WriteTransLog(this, "Check Status count/Status :" + VerificationRequestCount + " _MOBILEBANKING_RESP1 RechargeStatus:" + _MOBILEBANKING_RESP1.RechargeStatus);
                                            if (_MOBILEBANKING_RESP1.ResponseCode == "00" && _MOBILEBANKING_RESP1.RechargeStatus == "0")
                                            {
                                                ResponseCode = "0000";
                                            }
                                            else
                                            {
                                                ResponseCode = _MOBILEBANKING_RESP1.ResponseCode;
                                            }
                                        });
                                        taskRev.Wait(Convert.ToInt32(TimeSpan.FromSeconds(40000).TotalMilliseconds));
                                        VerificationRequestCount = VerificationRequestCount + 1;
                                        if (_MOBILEBANKING_RESP1.RechargeStatus == "1")
                                        {
                                            var task2 = Task.Factory.StartNew(() =>
                                            {
                                                _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For BT Recharge For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                                            });
                                        }
                                    }
                                    while (VerificationRequestCount < 4 && _MOBILEBANKING_RESP1.RechargeStatus == "3");
                                }
                                while (!taskvr.IsCompleted && VerificationRequestCount < 4);
                            }
                        }
                    }
                    catch (HttpException ex)
                    {
                        _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        System.Threading.Thread.Sleep(8000);
                        ProcessBTRechargeStatus_Eload(ref _MOBILEBANKING_RESP1, _MOBILEBANKING_REQ, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient);
                        return;
                    }
                });
                task.Wait(TimeSpan.FromSeconds(CONFIGURATIONCONFIGDATA.TransactionTimeOut));
                _MOBILEBANKING_RESP1.ResponseCode = CommanDetails.GetResponseCodeHost(ResponseCode);
                _CommanDetails.SystemLogger.WriteTransLog(this, "_MOBILEBANKING_RESP1.ResponseCode:- " + _MOBILEBANKING_RESP1.ResponseCode);
                _MOBILEBANKING_RESP1.ResponseDesc = CommanDetails.GetResponseCodeDescription(ResponseCode).Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString()).Replace("@primaryaccount", _MOBILEBANKING_REQ.RechargeMobileNumber.ToString()).Replace("@MSGID", _MOBILEBANKING_REQ.ReferenceNumber);
                _CommanDetails.SystemLogger.WriteTransLog(this, "_MOBILEBANKING_RESP1.ResponseDesc:- " + _MOBILEBANKING_RESP1.ResponseDesc);
                _MOBILEBANKING_RESP.ResponseCode = _MOBILEBANKING_RESP1.ResponseCode;
                _MOBILEBANKING_RESP.ResponseDesc = _MOBILEBANKING_RESP1.ResponseDesc;



                ///*sms 10042020*/
                if (!task.IsCompleted)
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, "Response Code Revived In Case Of  not task complete :" + ResponseCode);
                     ProcessBTRechargeStatus_Eload(ref _MOBILEBANKING_RESP, _MOBILEBANKING_REQ, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient);
                }
                _MOBILEBANKING_RESP.BTFUNCTIONID = _MOBILEBANKING_RESP1.BTFUNCTIONID;

                _MOBILEBANKING_RESP.BTStatusCode = _MOBILEBANKING_RESP1.BTStatusCode;
                _MOBILEBANKING_RESP.BTTransStatus = _MOBILEBANKING_RESP1.BTTransStatus;
                _MOBILEBANKING_RESP.BTStatusDiscription = _MOBILEBANKING_RESP1.BTStatusDiscription;
                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
            }
            catch (Exception ex)
            {
                System.Threading.Thread.Sleep(8000);
                ProcessBTRechargeStatus_Eload(ref _MOBILEBANKING_RESP, _MOBILEBANKING_REQ, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient);
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
            }
        }

        public void ProcessBTRechargeStatus_Eload(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                          System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                          System.Security.Cryptography.X509Certificates.X509Chain chain,
                          System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };

                var BTURL = ConfigurationManager.AppSettings["BT"].ToString();
                var client = (HttpWebRequest)WebRequest.Create(BTURL + "fcgi-bin/getProvStatus");
                //WebProxy myproxy = new WebProxy("172.30.10.222", 3128);
                //myproxy.BypassProxyOnLocal = false;
                //client.Proxy = myproxy;
                client.Headers.Clear();
                client.Headers.Add("USERID", BTConfigurationData.BTEloadUsername);
                client.Headers.Add("PASSWD", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(BTConfigurationData.BTEloadPassword)));
                client.Headers.Add("REQTYPE", Convert.ToString((int)enumRechargeRequestType.BTGETSTATUS));
                client.Headers.Add("REQID", _MOBILEBANKING_REQ.MSGID);
                client.Headers.Add("DATE-TIME", DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss"));

                _CommanDetails.SystemLogger.WriteTransLog(this, "****BT Recharge Get Status Request Send To BT For Reference Number " +
                    _MOBILEBANKING_REQ.MSGID + Environment.NewLine + client.Headers.ToString());
                var postData = "";
                var data = Encoding.ASCII.GetBytes(postData);
                client.Method = "POST";
                client.KeepAlive = false;
                client.ContentLength = data.Length;
                using (var stream = client.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response = (HttpWebResponse)client.GetResponse();
                string EmpResponse = new StreamReader(response.GetResponseStream()).ReadToEnd();

                Dictionary<string, string> _HeaderValue = new System.Collections.Generic.Dictionary<string, string>();
                WebHeaderCollection responseHeadersCollection = response.Headers;

                _CommanDetails.SystemLogger.WriteTransLog(this, "responseHeadersCollection  :" + responseHeadersCollection.ToString());
                _CommanDetails.SystemLogger.WriteTransLog(this, "responseHeadersCollection.GetValues(Recharge-status)[0].ToString()  :" + responseHeadersCollection.GetValues("Recharge-status")[0].ToString());
                _CommanDetails.SystemLogger.WriteTransLog(this, "responseHeadersCollection.GetValues(Respcode)[0].ToString()  :" + responseHeadersCollection.GetValues("Respcode")[0].ToString());

                _CommanDetails.SystemLogger.WriteTransLog(this, "****BT Recharge Get Status Response Recieved from BT For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + " Response Code : " + responseHeadersCollection.GetValues("Respcode")[0].ToString() + Environment.NewLine
                                                          + Environment.NewLine + " Recharge-status: " + responseHeadersCollection.GetValues("Recharge-status")[0].ToString());

                if (responseHeadersCollection.GetValues("Recharge-status")[0].ToString() == "0")
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost("0000");
                    //_MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription("0000").Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString()).Replace("@primaryaccount", _MOBILEBANKING_REQ.RechargeMobileNumber.ToString()).Replace("@MSGID", _CREATETRANSACTION_FSFS_RES.FCUBS_BODY.TransactionDetails.FCCREF);
                   _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription("0000").Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString()).Replace("@primaryaccount", _MOBILEBANKING_REQ.RechargeMobileNumber.ToString()).Replace("@MSGID", _MOBILEBANKING_REQ.ReferenceNumber);
                    _MOBILEBANKING_RESP.MSGSTAT = responseHeadersCollection.GetValues("Recharge-status")[0].ToString();
                    _CommanDetails.SystemLogger.WriteTransLog(this, "_MOBILEBANKING_RESP.ResponseCode  :" + _MOBILEBANKING_RESP.ResponseCode);
                    _CommanDetails.SystemLogger.WriteTransLog(this, " _MOBILEBANKING_RESP.ResponseDesc   :" + _MOBILEBANKING_RESP.ResponseDesc);
                    _CommanDetails.SystemLogger.WriteTransLog(this, " response from db   :");
                }
                else
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(responseHeadersCollection.GetValues("Respcode")[0].ToString());
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(responseHeadersCollection.GetValues("Respcode")[0].ToString());
                    _MOBILEBANKING_RESP.RechargeStatus = responseHeadersCollection.GetValues("Recharge-status")[0].ToString();
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectBT);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectBT);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        #endregion BT New Code

        public void ProcessBPCOutstandingAmount(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType)
        {
            MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = new MOBILEBANKING_RESP();
            try
            {
                if (!CONFIGURATIONCONFIGDATA.BPCACTIVE)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCIsUnderMaintenance);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCIsUnderMaintenance);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    return;
                }
                string RequestSoapString = string.Empty;
                RequestSoapString = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:urn=\"urn:sap-com:document:sap:rfc:functions\">"
                                        + "<soapenv:Header/>"
                                        + "<soapenv:Body>"
                                         + "<urn:ZISU_CUSTOMER_MPAY>"
                                         + "<GV_COLLECTION_AMOUNT>0</GV_COLLECTION_AMOUNT>"
                                         + "<GV_CONTRACT_ACCOUNT>" + _MOBILEBANKING_REQ.ConsumerNumber + "</GV_CONTRACT_ACCOUNT>"
                                         + "<GV_DEBIT_ACCOUNTNO>0</GV_DEBIT_ACCOUNTNO>"
                                         + "<GV_REQUEST_ID>" + Convert.ToString((int)enumBPC.ReturnOutstandingAmount) + "</GV_REQUEST_ID>"
                                         + "<GV_TRANSACTIONID>0</GV_TRANSACTIONID>"
                                         + "</urn:ZISU_CUSTOMER_MPAY>"
                                        + "</soapenv:Body>"
                                        + "</soapenv:Envelope>";
                string EmpResponse = string.Empty;

                HttpWebRequest request = CreateWebRequestBPC();
                XmlDocument soapEnvelopeXml = new XmlDocument();
                soapEnvelopeXml.LoadXml(RequestSoapString);
                using (Stream stream = request.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }

                FormattedXML = XDocument.Parse(RequestSoapString);

                _CommanDetails.SystemLogger.WriteTransLog(this, "**** BPC Get Outstanding Amount Transaction Request Send To BPC For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + FormattedXML.ToString());

                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationRequestMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);

                string CustomerName = string.Empty;
                string OutstandingAmount = string.Empty;
                string Status = string.Empty;
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        EmpResponse = rd.ReadToEnd();
                        doc.LoadXml(EmpResponse);
                        _MOBILEBANKING_RESP1.ResponseCode = doc.GetElementsByTagName("GV_STATUS")[0].InnerText;
                        if (_MOBILEBANKING_RESP1.ResponseCode == "1")
                        {
                            CustomerName = doc.GetElementsByTagName("GV_CUSTOMER_NAME")[0].InnerText;
                            OutstandingAmount = doc.GetElementsByTagName("GV_OUTSTANDING_AMOUNT")[0].InnerText;
                            Status = doc.GetElementsByTagName("GV_STATUS")[0].InnerText;
                        }
                        else
                        {
                            Status = doc.GetElementsByTagName("GV_STATUS")[0].InnerText;
                        }
                    }

                }

                #region Loger
                try
                {
                    FormattedXML = XDocument.Parse(EmpResponse);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("*****  BPC Get Outstanding Amount Transaction Response Recieved from BPC For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                    FormattedXML.ToString() + Environment.NewLine));
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion
                if (Status == "1")
                {
                    if (Convert.ToDecimal(OutstandingAmount) > 0)
                    {
                        _MOBILEBANKING_RESP.OutstandingAmount = OutstandingAmount;
                        _MOBILEBANKING_RESP.ConsumerName = CustomerName;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.Approved);

                        try
                        {
                            var taskinsert = Task.Factory.StartNew(() =>
                             {
                                 IMPSTransactions.INSERT_RECENTTRANSACTION(_MOBILEBANKING_REQ.ConsumerNumber, CustomerName, OutstandingAmount, _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, enumSource.BPCBILLPAYMENT.ToString());
                             });
                        }
                        catch { }
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCBillalreadypaid);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCBillalreadypaid);
                    }
                }
                else
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCInvalidConsumerNumber);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCInvalidConsumerNumber);
                }

                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectBPC);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectBPC);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessBPCBillPay(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient)
        {
            MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = new MOBILEBANKING_RESP();
            try
            {
                try
                {
                    string RequestSoapString = string.Empty;
                    RequestSoapString = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:urn=\"urn:sap-com:document:sap:rfc:functions\">"
                                           + "<soapenv:Header/>"
                                           + "<soapenv:Body>"
                                            + "<urn:ZISU_CUSTOMER_MPAY>"
                                            + "<GV_COLLECTION_AMOUNT>" + _MOBILEBANKING_REQ.TXNAMT + "</GV_COLLECTION_AMOUNT>"
                                            + "<GV_CONTRACT_ACCOUNT>" + _MOBILEBANKING_REQ.ConsumerNumber + "</GV_CONTRACT_ACCOUNT>"
                                            + "<GV_DEBIT_ACCOUNTNO>" + _MOBILEBANKING_REQ.REMITTERACC + "</GV_DEBIT_ACCOUNTNO>"
                                            + "<GV_REQUEST_ID>" + Convert.ToString((int)enumBPC.BankSystemTransfer) + "</GV_REQUEST_ID>"
                                            + "<GV_TRANSACTIONID>" + _MOBILEBANKING_REQ.MSGID + "</GV_TRANSACTIONID>"
                                            + "</urn:ZISU_CUSTOMER_MPAY>"
                                           + "</soapenv:Body>"
                                           + "</soapenv:Envelope>";
                    string EmpResponse = string.Empty;

                    HttpWebRequest request = CreateWebRequestBPC();
                    XmlDocument soapEnvelopeXml = new XmlDocument();
                    soapEnvelopeXml.LoadXml(RequestSoapString);
                    using (Stream stream = request.GetRequestStream())
                    {
                        soapEnvelopeXml.Save(stream);
                    }
                    FormattedXML = XDocument.Parse(RequestSoapString);

                    _CommanDetails.SystemLogger.WriteTransLog(this, "**** BPC Bill Payment Transaction Request Send To BPC For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + FormattedXML.ToString());
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);

                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                        {
                            EmpResponse = rd.ReadToEnd();
                            doc.LoadXml(EmpResponse);
                            _MOBILEBANKING_RESP1.ResponseCode = doc.GetElementsByTagName("GV_STATUS")[0].InnerText;
                        }
                    }
                    #region Loger
                    try
                    {
                        FormattedXML = XDocument.Parse(EmpResponse);
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("*****  BPC Bill Payment Transaction Response Recieved from BPC For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine));
                    }
                    catch (Exception ex)
                    {
                        _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    }
                    #endregion Loger
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectBPC);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectBPC);
                    _MOBILEBANKING_RESP.ResponseData = null;

                    if (ex.Message.ToString().Contains("The request was canceled"))
                    {
                        var task = Task.Factory.StartNew(() =>
                        {
                            _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For BPC Bill Payment For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                            _ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                        });
                    }
                    else if (ex.Message.ToString().Contains("The operation has timed out"))
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                    }
                    return;
                }

                string Status = _MOBILEBANKING_RESP1.ResponseCode;
                if (Status == "1")
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCSuccess);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCSuccess).Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString()).Replace("@primaryaccount", _MOBILEBANKING_REQ.ConsumerNumber.ToString()).Replace("@MSGID", _CREATETRANSACTION_FSFS_RES.FCUBS_BODY.TransactionDetails.FCCREF);
                }
                else
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCUnSuccess);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCUnSuccess);
                    var task = Task.Factory.StartNew(() =>
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For BPC Bill Payment For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                        //_ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                    });
                }

                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectBPC);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectBPC);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessTCellPrepaidRecharge(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg)
        {
            MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = new MOBILEBANKING_RESP();
            try
            {
                string TcellRequest = string.Empty;
                TcellRequest = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ars=\"http://www.huawei.com/bme/cbsinterface/arservices\" xmlns:cbs=\"http://www.huawei.com/bme/cbsinterface/cbscommon\" xmlns:arc=\"http://cbs.huawei.com/ar/wsservice/arcommon\">"
                               + "<soapenv:Header/>"
                                  + "<soapenv:Body>"
                                    + "<ars:RechargeRequestMsg>"
                                    + "<RequestHeader>"
                                    + "<cbs:Version>" + TCELLPARAM.Version + "</cbs:Version>"
                                    + "<cbs:MessageSeq>" + _MOBILEBANKING_REQ.MSGID + "</cbs:MessageSeq>"
                                    + "<cbs:OwnershipInfo>"
                                    + "<cbs:BEID>" + TCELLPARAM.BEID + "</cbs:BEID>"
                                    + "</cbs:OwnershipInfo>"
                                    + "<cbs:AccessSecurity>"
                                    + "<cbs:LoginSystemCode>" + TCELLPARAM.LoginSystemCode + "</cbs:LoginSystemCode>"
                                    + "<cbs:Password>" + TCELLPARAM.Password + "</cbs:Password>"
                                    + "<cbs:RemoteIP>" + TCELLPARAM.RemoteIP + "</cbs:RemoteIP>"
                                    + "</cbs:AccessSecurity>"
                                    + "<cbs:OperatorInfo>"
                                    + "<cbs:OperatorID>" + TCELLPARAM.OperatorID + "</cbs:OperatorID>"
                                    + "<cbs:ChannelID>" + TCELLPARAM.ChannelID + "</cbs:ChannelID>"
                                    + "</cbs:OperatorInfo>"
                                    + "<cbs:AccessMode>" + TCELLPARAM.AccessMode + "</cbs:AccessMode>"
                                    + "<cbs:MsgLanguageCode>" + TCELLPARAM.MsgLanguageCode + "</cbs:MsgLanguageCode>"
                                    + "<cbs:TimeFormat>"
                                    + "<cbs:TimeType>" + TCELLPARAM.TimeType + "</cbs:TimeType>"
                                    + "<cbs:TimeZoneID>" + TCELLPARAM.TimeZoneID + "</cbs:TimeZoneID>"
                                    + "</cbs:TimeFormat>"
                                    + "</RequestHeader>"
                                    + "<RechargeRequest>"
                                    + "<ars:RechargeSerialNo>" + _MOBILEBANKING_REQ.MSGID + "</ars:RechargeSerialNo>"
                                    + "<ars:RechargeType>" + TCELLPARAM.RechargeType + "</ars:RechargeType>"
                                    + "<ars:RechargeObj>"
                                    + "<ars:SubAccessCode>"
                                    + "<arc:PrimaryIdentity>" + _MOBILEBANKING_REQ.RechargeMobileNumber + "</arc:PrimaryIdentity>"
                                    + "</ars:SubAccessCode>"
                                    + "</ars:RechargeObj>"
                                    + "<ars:RechargeInfo>"
                                    + "<ars:CashPayment>"
                                    + "<ars:Amount>" + Convert.ToInt16(_MOBILEBANKING_REQ.TXNAMT) * 10000 + "</ars:Amount>"
                                    + "<ars:BankInfo>"
                                    + "<arc:BankCode>" + TCELLPARAM.BankCode + "</arc:BankCode>"
                                    + "<arc:BankBranchCode>" + TCELLPARAM.BankBranchCode + "</arc:BankBranchCode>"
                                    + "</ars:BankInfo>"
                                    + "</ars:CashPayment>"
                                    + "</ars:RechargeInfo>"
                                    + "</RechargeRequest>"
                                    + "</ars:RechargeRequestMsg>"
                                    + "</soapenv:Body>"
                                    + "</soapenv:Envelope>";
                string EmpResponse = string.Empty;

                HttpWebRequest request = CreateTcellPrepaidWebRequest();
                XmlDocument soapEnvelopeXml = new XmlDocument();
                soapEnvelopeXml.LoadXml(TcellRequest);
                using (Stream stream = request.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }

                FormattedXML = XDocument.Parse(TcellRequest);
                _CommanDetails.SystemLogger.WriteTransLog(this, "****Tcell Prepaid Recharge Request Send To Tcell For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + FormattedXML.ToString() + Environment.NewLine);
                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, null, null, null, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        EmpResponse = rd.ReadToEnd();
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(EmpResponse);
                        TCELLPARAM_RESP.ResultCode = doc.GetElementsByTagName("cbs:ResultCode")[0].InnerText;
                        TCELLPARAM_RESP.ResultDesc = doc.GetElementsByTagName("cbs:ResultDesc")[0].InnerText;
                    }
                }
                try
                {
                    FormattedXML = XDocument.Parse(EmpResponse);
                    _CommanDetails.SystemLogger.WriteTransLog(this, "****Tcell Prepaid Recharge Response Recieved From Tcell For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + FormattedXML.ToString());
                }
                catch (Exception ex)
                { }
                if (TCELLPARAM_RESP.ResultCode == "0")
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, "****TCELLPARAM_RESP.ResultCode*****" + TCELLPARAM_RESP.ResultCode);
                    //_MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.TCELLSuccessPrepaid);
                    //_MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.TCELLSuccessPrepaid).Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString()).Replace("@primaryaccount", _MOBILEBANKING_REQ.RechargeMobileNumber.ToString()).Replace("@MSGID", _CREATETRANSACTION_FSFS_RES.FCUBS_BODY.TransactionDetails.FCCREF);
                    //Change UAT
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.TCELLSuccessPrepaid);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.TCELLSuccessPrepaid).Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString()).Replace("@primaryaccount", _MOBILEBANKING_REQ.RechargeMobileNumber.ToString()).Replace("@MSGID", _MOBILEBANKING_REQ.ReferenceNumber);
                    _MOBILEBANKING_RESP.MSGSTAT = CommanDetails.GetResponseCodeDescription(ConstResponseCode.TCELLSuccessPrepaid);
                }
                else
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(TCELLPARAM_RESP.ResultCode);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(TCELLPARAM_RESP.ResultCode);
                    _MOBILEBANKING_RESP.MSGSTAT = CommanDetails.GetResponseCodeDescription(TCELLPARAM_RESP.ResultCode);
                    var task = Task.Factory.StartNew(() =>
                    {
                       // _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For Tcell Prepaid Recharge For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                        _ProcessHost.ProcessReversalToHost(RequestMsg, _MOBILEBANKING_REQ);
                    });
                }
                //_ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, null, null, null, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectTcell);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectTcell);
                _MOBILEBANKING_RESP.MSGSTAT = _MOBILEBANKING_RESP.ResponseDesc;
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessTCellGetOutstandingAmountPostpaid(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType)
        {
            MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = new MOBILEBANKING_RESP();
            try
            {
                if (!CONFIGURATIONCONFIGDATA.TCELLACTIVE)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCIsUnderMaintenance);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCIsUnderMaintenance);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    return;
                }
                string TcellRequest = string.Empty;
                TcellRequest = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ars=\"http://www.huawei.com/bme/cbsinterface/arservices\" xmlns:cbs=\"http://www.huawei.com/bme/cbsinterface/cbscommon\" xmlns:arc=\"http://cbs.huawei.com/ar/wsservice/arcommon\">"
                               + "<soapenv:Header/>"
                                  + "<soapenv:Body>"
                                    + "<ars:QueryBalanceRequestMsg>"
                                    + "<RequestHeader>"
                                    + "<cbs:Version>" + TCELLPARAM.Version + "</cbs:Version>"
                                    + "<cbs:MessageSeq>" + _MOBILEBANKING_REQ.MSGID + "</cbs:MessageSeq>"
                                    + "<cbs:OwnershipInfo>"
                                    + "<cbs:BEID>" + TCELLPARAM.BEID + "</cbs:BEID>"
                                    + "</cbs:OwnershipInfo>"
                                    + "<cbs:AccessSecurity>"
                                    + "<cbs:LoginSystemCode>" + TCELLPARAM.LoginSystemCode + "</cbs:LoginSystemCode>"
                                    + "<cbs:Password>" + TCELLPARAM.Password + "</cbs:Password>"
                                    + "<cbs:RemoteIP>" + TCELLPARAM.RemoteIP + "</cbs:RemoteIP>"
                                    + "</cbs:AccessSecurity>"
                                    + "<cbs:OperatorInfo>"
                                    + "<cbs:OperatorID>" + TCELLPARAM.OperatorID + "</cbs:OperatorID>"
                                    + "<cbs:ChannelID>" + TCELLPARAM.ChannelID + "</cbs:ChannelID>"
                                    + "</cbs:OperatorInfo>"
                                    + "<cbs:AccessMode>" + TCELLPARAM.AccessMode + "</cbs:AccessMode>"
                                    + "<cbs:MsgLanguageCode>" + TCELLPARAM.MsgLanguageCode + "</cbs:MsgLanguageCode>"
                                    + "<cbs:TimeFormat>"
                                    + "<cbs:TimeType>" + TCELLPARAM.TimeType + "</cbs:TimeType>"
                                    + "<cbs:TimeZoneID>" + TCELLPARAM.TimeZoneID + "</cbs:TimeZoneID>"
                                    + "</cbs:TimeFormat>"
                                    + "</RequestHeader>"
                                    + "<QueryBalanceRequest>"
                                    + "<ars:QueryObj>"
                                    + "<ars:SubAccessCode>"
                                    + "<arc:PrimaryIdentity>" + _MOBILEBANKING_REQ.RechargeMobileNumber + "</arc:PrimaryIdentity>"
                                    + "</ars:SubAccessCode>"
                                    + "</ars:QueryObj>"
                                    + "</QueryBalanceRequest>"
                                    + "</ars:QueryBalanceRequestMsg>"
                                    + "</soapenv:Body>"
                                    + "</soapenv:Envelope>";
                string EmpResponse = string.Empty;

                HttpWebRequest request = CreateWebRequest();
                XmlDocument soapEnvelopeXml = new XmlDocument();
                soapEnvelopeXml.LoadXml(TcellRequest);
                using (Stream stream = request.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }

                FormattedXML = XDocument.Parse(TcellRequest);
                _CommanDetails.SystemLogger.WriteTransLog(this, "****Tcell Get Postpaid Outstanding Amount Request Send To Tcell For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + FormattedXML.ToString());
                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        EmpResponse = rd.ReadToEnd();
                        doc.LoadXml(EmpResponse);
                        TCELLPARAM_RESP.ResultCode = doc.GetElementsByTagName("cbs:ResultCode")[0].InnerText;
                        TCELLPARAM_RESP.ResultDesc = doc.GetElementsByTagName("cbs:ResultDesc")[0].InnerText;
                    }

                }

                try
                {
                    FormattedXML = XDocument.Parse(EmpResponse);
                    _CommanDetails.SystemLogger.WriteTransLog(this, "****Tcell Get Postpaid Outstanding Amount Response Recieved From Tcell For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + FormattedXML.ToString());
                }
                catch (Exception ex)
                { }
                if (TCELLPARAM_RESP.ResultCode == "0")
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.Approved);
                    _MOBILEBANKING_RESP.MSGSTAT = _MOBILEBANKING_RESP.ResponseDesc;
                    XmlNodeList _OutstandingAmt = doc.GetElementsByTagName("ars:OutStandingAmount");
                    Double TotalOutstandingAmount = 0;
                    foreach (XmlNode outstandingdata in _OutstandingAmt)
                    {
                        TotalOutstandingAmount += Convert.ToDouble(outstandingdata.InnerText);
                    }

                    try
                    {
                        if ((TotalOutstandingAmount / 10000).ToString().Split('.').Count() > 1)
                        {
                            if ((TotalOutstandingAmount / 10000).ToString().Split('.')[1].Length > 2)
                                _MOBILEBANKING_RESP.OutstandingAmount = RoundUp((TotalOutstandingAmount / 10000), 2).ToString();
                            else
                                _MOBILEBANKING_RESP.OutstandingAmount = (TotalOutstandingAmount / 10000).ToString();
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.OutstandingAmount = (TotalOutstandingAmount / 10000).ToString();
                        }
                    }
                    catch { }
                }
                else
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(TCELLPARAM_RESP.ResultCode);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(TCELLPARAM_RESP.ResultCode);
                    _MOBILEBANKING_RESP.MSGSTAT = _MOBILEBANKING_RESP.ResponseDesc;
                }

                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectTcell);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectTcell);
                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessTCellRechargePostpaid(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg)
        {
            MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = new MOBILEBANKING_RESP();
            try
            {
                string TcellRequest = string.Empty;
                TcellRequest = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ars=\"http://www.huawei.com/bme/cbsinterface/arservices\" xmlns:cbs=\"http://www.huawei.com/bme/cbsinterface/cbscommon\" xmlns:arc=\"http://cbs.huawei.com/ar/wsservice/arcommon\">"
                               + "<soapenv:Header/>"
                                  + "<soapenv:Body>"
                                    + "<ars:PaymentRequestMsg>"
                                    + "<RequestHeader>"
                                    + "<cbs:Version>" + TCELLPARAM.Version + "</cbs:Version>"
                                    + "<cbs:MessageSeq>" + _MOBILEBANKING_REQ.MSGID + "</cbs:MessageSeq>"
                                    + "<cbs:OwnershipInfo>"
                                    + "<cbs:BEID>" + TCELLPARAM.BEID + "</cbs:BEID>"
                                    + "</cbs:OwnershipInfo>"
                                    + "<cbs:AccessSecurity>"
                                    + "<cbs:LoginSystemCode>" + TCELLPARAM.LoginSystemCode + "</cbs:LoginSystemCode>"
                                    + "<cbs:Password>" + TCELLPARAM.Password + "</cbs:Password>"
                                    + "<cbs:RemoteIP>" + TCELLPARAM.RemoteIP + "</cbs:RemoteIP>"
                                    + "</cbs:AccessSecurity>"
                                    + "<cbs:OperatorInfo>"
                                    + "<cbs:OperatorID>" + TCELLPARAM.OperatorID + "</cbs:OperatorID>"
                                    + "<cbs:ChannelID>" + TCELLPARAM.ChannelID + "</cbs:ChannelID>"
                                    + "</cbs:OperatorInfo>"
                                    + "<cbs:AccessMode>" + TCELLPARAM.AccessMode + "</cbs:AccessMode>"
                                    + "<cbs:MsgLanguageCode>" + TCELLPARAM.MsgLanguageCode + "</cbs:MsgLanguageCode>"
                                    + "<cbs:TimeFormat>"
                                    + "<cbs:TimeType>" + TCELLPARAM.TimeType + "</cbs:TimeType>"
                                    + "<cbs:TimeZoneID>" + TCELLPARAM.TimeZoneID + "</cbs:TimeZoneID>"
                                    + "</cbs:TimeFormat>"
                                    + "</RequestHeader>"

                                    + "<PaymentRequest>"
                                    + "<ars:PaymentSerialNo>" + _MOBILEBANKING_REQ.MSGID + "</ars:PaymentSerialNo>"
                                    + "<ars:PaymentType>" + TCELLPARAM.RechargeType + "</ars:PaymentType>"
                                    + "<ars:OpType>" + TCELLPARAM.opType + "</ars:OpType>"
                                    + "<ars:PaymentObj>"
                                    + "<ars:AcctAccessCode>"
                                    + "<arc:PrimaryIdentity>" + _MOBILEBANKING_REQ.RechargeMobileNumber + "</arc:PrimaryIdentity>"
                                    + "</ars:AcctAccessCode>"
                                    + "</ars:PaymentObj>"
                                    + "<ars:PaymentInfo>"
                                    + "<ars:CashPayment>"
                                    + "<ars:Amount>" + Convert.ToDouble(_MOBILEBANKING_REQ.TXNAMT) * 10000 + "</ars:Amount>"
                                    + "<ars:BankInfo>"
                                    + "<arc:BankCode>" + TCELLPARAM.BankCode + "</arc:BankCode>"
                                    + "<arc:BankBranchCode>" + TCELLPARAM.BankBranchCode + "</arc:BankBranchCode>"
                                    + "</ars:BankInfo>"
                                    + "</ars:CashPayment>"
                                    + "</ars:PaymentInfo>"
                                    + "</PaymentRequest>"
                                    + "</ars:PaymentRequestMsg>"
                                    + "</soapenv:Body>"
                                    + "</soapenv:Envelope>";
                string EmpResponse = string.Empty;

                HttpWebRequest request = CreateWebRequest();
                XmlDocument soapEnvelopeXml = new XmlDocument();
                soapEnvelopeXml.LoadXml(TcellRequest);
                using (Stream stream = request.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }
                FormattedXML = XDocument.Parse(TcellRequest);
                _CommanDetails.SystemLogger.WriteTransLog(this, "****Tcell Postpaid Recharge Request Send To Tcell For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + FormattedXML.ToString() + Environment.NewLine);
                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, null, null, null, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        EmpResponse = rd.ReadToEnd();
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(EmpResponse);
                        TCELLPARAM_RESP.ResultCode = doc.GetElementsByTagName("cbs:ResultCode")[0].InnerText;
                        TCELLPARAM_RESP.ResultDesc = doc.GetElementsByTagName("cbs:ResultDesc")[0].InnerText;
                    }
                }

                try
                {
                    FormattedXML = XDocument.Parse(EmpResponse);
                    _CommanDetails.SystemLogger.WriteTransLog(this, "****Tcell Postpaid Recharge Response Recieved From Tcell For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + FormattedXML.ToString());
                }
                catch (Exception ex)
                { }

                if (TCELLPARAM_RESP.ResultCode == "0")
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.TCELLSuccessPostpaid);
                 //   _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.TCELLSuccessPostpaid).Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString()).Replace("@primaryaccount", _MOBILEBANKING_REQ.RechargeMobileNumber.ToString()).Replace("@MSGID", _CREATETRANSACTION_FSFS_RES.FCUBS_BODY.TransactionDetails.FCCREF);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.TCELLSuccessPostpaid).Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString()).Replace("@primaryaccount", _MOBILEBANKING_REQ.RechargeMobileNumber.ToString()).Replace("@MSGID",_MOBILEBANKING_REQ.ReferenceNumber);
                    _MOBILEBANKING_RESP.MSGSTAT = _MOBILEBANKING_RESP.ResponseDesc;
                }
                else
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(TCELLPARAM_RESP.ResultCode);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(TCELLPARAM_RESP.ResultCode);
                    var task = Task.Factory.StartNew(() =>
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For Tcell Postpaid Recharge For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                        _ProcessHost.ProcessReversalToHost(RequestMsg, _MOBILEBANKING_REQ);
                    });
                }
                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, null, null, null, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectTcell);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectTcell);
                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessTCellGetOutstandingAmountLeaseLine(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType)
        {
            MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = new MOBILEBANKING_RESP();
            try
            {
                if (!CONFIGURATIONCONFIGDATA.TCELLACTIVE)
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCIsUnderMaintenance);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCIsUnderMaintenance);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    _MOBILEBANKING_RESP.ResponseData = null;
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    return;
                }
                string TcellRequest = string.Empty;

                TcellRequest = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ars=\"http://www.huawei.com/bme/cbsinterface/arservices\" xmlns:cbs=\"http://www.huawei.com/bme/cbsinterface/cbscommon\" xmlns:arc=\"http://cbs.huawei.com/ar/wsservice/arcommon\">"
                               + "<soapenv:Header/>"
                                  + "<soapenv:Body>"
                                    + "<ars:QueryBalanceRequestMsg>"
                                    + "<RequestHeader>"
                                    + "<cbs:Version>" + TCELLPARAM.Version + "</cbs:Version>"
                                    + "<cbs:MessageSeq>" + _MOBILEBANKING_REQ.MSGID + "</cbs:MessageSeq>"
                                    + "<cbs:OwnershipInfo>"
                                    + "<cbs:BEID>" + TCELLPARAM.BEID + "</cbs:BEID>"
                                    + "</cbs:OwnershipInfo>"
                                    + "<cbs:AccessSecurity>"
                                    + "<cbs:LoginSystemCode>" + TCELLPARAM.LoginSystemCode + "</cbs:LoginSystemCode>"
                                    + "<cbs:Password>" + TCELLPARAM.Password + "</cbs:Password>"
                                    + "<cbs:RemoteIP>" + TCELLPARAM.RemoteIP + "</cbs:RemoteIP>"
                                    + "</cbs:AccessSecurity>"
                                    + "<cbs:OperatorInfo>"
                                    + "<cbs:OperatorID>" + TCELLPARAM.OperatorID + "</cbs:OperatorID>"
                                    + "<cbs:ChannelID>" + TCELLPARAM.ChannelID + "</cbs:ChannelID>"
                                    + "</cbs:OperatorInfo>"
                                    + "<cbs:AccessMode>" + TCELLPARAM.AccessMode + "</cbs:AccessMode>"
                                    + "<cbs:MsgLanguageCode>" + TCELLPARAM.MsgLanguageCode + "</cbs:MsgLanguageCode>"
                                    + "<cbs:TimeFormat>"
                                    + "<cbs:TimeType>" + TCELLPARAM.TimeType + "</cbs:TimeType>"
                                    + "<cbs:TimeZoneID>" + TCELLPARAM.TimeZoneID + "</cbs:TimeZoneID>"
                                    + "</cbs:TimeFormat>"
                                    + "</RequestHeader>"
                                    + "<QueryBalanceRequest>"
                                    + "<ars:QueryObj>"
                                    + "<ars:SubAccessCode>"
                                    + "<arc:PrimaryIdentity>" + _MOBILEBANKING_REQ.LeaseLineNumber + "</arc:PrimaryIdentity>"
                                    + "</ars:SubAccessCode>"
                                    + "</ars:QueryObj>"
                                    + "</QueryBalanceRequest>"
                                    + "</ars:QueryBalanceRequestMsg>"
                                    + "</soapenv:Body>"
                                    + "</soapenv:Envelope>";

                string EmpResponse = string.Empty;

                HttpWebRequest request = CreateWebRequest();
                XmlDocument soapEnvelopeXml = new XmlDocument();
                soapEnvelopeXml.LoadXml(TcellRequest);
                using (Stream stream = request.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }

                FormattedXML = XDocument.Parse(TcellRequest);
                _CommanDetails.SystemLogger.WriteTransLog(this, "****Tcell Get Lease Line Outstanding Amount Request Send To Tcell For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + FormattedXML.ToString());
                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        EmpResponse = rd.ReadToEnd();
                        doc.LoadXml(EmpResponse);
                        TCELLPARAM_RESP.ResultCode = doc.GetElementsByTagName("cbs:ResultCode")[0].InnerText;
                        TCELLPARAM_RESP.ResultDesc = doc.GetElementsByTagName("cbs:ResultDesc")[0].InnerText;
                    }
                }

                try
                {
                    FormattedXML = XDocument.Parse(EmpResponse);
                    _CommanDetails.SystemLogger.WriteTransLog(this, "****Tcell Get Lease Line Outstanding Amount Response Recieved From Tcell For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + FormattedXML.ToString());
                }
                catch (Exception ex)
                { }
                if (TCELLPARAM_RESP.ResultCode == "0")
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.Approved);
                    _MOBILEBANKING_RESP.MSGSTAT = _MOBILEBANKING_RESP.ResponseDesc;
                    XmlNodeList _OutstandingAmt = doc.GetElementsByTagName("ars:OutStandingAmount");
                    Double TotalOutstandingAmount = 0;
                    foreach (XmlNode outstandingdata in _OutstandingAmt)
                    {
                        TotalOutstandingAmount += Convert.ToDouble(outstandingdata.InnerText);
                    }
                    try
                    {
                        if ((TotalOutstandingAmount / 10000).ToString().Split('.').Count() > 1)
                        {
                            if ((TotalOutstandingAmount / 10000).ToString().Split('.')[1].Length > 2)
                                _MOBILEBANKING_RESP.OutstandingAmount = RoundUp((TotalOutstandingAmount / 10000), 2).ToString();
                            else
                                _MOBILEBANKING_RESP.OutstandingAmount = (TotalOutstandingAmount / 10000).ToString();
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.OutstandingAmount = (TotalOutstandingAmount / 10000).ToString();
                        }
                    }
                    catch { }

                    try
                    {
                        var taskinsert = Task.Factory.StartNew(() =>
                        {
                            IMPSTransactions.INSERT_RECENTTRANSACTION(_MOBILEBANKING_REQ.ConsumerNumber, "", Convert.ToString(TotalOutstandingAmount / 10000).ToString(), _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, enumSource.TCELLLEASELINEPAYMENT.ToString());
                        });
                    }
                    catch { }
                }
                else
                {
                    if (TCELLPARAM_RESP.ResultCode == "118100043")
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidServiceNumber);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidServiceNumber);
                        _MOBILEBANKING_RESP.MSGSTAT = _MOBILEBANKING_RESP.ResponseDesc;
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(TCELLPARAM_RESP.ResultCode);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(TCELLPARAM_RESP.ResultCode);
                        _MOBILEBANKING_RESP.MSGSTAT = _MOBILEBANKING_RESP.ResponseDesc;
                    }
                }

                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectTcell);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectTcell);
                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessTCellGetOutstandingAmountLeaseLineNEW(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType)
        {
           
            try
            {
                TASHILEASEDLINEOUTSTANDING_RESP _TCELLOUTRESP = new TASHILEASEDLINEOUTSTANDING_RESP();
                string TcellOutstandingURL = ConfigurationManager.AppSettings["TashiOutstanding_URL"].Replace("@serviceid",_MOBILEBANKING_REQ.LeaseLineNumber.ToString());
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender1, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };
                _CommanDetails.SystemLogger.WriteTransLog(this, TcellOutstandingURL);
                _MOBILEBANKING_REQ.AccessToken = "Bearer " + _MOBILEBANKING_REQ.AccessToken;
                HttpClient clientSave = new HttpClient();
                clientSave.BaseAddress = new Uri(TcellOutstandingURL);
                clientSave.DefaultRequestHeaders.Accept.Clear();
                clientSave.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                clientSave.DefaultRequestHeaders.Add("route", "queryservice");//fixed value
                clientSave.DefaultRequestHeaders.Add("sourcenode", "PNB");//fixed value
                clientSave.DefaultRequestHeaders.Add("Authorization", _MOBILEBANKING_REQ.AccessToken);
                _CommanDetails.SystemLogger.WriteTransLog(this, "Token" + _MOBILEBANKING_REQ.AccessToken);
               // _CommanDetails.SystemLogger.WriteTransLog(this, "before send");
                 var Saveresponse = clientSave.PostAsJsonAsync(TcellOutstandingURL, _MOBILEBANKING_REQ).Result;//send req to tashicell
                //var Cust_valildation_res = "{\"request_id\":\"930081664767594496\",\"source_node\":\"PNB\",\"result_code\":\"0\",\"result_desc\":\"Success\",\"account_info\":{\"basic_details\":[{\"id\":\"account_id\",\"value\":\"1000070399\"},{\"id\":\"account_connection_type\",\"value\":\"3\"},{\"id\":\"profile_id\",\"value\":\"500001558\"},{\"id\":\"billing_title\",\"value\":\"\"},{\"id\":\"billing_name\",\"value\":\"DunningSuspension201\"},{\"id\":\"billing_name_unicode\",\"value\":\"\"},{\"id\":\"last_name\",\"value\":\"\"},{\"id\":\"language_id\",\"value\":\"1\"},{\"id\":\"preferred_currency\",\"value\":\"1022\"},{\"id\":\"billing_region\",\"value\":\"1\"},{\"id\":\"tax_code\",\"value\":\"0\"},{\"id\":\"dispatch_mode\",\"value\":\"1\"},{\"id\":\"dispatch_fax_number\",\"value\":\"\"},{\"id\":\"dunning_schedule\",\"value\":\"212\"},{\"id\":\"dunning_schedule_value\",\"value\":\"ILLHighDunning_Individual\"},{\"id\":\"risk_category\",\"value\":\"1\"},{\"id\":\"risk_category_value\",\"value\":\"High\"},{\"id\":\"email_notification\",\"value\":\"0\"},{\"id\":\"notification_email_id\",\"value\":\"\"},{\"id\":\"sms_notification\",\"value\":\"0\"},{\"id\":\"sms_numbers\",\"value\":\"\"},{\"id\":\"create_user\",\"value\":\"b19e6994-c285-4bed-8385-3e2b6f8e3325\"},{\"id\":\"modify_user\",\"value\":\"0\"},{\"id\":\"create_date\",\"value\":\"01052021000000\"},{\"id\":\"modify_date\",\"value\":\"31121999190000\"},{\"id\":\"entity_id\",\"value\":\"200\"},{\"id\":\"account_type\",\"value\":\"\"},{\"id\":\"itemized_required\",\"value\":\"0\"},{\"id\":\"tax_plan_group\",\"value\":\"1\"},{\"id\":\"tax_plan_group_value\",\"value\":\"Multipletaxes\"},{\"id\":\"bill_cycle_id\",\"value\":\"104\"},{\"id\":\"bill_cycle_id_value\",\"value\":\"Monthly\"},{\"id\":\"dunning_cancellation\",\"value\":\"0\"},{\"id\":\"dunning_enabled\",\"value\":\"0\"},{\"id\":\"summary_dispatch\",\"value\":\"2\"},{\"id\":\"itemized_dispatch\",\"value\":\"0\"},{\"id\":\"card_number\",\"value\":\"\"},{\"id\":\"bundle_id\",\"value\":\"\"},{\"id\":\"bundle_name\",\"value\":\"\"},{\"id\":\"sbb_based\",\"value\":\"N\"},{\"id\":\"profile_name\",\"value\":\"UatInvoiceTesting\"},{\"id\":\"bill_cycle_day\",\"value\":\"1\"},{\"id\":\"activation_date\",\"value\":\"01052021000000\"},{\"id\":\"first_name_ol\",\"value\":\"\"},{\"id\":\"middle_name_ol\",\"value\":\"\"},{\"id\":\"last_name_ol\",\"value\":\"\"},{\"id\":\"operator_contact_id1\",\"value\":\"\"},{\"id\":\"operator_contact_id2\",\"value\":\"\"},{\"id\":\"credit_limit\",\"value\":\"99999\"},{\"id\":\"subscriber_category\",\"value\":\"3\"},{\"id\":\"subscriber_sub_category\",\"value\":\"6\"},{\"id\":\"profile_type\",\"value\":\"1\"},{\"id\":\"grp_account_id\",\"value\":\"\"},{\"id\":\"status\",\"value\":\"1\"},{\"id\":\"social_security_number\",\"value\":\"\"},{\"id\":\"account_code\",\"value\":\"11.701\"},{\"id\":\"customer_code\",\"value\":\"10.955\"},{\"id\":\"bill_network_type\",\"value\":\"\"},{\"id\":\"temporary_credit_limit\",\"value\":\"\"},{\"id\":\"exempted_billing_region\",\"value\":\"\"},{\"id\":\"gl_posting_required\",\"value\":\"Y\"},{\"id\":\"parent_seq_id\",\"value\":\"\"},{\"id\":\"total_credit\",\"value\":\"0.000000\"},{\"id\":\"outstanding_balance\",\"value\":\"2000.000000\"},{\"id\":\"last_bill_id\",\"value\":\"517\"},{\"id\":\"last_payment_amount\",\"value\":\"2000.000000\"},{\"id\":\"last_invoice_amount\",\"value\":\"2000.000000\"},{\"id\":\"last_payment_date\",\"value\":\"05-01-202200:00:00\"},{\"id\":\"last_bill_run\",\"value\":\"01-07-202100:00:00\"},{\"id\":\"next_bill_date\",\"value\":\"01-09-202100:00:00\"},{\"id\":\"from_bill_date\",\"value\":\"01-07-202100:00:00\"},{\"id\":\"to_bill_date\",\"value\":\"31-07-202123:59:59\"},{\"id\":\"bill_date\",\"value\":\"01-08-202100:00:00\"},{\"id\":\"bill_due_date\",\"value\":\"30-08-202100:00:00\"},{\"id\":\"last_invoice_total_amount\",\"value\":\"6000.000000\"},{\"id\":\"unbilled_usage\",\"value\":\"1739.535484\"}]},\"service_info\":{\"basic_details\":[{\"id\":\"service_type_id\",\"value\":\"53971\"},{\"id\":\"account_id\",\"value\":\"1000070399\"},{\"id\":\"profile_id\",\"value\":\"500001558\"},{\"id\":\"service_code\",\"value\":\"2\"},{\"id\":\"service_code_value\",\"value\":\"ILL\"},{\"id\":\"service_name\",\"value\":\"testservice\"},{\"id\":\"rate_plan_id\",\"value\":\"109\"},{\"id\":\"rate_plan_name\",\"value\":\"Broadband_Main_Offering\"},{\"id\":\"email_notification\",\"value\":\"0\"},{\"id\":\"notification_email_id\",\"value\":\"\"},{\"id\":\"sms_notification\",\"value\":\"0\"},{\"id\":\"sms_numbers\",\"value\":\"\"},{\"id\":\"connection_type\",\"value\":\"1\"},{\"id\":\"service_id\",\"value\":\"100000708\"},{\"id\":\"activation_date\",\"value\":\"01052021000000\"},{\"id\":\"registration_date\",\"value\":\"01052021000000\"},{\"id\":\"imsi_number\",\"value\":\"0\"},{\"id\":\"sim_type\",\"value\":\"\"},{\"id\":\"credit_limit\",\"value\":\"\"},{\"id\":\"title\",\"value\":\"\"},{\"id\":\"first_name\",\"value\":\"\"},{\"id\":\"middle_name\",\"value\":\"\"},{\"id\":\"last_name\",\"value\":\"\"},{\"id\":\"contract_start_date\",\"value\":\"01052021000000\"},{\"id\":\"contract_end_date\",\"value\":\"01012000003000\"},{\"id\":\"language_id\",\"value\":\"1\"},{\"id\":\"language_name\",\"value\":\"English\"},{\"id\":\"entity_id\",\"value\":\"200\"},{\"id\":\"status\",\"value\":\"1\"},{\"id\":\"status_value\",\"value\":\"Active\"},{\"id\":\"dunning_enabled\",\"value\":\"0\"},{\"id\":\"itemized_required\",\"value\":\"1\"},{\"id\":\"bill_cycle_id\",\"value\":\"104\"},{\"id\":\"credit_class\",\"value\":\"\"},{\"id\":\"suspension_reason\",\"value\":\"1\"},{\"id\":\"create_user_id\",\"value\":\"b19e6994-c285-4bed-8385-3e2b6f8e3325\"},{\"id\":\"is_parent\",\"value\":\"0\"},{\"id\":\"parent_seq_id\",\"value\":\"\"},{\"id\":\"status_change_date\",\"value\":\"05-01-202215:10:02\"},{\"id\":\"isam_id\",\"value\":\"\"},{\"id\":\"icc_id\",\"value\":\"\"},{\"id\":\"dunning_required\",\"value\":\"1\"},{\"id\":\"remarks\",\"value\":\"\"},{\"id\":\"sales_rep_id\",\"value\":\"b19e6994-c285-4bed-8385-3e2b6f8e3325\"},{\"id\":\"sales_rep_name\",\"value\":\"admin6d\"},{\"id\":\"eai_project_key\",\"value\":\"\"},{\"id\":\"service_name_ol\",\"value\":\"\"},{\"id\":\"tax_applicable\",\"value\":\"true\"},{\"id\":\"total_usage\",\"value\":\"0.000000\"},{\"id\":\"suspension_name\",\"value\":\"\"},{\"id\":\"link_type\",\"value\":\"3\"},{\"id\":\"isp_service_connection\",\"value\":\"5\"},{\"id\":\"transmission_type\",\"value\":\"3\"},{\"id\":\"pass_code\",\"value\":\"9121\"},{\"id\":\"preferred_installation_date\",\"value\":\"null\"},{\"id\":\"date_of_birth\",\"value\":\"null\"},{\"id\":\"account_code\",\"value\":\"11.701\"},{\"id\":\"ip_number\",\"value\":\"10.0.45.40\"},{\"id\":\"vlan_number\",\"value\":\"2345678\"},{\"id\":\"ip_management\",\"value\":\"\"},{\"id\":\"vlan_management\",\"value\":\"\"},{\"id\":\"network_status\",\"value\":\"1\"},{\"id\":\"network_activation_date\",\"value\":\"05012022122059\"},{\"id\":\"bill_cycle_day\",\"value\":\"\"},{\"id\":\"short_plan_desc\",\"value\":\"\"},{\"id\":\"total_credit\",\"value\":\"0.000000\"},{\"id\":\"outstanding_balance\",\"value\":\"2000.000000\"},{\"id\":\"last_bill_id\",\"value\":\"\"},{\"id\":\"last_bill_run\",\"value\":\"01-07-202100:00:00\"},{\"id\":\"next_bill_date\",\"value\":\"01-09-202100:00:00\"},{\"id\":\"from_bill_date\",\"value\":\"01-07-202100:00:00\"},{\"id\":\"to_bill_date\",\"value\":\"31-07-202123:59:59\"},{\"id\":\"bill_date\",\"value\":\"01-08-202100:00:00\"},{\"id\":\"bill_due_date\",\"value\":\"30-08-202100:00:00\"},{\"id\":\"last_invoice_total_amount\",\"value\":\"6000.000000\"},{\"id\":\"deposit_balance\",\"value\":\"0.000000\"},{\"id\":\"unbilled_usage\",\"value\":\"1741.935484\"},{\"id\":\"last_payment_date\",\"value\":\"2022-01-05\"}]}}";
                string Cust_valildation_res = Saveresponse.Content.ReadAsStringAsync().Result;
               // _CommanDetails.SystemLogger.WriteTransLog(this, "after send");
                _CommanDetails.SystemLogger.WriteTransLog(this, "****Tcell Get Lease Line Outstanding Amount Response Recieved From Tcell For Reference Number " + Cust_valildation_res  + Environment.NewLine);
                _TCELLOUTRESP = JS.Deserialize<TASHILEASEDLINEOUTSTANDING_RESP>(Cust_valildation_res);

                if (_TCELLOUTRESP.result_code == "0")
                {
                    _MOBILEBANKING_RESP.OutstandingAmount = _TCELLOUTRESP.account_info.basic_details.First(item => item.id == "outstanding_balance").value;
                    _MOBILEBANKING_RESP.AccountId = _TCELLOUTRESP.account_info.basic_details.First(item => item.id == "account_id").value;
                    _CommanDetails.SystemLogger.WriteTransLog(this, _TCELLOUTRESP.result_code);
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.Approved);
                    _MOBILEBANKING_RESP.MSGSTAT = _MOBILEBANKING_RESP.ResponseDesc;
                    Double TotalOutstandingAmount = 0;
                    TotalOutstandingAmount += Convert.ToDouble(_MOBILEBANKING_RESP.OutstandingAmount);
                    try
                    {
                        TotalOutstandingAmount = Math.Round(TotalOutstandingAmount, 2);
                        _MOBILEBANKING_RESP.OutstandingAmount = TotalOutstandingAmount.ToString(); 
                    }
                    catch { }
                    //try
                    //{
                    //    var taskinsert = Task.Factory.StartNew(() =>
                    //    {
                    //        IMPSTransactions.INSERT_RECENTTRANSACTION(_MOBILEBANKING_REQ.ConsumerNumber, "", Convert.ToString(TotalOutstandingAmount / 10000).ToString(), _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, enumSource.TCELLLEASELINEPAYMENT.ToString());
                    //    });
                    //}
                    //catch { }
                }
                else
                {
                    if (_TCELLOUTRESP.result_code == "SC1057")
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidServiceNumber);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidServiceNumber);
                        _MOBILEBANKING_RESP.MSGSTAT = _MOBILEBANKING_RESP.ResponseDesc;
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_TCELLOUTRESP.result_code);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_TCELLOUTRESP.result_code);
                        _MOBILEBANKING_RESP.MSGSTAT = _MOBILEBANKING_RESP.ResponseDesc;
                    }
                }
                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectTcell);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectTcell);
                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void   ProcessTCellRechargeLeaseLineNEW(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType)
        {
            try
            {
                TcellPayment _TCELLPaymentReq = new TcellPayment();
                Payment_details _paymentDetail = new Payment_details();
                Order_information _OrderInfo = new Order_information();
                Payment _Payment = new Payment();
                Mode_details _modeDetail = new Mode_details();
                TcellPaymentResp _TashiResp = new TcellPaymentResp();
                string TcellPaymentURL = ConfigurationManager.AppSettings["TashiPayment_URL"].ToString();
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender1, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

                _OrderInfo.order_type = "MakePayment";

                _Payment.account_id = _MOBILEBANKING_REQ.Account_Id;
                _Payment.amount = _MOBILEBANKING_REQ.TXNAMT.ToString();
                _Payment.comment = "order payment";
                _Payment.currency_code = "1022";
                _Payment.invoice_ids = "";
                _Payment.invoice_amounts = "";
                _Payment.transaction_id = _MOBILEBANKING_REQ.ReferenceNumber;

                _paymentDetail.payment_mode = "2";
                _paymentDetail.amount_paid = _MOBILEBANKING_REQ.TXNAMT.ToString();
                _paymentDetail.reference_external_id = _MOBILEBANKING_REQ.ReferenceNumber;
                _paymentDetail.card_holder_name = "DPNB";
                _paymentDetail.beneficiary = "DPNB - CD(TICL)";
                _paymentDetail.mode_detail = new List<Mode_details>();
                _paymentDetail.mode_detail.Add(new Mode_details { key = "bank_id", value = "17" });
                _paymentDetail.mode_detail.Add(new Mode_details { key = "bank_name", value = "PNB" });

                _TCELLPaymentReq.order_information = _OrderInfo;
                _OrderInfo.payment = _Payment;
                _TCELLPaymentReq.order_information.payment.payment_detail = new List<Payment_details>();
                _TCELLPaymentReq.order_information.payment.payment_detail.Add(_paymentDetail);
               // string jsonString = JS.Serialize(_TCELLPaymentReq);

                _MOBILEBANKING_REQ.AccessToken = "Bearer " + _MOBILEBANKING_REQ.AccessToken;
                HttpClient clientSave = new HttpClient();
                clientSave.BaseAddress = new Uri(TcellPaymentURL);
                clientSave.DefaultRequestHeaders.Accept.Clear();
                clientSave.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //// Live Details
                clientSave.DefaultRequestHeaders.Add("route", "makepayment");
                clientSave.DefaultRequestHeaders.Add("sourcenode", "PNB");
                clientSave.DefaultRequestHeaders.Add("Authorization", _MOBILEBANKING_REQ.AccessToken);
                _CommanDetails.SystemLogger.WriteTransLog(this, "Token" + _MOBILEBANKING_REQ.AccessToken);
                string EmpSaveResponse = string.Empty;
                _CommanDetails.SystemLogger.WriteTransLog(this, "before send");
                var Saveresponse = clientSave.PostAsJsonAsync(TcellPaymentURL, _TCELLPaymentReq).Result;
                //var Cust_valildation_res = "{\"request_id\":\"930843289346871296\",\"source_node\":\"PNB\",\"result_code\":\"0\",\"result_desc\":\"Success\",\"message\":\"Theorderwascreatedsuccessfully\",\"dataset\":{\"param\":[{\"id\":\"order_id\",\"value\":\"930843289359454208\"},{\"id\":\"sub_order_id\",\"value\":\"930843289359454209\"}]}}";
                 string Cust_valildation_res = Saveresponse.Content.ReadAsStringAsync().Result;
                _CommanDetails.SystemLogger.WriteTransLog(this, "after send");
                _CommanDetails.SystemLogger.WriteTransLog(this, "****Tcell Get Lease Line Payment Response Recieved From Tcell For Reference Number " + Cust_valildation_res + Environment.NewLine);
                _TashiResp = JS.Deserialize<TcellPaymentResp>(Cust_valildation_res);

                if (_TashiResp.result_code == "0")
                {
                    _MOBILEBANKING_REQ.TXNID = _TashiResp.dataset.param.First(item => item.id == "order_id").value;
                    _MOBILEBANKING_REQ.TxnRRN = _TashiResp.dataset.param.First(item => item.id == "sub_order_id").value;
                    _CommanDetails.SystemLogger.WriteTransLog(this, _TashiResp.result_code);
                   // _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                   // _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.Approved);
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.TCELLSuccessLeaseLine);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.TCELLSuccessLeaseLine).Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString()).Replace("@primaryaccount", _MOBILEBANKING_REQ.LeaseLineNumber.ToString()).Replace("@MSGID", _MOBILEBANKING_REQ.ReferenceNumber);
                    _MOBILEBANKING_RESP.MSGSTAT = _MOBILEBANKING_RESP.ResponseDesc;
                }
                else
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_TashiResp.result_code.ToString());
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_TashiResp.result_desc.ToString());
                    _MOBILEBANKING_RESP.MSGSTAT = _MOBILEBANKING_RESP.ResponseDesc;
                }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectTcell);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectTcell);
                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        //added by sk
        public void ProcessTCellRechargeLeaseLine(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg)
        {
            try
            {
                MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = new MOBILEBANKING_RESP();
                string TcellRequest = string.Empty;
                TcellRequest = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ars=\"http://www.huawei.com/bme/cbsinterface/arservices\" xmlns:cbs=\"http://www.huawei.com/bme/cbsinterface/cbscommon\" xmlns:arc=\"http://cbs.huawei.com/ar/wsservice/arcommon\">"
                              + "<soapenv:Header/>"
                                 + "<soapenv:Body>"
                                   + "<ars:PaymentRequestMsg>"
                                   + "<RequestHeader>"
                                   + "<cbs:Version>" + TCELLPARAM.Version + "</cbs:Version>"
                                   + "<cbs:MessageSeq>" + _MOBILEBANKING_REQ.MSGID + "</cbs:MessageSeq>"
                                   + "<cbs:OwnershipInfo>"
                                   + "<cbs:BEID>" + TCELLPARAM.BEID + "</cbs:BEID>"
                                   + "</cbs:OwnershipInfo>"
                                   + "<cbs:AccessSecurity>"
                                   + "<cbs:LoginSystemCode>" + TCELLPARAM.LoginSystemCode + "</cbs:LoginSystemCode>"
                                   + "<cbs:Password>" + TCELLPARAM.Password + "</cbs:Password>"
                                   + "<cbs:RemoteIP>" + TCELLPARAM.RemoteIP + "</cbs:RemoteIP>"
                                   + "</cbs:AccessSecurity>"
                                   + "<cbs:OperatorInfo>"
                                   + "<cbs:OperatorID>" + TCELLPARAM.OperatorID + "</cbs:OperatorID>"
                                   + "<cbs:ChannelID>" + TCELLPARAM.ChannelID + "</cbs:ChannelID>"
                                   + "</cbs:OperatorInfo>"
                                   + "<cbs:AccessMode>" + TCELLPARAM.AccessMode + "</cbs:AccessMode>"
                                   + "<cbs:MsgLanguageCode>" + TCELLPARAM.MsgLanguageCode + "</cbs:MsgLanguageCode>"
                                   + "<cbs:TimeFormat>"
                                   + "<cbs:TimeType>" + TCELLPARAM.TimeType + "</cbs:TimeType>"
                                   + "<cbs:TimeZoneID>" + TCELLPARAM.TimeZoneID + "</cbs:TimeZoneID>"
                                   + "</cbs:TimeFormat>"
                                   + "</RequestHeader>"
                                   + "<PaymentRequest>"
                                   + "<ars:PaymentSerialNo>" + _MOBILEBANKING_REQ.MSGID + "</ars:PaymentSerialNo>"
                                   + "<ars:PaymentType>" + TCELLPARAM.RechargeType + "</ars:PaymentType>"
                                   + "<ars:OpType>" + TCELLPARAM.opType + "</ars:OpType>"
                                   + "<ars:PaymentObj>"
                                   + "<ars:AcctAccessCode>"
                                   + "<arc:PrimaryIdentity>" + _MOBILEBANKING_REQ.LeaseLineNumber + "</arc:PrimaryIdentity>"
                                   + "</ars:AcctAccessCode>"
                                   + "</ars:PaymentObj>"
                                   + "<ars:PaymentInfo>"
                                   + "<ars:CashPayment>"
                                   + "<ars:Amount>" + Convert.ToDouble(_MOBILEBANKING_REQ.TXNAMT) * 10000 + "</ars:Amount>"
                                   + "<ars:BankInfo>"
                                   + "<arc:BankCode>" + TCELLPARAM.BankCode + "</arc:BankCode>"
                                   + "<arc:BankBranchCode>" + TCELLPARAM.BankBranchCode + "</arc:BankBranchCode>"
                                   + "</ars:BankInfo>"
                                   + "</ars:CashPayment>"
                                   + "</ars:PaymentInfo>"
                                   + "</PaymentRequest>"
                                   + "</ars:PaymentRequestMsg>"
                                   + "</soapenv:Body>"
                                   + "</soapenv:Envelope>";
                string EmpResponse = string.Empty;

                HttpWebRequest request = CreateWebRequest();
                XmlDocument soapEnvelopeXml = new XmlDocument();
                soapEnvelopeXml.LoadXml(TcellRequest);
                using (Stream stream = request.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }
                FormattedXML = XDocument.Parse(TcellRequest);
                _CommanDetails.SystemLogger.WriteTransLog(this, "****Tcell Lease Line Recharge Request Send To Tcell For Reference Number " + _MOBILEBANKING_REQ.ReferenceNumber + Environment.NewLine + FormattedXML.ToString() + Environment.NewLine);
                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, null, null, null, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        EmpResponse = rd.ReadToEnd();
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(EmpResponse);
                        TCELLPARAM_RESP.ResultCode = doc.GetElementsByTagName("cbs:ResultCode")[0].InnerText;
                        TCELLPARAM_RESP.ResultDesc = doc.GetElementsByTagName("cbs:ResultDesc")[0].InnerText;
                    }

                }

                try
                {
                    FormattedXML = XDocument.Parse(EmpResponse);
                    _CommanDetails.SystemLogger.WriteTransLog(this, "****Tcell Lease Line Recharge Response Recieved From Tcell For Reference Number " + _MOBILEBANKING_REQ.ReferenceNumber + Environment.NewLine + FormattedXML.ToString());
                }
                catch (Exception ex)
                { }

                if (TCELLPARAM_RESP.ResultCode == "0")
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.TCELLSuccessLeaseLine);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.TCELLSuccessLeaseLine).Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString()).Replace("@primaryaccount", _MOBILEBANKING_REQ.LeaseLineNumber.ToString()).Replace("@MSGID", _MOBILEBANKING_REQ.ReferenceNumber);
                    _MOBILEBANKING_RESP.MSGSTAT = _MOBILEBANKING_RESP.ResponseDesc;
                }
                else
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(TCELLPARAM_RESP.ResultCode);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(TCELLPARAM_RESP.ResultCode);
                    _MOBILEBANKING_RESP.MSGSTAT = _MOBILEBANKING_RESP.ResponseDesc;
                    
                    var task = Task.Factory.StartNew(() =>
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(this, "****Reversal Generated For Tcell Lease Line Recharge For Reference Number " + _MOBILEBANKING_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine);
                        _ProcessHost.ProcessReversalToHost(RequestMsg, _MOBILEBANKING_REQ);
                    });
                }

                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, null, null, null, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectTcell);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectTcell);
                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        //public void ProcessTCellRechargeLeaseLine(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg _SwitchConsumerRequestReqMsg)
        //{
        //    MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = new MOBILEBANKING_RESP();
        //    try
        //    {
        //        string TcellRequest = string.Empty;
        //        TcellRequest = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ars=\"http://www.huawei.com/bme/cbsinterface/arservices\" xmlns:cbs=\"http://www.huawei.com/bme/cbsinterface/cbscommon\" xmlns:arc=\"http://cbs.huawei.com/ar/wsservice/arcommon\">"
        //                       + "<soapenv:Header/>"
        //                          + "<soapenv:Body>"
        //                            + "<ars:PaymentRequestMsg>"
        //                            + "<RequestHeader>"
        //                            + "<cbs:Version>" + TCELLPARAM.Version + "</cbs:Version>"
        //                            + "<cbs:MessageSeq>" + _MOBILEBANKING_REQ.MSGID + "</cbs:MessageSeq>"
        //                            + "<cbs:OwnershipInfo>"
        //                            + "<cbs:BEID>" + TCELLPARAM.BEID + "</cbs:BEID>"
        //                            + "</cbs:OwnershipInfo>"
        //                            + "<cbs:AccessSecurity>"
        //                            + "<cbs:LoginSystemCode>" + TCELLPARAM.LoginSystemCode + "</cbs:LoginSystemCode>"
        //                            + "<cbs:Password>" + TCELLPARAM.Password + "</cbs:Password>"
        //                            + "<cbs:RemoteIP>" + TCELLPARAM.RemoteIP + "</cbs:RemoteIP>"
        //                            + "</cbs:AccessSecurity>"
        //                            + "<cbs:OperatorInfo>"
        //                            + "<cbs:OperatorID>" + TCELLPARAM.OperatorID + "</cbs:OperatorID>"
        //                            + "<cbs:ChannelID>" + TCELLPARAM.ChannelID + "</cbs:ChannelID>"
        //                            + "</cbs:OperatorInfo>"
        //                            + "<cbs:AccessMode>" + TCELLPARAM.AccessMode + "</cbs:AccessMode>"
        //                            + "<cbs:MsgLanguageCode>" + TCELLPARAM.MsgLanguageCode + "</cbs:MsgLanguageCode>"
        //                            + "<cbs:TimeFormat>"
        //                            + "<cbs:TimeType>" + TCELLPARAM.TimeType + "</cbs:TimeType>"
        //                            + "<cbs:TimeZoneID>" + TCELLPARAM.TimeZoneID + "</cbs:TimeZoneID>"
        //                            + "</cbs:TimeFormat>"
        //                            + "</RequestHeader>"
        //                            + "<PaymentRequest>"
        //                            + "<ars:PaymentSerialNo>" + _MOBILEBANKING_REQ.MSGID + "</ars:PaymentSerialNo>"
        //                            + "<ars:PaymentType>" + TCELLPARAM.RechargeType + "</ars:PaymentType>"
        //                            + "<ars:OpType>" + TCELLPARAM.opType + "</ars:OpType>"
        //                            + "<ars:PaymentObj>"
        //                            + "<ars:AcctAccessCode>"
        //                            + "<arc:PrimaryIdentity>" + _MOBILEBANKING_REQ.LeaseLineNumber + "</arc:PrimaryIdentity>"
        //                            + "</ars:AcctAccessCode>"
        //                            + "</ars:PaymentObj>"
        //                            + "<ars:PaymentInfo>"
        //                            + "<ars:CashPayment>"
        //                            + "<ars:Amount>" + Convert.ToInt16(_MOBILEBANKING_REQ.TXNAMT) * 10000 + "</ars:Amount>"
        //                            + "<ars:BankInfo>"
        //                            + "<arc:BankCode>" + TCELLPARAM.BankCode + "</arc:BankCode>"
        //                            + "<arc:BankBranchCode>" + TCELLPARAM.BankBranchCode + "</arc:BankBranchCode>"
        //                            + "</ars:BankInfo>"
        //                            + "</ars:CashPayment>"
        //                            + "</ars:PaymentInfo>"
        //                            + "</PaymentRequest>"
        //                            + "</ars:PaymentRequestMsg>"
        //                            + "</soapenv:Body>"
        //                            + "</soapenv:Envelope>";
        //        string EmpResponse = string.Empty;

        //        HttpWebRequest request = CreateWebRequest();
        //        XmlDocument soapEnvelopeXml = new XmlDocument();
        //        soapEnvelopeXml.LoadXml(TcellRequest);
        //        using (Stream stream = request.GetRequestStream())
        //        {
        //            soapEnvelopeXml.Save(stream);
        //        }
        //        FormattedXML = XDocument.Parse(TcellRequest);
        //        _CommanDetails.SystemLogger.WriteTransLog(this, "****Tcell Lease Line Recharge Request Send To Tcell For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + FormattedXML.ToString() + Environment.NewLine);
        //        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
        //        using (WebResponse response = request.GetResponse())
        //        {
        //            using (StreamReader rd = new StreamReader(response.GetResponseStream()))
        //            {
        //                EmpResponse = rd.ReadToEnd();
        //                XmlDocument doc = new XmlDocument();
        //                doc.LoadXml(EmpResponse);
        //                TCELLPARAM_RESP.ResultCode = doc.GetElementsByTagName("cbs:ResultCode")[0].InnerText;
        //                TCELLPARAM_RESP.ResultDesc = doc.GetElementsByTagName("cbs:ResultDesc")[0].InnerText;
        //            }

        //        }

        //        try
        //        {
        //            FormattedXML = XDocument.Parse(EmpResponse);
        //            _CommanDetails.SystemLogger.WriteTransLog(this, "****Tcell Lease Line Recharge Response Recieved From Tcell For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + FormattedXML.ToString());
        //        }
        //        catch (Exception ex)
        //        { }

        //        if (TCELLPARAM_RESP.ResultCode == "0")
        //        {
        //            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.TCELLSuccessLeaseLine);
        //        //    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.TCELLSuccessLeaseLine).Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString()).Replace("@primaryaccount", _MOBILEBANKING_REQ.LeaseLineNumber.ToString()).Replace("@MSGID", _CREATETRANSACTION_FSFS_RES.FCUBS_BODY.TransactionDetails.FCCREF);
        //            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.TCELLSuccessLeaseLine).Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString()).Replace("@primaryaccount", _MOBILEBANKING_REQ.LeaseLineNumber.ToString()).Replace("@MSGID",_MOBILEBANKING_REQ.ReferenceNumber);
        //        }
        //        else
        //        {
        //            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(TCELLPARAM_RESP.ResultCode);
        //            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(TCELLPARAM_RESP.ResultCode);
        //            var task = Task.Factory.StartNew(() =>
        //            {
        //                _CommanDetails.SystemLogger.WriteTransLog(this, "****Reversal Generated For Tcell Lease Line Recharge For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
        //                //old code commented by sk  
        //             //_ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
        //                //added by sk
        //                _SwitchConsumerRequestReqMsg.CommandType = MaxiSwitch.API.Terminal.enumCommandTypeEnum.ReversalAdviceRequestMessage;
        //                _ProcessHost.ProcessReversalToHost(_SwitchConsumerRequestReqMsg, _MOBILEBANKING_REQ);
        //            });
        //        }

        //        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
        //    }
        //    catch (Exception ex)
        //    {
        //        _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
        //        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectTcell);
        //        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectTcell);
        //        _MOBILEBANKING_RESP.ResponseData = null;
        //    }
        //}

        public void ProcessGetListOfNPPFLoanAccount(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient)
        {
            MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = new MOBILEBANKING_RESP();
            try
            {
                string TcellRequest = string.Empty;
                string[] _Param = new string[8];
                _Param[0] = "nb54Z@5C%7778@2018";
                _Param[1] = "6";
                _Param[2] = "07";
                _Param[3] = "";
                _Param[4] = "";
                _Param[5] = "";
                _Param[6] = "";
                _Param[7] = _MOBILEBANKING_REQ.MSGID;
                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
                _CommanDetails.SystemLogger.WriteTransLog(this, "d " + _MOBILEBANKING_REQ.MSGID + " and National ID : " + _MOBILEBANKING_REQ.NationalID + Environment.NewLine + Environment.NewLine);
                string[] NppfResponse = new string[20];
                try
                {
                    //WebProxy myproxy = new WebProxy("172.30.10.222", 3128);
                    //myproxy.BypassProxyOnLocal = true;
                    //_NppfClient.Proxy = myproxy;
                    NppfResponse = _NppfClient.MakeEmiEnquiryByNationalId(_Param, _MOBILEBANKING_REQ.NationalID);

                    try
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(this, "**** Get List Of NPPF Loan Account Response Recieved From NPPF For Reference Number " + _MOBILEBANKING_REQ.MSGID + " and NPPF Unique Number is : " + NppfResponse[1].ToString() + Environment.NewLine + NppfResponse[6].ToString());
                    }
                    catch (Exception ex)
                    { }
                    if (NppfResponse[5].ToString() != ConstResponseCode.Approved)
                    {
                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidNationalID);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidNationalID);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);

                        string[] LoanDetails = NppfResponse[6].ToString().Split('\n');
                        _MOBILEBANKING_RESP.NationalID = LoanDetails[1].Split(':')[1].ToString().Replace("\r", string.Empty).Trim();
                        _MOBILEBANKING_RESP.LoanHolderName = LoanDetails[2].Split(':')[1].ToString().Replace("\r", string.Empty).Trim();
                        DataTable Loans = new DataTable();
                        Loans.TableName = "LoanDetails";
                        Loans.Columns.Add("LoanNumber");
                        Loans.Columns.Add("LoanProduct");
                        Loans.Columns.Add("EMI");
                        Loans.Columns.Add("DueDate");
                        for (int i = 4; i < LoanDetails.Count() - 4; )
                        {
                            Loans.Rows.Add(LoanDetails[i].ToString().Split(':')[1].ToString().Replace('\r', ' ').Trim(), LoanDetails[i + 1].ToString().Split(':')[1].ToString().Replace('\r', ' ').Trim(), LoanDetails[i + 2].ToString().Split(':')[1].ToString().Replace('\r', ' ').Trim(), LoanDetails[i + 3].ToString().Split(':')[1].ToString().Replace('\r', ' ').Trim());
                            i = i + 5;
                        }
                        _MOBILEBANKING_RESP.NPPFLOANAC = Loans;
                    }
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectNPPF);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectNPPF);
                    _MOBILEBANKING_RESP.ResponseData = null;
                    return;
                }
                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectTcell);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectTcell);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessGetListOfNPPFRentAccount(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient)
        {
            MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = new MOBILEBANKING_RESP();
            try
            {
                string TcellRequest = string.Empty;
                string[] _Param = new string[8];
                _Param[0] = "nb54Z@5C%7778@2018";
                _Param[1] = "6";
                _Param[2] = "08";
                _Param[3] = "";
                _Param[4] = "";
                _Param[5] = "";
                _Param[6] = "";
                _Param[7] = _MOBILEBANKING_REQ.MSGID;

                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
                _CommanDetails.SystemLogger.WriteTransLog(this, "****Get List Of NPPF Rent Account Request Send To NPPF For Reference Number " + _MOBILEBANKING_REQ.MSGID + " and National ID : " + _MOBILEBANKING_REQ.NationalID + Environment.NewLine + Environment.NewLine);
                string[] NppfResponse = new string[20];
                try
                {
                    //WebProxy myproxy = new WebProxy("172.30.10.222", 3128);
                    //myproxy.BypassProxyOnLocal = true;
                    //_NppfClient.Proxy = myproxy;
                    NppfResponse = _NppfClient.MakeRentalEnquiry(_Param, _MOBILEBANKING_REQ.NationalID);
                    try
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(this, "**** Get List Of NPPF Rent Account Response Recieved From NPPF For Reference Number " + _MOBILEBANKING_REQ.MSGID + " and NPPF Unique Number is : " + NppfResponse[1].ToString() + Environment.NewLine + NppfResponse[6].ToString());
                    }
                    catch (Exception ex)
                    { }
                    if (NppfResponse[5].ToString() != ConstResponseCode.Approved)
                    {
                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidNationalID);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidNationalID);
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);

                        string[] LoanDetails = NppfResponse[6].ToString().Split('\n');
                        _MOBILEBANKING_RESP.LoanHolderName = LoanDetails[1].Split(':')[1].ToString().Replace("\r", string.Empty).Trim();
                        _MOBILEBANKING_RESP.FlatCode = LoanDetails[3].Split(':')[1].ToString().Replace("\r", string.Empty).Trim(); ;
                        DataTable Rent = new DataTable();
                        Rent.TableName = "LoanDetails";
                        Rent.Columns.Add("Month");
                        Rent.Columns.Add("Year");
                        Rent.Columns.Add("RentAmount");
                        Rent.Columns.Add("DueDate");
                        for (int i = 5; i < LoanDetails.Count() - 5; )
                        {
                            Rent.Rows.Add(LoanDetails[i].ToString().Split(':')[1].ToString().Replace("\r", string.Empty).Trim(), LoanDetails[i + 1].ToString().Split(':')[1].ToString().Replace("\r", string.Empty).Trim(), LoanDetails[i + 2].ToString().Split(':')[1].ToString().Replace("\r", string.Empty).Trim(), LoanDetails[i + 3].ToString().Split(':')[1].ToString().Replace("\r", string.Empty).Trim());
                            i = i + 5;
                        }
                        _MOBILEBANKING_RESP.NPPFLOANAC = Rent;
                    }
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectNPPF);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectNPPF);
                    _MOBILEBANKING_RESP.ResponseData = null;
                    return;
                }
                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectTcell);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectTcell);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessNPPFLoanPayment(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient)
        {
            MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = new MOBILEBANKING_RESP();
            try
            {
                string TcellRequest = string.Empty;
                string[] _Param = new string[8];
                _Param[0] = "nb54Z@5C%7778@2018";
                _Param[1] = "6";
                _Param[2] = "13";
                _Param[3] = NullToString(_MOBILEBANKING_REQ.NPPFLoanAcc);
                _Param[4] = "002";
                _Param[5] = NullToString(_MOBILEBANKING_REQ.TXNAMT);
                _Param[6] = NullToString(_MOBILEBANKING_REQ.Remark);
                _Param[7] = NullToString(_MOBILEBANKING_REQ.MSGID);

                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
                _CommanDetails.SystemLogger.WriteTransLog(this, "**** NPPF Loan Payment Request Send To NPPF For Reference Number " + _MOBILEBANKING_REQ.MSGID + " and National ID : " + _MOBILEBANKING_REQ.NationalID + Environment.NewLine + Environment.NewLine);
                try
                {

                    string[] NppfResponse = _NppfClient.MakeLoanRepayment(_Param);

                    _CommanDetails.SystemLogger.WriteTransLog(this, "NPPF RESP Service ID         : " + NppfResponse[0].ToString());
                    _CommanDetails.SystemLogger.WriteTransLog(this, "NPPF RESP Req ID             : " + NppfResponse[1].ToString());
                    _CommanDetails.SystemLogger.WriteTransLog(this, "NPPF RESP Maximus Req Number : " + NppfResponse[2].ToString());
                    _CommanDetails.SystemLogger.WriteTransLog(this, "NPPF RESP TXN Number         : " + NppfResponse[3].ToString());
                    _CommanDetails.SystemLogger.WriteTransLog(this, "NPPF RESP TXN Date           : " + NppfResponse[4].ToString());
                    _CommanDetails.SystemLogger.WriteTransLog(this, "NPPF RESP Response Code      : " + NppfResponse[5].ToString());
                    _CommanDetails.SystemLogger.WriteTransLog(this, "NPPF RESP Resp Message       : " + NppfResponse[6].ToString());
                    _CommanDetails.SystemLogger.WriteTransLog(this, "NPPF RESP Principal paid     : " + NppfResponse[7].ToString());
                    _CommanDetails.SystemLogger.WriteTransLog(this, "NPPF RESP Interest paid      : " + NppfResponse[8].ToString());
                    _CommanDetails.SystemLogger.WriteTransLog(this, "NPPF RESP Contains Penal     : " + NppfResponse[9].ToString());
                    _CommanDetails.SystemLogger.WriteTransLog(this, "NPPF RESP total amount paid  : " + NppfResponse[10].ToString());
                    _CommanDetails.SystemLogger.WriteTransLog(this, "NPPF RESP Current balance    : " + NppfResponse[11].ToString());
                    try
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(this, "**** NPPF Loan Payment Response Recieved From NPPF For Reference Number " + _MOBILEBANKING_REQ.MSGID + " and NPPF Unique Number is : " + NppfResponse[1].ToString() + Environment.NewLine + NppfResponse[6].ToString());
                    }
                    catch (Exception ex)
                    {
                        _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    }
                    if (NppfResponse[5].ToString() != ConstResponseCode.Approved)
                    {
                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        if (NppfResponse[5].ToString() == ConstNPPFResponseCode.InvalidNationalID)
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidNationalID);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidNationalID);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.NPPFTIMEOUT);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.NPPFTIMEOUT);
                        }
                        var task = Task.Factory.StartNew(() =>
                        {
                            _CommanDetails.SystemLogger.WriteTransLog(this, "****Reversal Generated For NPPF Loan Payment For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                            _ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                        });
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.NPPFPAYMENTSUCCESS);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.NPPFPAYMENTSUCCESS).Replace("@primaryaccount", _MOBILEBANKING_REQ.NPPFLoanAcc).Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString()).Replace("@MSGID", _CREATETRANSACTION_FSFS_RES.FCUBS_BODY.TransactionDetails.FCCREF).Replace("@Balance", NppfResponse[11].ToString().Replace("Dr", string.Empty));
                    }
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectNPPF);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectNPPF);
                    _MOBILEBANKING_RESP.ResponseData = null;

                    if (ex.Message.ToString().Contains("The request was canceled"))
                    {
                        var task = Task.Factory.StartNew(() =>
                        {
                            _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For NPPF Loan Payment For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                            _ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                        });
                    }
                    else if (ex.Message.ToString().Contains("The operation has timed out"))
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.NPPFTIMEOUT);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.NPPFTIMEOUT);
                    }
                    return;
                }
                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectNPPF);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectNPPF);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessNPPFRentPayment(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient)
        {
            MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = new MOBILEBANKING_RESP();
            try
            {
                string TcellRequest = string.Empty;
                string[] _Param = new string[16];
                _Param[0] = "nb54Z@5C%7778@2018";
                _Param[1] = "6";
                _Param[2] = "22";
                _Param[3] = "";
                _Param[4] = Months[_MOBILEBANKING_REQ.RentMonth.Trim().ToUpper()].ToString();
                _Param[5] = NullToString(_MOBILEBANKING_REQ.TXNAMT);
                _Param[6] = NullToString(_MOBILEBANKING_REQ.Remark);
                _Param[7] = NullToString(_MOBILEBANKING_REQ.MSGID);
                _Param[8] = "";
                _Param[9] = NullToString(_MOBILEBANKING_REQ.NationalID);
                _Param[10] = "002";
                _Param[11] = NullToString(_MOBILEBANKING_REQ.MobileNumber);
                _Param[12] = NullToString(_MOBILEBANKING_REQ.RentYear);
                _Param[13] = NullToString(_MOBILEBANKING_REQ.FlatNumber);
                _Param[14] = "";
                _Param[15] = "";


                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
                _CommanDetails.SystemLogger.WriteTransLog(this, "**** NPPF Rent Payment Request Send To NPPF For Reference Number " + _MOBILEBANKING_REQ.MSGID + ", National ID : " + _MOBILEBANKING_REQ.NationalID + ", Rent Year : " + _Param[12].ToString() +
                                                                 ", Rent Month : " + _Param[4].ToString() + ", Flat Number : " + _MOBILEBANKING_REQ.FlatNumber + Environment.NewLine + Environment.NewLine);

                try
                {

                    string[] NppfResponse = _NppfClient.MakeRentalPayment(_Param, _MOBILEBANKING_REQ.NationalID);

                    _CommanDetails.SystemLogger.WriteTransLog(this, "NPPF RESP Service ID         : " + NppfResponse[0].ToString());
                    _CommanDetails.SystemLogger.WriteTransLog(this, "NPPF RESP Req ID             : " + NppfResponse[1].ToString());
                    _CommanDetails.SystemLogger.WriteTransLog(this, "NPPF RESP Maximus Req Number : " + NppfResponse[2].ToString());
                    _CommanDetails.SystemLogger.WriteTransLog(this, "NPPF RESP TXN Number         : " + NppfResponse[3].ToString());
                    _CommanDetails.SystemLogger.WriteTransLog(this, "NPPF RESP TXN Date           : " + NppfResponse[4].ToString());
                    _CommanDetails.SystemLogger.WriteTransLog(this, "NPPF RESP Response Code      : " + NppfResponse[5].ToString());
                    try
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(this, "**** NPPF Rent Payment Response Recieved From NPPF For Reference Number " + _MOBILEBANKING_REQ.MSGID + " and NPPF Unique Number is : " + NppfResponse[1].ToString() + Environment.NewLine + NppfResponse[6].ToString());
                    }
                    catch (Exception ex)
                    {
                        _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    }
                    if (NppfResponse[5].ToString() != ConstResponseCode.Approved)
                    {
                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        if (NppfResponse[5].ToString() == ConstNPPFResponseCode.InvalidServiceID)
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidServiceID);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidServiceID);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.NPPFTIMEOUT);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.NPPFTIMEOUT);
                        }
                        var task = Task.Factory.StartNew(() =>
                        {
                            _CommanDetails.SystemLogger.WriteTransLog(this, "****Reversal Generated For NPPF Rent Payment For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                            _ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                        });
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.NPPFRENTPAYMENTSUCCESS);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.NPPFRENTPAYMENTSUCCESS).Replace("@primaryaccount", _MOBILEBANKING_REQ.FlatNumber).Replace("@MSGID", _CREATETRANSACTION_FSFS_RES.FCUBS_BODY.TransactionDetails.FCCREF).Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString());
                    }
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectNPPF);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectNPPF);
                    _MOBILEBANKING_RESP.ResponseData = null;

                    if (ex.Message.ToString().Contains("The request was canceled"))
                    {
                        var task = Task.Factory.StartNew(() =>
                        {
                            _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For NPPF Rent Payment For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                            _ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                        });
                    }
                    else if (ex.Message.ToString().Contains("The operation has timed out"))
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.NPPFTIMEOUT);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.NPPFTIMEOUT);
                    }
                    return;
                }
                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectNPPF);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectNPPF);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessWaterOutstandingDetails(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient)
        {
            MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = new MOBILEBANKING_RESP();
            try
            {
                string TcellRequest = string.Empty;
                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
                _CommanDetails.SystemLogger.WriteTransLog(this, "****Get water outstanding details Request Send For Reference Number " + _MOBILEBANKING_REQ.MSGID + " and Water Account Number : " + _MOBILEBANKING_REQ.WaterBillNumber + Environment.NewLine + Environment.NewLine);
                string Response = string.Empty;
                try
                {
                    Response = CreateWaterWebRequest(_MOBILEBANKING_REQ.LoanAccountNumber);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("It has been logged"))
                    {
                        _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.NoOutstandingAmount);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.NoOutstandingAmount);
                        _MOBILEBANKING_RESP.ResponseData = null;
                    }
                    else
                    {
                        _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectWater);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectWater);
                        _MOBILEBANKING_RESP.ResponseData = null;
                    }
                    return;
                }
                try
                {
                    var list = JsonConvert.DeserializeObject<List<WaterResponse>>(Response);
                    try
                    {
                        ProcessMessage._WaterDetails.Add(_MOBILEBANKING_REQ.DeviceID + "_" + _MOBILEBANKING_REQ.WaterBillNumber, list);
                    }
                    catch (Exception ex)
                    {
                        _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
                    }
                    try
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(this, "**** Get water outstanding details Response Recieved For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + JToken.FromObject(Response));
                    }
                    catch (Exception ex)
                    { }
                    if (list == null || list.Count < 1)
                    {
                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidAccount);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidAccount);
                    }
                    else
                    {
                        double TotalAmount = 0;
                        double TotalPenality = 0;
                        DataTable _dtwaterdetails = new DataTable();
                        _dtwaterdetails.Columns.Add("MonthYear");
                        _dtwaterdetails.Columns.Add("SewerageCharges");
                        _dtwaterdetails.Columns.Add("WaterCharges");
                        for (int i = 0; i < list.Count; )
                        {
                            _dtwaterdetails.Rows.Add(list[i].billMonth.ToString(), list[i + 1].TotalAmount.ToString(), list[i].TotalAmount.ToString());
                            TotalAmount += Convert.ToDouble(list[i + 1].TotalAmount.ToString()) + Convert.ToDouble(list[i].TotalAmount.ToString());
                            TotalPenality += Convert.ToDouble(list[i + 1].PenaltyAmount.ToString()) + Convert.ToDouble(list[i].PenaltyAmount.ToString());
                            i = i + 2;
                        }
                        _MOBILEBANKING_RESP.TotalAmount = TotalAmount.ToString("N2");
                        _MOBILEBANKING_RESP.TotalPenalityAmount = TotalPenality.ToString("N2");
                        _MOBILEBANKING_RESP.GrandTotal = (Convert.ToDouble(TotalAmount) + Convert.ToDouble(TotalPenality)).ToString();
                        _MOBILEBANKING_RESP.WaterDetails = _dtwaterdetails;
                        _MOBILEBANKING_RESP.WaterDetails.TableName = "WaterDetails";
                        if (Convert.ToDecimal(_MOBILEBANKING_RESP.GrandTotal) > 0)
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.Approved);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.NoOutstandingAmount);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("It has been logged"))
                    {
                        _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.NoOutstandingAmount);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.NoOutstandingAmount);
                        _MOBILEBANKING_RESP.ResponseData = null;
                    }
                    else
                    {
                        _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectWater);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectWater);
                        _MOBILEBANKING_RESP.ResponseData = null;
                    }
                    return;
                }
                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectWater);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectWater);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessWaterBillPay(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient)
        {
            MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = new MOBILEBANKING_RESP();
            try
            {
                #region Get Data From Saved List
                var List = new List<WaterResponse>();
                try
                {
                    List = ProcessMessage._WaterDetails[_MOBILEBANKING_REQ.DeviceID + "_" + _MOBILEBANKING_REQ.WaterBillNumber];
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion Get Data From Saved List

                #region Save Request to water server
                try
                {
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                        {
                            return true;
                        };

                    HttpClient clientSave = new HttpClient();
                    clientSave.BaseAddress = new Uri(ConfigurationManager.AppSettings["WATER"].ToString());
                    clientSave.DefaultRequestHeaders.Accept.Clear();
                    clientSave.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    List<string> LedgerSaveID = new System.Collections.Generic.List<string>();
                    for (int i = 0; i < List.Count; )
                    {
                        LedgerSaveID.Add(List[i].LedgerEntryId);
                    }
                    WATERREQ _WATERREQSAVE = new WATERREQ
                    {
                        LedgerEntryId = LedgerSaveID,
                    };
                    #region Loger
                    try
                    {
                        string MobileResponseData = string.Empty;
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(WATERREQ));
                                _serelized.Serialize(xmlWriter, _WATERREQSAVE);
                            }
                            MobileResponseData = stringWriter.ToString();

                        }
                        XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Water Save Record Transaction Request Send to Water Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine));
                    }
                    catch (Exception ex)
                    {
                        _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    }
                    #endregion
                    string EmpSaveResponse = string.Empty;
                    var Saveresponse = clientSave.PostAsJsonAsync("/saveToMappingTableForWater/", _WATERREQSAVE).Result;
                    Saveresponse.EnsureSuccessStatusCode();
                    if (Saveresponse.IsSuccessStatusCode)
                    {
                        EmpSaveResponse = Saveresponse.Content.ReadAsStringAsync().Result;
                        try
                        {
                            _WaterResponse = JsonConvert.DeserializeObject<WaterResponse>(EmpSaveResponse);
                            #region Loger
                            try
                            {
                                string MobileResponseData = string.Empty;
                                using (var stringWriter = new StringWriter())
                                {
                                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                                    {
                                        XmlSerializer _serelized = new XmlSerializer(typeof(WaterResponse));
                                        _serelized.Serialize(xmlWriter, _WaterResponse);
                                    }
                                    MobileResponseData = stringWriter.ToString();

                                }
                                XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Water Save Record Transaction Response Recieved from Water Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                FormattedXML.ToString() + Environment.NewLine));
                            }
                            catch (Exception ex)
                            {
                                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        { }
                    }
                    else
                    {
                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToProcess);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToProcess);
                    return;
                }

                #endregion Save Request to water server

                #region Bill Pay Request

                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                System.Security.Cryptography.X509Certificates.X509Chain chain,
                System.Net.Security.SslPolicyErrors sslPolicyErrors)
                    {
                        return true;
                    };

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WATER"].ToString());
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                List<string> LedgerID = new System.Collections.Generic.List<string>();
                for (int i = 0; i < List.Count; )
                {
                    LedgerID.Add(List[i].LedgerEntryId);
                }
                WATERREQ _WATERREQ = new WATERREQ
                {
                    LedgerEntryId = LedgerID,
                    AutharizationNo = "",
                    TotalAmount = _MOBILEBANKING_REQ.TXNAMT,
                    PenaltyAmount = _MOBILEBANKING_REQ.PenaltiAmount,
                    bfs_bfsTxnId = _CREATETRANSACTION_FSFS_RES.FCUBS_BODY.TransactionDetails.FCCREF,
                    bankId = int.Parse(CONFIGURATIONCONFIGDATA.BankCode.ToString()),
                    accountNo = _MOBILEBANKING_REQ.WaterBillNumber,
                    orderNo = _WaterResponse.orderNo,
                };

                string TcellRequest = string.Empty;
                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);

                #region Loger
                try
                {
                    string MobileResponseData = string.Empty;
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(WATERREQ));
                            _serelized.Serialize(xmlWriter, _WATERREQ);
                        }
                        MobileResponseData = stringWriter.ToString();

                    }
                    XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Water Bill Payment Transaction Request Send to Water Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                    FormattedXML.ToString() + Environment.NewLine));
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion


                string EmpResponse = string.Empty;
                var response = client.PostAsJsonAsync("/updateLegerPaymentDetail/", _WATERREQ).Result;
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    EmpResponse = response.Content.ReadAsStringAsync().Result;
                    try
                    {
                        _WaterResponse = JsonConvert.DeserializeObject<WaterResponse>(EmpResponse);
                        try
                        {
                            #region Loger
                            try
                            {
                                string MobileResponseData = string.Empty;
                                using (var stringWriter = new StringWriter())
                                {
                                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                                    {
                                        XmlSerializer _serelized = new XmlSerializer(typeof(WaterResponse));
                                        _serelized.Serialize(xmlWriter, _WaterResponse);
                                    }
                                    MobileResponseData = stringWriter.ToString();

                                }
                                XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Water Bill Payment Transaction Response Recieved from Water Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                                FormattedXML.ToString() + Environment.NewLine));
                            }
                            catch (Exception ex)
                            {
                                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        { }
                    }
                    catch (Exception ex)
                    {
                        _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectNPPF);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectNPPF);
                        _MOBILEBANKING_RESP.ResponseData = null;
                        return;
                    }
                }
                else
                {
                    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToProcess);
                }
                #endregion Bill Pay Request

                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectWater);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectWater);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessTaxOutstandingDetails(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient)
        {
            MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = new MOBILEBANKING_RESP();
            try
            {
                string TcellRequest = string.Empty;
                string MobileResponseData = string.Empty;
                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
                _CommanDetails.SystemLogger.WriteTransLog(this, "****Get Tax outstanding details Request Send For Reference Number " + _MOBILEBANKING_REQ.MSGID + " and Voucher Number : " + _MOBILEBANKING_REQ.VOUCHERNUMBER + Environment.NewLine + Environment.NewLine);

                try
                {
                    //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                    //                       System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    //                       System.Security.Cryptography.X509Certificates.X509Chain chain,
                    //                       System.Net.Security.SslPolicyErrors sslPolicyErrors)
                    //{
                    //    return true;
                    //};

                    _RRCORESP = _RRCOSERVICE.getDepositVoucherDetails(_MOBILEBANKING_REQ.VOUCHERNUMBER);
                }
                catch (System.ServiceModel.FaultException<RRCO.BobFaultBean> ex)
                {

                    string Response = ex.Detail.faultCode.ToString().Trim();
                    #region Loger
                    try
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Get Tax outstanding details Transaction Response Recieved from RRCO For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + " and Response Code is : " + Response + Environment.NewLine + Environment.NewLine));
                    }
                    catch (Exception exx)
                    {
                        _CommanDetails.SystemLogger.WriteErrorLog(null, exx);
                    }
                    #endregion
                    if (Response == ConstRRCOResponseCode.VoucherNotFound)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidVoucher);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidVoucher);
                    }
                    else if (Response == ConstRRCOResponseCode.VoucherCancelled)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.VoucherCanceled);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.VoucherCanceled);
                    }
                    else if (Response == ConstRRCOResponseCode.VoucherPaidPendingReconciled)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.VoucherReconciledPending);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.VoucherReconciledPending);
                    }
                    else if (Response == ConstRRCOResponseCode.VoucherPaidReconciled)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.VoucherReconciled);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.VoucherReconciled);
                    }
                    else if (Response == ConstRRCOResponseCode.VoucherExpired)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.VoucherExpired);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.VoucherExpired);
                    }
                    else if (Response == ConstRRCOResponseCode.BadRequest)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToProcess);
                    }
                    else if (Response == ConstRRCOResponseCode.UnAthorizedAccess)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToProcess);
                    }
                    return;
                }
                try
                {
                    _MOBILEBANKING_RESP.ConsumerName = _RRCORESP.name;
                    _MOBILEBANKING_RESP.OutstandingAmount = _RRCORESP.depVouDueAmount.ToString();
                    _MOBILEBANKING_RESP.LastDate = _RRCORESP.depVouDueDate.ToString("dd-MMM-yyyy");
                    _MOBILEBANKING_RESP.VoucherNumber = _RRCORESP.depositVoucherNo;
                    _MOBILEBANKING_RESP.TPN = _RRCORESP.tpn;
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.Approved);

                    #region Loger
                    try
                    {
                        using (var stringWriter = new StringWriter())
                        {
                            using (var xmlWriter = XmlWriter.Create(stringWriter))
                            {
                                XmlSerializer _serelized = new XmlSerializer(typeof(RRCO.DepositVoucherMaster));
                                _serelized.Serialize(xmlWriter, _RRCORESP);
                            }
                            MobileResponseData = stringWriter.ToString();

                        }
                        XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Get Tax outstanding details Transaction Response Recieved from RRCO For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                        FormattedXML.ToString() + Environment.NewLine));
                    }
                    catch (Exception ex)
                    {
                        _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectRRCO);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectRRCO);
                    _MOBILEBANKING_RESP.ResponseData = null;
                    return;
                }
                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectRRCO);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectRRCO);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessTaxPayment(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient)
        {
            MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = new MOBILEBANKING_RESP();
            try
            {
                string TcellRequest = string.Empty;
                string MobileResponseData = string.Empty;
                int RRCOResponse = -1;
                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
                _CommanDetails.SystemLogger.WriteTransLog(this, "**** Tax Payment Request Send to RRCO For Reference Number " + _MOBILEBANKING_REQ.MSGID + " and Voucher Number : " + _MOBILEBANKING_REQ.VOUCHERNUMBER + Environment.NewLine + Environment.NewLine);

                try
                {
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                                           System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                           System.Security.Cryptography.X509Certificates.X509Chain chain,
                                           System.Net.Security.SslPolicyErrors sslPolicyErrors)
                    {
                        return true;
                    };

                    RRCOResponse = _RRCOSERVICE.sendPaymentStatus(_MOBILEBANKING_REQ.VOUCHERNUMBER, 1);
                }
                catch (System.ServiceModel.FaultException<RRCO.BobFaultBean> ex)
                {

                    string Response = ex.Detail.faultCode.ToString().Trim();
                    #region Loger
                    try
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Tax Payment Transaction Response Recieved from RRCO For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + " and Response Code is : " + Response + Environment.NewLine + Environment.NewLine));
                    }
                    catch (Exception exx)
                    {
                        _CommanDetails.SystemLogger.WriteErrorLog(null, exx);
                    }
                    #endregion
                    if (Response.ToString() == ConstRRCOResponseCode.VoucherNotFound)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidVoucher);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidVoucher);
                    }
                    else if (Response.ToString() == ConstRRCOResponseCode.VoucherCancelled)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.VoucherCanceled);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.VoucherCanceled);
                    }
                    else if (Response.ToString() == ConstRRCOResponseCode.VoucherPaidPendingReconciled)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.VoucherReconciledPending);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.VoucherReconciledPending);
                    }
                    else if (Response.ToString() == ConstRRCOResponseCode.VoucherPaidReconciled)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.VoucherReconciled);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.VoucherReconciled);
                    }
                    else if (Response.ToString() == ConstRRCOResponseCode.VoucherExpired)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.VoucherExpired);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.VoucherExpired);
                    }
                    else if (Response.ToString() == ConstRRCOResponseCode.BadRequest)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToProcess);
                    }
                    else if (Response.ToString() == ConstRRCOResponseCode.UnAthorizedAccess)
                    {
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToProcess);
                    }

                    //var task = Task.Factory.StartNew(() =>
                    //{
                    //    _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For RRCO Tax Payment For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                    //    _ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                    //});
                    return;
                }
                try
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.RRCOSuccess);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.RRCOSuccess).Replace("@RRN", _CREATETRANSACTION_FSFS_RES.FCUBS_BODY.TransactionDetails.FCCREF).Replace("@DV", _MOBILEBANKING_REQ.VOUCHERNUMBER).Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString("N2")).Replace("@TPN", _MOBILEBANKING_REQ.TPN).Replace("@Name", _MOBILEBANKING_REQ.ConsumerName).Replace("@Date", DateTime.Now.ToString("dd-MMM-yyyy")).Replace("\n", Environment.NewLine);

                    #region Loger
                    try
                    {
                        try
                        {
                            int status = -1;
                            DataTable DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA_FORMAIL(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, out status);
                            _CommanDetails.SystemLogger.WriteTransLog(this, "Account Number :" + _MOBILEBANKING_REQ.CUST_AC_NO);
                            _CommanDetails.SystemLogger.WriteTransLog(this, "Mobile Number  :" + _MOBILEBANKING_REQ.MobileNumber);
                            _CommanDetails.SystemLogger.WriteTransLog(this, "Varify Account From CBS :" + status);
                            if (status == 0)
                            {
                                _MOBILEBANKING_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();
                                _ProcessMessage.ProcessSendApproveTransactionRRCO(_MOBILEBANKING_REQ, _MOBILEBANKING_RESP.ResponseDesc.Replace("\n", "").Replace("\\n", ""));
                            }
                        }
                        catch { }

                        _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Tax Payment Transaction Response Recieved from RRCO For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + " and Response Code is : " + RRCOResponse + Environment.NewLine + Environment.NewLine));
                    }
                    catch (Exception ex)
                    {
                        _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectRRCO);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectRRCO);
                    _MOBILEBANKING_RESP.ResponseData = null;
                    return;
                }
                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP1, 7);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectRRCO);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectRRCO);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        ////// RICB
        public void ProcessRicbCreditInvestmentOutstandingAmount(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType)
        {
            string TcellRequest = string.Empty;
            _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
            _CommanDetails.SystemLogger.WriteTransLog(this, "****Get Ricb Credit Investment outstanding details Request Send TO RICB For Reference Number " + _MOBILEBANKING_REQ.MSGID + " and Account Number : " + _MOBILEBANKING_REQ.NationalID + Environment.NewLine + Environment.NewLine);
            string Response = string.Empty;
            try
            {
                Response = CreateRicbLoanWebRequest(_MOBILEBANKING_REQ.NationalID, "credit");

                _CommanDetails.SystemLogger.WriteTransLog(this, "**** Get Credit Investment outstanding details Response Recieved For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectRICB);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectRICB);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
            try
            {
                string ResponseCode = "-1";
                var doc = new HtmlDocument();
                doc.LoadHtml(Response);
                var inputTags = doc.DocumentNode.Descendants("input").ToList();
                foreach (var data in inputTags)
                {
                    try { if (data.GetAttributeValue("name", null).ToString() == "name")_MOBILEBANKING_RESP.LoanHolderName = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "account_no")_MOBILEBANKING_RESP.LoanAccountNumber = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "cid")_MOBILEBANKING_RESP.NationalID = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "sanction_amount")_MOBILEBANKING_RESP.SanctionAmount = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "emi")_MOBILEBANKING_RESP.OverDueAmount = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "emi")_MOBILEBANKING_RESP.EMIAMOUNT = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "department_code")_MOBILEBANKING_RESP.DepartmentCode = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                    try
                    {
                        if (data.GetAttributeValue("name", null).ToString() == "code")
                        {
                            _MOBILEBANKING_RESP.ResponseCode = data.GetAttributeValue("value", null).ToString();
                            try { _CommanDetails.SystemLogger.WriteTransLog(this, "Response Recieved from RICB with RC : " + data.GetAttributeValue("value", null).ToString() + " for reference number :" + _MOBILEBANKING_REQ.MSGID); }
                            catch { }
                        }
                    }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "serial_no")_MOBILEBANKING_RESP.SerialNumber = data.GetAttributeValue("value", null).ToString(); }
                    catch { }

                }
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectRICB);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectRICB);
                _MOBILEBANKING_RESP.ResponseData = null;
                return;
            }
            _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

        }

        public void ProcessRicbLifeInsuranceOutstandingAmount(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType)
        {
            string TcellRequest = string.Empty;
            _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
            _CommanDetails.SystemLogger.WriteTransLog(this, "****Get Ricb Life Insurance outstanding details Request Send to RICB For Reference Number " + _MOBILEBANKING_REQ.MSGID + " and Account Number : " + _MOBILEBANKING_REQ.NationalID + Environment.NewLine + Environment.NewLine);
            string Response = string.Empty;
            try
            {
                Response = CreateRicbLoanWebRequest(_MOBILEBANKING_REQ.NationalID, "life_insurance");

                _CommanDetails.SystemLogger.WriteTransLog(this, "**** Get Life Insurance outstanding details Response Recieved For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectRICB);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectRICB);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
            try
            {
                string ResponseCode = "-1";
                var doc = new HtmlDocument();
                doc.LoadHtml(Response);
                var inputTags = doc.DocumentNode.Descendants("input").ToList();
                foreach (var data in inputTags)
                {
                    try { if (data.GetAttributeValue("name", null).ToString() == "name")_MOBILEBANKING_RESP.LoanHolderName = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "account_no")_MOBILEBANKING_RESP.LoanAccountNumber = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "cid")_MOBILEBANKING_RESP.NationalID = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "sanction_amount")_MOBILEBANKING_RESP.SanctionAmount = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "premium")_MOBILEBANKING_RESP.OverDueAmount = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "emi")_MOBILEBANKING_RESP.EMIAMOUNT = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "department_code")_MOBILEBANKING_RESP.DepartmentCode = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                    try
                    {
                        if (data.GetAttributeValue("name", null).ToString() == "code")
                        {
                            _MOBILEBANKING_RESP.ResponseCode = data.GetAttributeValue("value", null).ToString();
                            try { _CommanDetails.SystemLogger.WriteTransLog(this, "Response Recieved from RICB with RC : " + data.GetAttributeValue("value", null).ToString() + " and Mode Of Txn is :" + _MOBILEBANKING_RESP.MODEOFTXN.ToString().ToUpper() + " for reference number :" + _MOBILEBANKING_REQ.MSGID); }
                            catch { }
                        }
                    }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "serial_no")_MOBILEBANKING_RESP.SerialNumber = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "mode")_MOBILEBANKING_RESP.MODEOFTXN = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                }

                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);

                if (Convert.ToInt32(_MOBILEBANKING_RESP.ResponseCode) == Convert.ToInt32(ConstResponseCode.Approved))
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.Approved);
                    try
                    {
                        DataTable dtinstallment = new DataTable();
                        dtinstallment.Columns.Add("Installment");
                        if (_MOBILEBANKING_RESP.MODEOFTXN.ToString().ToUpper() == "MONTHLY")
                        {
                            for (int i = 1; i <= 12; i++)
                            {
                                dtinstallment.Rows.Add(i);
                            }
                            _MOBILEBANKING_RESP.INSTALLMENT = dtinstallment;
                            _MOBILEBANKING_RESP.INSTALLMENT.TableName = "INSTALLMENT";
                        }
                        else if (_MOBILEBANKING_RESP.MODEOFTXN.ToString().ToUpper() == "QUARTERLY")
                        {
                            for (int i = 1; i <= 4; i++)
                            {
                                dtinstallment.Rows.Add(i);
                            }
                            _MOBILEBANKING_RESP.INSTALLMENT = dtinstallment;
                            _MOBILEBANKING_RESP.INSTALLMENT.TableName = "INSTALLMENT";
                        }
                        else if (_MOBILEBANKING_RESP.MODEOFTXN.ToString().ToUpper() == "HALF YEARLY")
                        {
                            for (int i = 1; i <= 2; i++)
                            {
                                dtinstallment.Rows.Add(i);
                            }
                            _MOBILEBANKING_RESP.INSTALLMENT = dtinstallment;
                            _MOBILEBANKING_RESP.INSTALLMENT.TableName = "INSTALLMENT";
                        }
                        else if (_MOBILEBANKING_RESP.MODEOFTXN.ToString().ToUpper() == "YEARLY")
                        {
                            for (int i = 1; i <= 1; i++)
                            {
                                dtinstallment.Rows.Add(i);
                            }
                            _MOBILEBANKING_RESP.INSTALLMENT = dtinstallment;
                            _MOBILEBANKING_RESP.INSTALLMENT.TableName = "INSTALLMENT";
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidMode);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidMode);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectRICB);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectRICB);
                        _MOBILEBANKING_RESP.ResponseData = null;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectRICB);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectRICB);
                _MOBILEBANKING_RESP.ResponseData = null;
                return;
            }
            _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

        }

        public void ProcessRicbAnnuityOutstandingAmount(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType)
        {
            string TcellRequest = string.Empty;
            _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
            _CommanDetails.SystemLogger.WriteTransLog(this, "****Get Ricb annuity outstanding details Request Send to RICB For Reference Number " + _MOBILEBANKING_REQ.MSGID + " and Account Number : " + _MOBILEBANKING_REQ.NationalID + Environment.NewLine + Environment.NewLine);
            string Response = string.Empty;
            try
            {
                Response = CreateRicbLoanWebRequest(_MOBILEBANKING_REQ.NationalID, "annuity");

                _CommanDetails.SystemLogger.WriteTransLog(this, "**** Get annuity outstanding details Response Recieved For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectRICB);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectRICB);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
            try
            {
                string ResponseCode = "-1";
                var doc = new HtmlDocument();
                doc.LoadHtml(Response);
                var inputTags = doc.DocumentNode.Descendants("input").ToList();
                foreach (var data in inputTags)
                {
                    try { if (data.GetAttributeValue("name", null).ToString() == "name")_MOBILEBANKING_RESP.LoanHolderName = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "account_no")_MOBILEBANKING_RESP.LoanAccountNumber = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "cid")_MOBILEBANKING_RESP.NationalID = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "sanction_amount")_MOBILEBANKING_RESP.SanctionAmount = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "premium")_MOBILEBANKING_RESP.OverDueAmount = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "emi")_MOBILEBANKING_RESP.EMIAMOUNT = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "department_code")_MOBILEBANKING_RESP.DepartmentCode = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                    try
                    {
                        if (data.GetAttributeValue("name", null).ToString() == "code")
                        {
                            _MOBILEBANKING_RESP.ResponseCode = data.GetAttributeValue("value", null).ToString();
                            try { _CommanDetails.SystemLogger.WriteTransLog(this, "Response Recieved from RICB with RC : " + data.GetAttributeValue("value", null).ToString() + " and Mode Of Txn is :" + _MOBILEBANKING_RESP.MODEOFTXN.ToString().ToUpper() + " for reference number :" + _MOBILEBANKING_REQ.MSGID); }
                            catch { }
                        }
                    }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "serial_no")_MOBILEBANKING_RESP.SerialNumber = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "mode")_MOBILEBANKING_RESP.MODEOFTXN = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                }
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_MOBILEBANKING_RESP.ResponseCode);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);

                if (Convert.ToInt32(_MOBILEBANKING_RESP.ResponseCode) == Convert.ToInt32(ConstResponseCode.Approved))
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.Approved);
                    try
                    {
                        DataTable dtinstallment = new DataTable();
                        dtinstallment.Columns.Add("Installment");
                        if (_MOBILEBANKING_RESP.MODEOFTXN.ToString().ToUpper() == "MONTHLY")
                        {
                            for (int i = 1; i <= 12; i++)
                            {
                                dtinstallment.Rows.Add(i);
                            }
                            _MOBILEBANKING_RESP.INSTALLMENT = dtinstallment;
                            _MOBILEBANKING_RESP.INSTALLMENT.TableName = "INSTALLMENT";
                        }
                        else if (_MOBILEBANKING_RESP.MODEOFTXN.ToString().ToUpper() == "QUATERLY")
                        {
                            for (int i = 1; i <= 4; i++)
                            {
                                dtinstallment.Rows.Add(i);
                            }
                            _MOBILEBANKING_RESP.INSTALLMENT = dtinstallment;
                            _MOBILEBANKING_RESP.INSTALLMENT.TableName = "INSTALLMENT";
                        }
                        else if (_MOBILEBANKING_RESP.MODEOFTXN.ToString().ToUpper() == "HALF YEARLY")
                        {
                            for (int i = 1; i <= 2; i++)
                            {
                                dtinstallment.Rows.Add(i);
                            }
                            _MOBILEBANKING_RESP.INSTALLMENT = dtinstallment;
                        }
                        else if (_MOBILEBANKING_RESP.MODEOFTXN.ToString().ToUpper() == "YEARLY")
                        {
                            for (int i = 1; i <= 1; i++)
                            {
                                dtinstallment.Rows.Add(i);
                            }
                            _MOBILEBANKING_RESP.INSTALLMENT = dtinstallment;
                            _MOBILEBANKING_RESP.INSTALLMENT.TableName = "INSTALLMENT";
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidMode);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidMode);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectRICB);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectRICB);
                        _MOBILEBANKING_RESP.ResponseData = null;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectRICB);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectRICB);
                _MOBILEBANKING_RESP.ResponseData = null;
                return;
            }
            _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

        }

        public void ProcessRicbLifeInusrancePayment(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType)
        {
            string TcellRequest = string.Empty;
            _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
            _CommanDetails.SystemLogger.WriteTransLog(this, "****Life Insurance Payment Request Send to RICB For Reference Number " + _MOBILEBANKING_REQ.MSGID + " and Account Number : " + _MOBILEBANKING_REQ.NationalID + Environment.NewLine + Environment.NewLine);
            string Response = string.Empty;
            try
            {
                Response = CreateRicbPaymentWebRequest(_MOBILEBANKING_REQ.SERIALNO, _MOBILEBANKING_REQ.MSGID, _MOBILEBANKING_REQ.REMITTERNAME, _MOBILEBANKING_REQ.REMITTERACC, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.Remark, "life_pay", _MOBILEBANKING_REQ.TXNAMT.ToString());

                _CommanDetails.SystemLogger.WriteTransLog(this, "**** Life Insurance Payment Response Recieved For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectRICB);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectRICB);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
            try
            {
                string ResponseCode = "-1";
                var doc = new HtmlDocument();
                doc.LoadHtml(Response);
                var inputTags = doc.DocumentNode.Descendants("input").ToList();
                foreach (var data in inputTags)
                {
                    try
                    {
                        if (data.GetAttributeValue("name", null).ToString() == "code")
                        {
                            _MOBILEBANKING_RESP.ResponseCode = data.GetAttributeValue("value", null).ToString();
                            try { _CommanDetails.SystemLogger.WriteTransLog(this, "Response Recieved from RICB with RC : " + data.GetAttributeValue("value", null).ToString() + " for reference number :" + _MOBILEBANKING_REQ.MSGID); }
                            catch { }
                        }
                    }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "display_message")_MOBILEBANKING_RESP.ResponseDesc = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                }
                if (string.IsNullOrEmpty(_MOBILEBANKING_RESP.ResponseCode))
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidLifeInsuranceAccount);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidLifeInsuranceAccount);
                }
                else
                {
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode).Replace("@AMT", _MOBILEBANKING_REQ.TXNAMT.ToString("N2"))
                                                                                                                                 .Replace("@ACC", _MOBILEBANKING_REQ.NationalID)
                                                                                                                                 .Replace("@MSGID", _CREATETRANSACTION_FSFS_RES.FCUBS_BODY.TransactionDetails.FCCREF.ToString());
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_MOBILEBANKING_RESP.ResponseCode);
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectWater);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectWater);
                _MOBILEBANKING_RESP.ResponseData = null;
                return;
            }
            _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

        }

        public void ProcessRicbLifeAnnuityPayment(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType)
        {
            string TcellRequest = string.Empty;
            _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
            _CommanDetails.SystemLogger.WriteTransLog(this, "****Life Annuity Payment Request Send For Reference Number " + _MOBILEBANKING_REQ.MSGID + " and Account Number : " + _MOBILEBANKING_REQ.NationalID + Environment.NewLine + Environment.NewLine);
            string Response = string.Empty;
            try
            {
                Response = CreateRicbPaymentWebRequest(_MOBILEBANKING_REQ.SERIALNO, _MOBILEBANKING_REQ.MSGID, _MOBILEBANKING_REQ.REMITTERNAME, _MOBILEBANKING_REQ.REMITTERACC, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.Remark, "annuity_pay", _MOBILEBANKING_REQ.TXNAMT.ToString());

                _CommanDetails.SystemLogger.WriteTransLog(this, "**** Life Annuity Payment Response Recieved For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectRICB);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectRICB);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
            try
            {
                string ResponseCode = "-1";
                var doc = new HtmlDocument();
                doc.LoadHtml(Response);
                var inputTags = doc.DocumentNode.Descendants("input").ToList();
                foreach (var data in inputTags)
                {
                    try
                    {
                        if (data.GetAttributeValue("name", null).ToString() == "code")
                        {
                            _MOBILEBANKING_RESP.ResponseCode = data.GetAttributeValue("value", null).ToString();
                            try { _CommanDetails.SystemLogger.WriteTransLog(this, "Response Recieved from RICB with RC : " + data.GetAttributeValue("value", null).ToString() + " for reference number :" + _MOBILEBANKING_REQ.MSGID); }
                            catch { }
                        }
                    }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "display_message")_MOBILEBANKING_RESP.ResponseDesc = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                }
                if (string.IsNullOrEmpty(_MOBILEBANKING_RESP.ResponseCode))
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidLifeInsuranceAccount);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidLifeInsuranceAccount);
                }
                else
                {
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode).Replace("@AMT", _MOBILEBANKING_REQ.TXNAMT.ToString("N2"))
                                                                                                                                 .Replace("@ACC", _MOBILEBANKING_REQ.NationalID)
                                                                                                                                 .Replace("@MSGID", _CREATETRANSACTION_FSFS_RES.FCUBS_BODY.TransactionDetails.FCCREF.ToString());
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_MOBILEBANKING_RESP.ResponseCode);
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectWater);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectWater);
                _MOBILEBANKING_RESP.ResponseData = null;
                return;
            }
            _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

        }

        public void ProcessRicbCreditPayment(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType)
        {
            string TcellRequest = string.Empty;
            _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
            _CommanDetails.SystemLogger.WriteTransLog(this, "**** Ricb Credit Payment Request Send to RICB For Reference Number " + _MOBILEBANKING_REQ.MSGID + " and Account Number : " + _MOBILEBANKING_REQ.NationalID + Environment.NewLine + Environment.NewLine);
            string Response = string.Empty;
            try
            {
                Response = CreateRicbPaymentWebRequest(_MOBILEBANKING_REQ.SERIALNO, _MOBILEBANKING_REQ.MSGID, _MOBILEBANKING_REQ.REMITTERNAME, _MOBILEBANKING_REQ.REMITTERACC, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.Remark, "credit_payment", _MOBILEBANKING_REQ.TXNAMT.ToString());

                _CommanDetails.SystemLogger.WriteTransLog(this, "**** Ricb Credit Payment Response Recieved For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectRICB);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectRICB);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
            try
            {
                string ResponseCode = "-1";
                var doc = new HtmlDocument();
                doc.LoadHtml(Response);
                var inputTags = doc.DocumentNode.Descendants("input").ToList();
                foreach (var data in inputTags)
                {
                    try
                    {
                        if (data.GetAttributeValue("name", null).ToString() == "code")
                        {
                            _MOBILEBANKING_RESP.ResponseCode = data.GetAttributeValue("value", null).ToString();
                            try { _CommanDetails.SystemLogger.WriteTransLog(this, "Response Recieved from RICB with RC : " + data.GetAttributeValue("value", null).ToString() + " for reference number :" + _MOBILEBANKING_REQ.MSGID); }
                            catch { }
                        }
                    }
                    catch { }
                    try { if (data.GetAttributeValue("name", null).ToString() == "display_message")_MOBILEBANKING_RESP.ResponseDesc = data.GetAttributeValue("value", null).ToString(); }
                    catch { }
                }
                if (string.IsNullOrEmpty(_MOBILEBANKING_RESP.ResponseCode))
                {
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.InvalidLifeInsuranceAccount);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.InvalidLifeInsuranceAccount);
                }
                else
                {
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode).Replace("@AMT", _MOBILEBANKING_REQ.TXNAMT.ToString("N2"))
                                                                                                                                 .Replace("@ACC", _MOBILEBANKING_REQ.NationalID)
                                                                                                                                 .Replace("@MSGID", _CREATETRANSACTION_FSFS_RES.FCUBS_BODY.TransactionDetails.FCCREF.ToString()); ;
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_MOBILEBANKING_RESP.ResponseCode);
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectRICB);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectRICB);
                _MOBILEBANKING_RESP.ResponseData = null;
                return;
            }
            _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

        }


        //////// BT PostPaid, LandLine & BroadBand
        public void ProcessBTPostpaidOutstandingAmount(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient)
        {
            try
            {
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                //    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain,
                //    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{
                //    return true;
                //};

                //_ProcessMessage.ProcessSendRemitterSMS(_MOBILEBANKING_REQ,"Hi testing","Testmail","hello");archana

                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                string strSHA512 = SHA512(_MOBILEBANKING_REQ.MSGID.Substring(4) + "-" + BTConfigurationData.SourceNumber);
                HttpClient clientSave = null;
                CreateBtPostPaidWebRequest(ref clientSave, strSHA512);
                BTPOSTPAIDDETAIL _BTPOSTPAIDDETAIL = new BTPOSTPAIDDETAIL
                {
                    OPERATION = "QUERYBILLDETAILS",
                    REQTYPE = 90,
                    REFID = _MOBILEBANKING_REQ.MSGID.Substring(4),
                    SESSIONID = _MOBILEBANKING_REQ.MSGID,
                    REQTIME = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    SRCMSISDN = BTConfigurationData.SourceNumber,
                    SERVICEID = 44,
                    SERVICENO = _MOBILEBANKING_REQ.RechargeMobileNumber,
                    INCLUDEACCOUNTDETAILS = "0",
                    INCLUDEBILLDETAILS = "1",
                    DESCRIPTION = "test",

                };
                BTPOSTPAIDREQ _BTPOSTPAIDREQ = new BTPOSTPAIDREQ
                {
                    MFS = _BTPOSTPAIDDETAIL,
                };
                #region Loger
                try
                {
                    string MobileResponseData = string.Empty;
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDREQ));
                            _serelized.Serialize(xmlWriter, _BTPOSTPAIDREQ);
                        }
                        MobileResponseData = stringWriter.ToString();

                    }
                    XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Get BT Postpaid Outstanding Amount Transaction Request Send to BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                    FormattedXML.ToString() + Environment.NewLine));
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion
                string EmpSaveResponse = string.Empty;
                var Saveresponse = clientSave.PostAsJsonAsync("/fcgi-bin/MFS_QUERYBILLDETAILS/", _BTPOSTPAIDREQ).Result;
                Saveresponse.EnsureSuccessStatusCode();
                if (Saveresponse.IsSuccessStatusCode)
                {
                    EmpSaveResponse = Saveresponse.Content.ReadAsStringAsync().Result;
                    try
                    {
                        _BTPOSTPAIDRESP = JsonConvert.DeserializeObject<BTPOSTPAIDRESPHEADER>(EmpSaveResponse);
                        if (Convert.ToInt32(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            _BTPOSTPAIDRESP.MFS.BILLDETAILS.BILLSUMMARY.BILLDETAILS = _BTPOSTPAIDRESP.MFS.BILLDETAILS.BILLSUMMARY.BILLDETAILS.OrderBy(x => Convert.ToDateTime(x.DUEDATE)).ToList();
                        }
                        #region Loger
                        try
                        {
                            string MobileResponseData = string.Empty;
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDRESPHEADER));
                                    _serelized.Serialize(xmlWriter, _BTPOSTPAIDRESP);
                                }
                                MobileResponseData = stringWriter.ToString();

                            }
                            XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Get BT Postpaid Outstanding Amount Transaction Response Recieved from BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                            FormattedXML.ToString() + Environment.NewLine));
                        }
                        catch (Exception ex)
                        {
                            _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        }
                        #endregion

                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);

                        if (Convert.ToInt32(_MOBILEBANKING_RESP.ResponseCode) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            _MOBILEBANKING_RESP.BTOUTSTANDINGDETAILS = _BTPOSTPAIDRESP;
                            _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                            _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.Approved);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                            _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        }
                    }
                    catch (Exception ex)
                    { }
                }
                else
                {
                    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToProcess);
                    return;
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectBT);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectBT);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }
        public void ProcessBTPreBroadBandOutstandingAmount(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient)
        {
            try
            {
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                //    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain,
                //    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{
                //    return true;
                //};

                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                string strSHA512 = SHA512(_MOBILEBANKING_REQ.MSGID.Substring(4) + "-" + BTConfigurationData.SourceNumber);
                HttpClient clientSave = null;
                CreateBtPostPaidWebRequest(ref clientSave, strSHA512);
                BTPOSTPAIDDETAIL _BTPOSTPAIDDETAIL = new BTPOSTPAIDDETAIL
                {
                    OPERATION = "QUERYBILLDETAILS",
                    REQTYPE = 90,
                    REFID = _MOBILEBANKING_REQ.MSGID.Substring(4),
                    SESSIONID = _MOBILEBANKING_REQ.MSGID,
                    REQTIME = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    SRCMSISDN = BTConfigurationData.SourceNumber,
                    SERVICEID = 36,
                    SERVICENO = _MOBILEBANKING_REQ.RechargeMobileNumber,
                    INCLUDEACCOUNTDETAILS = "0",
                    INCLUDEBILLDETAILS = "1",
                    DESCRIPTION = "test",

                };
                BTPOSTPAIDREQ _BTPOSTPAIDREQ = new BTPOSTPAIDREQ
                {
                    MFS = _BTPOSTPAIDDETAIL,
                };
                #region Loger
                try
                {
                    string MobileResponseData = string.Empty;
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDREQ));
                            _serelized.Serialize(xmlWriter, _BTPOSTPAIDREQ);
                        }
                        MobileResponseData = stringWriter.ToString();

                    }
                    XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Get BT Broad Band Outstanding Amount Transaction Request Send to BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                    FormattedXML.ToString() + Environment.NewLine));
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion
                string EmpSaveResponse = string.Empty;
                var Saveresponse = clientSave.PostAsJsonAsync("/fcgi-bin/MFS_QUERYBILLDETAILS/", _BTPOSTPAIDREQ).Result;

                _CommanDetails.SystemLogger.WriteTransLog(this, "1");
                _CommanDetails.SystemLogger.WriteTransLog(this, "1 ::" + Saveresponse.ToString());



                Saveresponse.EnsureSuccessStatusCode();
                if (Saveresponse.IsSuccessStatusCode)
                {

                    EmpSaveResponse = Saveresponse.Content.ReadAsStringAsync().Result;

                    _CommanDetails.SystemLogger.WriteTransLog(this, "Plan ::" + EmpSaveResponse.ToString());

                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Get BT Broad Band Outstanding Amount Transaction Response Recieved from BT Server For Reference Number : " + Environment.NewLine));
                    try
                    {
                        _BTPREPAIDRESPHEADER = JsonConvert.DeserializeObject<BTPREPAIDRESPHEADER>(EmpSaveResponse);


                        if (Convert.ToInt32(_BTPREPAIDRESPHEADER.MFS.RESPONSE.RESPCODE) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        {

                            //  _BTPREPAIDRESPHEADER.MFS.BILLDETAILS.PACKAGEDETAILS = _BTPOSTPAIDRESP.MFS.BILLDETAILS.PACKAGEDETAILS.OrderBy(x => (x.PACKAGEAMOUNT)).ToList();


                        }
                        #region Loger
                        try
                        {
                            string MobileResponseData = string.Empty;
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(BTPREPAIDRESPHEADER));
                                    _serelized.Serialize(xmlWriter, _BTPREPAIDRESPHEADER);
                                }
                                MobileResponseData = stringWriter.ToString();

                            }
                            XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Get BT Broad Band Outstanding Amount Transaction Response Recieved from BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                            FormattedXML.ToString() + Environment.NewLine));
                        }
                        catch (Exception ex)
                        {
                            _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        }
                        #endregion
                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_BTPREPAIDRESPHEADER.MFS.RESPONSE.RESPCODE);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_BTPREPAIDRESPHEADER.MFS.RESPONSE.RESPCODE);

                        if (Convert.ToInt32(_MOBILEBANKING_RESP.ResponseCode) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            _MOBILEBANKING_RESP.BTPREPAIDRESPHEADER = _BTPREPAIDRESPHEADER;
                            _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                            _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.Approved);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                            _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        }

                    }
                    catch (Exception ex)
                    {
                        _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToProcess);
                        return;
                    }
                }
                else
                {
                    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToProcess);
                    return;
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectBT);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectBT);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessBTBroadBandOutstandingAmount(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient)
        {
            try
            {
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                //    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain,
                //    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{
                //    return true;
                //};

                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                string strSHA512 = SHA512(_MOBILEBANKING_REQ.MSGID.Substring(4) + "-" + BTConfigurationData.SourceNumber);
                HttpClient clientSave = null;
                CreateBtPostPaidWebRequest(ref clientSave, strSHA512);
                BTPOSTPAIDDETAIL _BTPOSTPAIDDETAIL = new BTPOSTPAIDDETAIL
                {
                    OPERATION = "QUERYBILLDETAILS",
                    REQTYPE = 90,
                    REFID = _MOBILEBANKING_REQ.MSGID.Substring(4),
                    SESSIONID = _MOBILEBANKING_REQ.MSGID,
                    REQTIME = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    SRCMSISDN = BTConfigurationData.SourceNumber,
                    SERVICEID = 36,
                    SERVICENO = _MOBILEBANKING_REQ.RechargeMobileNumber,
                    INCLUDEACCOUNTDETAILS = "0",
                    INCLUDEBILLDETAILS = "1",
                    DESCRIPTION = "test",

                };
                BTPOSTPAIDREQ _BTPOSTPAIDREQ = new BTPOSTPAIDREQ
                {
                    MFS = _BTPOSTPAIDDETAIL,
                };
                #region Loger
                try
                {
                    string MobileResponseData = string.Empty;
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDREQ));
                            _serelized.Serialize(xmlWriter, _BTPOSTPAIDREQ);
                        }
                        MobileResponseData = stringWriter.ToString();

                    }
                    XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Get BT Broad Band Outstanding Amount Transaction Request Send to BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                    FormattedXML.ToString() + Environment.NewLine));
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion
                string EmpSaveResponse = string.Empty;
                var Saveresponse = clientSave.PostAsJsonAsync("/fcgi-bin/MFS_QUERYBILLDETAILS/", _BTPOSTPAIDREQ).Result;
                Saveresponse.EnsureSuccessStatusCode();
                if (Saveresponse.IsSuccessStatusCode)
                {
                    EmpSaveResponse = Saveresponse.Content.ReadAsStringAsync().Result;
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Get BT Broad Band Outstanding Amount Transaction Response Recieved from BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine));
                 
                    
                    try
                    {
                        _BTPOSTPAIDRESP = JsonConvert.DeserializeObject<BTPOSTPAIDRESPHEADER>(EmpSaveResponse);
                        if (Convert.ToInt32(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            _BTPOSTPAIDRESP.MFS.BILLDETAILS.BILLSUMMARY.BILLDETAILS = _BTPOSTPAIDRESP.MFS.BILLDETAILS.BILLSUMMARY.BILLDETAILS.OrderBy(x => Convert.ToDateTime(x.DUEDATE)).ToList();
                        }
                        #region Loger
                        try
                        {
                            string MobileResponseData = string.Empty;
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDRESPHEADER));
                                    _serelized.Serialize(xmlWriter, _BTPOSTPAIDRESP);
                                }
                                MobileResponseData = stringWriter.ToString();

                            }
                            XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Get BT Broad Band Outstanding Amount Transaction Response Recieved from BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                            FormattedXML.ToString() + Environment.NewLine));
                        }
                        catch (Exception ex)
                        {
                            _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        }
                        #endregion
                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);

                        if (Convert.ToInt32(_MOBILEBANKING_RESP.ResponseCode) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            _MOBILEBANKING_RESP.BTOUTSTANDINGDETAILS = _BTPOSTPAIDRESP;
                            _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                            _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.Approved);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                            _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        }

                    }
                    catch (Exception ex)
                    {
                        _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToProcess);
                        return;
                    }
                }
                else
                {
                    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToProcess);
                    return;
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectBT);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectBT);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessBTLandLineOutstandingAmount(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient)
        {
            try
            {
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                //    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain,
                //    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{
                //    return true;
                //};
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                string strSHA512 = SHA512(_MOBILEBANKING_REQ.MSGID.Substring(4) + "-" + BTConfigurationData.SourceNumber);
                HttpClient clientSave = null;
                CreateBtPostPaidWebRequest(ref clientSave, strSHA512);
                BTPOSTPAIDDETAIL _BTPOSTPAIDDETAIL = new BTPOSTPAIDDETAIL
                {
                    OPERATION = "QUERYBILLDETAILS",
                    REQTYPE = 90,
                    REFID = _MOBILEBANKING_REQ.MSGID.Substring(4),
                    SESSIONID = _MOBILEBANKING_REQ.MSGID,
                    REQTIME = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    SRCMSISDN = BTConfigurationData.SourceNumber,
                    SERVICEID = 11,
                    SERVICENO = _MOBILEBANKING_REQ.RechargeMobileNumber,
                    INCLUDEACCOUNTDETAILS = "0",
                    INCLUDEBILLDETAILS = "1",
                    DESCRIPTION = "test",

                };
                BTPOSTPAIDREQ _BTPOSTPAIDREQ = new BTPOSTPAIDREQ
                {
                    MFS = _BTPOSTPAIDDETAIL,
                };
                #region Loger
                try
                {
                    string MobileResponseData = string.Empty;
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDREQ));
                            _serelized.Serialize(xmlWriter, _BTPOSTPAIDREQ);
                        }
                        MobileResponseData = stringWriter.ToString();

                    }
                    XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Get BT Land Line Outstanding Amount Transaction Request Send to BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                    FormattedXML.ToString() + Environment.NewLine));
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion
                string EmpSaveResponse = string.Empty;
                var Saveresponse = clientSave.PostAsJsonAsync("/fcgi-bin/MFS_QUERYBILLDETAILS/", _BTPOSTPAIDREQ).Result;
                Saveresponse.EnsureSuccessStatusCode();
                if (Saveresponse.IsSuccessStatusCode)
                {
                    EmpSaveResponse = Saveresponse.Content.ReadAsStringAsync().Result;
                    try
                    {
                        _BTPOSTPAIDRESP = JsonConvert.DeserializeObject<BTPOSTPAIDRESPHEADER>(EmpSaveResponse);
                        if (Convert.ToInt32(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            _BTPOSTPAIDRESP.MFS.BILLDETAILS.BILLSUMMARY.BILLDETAILS = _BTPOSTPAIDRESP.MFS.BILLDETAILS.BILLSUMMARY.BILLDETAILS.OrderBy(x => Convert.ToDateTime(x.DUEDATE)).ToList();
                        }
                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);

                        if (Convert.ToInt32(_MOBILEBANKING_RESP.ResponseCode) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            _MOBILEBANKING_RESP.BTOUTSTANDINGDETAILS = _BTPOSTPAIDRESP;
                            _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                            _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.Approved);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                            _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        }

                        #region Loger
                        try
                        {
                            string MobileResponseData = string.Empty;
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDRESPHEADER));
                                    _serelized.Serialize(xmlWriter, _BTPOSTPAIDRESP);
                                }
                                MobileResponseData = stringWriter.ToString();

                            }
                            XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Get BT Land Line Outstanding Amount Transaction Response Recieved from BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                            FormattedXML.ToString() + Environment.NewLine));
                        }
                        catch (Exception ex)
                        {
                            _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    { }
                }
                else
                {
                    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToProcess);
                    return;
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectBT);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectBT);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessBTBroadBankPrepaidOutstandingAmount(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient)
        {
            try
            {
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                //    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain,
                //    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{
                //    return true;
                //};
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                string strSHA512 = SHA512(_MOBILEBANKING_REQ.MSGID.Substring(4) + "-" + BTConfigurationData.SourceNumber);
                HttpClient clientSave = null;
                CreateBtPostPaidWebRequest(ref clientSave, strSHA512);
                BTPOSTPAIDDETAIL _BTPOSTPAIDDETAIL = new BTPOSTPAIDDETAIL
                {
                    OPERATION = "QUERYBILLDETAILS",
                    REQTYPE = 90,
                    REFID = _MOBILEBANKING_REQ.MSGID.Substring(4),
                    SESSIONID = _MOBILEBANKING_REQ.MSGID,
                    REQTIME = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    SRCMSISDN = BTConfigurationData.SourceNumber,
                    SERVICEID = 36,
                    SERVICENO = _MOBILEBANKING_REQ.RechargeMobileNumber,
                    INCLUDEACCOUNTDETAILS = "0",
                    INCLUDEBILLDETAILS = "1",
                    DESCRIPTION = "test",

                };
                BTPOSTPAIDREQ _BTPOSTPAIDREQ = new BTPOSTPAIDREQ
                {
                    MFS = _BTPOSTPAIDDETAIL,
                };
                #region Loger
                try
                {
                    string MobileResponseData = string.Empty;
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDREQ));
                            _serelized.Serialize(xmlWriter, _BTPOSTPAIDREQ);
                        }
                        MobileResponseData = stringWriter.ToString();

                    }
                    XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Get BT Prepaid Outstanding Amount Transaction Request Send to BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                    FormattedXML.ToString() + Environment.NewLine));
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion
                string EmpSaveResponse = string.Empty;
                var Saveresponse = clientSave.PostAsJsonAsync("/fcgi-bin/MFS_QUERYBILLDETAILS/", _BTPOSTPAIDREQ).Result;
                Saveresponse.EnsureSuccessStatusCode();
                if (Saveresponse.IsSuccessStatusCode)
                {
                    EmpSaveResponse = Saveresponse.Content.ReadAsStringAsync().Result;
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format(EmpSaveResponse));
                    try
                    {
                        _BTPOSTPAIDRESP = JsonConvert.DeserializeObject<BTPOSTPAIDRESPHEADER>(EmpSaveResponse);
                        if (Convert.ToInt32(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            _BTPOSTPAIDRESP.MFS.BILLDETAILS.BILLSUMMARY.BILLDETAILS = _BTPOSTPAIDRESP.MFS.BILLDETAILS.BILLSUMMARY.BILLDETAILS.OrderBy(x => Convert.ToDateTime(x.DUEDATE)).ToList();
                        }
                        #region Loger
                        try
                        {
                            string MobileResponseData = string.Empty;
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDRESPHEADER));
                                    _serelized.Serialize(xmlWriter, _BTPOSTPAIDRESP);
                                }
                                MobileResponseData = stringWriter.ToString();

                            }
                            XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Get BT Prepaid Outstanding Amount Transaction Response Recieved from BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                            FormattedXML.ToString() + Environment.NewLine));
                        }
                        catch (Exception ex)
                        {
                            _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        }
                        #endregion

                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);

                        if (Convert.ToInt32(_MOBILEBANKING_RESP.ResponseCode) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            //_MOBILEBANKING_REQ.BILLNO=_BTPOSTPAIDRESP.MFS.BILLDETAILS.BILLSUMMARY.BILLDETAILS[0]
                            //ProcessBTBroadBandPrepaid(ref _MOBILEBANKING_RESP, _MOBILEBANKING_REQ, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                            _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        }
                    }
                    catch (Exception ex)
                    {
                        _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToProcess);
                    }
                }
                else
                {
                    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToProcess);
                    return;
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectBT);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectBT);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessBTLeaseLineOutstandingAmount(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient)
        {
            try
            {
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                //    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain,
                //    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{
                //    return true;
                //};
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                _CommanDetails.SystemLogger.WriteTransLog(this, "1 :" );

                //string strSHA512 = SHA512(_MOBILEBANKING_REQ.MSGID.Substring(4) + "-" + BTConfigurationData.SourceNumber);

                string strSHA512 = SHA512(_MOBILEBANKING_REQ.MSGID.Substring(4) + "-" + BTConfigurationData.SourceNumberLeaseLine );

                _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("strSHA512 " + strSHA512));
                HttpClient clientSave = null;

                CreateBtLeaseLineWebRequest(ref clientSave, strSHA512);

                _CommanDetails.SystemLogger.WriteTransLog(this, "2 :");

                BTPOSTPAIDDETAIL _BTPOSTPAIDDETAIL = new BTPOSTPAIDDETAIL
                {
                    SRCMSISDN = BTConfigurationData.SourceNumberLeaseLine,
                    DESCRIPTION = "Query request",
                    SERVICENO = _MOBILEBANKING_REQ.LeaseLineNumber,
                    REQTYPE = 90,
                    SERVICEID = 37,
                    REQTIME = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    INCLUDEBILLDETAILS = "1",
                    OPERATION = "QUERYBILLDETAILS",
                    INCLUDEACCOUNTDETAILS = "1",
                    REFID = _MOBILEBANKING_REQ.MSGID.Substring(4),
                    //SESSIONID = _MOBILEBANKING_REQ.MSGID,
               
                };
                _CommanDetails.SystemLogger.WriteTransLog(this, "3 :");
                BTPOSTPAIDREQ _BTPOSTPAIDREQ = new BTPOSTPAIDREQ
                {
                    MFS = _BTPOSTPAIDDETAIL,
                };
                _CommanDetails.SystemLogger.WriteTransLog(this, "4 :");

                #region Loger
                try
                {
                    string MobileResponseData = string.Empty;
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDREQ));
                            _serelized.Serialize(xmlWriter, _BTPOSTPAIDREQ);
                        }
                        MobileResponseData = stringWriter.ToString();

                    }
                    XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Get BT LeaseLine Outstanding Amount Transaction Request Send to BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                    FormattedXML.ToString() + Environment.NewLine));
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                string EmpSaveResponse = string.Empty;
              
                var Saveresponse = clientSave.PostAsJsonAsync("/fcgi-bin/MFS_QUERYBILLDETAILS/", _BTPOSTPAIDREQ).Result;
                _CommanDetails.SystemLogger.WriteTransLog(this, "5 :");
                Saveresponse.EnsureSuccessStatusCode();
                _CommanDetails.SystemLogger.WriteTransLog(this, "6 :");
                if (Saveresponse.IsSuccessStatusCode)
                {
                    EmpSaveResponse = Saveresponse.Content.ReadAsStringAsync().Result;
                    try
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(this, "7 :");
                        _BTPOSTPAIDRESP = JsonConvert.DeserializeObject<BTPOSTPAIDRESPHEADER>(EmpSaveResponse);
                        if (Convert.ToInt32(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            _BTPOSTPAIDRESP.MFS.BILLDETAILS.BILLSUMMARY.BILLDETAILS = _BTPOSTPAIDRESP.MFS.BILLDETAILS.BILLSUMMARY.BILLDETAILS.OrderBy(x => Convert.ToDateTime(x.DUEDATE)).ToList();
                        }
                        #region Loger
                        try
                        {
                            string MobileResponseData = string.Empty;
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDRESPHEADER));
                                    _serelized.Serialize(xmlWriter, _BTPOSTPAIDRESP);
                                }
                                MobileResponseData = stringWriter.ToString();

                            }
                            XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Get BT LeaseLine Outstanding Amount Transaction Response Recieved from BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                            FormattedXML.ToString() + Environment.NewLine));
                        }
                        catch (Exception ex)
                        {
                            _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        }
                        #endregion

                        _CommanDetails.SystemLogger.WriteTransLog(this, "8 :");

                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.LeaseLineNumber;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);

                        if (Convert.ToInt32(_MOBILEBANKING_RESP.ResponseCode) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            _MOBILEBANKING_RESP.BTOUTSTANDINGDETAILS = _BTPOSTPAIDRESP;
                            _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                            _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.LeaseLineNumber;
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.Approved);
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                            _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        }
                    }
                    catch (Exception ex)
                    { }
                }
                else
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, "9 :");
                    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.LeaseLineNumber;
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToProcess);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToProcess);
                    return;
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "10 :");
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.UnableToConnectBT);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.UnableToConnectBT);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessBTPostpaidPayment(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, MaxiSwitch.API.Terminal.SwitchConsumerRequestReqMsg RequestMsg)
        {
            try
            {
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                //    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain,
                //    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{
                //    return true;
                //};
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                int result = -1;
                string Amount = string.Empty;
                if (int.TryParse(_MOBILEBANKING_REQ.TXNAMT.ToString(), out result))
                {
                    Amount = Convert.ToString(Convert.ToInt32(_MOBILEBANKING_REQ.TXNAMT.ToString()));
                }
                else
                {
                    Amount = Convert.ToString(_MOBILEBANKING_REQ.TXNAMT);
                }
                string strSHA512 = SHA512(_MOBILEBANKING_REQ.MSGID.Substring(4) + "-" + BTConfigurationData.SourceNumber + "-" + Amount);
                HttpClient clientSave = null;
                CreateBtPostPaidWebRequest(ref clientSave, strSHA512);
                BTPOSTPAIDDETAIL _BTPOSTPAIDDETAIL = new BTPOSTPAIDDETAIL
                {
                    OPERATION = "TRANSFER",
                    REQTYPE = 20,
                    REFID = _MOBILEBANKING_REQ.MSGID.Substring(4),
                    SESSIONID = _MOBILEBANKING_REQ.MSGID,
                    REQTIME = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    SRCMSISDN = BTConfigurationData.SourceNumber,
                    SERVICEID = 44,
                    AMOUNT = Amount,
                    SRCMPIN ="",
                    //SRCMPIN = BTConfigurationData.TPIN,
                    SERVICENO = _MOBILEBANKING_REQ.RechargeMobileNumber,
                    TRANSFERTYPE = "3",
                    SERVICEDETAILS = new SERVICEDETAILSPARAM { SERVICEREFID = "", SERVICENO = _MOBILEBANKING_REQ.RechargeMobileNumber, ACCOUNTNO = "", BILLNO = _MOBILEBANKING_REQ.BILLNO, USID = _MOBILEBANKING_REQ.USID, PPTYPE = "" },
                    COMMENT = _MOBILEBANKING_REQ.Remark,
                    DESCRIPTION = "Payment",
                    DESTNOTIFYNO = "",
                    DESTNOTIFYLAN = ""
                };
                BTPOSTPAIDREQ _BTPOSTPAIDREQ = new BTPOSTPAIDREQ
                {
                    MFS = _BTPOSTPAIDDETAIL,
                };

                #region Loger
                try
                {
                    string MobileResponseData = string.Empty;
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDREQ));
                            _serelized.Serialize(xmlWriter, _BTPOSTPAIDREQ);
                        }
                        MobileResponseData = stringWriter.ToString();

                    }
                    XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Postpaid Payment Transaction Request Send to BT Server For Reference Number : " + _MOBILEBANKING_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                    FormattedXML.ToString() + Environment.NewLine));
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, null, null, null, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                       
                string EmpSaveResponse = string.Empty;
                var Saveresponse = clientSave.PostAsJsonAsync("/fcgi-bin/MFS_TRANSFER/", _BTPOSTPAIDREQ).Result;
                Saveresponse.EnsureSuccessStatusCode();
                if (Saveresponse.IsSuccessStatusCode)
                {
                    EmpSaveResponse = Saveresponse.Content.ReadAsStringAsync().Result;
                    try
                    {
                        _BTPOSTPAIDRESP = JsonConvert.DeserializeObject<BTPOSTPAIDRESPHEADER>(EmpSaveResponse);
                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                        _MOBILEBANKING_REQ.TXNID = _BTPOSTPAIDRESP.MFS.TXID;
                        #region Loger
                        try
                        {
                            string MobileResponseData = string.Empty;
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDRESPHEADER));
                                    _serelized.Serialize(xmlWriter, _BTPOSTPAIDRESP);
                                }
                                MobileResponseData = stringWriter.ToString();

                            }
                            XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Postpaid Payment Transaction Response Recieved from BT Server For Reference Number : " + _MOBILEBANKING_REQ.ReferenceNumber + Environment.NewLine + Environment.NewLine +
                                                                            FormattedXML.ToString() + Environment.NewLine));
                        }
                        catch (Exception ex)
                        {
                            _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        }
                        #endregion
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _CommanDetails.SystemLogger.WriteTransLog(this, "**** postpaid payment response code :  " + _BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE + "    " + _MOBILEBANKING_RESP.ResponseCode + Environment.NewLine);
                      
                        _MOBILEBANKING_RESP.BTStatusCode = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE.ToString();
                        _MOBILEBANKING_RESP.BTTransStatus = _BTPOSTPAIDRESP.MFS.RESPTYPE.ToString();
                        _MOBILEBANKING_RESP.BTStatusDiscription = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPDESC.ToString();

                     

                        if (Convert.ToInt32(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            _MOBILEBANKING_RESP.TXREFID = _BTPOSTPAIDRESP.MFS.REFID;
                            ProcessBTPostpaidPaymentStatus(ref _MOBILEBANKING_RESP, _MOBILEBANKING_REQ, null, null, null, null, ref _BTPOSTPAIDRESP);
                        }

                        else if (Convert.ToInt32(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE) != Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            string _Action = CommanDetails.GetMFSResponseCodeHost(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                            if (_Action == "1")
                            {
                                MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = _MOBILEBANKING_RESP;
                                var task = Task.Factory.StartNew(() =>
                                {
                                    _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For BT POSTPAID For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                                    // _ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                                    if (CONFIGURATIONCONFIGDATA.ISBTreversal)
                                    {
                                        _ProcessHost.ProcessReversalTransactionToHost(ref _MOBILEBANKING_RESP1, _MOBILEBANKING_REQ);
                                    }
                                });
                            }
                            //System.Threading.Thread.Sleep(8000);
                            if (_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE == "2144")
                            {
                                _MOBILEBANKING_RESP.ResponseCode = "00";
                                _MOBILEBANKING_RESP.ResponseDesc = "Your transaction is pendding.Kindly contact BT at 1600 for support.Sorry for inconvenience caused.";
                            }
                            else
                            {
                                _MOBILEBANKING_RESP.ResponseCode = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE;
                                _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused.";
                            }
                         
                        }
                      
                        //else
                        //{
                        //    var task = Task.Factory.StartNew(() =>
                        //    {
                        //        //_CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For BT Postpaid Recharge For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                        //        //_ProcessHost.ProcessReversalToHost(RequestMsg, _MOBILEBANKING_REQ);
                        //    });
                        //}
                    }

                    catch (Exception ex)
                    { }
                    _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, null, null, null, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                }
                else
                {
                    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                    _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                    return;
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                _MOBILEBANKING_RESP.MSGSTAT = "FAILURE";
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessBTPostpaidPaymentStatus(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, ref BTPOSTPAIDRESPHEADER _BTPOSTPAIDRESPHEADER)
        {
            try
            {
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                //    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain,
                //    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{
                //    return true;
                //};
                _CommanDetails.SystemLogger.WriteTransLog(this, "Transaction send for postpaid payment status : " );
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                _CommanDetails.SystemLogger.WriteTransLog(this, "_MOBILEBANKING_REQ.MSGID.Substring(4) : " + _MOBILEBANKING_REQ.MSGID.Substring(4));
                _CommanDetails.SystemLogger.WriteTransLog(this, "_MOBILEBANKING_RESP.TXREFID : " + _MOBILEBANKING_RESP.TXREFID);
               

                string strSHA512 = SHA512(_MOBILEBANKING_REQ.MSGID.Substring(4) + "-" + _MOBILEBANKING_RESP.TXREFID);
                _CommanDetails.SystemLogger.WriteTransLog(this, "strSHA512 : " + strSHA512);
                HttpClient clientSave = null;
                CreateBtPostPaidWebRequest(ref clientSave, strSHA512);
                BTPOSTPAIDDETAIL _BTPOSTPAIDDETAIL = new BTPOSTPAIDDETAIL
                {
                    OPERATION = "CHECKSTATUS",
                    REQTYPE = 30,
                    REFID = _MOBILEBANKING_REQ.MSGID.Substring(4),
                    REQTIME = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    SRCMSISDN = BTConfigurationData.SourceNumber,
                    TXREFID = _MOBILEBANKING_RESP.TXREFID,
                   // USID = _MOBILEBANKING_REQ.USID,  //archana 
                    DESCRIPTION = "test",
                };
                BTPOSTPAIDREQ _BTPOSTPAIDREQ = new BTPOSTPAIDREQ
                {
                    MFS = _BTPOSTPAIDDETAIL,
                };
                #region Loger
                try
                {
                    string MobileResponseData = string.Empty;
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDREQ));
                            _serelized.Serialize(xmlWriter, _BTPOSTPAIDREQ);
                        }
                        MobileResponseData = stringWriter.ToString();

                    }
                    XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Postpaid Payment Status Transaction Request Send to BT Server For Reference Number : " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine +
                                                                    FormattedXML.ToString() + Environment.NewLine));
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion


                string EmpSaveResponse = string.Empty;
                var Saveresponse = clientSave.PostAsJsonAsync("/fcgi-bin/MFS_CHECKSTATUS/", _BTPOSTPAIDREQ).Result;
                Saveresponse.EnsureSuccessStatusCode();
                if (Saveresponse.IsSuccessStatusCode)
                {
                    EmpSaveResponse = Saveresponse.Content.ReadAsStringAsync().Result;
                    try
                    {
                        _BTPOSTPAIDRESPHEADER = JsonConvert.DeserializeObject<BTPOSTPAIDRESPHEADER>(EmpSaveResponse);

                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_BTPOSTPAIDRESPHEADER.MFS.RESPONSE.RESPCODE);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_BTPOSTPAIDRESPHEADER.MFS.RESPONSE.RESPCODE);
                        _CommanDetails.SystemLogger.WriteTransLog(this, "Status Check : " + _MOBILEBANKING_RESP.ResponseCode + "   " + _MOBILEBANKING_RESP.ResponseDesc);

                        _MOBILEBANKING_RESP.BTStatusCode = _BTPOSTPAIDRESPHEADER.MFS.RESPONSE.RESPCODE.ToString();
                        _MOBILEBANKING_RESP.BTTransStatus = _BTPOSTPAIDRESPHEADER.MFS.RESPTYPE.ToString();
                        _MOBILEBANKING_RESP.BTStatusDiscription = _BTPOSTPAIDRESPHEADER.MFS.RESPONSE.RESPDESC.ToString();
                       
                        
                        if (Convert.ToInt32(_MOBILEBANKING_RESP.ResponseCode) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            _CommanDetails.SystemLogger.WriteTransLog(this, "Convert.ToInt32(ConstResponseCode.BTApproved)" + Convert.ToInt32(ConstResponseCode.BTApproved));
                            _MOBILEBANKING_RESP.BTOUTSTANDINGDETAILS = _BTPOSTPAIDRESP;
                            _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                            _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BTPostPaidSuccess);
                            //_MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BTPostPaidSuccess).Replace("@Mobile", _MOBILEBANKING_REQ.RechargeMobileNumber).Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString("N2")).Replace("@RRN", _CREATETRANSACTION_FSFS_RES.FCUBS_BODY.TransactionDetails.FCCREF); 
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BTPostPaidSuccess).Replace("@Mobile", _MOBILEBANKING_REQ.RechargeMobileNumber).Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString("N2")).Replace("@RRN", _MOBILEBANKING_REQ.ReferenceNumber); 
                            
                            try
                            {
                                //_CommanDetails.SystemLogger.WriteTransLog(this, "Convert.ToInt32(ConstResponseCode.BTApproved try)" + Convert.ToInt32(ConstResponseCode.BTApproved));
                           
                                //int status = -1;
                                //DataTable DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA_FORMAIL(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, out status);
                                //_CommanDetails.SystemLogger.WriteTransLog(this, "Account Number :" + _MOBILEBANKING_REQ.CUST_AC_NO);
                                //_CommanDetails.SystemLogger.WriteTransLog(this, "Mobile Number  :" + _MOBILEBANKING_REQ.MobileNumber);
                                //_CommanDetails.SystemLogger.WriteTransLog(this, "Varify Account From CBS :" + status);
                                //if (status == 0)
                                //{
                                //    _MOBILEBANKING_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();
                                //    _ProcessMessage.ProcessSendApproveTransaction(_MOBILEBANKING_REQ, _MOBILEBANKING_RESP.ResponseDesc);
                                //}
                            }
                            catch { }
                        }

                        else if (Convert.ToInt32(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE) != Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            string _Action = CommanDetails.GetMFSResponseCodeHost(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                            if (_Action == "1")
                            {
                                MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = _MOBILEBANKING_RESP;
                                var task = Task.Factory.StartNew(() =>
                                {
                                    _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For BT POSTPAID For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                                    // _ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                                    if (CONFIGURATIONCONFIGDATA.ISBTreversal)
                                    {
                                        _ProcessHost.ProcessReversalTransactionToHost(ref _MOBILEBANKING_RESP1, _MOBILEBANKING_REQ);
                                    }
                                });
                            }
                            //System.Threading.Thread.Sleep(8000); if (_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE == "2144")

                            if (_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE == "2144")
                            {
                                _MOBILEBANKING_RESP.ResponseCode = "00";
                                _MOBILEBANKING_RESP.ResponseDesc = "Your transaction is pendding.Kindly contact BT at 1600 for support.Sorry for inconvenience caused.";
                            }
                            else
                            {
                            _MOBILEBANKING_RESP.ResponseCode = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE;
                            _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused.";
                            }
                                //  _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, null, null, null, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        }
                        //else
                        //{
                        //    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        //    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        //    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        //    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        //    var task = Task.Factory.StartNew(() =>
                        //    {
                        //        _CommanDetails.SystemLogger.WriteTransLog(this, "Status Check : else part if status is not success " );
                        //    });
                        //}

                      
                       // _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, null, null, null, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                     
                        #region Loger
                        try
                        {
                            string MobileResponseData = string.Empty;
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDRESPHEADER));
                                    _serelized.Serialize(xmlWriter, _BTPOSTPAIDRESPHEADER);
                                }
                                MobileResponseData = stringWriter.ToString();

                            }
                            XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Postpaid Payment Status Transaction Response Recieved from BT Server For Reference Number : " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine +
                                                                            FormattedXML.ToString() + Environment.NewLine));
                        }
                        catch (Exception ex)
                        {
                            _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    { }
                }
                else
                {
                    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                    _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused";//CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                    return;
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused";//CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessBTLeaseLinePayment(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient)
        {
            try
            {
            //    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
            //        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            //        System.Security.Cryptography.X509Certificates.X509Chain chain,
            //        System.Net.Security.SslPolicyErrors sslPolicyErrors)
            //    {
            //        return true;
            //    };
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                int result = -1;
                string Amount = string.Empty;
                if (int.TryParse(_MOBILEBANKING_REQ.TXNAMT.ToString(), out result))
                {
                    Amount = Convert.ToString(Convert.ToInt32(_MOBILEBANKING_REQ.TXNAMT.ToString()));
                }
                else
                {
                    Amount = Convert.ToString(_MOBILEBANKING_REQ.TXNAMT);
                }
                string strSHA512 = SHA512(_MOBILEBANKING_REQ.MSGID.Substring(4) + "-" + BTConfigurationData.SourceNumberLeaseLine + "-" + Amount);

                HttpClient clientSave = null;
                CreateBtLeaseLineWebRequest(ref clientSave, strSHA512);

                BTPOSTPAIDDETAIL _BTPOSTPAIDDETAIL = new BTPOSTPAIDDETAIL
                {

                    OPERATION = "TRANSFER",
                    REQTYPE = 20,
                    REFID = _MOBILEBANKING_REQ.MSGID.Substring(4),
                    //SESSIONID = _MOBILEBANKING_REQ.MSGID,
                    REQTIME = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    SERVICEID = 37,
                    AMOUNT = Amount,
                    SRCMSISDN = BTConfigurationData.SourceNumberLeaseLine,
                    //SRCMPIN = BTConfigurationData.LeasePIN,
                    SRCMPIN ="",
                    TRANSFERTYPE = "3",
                    SERVICEDETAILS = new SERVICEDETAILSPARAM { SERVICEREFID = "", SERVICENO = "", ACCOUNTNO = _MOBILEBANKING_REQ.LeaseLineNumber, BILLNO = _MOBILEBANKING_REQ.BILLNO, USID = _MOBILEBANKING_REQ.USID, PPTYPE = "" },
                    COMMENT = _MOBILEBANKING_REQ.Remark,
                    DESCRIPTION = "TRANSFER Test",
                    DESTNOTIFYNO = "",
                    DESTNOTIFYLAN = ""

                };
                BTPOSTPAIDREQ _BTPOSTPAIDREQ = new BTPOSTPAIDREQ
                {
                    MFS = _BTPOSTPAIDDETAIL,
                };
                #region Loger
                try
                {
                    string MobileResponseData = string.Empty;
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDREQ));
                            _serelized.Serialize(xmlWriter, _BTPOSTPAIDREQ);
                        }
                        MobileResponseData = stringWriter.ToString();

                    }
                    XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT LeaseLine Payment Transaction Request Send to BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                    FormattedXML.ToString() + Environment.NewLine));
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion
                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, null, null, null, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                  
                string EmpSaveResponse = string.Empty;
                var Saveresponse = clientSave.PostAsJsonAsync("/fcgi-bin/MFS_TRANSFER/", _BTPOSTPAIDREQ).Result;
                Saveresponse.EnsureSuccessStatusCode();
                if (Saveresponse.IsSuccessStatusCode)
                {
                    EmpSaveResponse = Saveresponse.Content.ReadAsStringAsync().Result;
                    try
                    {
                        _BTPOSTPAIDRESP = JsonConvert.DeserializeObject<BTPOSTPAIDRESPHEADER>(EmpSaveResponse);

                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.LeaseLineNumber;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                        _MOBILEBANKING_REQ.TXNID = _BTPOSTPAIDRESP.MFS.TXID;

                        _MOBILEBANKING_RESP.BTStatusCode = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE.ToString();
                        _MOBILEBANKING_RESP.BTTransStatus = _BTPOSTPAIDRESP.MFS.RESPTYPE.ToString();
                        _MOBILEBANKING_RESP.BTStatusDiscription = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPDESC.ToString();
                        #region Loger
                        try
                        {
                            string MobileResponseData = string.Empty;
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDRESPHEADER));
                                    _serelized.Serialize(xmlWriter, _BTPOSTPAIDRESP);
                                }
                                MobileResponseData = stringWriter.ToString();

                            }
                            XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT LeaseLine Payment Transaction Response Recieved from BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                            FormattedXML.ToString() + Environment.NewLine));
                        }
                        catch (Exception ex)
                        {
                            _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        }
                        #endregion

                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;

                        if (Convert.ToInt32(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            _MOBILEBANKING_RESP.TXREFID = _BTPOSTPAIDRESP.MFS.REFID;
                            ProcessBTLeaseLinePaymentStatus(ref _MOBILEBANKING_RESP, _MOBILEBANKING_REQ, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, ref _BTPOSTPAIDRESP);
                        }
                        else if (Convert.ToInt32(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE) != Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            string _Action = CommanDetails.GetMFSResponseCodeHost(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                            if (_Action == "1")
                            {
                                MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = _MOBILEBANKING_RESP;
                                var task = Task.Factory.StartNew(() =>
                                {
                                    _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For BT POSTPAID For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                                    // _ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                                    if (CONFIGURATIONCONFIGDATA.ISBTreversal)
                                    {
                                        _ProcessHost.ProcessReversalTransactionToHost(ref _MOBILEBANKING_RESP1, _MOBILEBANKING_REQ);
                                    }
                                });
                            }
                            //System.Threading.Thread.Sleep(8000);
                            if (_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE == "2144")
                            {
                                _MOBILEBANKING_RESP.ResponseCode = "00";
                                _MOBILEBANKING_RESP.ResponseDesc = "Your transaction is pendding.Kindly contact BT at 1600 for support.Sorry for inconvenience caused.";
                            }
                            else
                            {
                                _MOBILEBANKING_RESP.ResponseCode = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE;
                                _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused.";
                            }
                            _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, null, null, null, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        }
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, null, null, null, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    }
                    catch (Exception ex)
                    { }
                }
                else
                {
                    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.LeaseLineNumber;
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                    _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused";//CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                    return;
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused"; CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessBTCashInPayment(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient)
        {
            try
            {
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                //    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain,
                //    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{
                //    return true;
                //};
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                int result = -1;
                string Amount = string.Empty;
                if (int.TryParse(_MOBILEBANKING_REQ.TXNAMT.ToString(), out result))
                {
                    Amount = Convert.ToString(Convert.ToInt32(_MOBILEBANKING_REQ.TXNAMT.ToString()));
                }
                else
                {
                    Amount = Convert.ToString(_MOBILEBANKING_REQ.TXNAMT);
                }
                string strSHA512 = SHA512(_MOBILEBANKING_REQ.MSGID.Substring(4) + "-" + BTConfigurationData.SourceNumberBngul + "-" + Amount);
                HttpClient clientSave = null;
                CreateBNgulWebRequest(ref clientSave, strSHA512);

                BTPOSTPAIDDETAIL _BTPOSTPAIDDETAIL = new BTPOSTPAIDDETAIL
                {

                    OPERATION = "TRANSFER",
                    REQTYPE = 20,
                    REFID = _MOBILEBANKING_REQ.MSGID.Substring(4),
                    REQTIME = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    SERVICEID = 33,
                    AMOUNT = Amount,
                    SRCMSISDN = BTConfigurationData.SourceNumberBngul,
                    SRCMPIN = BTConfigurationData.BngulPIN,
                    TRANSFERTYPE = "3",

                    SERVICEDETAILS = new SERVICEDETAILSPARAM { SERVICEREFID = "", SERVICENO = _MOBILEBANKING_REQ.MobileNumber, ACCOUNTNO = "", BILLNO = "", USID = "", PPTYPE = "", SERVICETYPE = "1" },
                    WALLETDETAILS = new WALLETDETAILS { WALLETTYPE = "", DESTMSISDN = _MOBILEBANKING_REQ.MobileNumber, DESTMPIN = "", TRANSTYPE = "" },

                    COMMENT = _MOBILEBANKING_REQ.Remark,
                    DESCRIPTION = "TRANSFER Test",
                    DESTNOTIFYNO = "",
                    DESTNOTIFYLAN = ""

                };
                BTPOSTPAIDREQ _BTPOSTPAIDREQ = new BTPOSTPAIDREQ
                {
                    MFS = _BTPOSTPAIDDETAIL,
                };
                #region Loger
                try
                {
                    string MobileResponseData = string.Empty;
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDREQ));
                            _serelized.Serialize(xmlWriter, _BTPOSTPAIDREQ);
                        }
                        MobileResponseData = stringWriter.ToString();

                    }
                    XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BNgul Payment Transaction Request Send to BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                    FormattedXML.ToString() + Environment.NewLine));
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                string EmpSaveResponse = string.Empty;
                var Saveresponse = clientSave.PostAsJsonAsync("/fcgi-bin/MFS_TRANSFER/", _BTPOSTPAIDREQ).Result;
                Saveresponse.EnsureSuccessStatusCode();
                if (Saveresponse.IsSuccessStatusCode)
                {
                    EmpSaveResponse = Saveresponse.Content.ReadAsStringAsync().Result;
                    try
                    {
                        _BTPOSTPAIDRESP = JsonConvert.DeserializeObject<BTPOSTPAIDRESPHEADER>(EmpSaveResponse);

                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.MobileNumber;

                        //_MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                        //_MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);

                        //_CommanDetails.SystemLogger.WriteTransLog(this, "_MOBILEBANKING_RESP.ResponseCode :" + _BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                        //_CommanDetails.SystemLogger.WriteTransLog(this, "_MOBILEBANKING_RESP.ResponseDesc :" + _BTPOSTPAIDRESP.MFS.RESPONSE.RESPDESC);


                        _MOBILEBANKING_RESP.ResponseCode = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE.ToString();
                        _MOBILEBANKING_RESP.ResponseDesc = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPDESC.ToString();

                        #region Loger
                        try
                        {
                            string MobileResponseData = string.Empty;
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDRESPHEADER));
                                    _serelized.Serialize(xmlWriter, _BTPOSTPAIDRESP);
                                }
                                MobileResponseData = stringWriter.ToString();

                            }
                            XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BNgul Payment Transaction Response Recieved from BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                            FormattedXML.ToString() + Environment.NewLine));
                        }
                        catch (Exception ex)
                        {
                            _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        }
                        #endregion


                       

                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;

                        if (Convert.ToInt32(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            _MOBILEBANKING_RESP.TXREFID = _BTPOSTPAIDRESP.MFS.REFID;
                            ProcessCashInPaymentStatus(ref _MOBILEBANKING_RESP, _MOBILEBANKING_REQ, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, ref _BTPOSTPAIDRESP);
                        }
                        else if (_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE != ConstResponseCode.BTApproved)
                        {


                            _MOBILEBANKING_RESP.TXREFID = _BTPOSTPAIDRESP.MFS.REFID;
                            ProcessCashInPaymentStatus(ref _MOBILEBANKING_RESP, _MOBILEBANKING_REQ, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, ref _BTPOSTPAIDRESP);

                        }

                    }
                    catch (Exception ex)
                    { }
                }
                else
                {
                    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.MobileNumber;
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                    return;
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessCashInPaymentStatus(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, ref BTPOSTPAIDRESPHEADER _BTPOSTPAIDRESPHEADER)
        {
            try
            {
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                //    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain,
                //    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{
                //    return true;
                //};
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                string strSHA512 = SHA512(_MOBILEBANKING_REQ.MSGID.Substring(4) + "-" + _MOBILEBANKING_RESP.TXREFID);
                HttpClient clientSave = null;
                CreateBNgulWebRequest(ref clientSave, strSHA512);
                BTPOSTPAIDDETAIL _BTPOSTPAIDDETAIL = new BTPOSTPAIDDETAIL
                {
                    OPERATION = "CHECKSTATUS",
                    REQTYPE = 30,
                    SRCMSISDN = BTConfigurationData.SourceNumberBngul,
                    REFID = _MOBILEBANKING_REQ.MSGID.Substring(4),
                    REQTIME = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    TXREFID = _MOBILEBANKING_RESP.TXREFID,
                    DESCRIPTION = "Sample test",
                };
                BTPOSTPAIDREQ _BTPOSTPAIDREQ = new BTPOSTPAIDREQ
                {
                    MFS = _BTPOSTPAIDDETAIL,
                };
                #region Loger
                try
                {
                    string MobileResponseData = string.Empty;
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDREQ));
                            _serelized.Serialize(xmlWriter, _BTPOSTPAIDREQ);
                        }
                        MobileResponseData = stringWriter.ToString();

                    }
                    XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Bngul cash in Payment Status Transaction Request Send to BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                    FormattedXML.ToString() + Environment.NewLine));
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion
                string EmpSaveResponse = string.Empty;
                var Saveresponse = clientSave.PostAsJsonAsync("/fcgi-bin/MFS_CHECKSTATUS/", _BTPOSTPAIDREQ).Result;
                Saveresponse.EnsureSuccessStatusCode();
                if (Saveresponse.IsSuccessStatusCode)
                {
                    EmpSaveResponse = Saveresponse.Content.ReadAsStringAsync().Result;
                    try
                    {
                        _BTPOSTPAIDRESPHEADER = JsonConvert.DeserializeObject<BTPOSTPAIDRESPHEADER>(EmpSaveResponse);

                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.MobileNumber;


                        //_MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_BTPOSTPAIDRESPHEADER.MFS.RESPONSE.RESPCODE);
                        //_MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_BTPOSTPAIDRESPHEADER.MFS.RESPONSE.RESPCODE);


                        _MOBILEBANKING_RESP.ResponseCode = _BTPOSTPAIDRESPHEADER.MFS.RESPONSE.RESPCODE.ToString();
                        _MOBILEBANKING_RESP.ResponseDesc = _BTPOSTPAIDRESPHEADER.MFS.RESPONSE.RESPDESC.ToString();

                        if (Convert.ToInt32(_MOBILEBANKING_RESP.ResponseCode) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            _MOBILEBANKING_RESP.BTOUTSTANDINGDETAILS = _BTPOSTPAIDRESP;
                            _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                            _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.MobileNumber;
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BNgulCashInSucess);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BNgulCashInSucess).Replace("@MobileNumber", _MOBILEBANKING_REQ.MobileNumber).Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString("N2")).Replace("@RRN", _CREATETRANSACTION_FSFS_RES.FCUBS_BODY.TransactionDetails.FCCREF); ;
                            try
                            {
                                int status = -1;
                                DataTable DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA_FORMAIL(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, out status);
                                _CommanDetails.SystemLogger.WriteTransLog(this, "Account Number :" + _MOBILEBANKING_REQ.CUST_AC_NO);
                                _CommanDetails.SystemLogger.WriteTransLog(this, "Mobile Number  :" + _MOBILEBANKING_REQ.MobileNumber);
                                _CommanDetails.SystemLogger.WriteTransLog(this, "Varify Account From CBS :" + status);
                                if (status == 0)
                                {
                                    _MOBILEBANKING_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();
                                    _ProcessMessage.ProcessSendApproveTransaction(_MOBILEBANKING_REQ, _MOBILEBANKING_RESP.ResponseDesc);
                                }
                            }
                            catch { }
                        }
                        else
                        {
                            _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                            _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.MobileNumber;
                            var task = Task.Factory.StartNew(() =>
                            {
                                _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For Bngul cash in For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                                _ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                             

                            });
                        }

                        #region Loger
                        try
                        {
                            string MobileResponseData = string.Empty;
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDRESPHEADER));
                                    _serelized.Serialize(xmlWriter, _BTPOSTPAIDRESPHEADER);
                                }
                                MobileResponseData = stringWriter.ToString();

                            }
                            XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** Bngul cash in Payment Status Transaction Response Recieved from BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                            FormattedXML.ToString() + Environment.NewLine));
                        }
                        catch (Exception ex)
                        {
                            _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    { }
                }
                else
                {
                    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.MobileNumber;
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                    return;
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessBTLeaseLinePaymentStatus(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, ref BTPOSTPAIDRESPHEADER _BTPOSTPAIDRESPHEADER)
        {
            try
            {
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                //    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain,
                //    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{
                //    return true;
                //};
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                //REFID-TXREFID
                string strSHA512 = SHA512(_MOBILEBANKING_REQ.MSGID.Substring(4) + "-" + _MOBILEBANKING_RESP.TXREFID);

                HttpClient clientSave = null;
                CreateBtLeaseLineWebRequest(ref clientSave, strSHA512);

                BTPOSTPAIDDETAIL _BTPOSTPAIDDETAIL = new BTPOSTPAIDDETAIL
                {
                    OPERATION = "CHECKSTATUS",
                    REQTYPE = 30,
                    SRCMSISDN = BTConfigurationData.SourceNumberLeaseLine,
                    REFID = _MOBILEBANKING_REQ.MSGID.Substring(4),
                    REQTIME = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    TXREFID = _MOBILEBANKING_RESP.TXREFID,
                 //   USID = _MOBILEBANKING_REQ.USID,  //archana 
                    DESCRIPTION = "Sample test",
                };
                BTPOSTPAIDREQ _BTPOSTPAIDREQ = new BTPOSTPAIDREQ
                {
                    MFS = _BTPOSTPAIDDETAIL,
                };
                #region Loger
                try
                {
                    string MobileResponseData = string.Empty;
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDREQ));
                            _serelized.Serialize(xmlWriter, _BTPOSTPAIDREQ);
                        }
                        MobileResponseData = stringWriter.ToString();

                    }
                    XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT LeaseLine Payment Status Transaction Request Send to BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                    FormattedXML.ToString() + Environment.NewLine));
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion
                string EmpSaveResponse = string.Empty;
                var Saveresponse = clientSave.PostAsJsonAsync("/fcgi-bin/MFS_CHECKSTATUS/", _BTPOSTPAIDREQ).Result;
                Saveresponse.EnsureSuccessStatusCode();
                if (Saveresponse.IsSuccessStatusCode)
                {
                    EmpSaveResponse = Saveresponse.Content.ReadAsStringAsync().Result;
                    try
                    {
                        _BTPOSTPAIDRESPHEADER = JsonConvert.DeserializeObject<BTPOSTPAIDRESPHEADER>(EmpSaveResponse);

                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.LeaseLineNumber;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_BTPOSTPAIDRESPHEADER.MFS.RESPONSE.RESPCODE);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_BTPOSTPAIDRESPHEADER.MFS.RESPONSE.RESPCODE);

                        _MOBILEBANKING_RESP.BTStatusCode = _BTPOSTPAIDRESPHEADER.MFS.RESPONSE.RESPCODE.ToString();
                        _MOBILEBANKING_RESP.BTTransStatus = _BTPOSTPAIDRESPHEADER.MFS.RESPTYPE.ToString();
                        _MOBILEBANKING_RESP.BTStatusDiscription = _BTPOSTPAIDRESPHEADER.MFS.RESPONSE.RESPDESC.ToString();

                        if (Convert.ToInt32(_MOBILEBANKING_RESP.ResponseCode) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            _MOBILEBANKING_RESP.BTOUTSTANDINGDETAILS = _BTPOSTPAIDRESP;
                            _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                            _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.LeaseLineNumber;
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BTLeaseLineSuccess);
                           // _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BTLeaseLineSuccess).Replace("@Leaselineserviceno", _MOBILEBANKING_REQ.LeaseLineNumber).Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString("N2")).Replace("@RRN", _CREATETRANSACTION_FSFS_RES.FCUBS_BODY.TransactionDetails.FCCREF); 
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BTLeaseLineSuccess).Replace("@Leaselineserviceno", _MOBILEBANKING_REQ.LeaseLineNumber).Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString("N2")).Replace("@RRN", _MOBILEBANKING_REQ.ReferenceNumber); 
                            
                            try
                            {
                                int status = -1;
                                DataTable DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA_FORMAIL(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, out status);
                                _CommanDetails.SystemLogger.WriteTransLog(this, "Account Number :" + _MOBILEBANKING_REQ.CUST_AC_NO);
                                _CommanDetails.SystemLogger.WriteTransLog(this, "Mobile Number  :" + _MOBILEBANKING_REQ.MobileNumber);
                                _CommanDetails.SystemLogger.WriteTransLog(this, "Varify Account From CBS :" + status);
                                if (status == 0)
                                {
                                    _MOBILEBANKING_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();
                                    _ProcessMessage.ProcessSendApproveTransaction(_MOBILEBANKING_REQ, _MOBILEBANKING_RESP.ResponseDesc);

                                    //Adding history
                                    IMPSTransactions.INSERT_RECENTTRANSACTION(_MOBILEBANKING_REQ.LeaseLineNumber, _MOBILEBANKING_REQ.REMITTERNAME, _MOBILEBANKING_REQ.TXNAMT.ToString(), _MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, _MOBILEBANKING_REQ.DeviceID, enumSource.BTLEASELINEPAYMENT.ToString());

                                }
                            }
                            catch { }
                        }

                        //else
                        //{
                        //    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        //    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        //    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        //    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.LeaseLineNumber;
                        //    var task = Task.Factory.StartNew(() =>
                        //    {
                        //        _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For BT LeaseLine For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                        //        _ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                        //    });
                        //}
                        else if (Convert.ToInt32(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE) != Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            string _Action = CommanDetails.GetMFSResponseCodeHost(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                            if (_Action == "1")
                            {
                                MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = _MOBILEBANKING_RESP;
                                var task = Task.Factory.StartNew(() =>
                                {
                                    _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For BT POSTPAID For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                                    // _ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                                    if (CONFIGURATIONCONFIGDATA.ISBTreversal)
                                    {
                                        _ProcessHost.ProcessReversalTransactionToHost(ref _MOBILEBANKING_RESP1, _MOBILEBANKING_REQ);
                                    }
                                });
                            }
                            //System.Threading.Thread.Sleep(8000);
                            if (_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE == "2144")
                            {
                                _MOBILEBANKING_RESP.ResponseCode = "00";
                                _MOBILEBANKING_RESP.ResponseDesc = "Your transaction is pendding.Kindly contact BT at 1600 for support.Sorry for inconvenience caused.";
                            }
                            else
                            {
                                _MOBILEBANKING_RESP.ResponseCode = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE;
                                _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused.";
                            }
                           // _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, null, null, null, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        }
                        #region Loger
                        try
                        {
                            string MobileResponseData = string.Empty;
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDRESPHEADER));
                                    _serelized.Serialize(xmlWriter, _BTPOSTPAIDRESPHEADER);
                                }
                                MobileResponseData = stringWriter.ToString();

                            }
                            XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT LeaseLine Payment Status Transaction Response Recieved from BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                            FormattedXML.ToString() + Environment.NewLine));
                        }
                        catch (Exception ex)
                        {
                            _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    { }
                }
                else
                {
                    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.LeaseLineNumber;
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                    _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused";// CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                    return;
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused";//CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }
 
        public void ProcessBTBroadBandPostpaidPayment(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient)
        {
            try
            {
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                //    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain,
                //    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{
                //    return true;
                //};
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                int result = -1;
                string Amount = string.Empty;
                if (int.TryParse(_MOBILEBANKING_REQ.TXNAMT.ToString(), out result))
                {
                    Amount = Convert.ToString(Convert.ToInt32(_MOBILEBANKING_REQ.TXNAMT.ToString()));
                }
                else
                {
                    Amount = Convert.ToString(_MOBILEBANKING_REQ.TXNAMT);
                }
                string strSHA512 = SHA512(_MOBILEBANKING_REQ.MSGID.Substring(4) + "-" + BTConfigurationData.SourceNumber + "-" + Amount);
                HttpClient clientSave = null;
                CreateBtPostPaidWebRequest(ref clientSave, strSHA512);
                BTPOSTPAIDDETAIL _BTPOSTPAIDDETAIL = new BTPOSTPAIDDETAIL
                {
                    OPERATION = "TRANSFER",
                    REQTYPE = 20,
                    REFID = _MOBILEBANKING_REQ.MSGID.Substring(4),
                    SESSIONID = _MOBILEBANKING_REQ.MSGID,
                    REQTIME = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    SRCMSISDN = BTConfigurationData.SourceNumber,
                    SERVICEID = 36,
                    AMOUNT = Amount,
                    //SRCMPIN = BTConfigurationData.TPIN,
                    SRCMPIN = "",
                    SERVICENO = _MOBILEBANKING_REQ.RechargeMobileNumber,
                    TRANSFERTYPE = "3",
                    SERVICEDETAILS = new SERVICEDETAILSPARAM { SERVICEREFID = "", SERVICENO = "", ACCOUNTNO = _MOBILEBANKING_REQ.RechargeMobileNumber, BILLNO = _MOBILEBANKING_REQ.BILLNO, USID = _MOBILEBANKING_REQ.USID, PPTYPE = "" },
                    COMMENT = _MOBILEBANKING_REQ.Remark,
                    DESCRIPTION = "Payment",
                    DESTNOTIFYNO = "",
                    DESTNOTIFYLAN = ""

                };
                BTPOSTPAIDREQ _BTPOSTPAIDREQ = new BTPOSTPAIDREQ
                {
                    MFS = _BTPOSTPAIDDETAIL,
                };
                #region Loger
                try
                {
                    string MobileResponseData = string.Empty;
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDREQ));
                            _serelized.Serialize(xmlWriter, _BTPOSTPAIDREQ);
                        }
                        MobileResponseData = stringWriter.ToString();

                    }
                    XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Broad Band PostPaid Payment Transaction Request Send to BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                    FormattedXML.ToString() + Environment.NewLine));
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion


                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, null, null, null, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                   
                string EmpSaveResponse = string.Empty;
                var Saveresponse = clientSave.PostAsJsonAsync("/fcgi-bin/MFS_TRANSFER/", _BTPOSTPAIDREQ).Result;
                Saveresponse.EnsureSuccessStatusCode();
                if (Saveresponse.IsSuccessStatusCode)
                {
                    EmpSaveResponse = Saveresponse.Content.ReadAsStringAsync().Result;
                    try
                    {
                        _BTPOSTPAIDRESP = JsonConvert.DeserializeObject<BTPOSTPAIDRESPHEADER>(EmpSaveResponse);
                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_REQ.TXNID = _BTPOSTPAIDRESP.MFS.TXID;

                        _MOBILEBANKING_RESP.BTStatusCode = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE.ToString();
                        _MOBILEBANKING_RESP.BTTransStatus = _BTPOSTPAIDRESP.MFS.RESPTYPE.ToString();
                        _MOBILEBANKING_RESP.BTStatusDiscription = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPDESC.ToString();
                       
                        #region Loger
                        try
                        {
                            string MobileResponseData = string.Empty;
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDRESPHEADER));
                                    _serelized.Serialize(xmlWriter, _BTPOSTPAIDRESP);
                                }
                                MobileResponseData = stringWriter.ToString();

                            }
                            XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Broad Band PostPaid Payment Transaction Response Recieved from BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                            FormattedXML.ToString() + Environment.NewLine));
                        }
                        catch (Exception ex)
                        {
                            _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        }
                        #endregion




                        if (Convert.ToInt32(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            _MOBILEBANKING_RESP.TXREFID = _BTPOSTPAIDRESP.MFS.REFID;
                            ProcessBTBroadBandPostpaidPaymentStatus(ref _MOBILEBANKING_RESP, _MOBILEBANKING_REQ, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, ref _BTPOSTPAIDRESP);
                        }

                        else if (Convert.ToInt32(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE) != Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            string _Action = CommanDetails.GetMFSResponseCodeHost(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                            if (_Action == "1")
                            {
                                MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = _MOBILEBANKING_RESP;
                                var task = Task.Factory.StartNew(() =>
                                {
                                    _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For BT POSTPAID For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                                    // _ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                                    if (CONFIGURATIONCONFIGDATA.ISBTreversal)
                                    {
                                        _ProcessHost.ProcessReversalTransactionToHost(ref _MOBILEBANKING_RESP1, _MOBILEBANKING_REQ);
                                    }
                                });
                            }
                            if (_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE == "2144")
                            {
                                _MOBILEBANKING_RESP.ResponseCode = "00";
                                _MOBILEBANKING_RESP.ResponseDesc = "Your transaction is pendding.Kindly contact BT at 1600 for support.Sorry for inconvenience caused.";
                            }
                            else
                            {
                            _MOBILEBANKING_RESP.ResponseCode = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE;
                            _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused.";
                            
                            }
                                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, null, null, null, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        }
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, null, null, null, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    }
                    catch (Exception ex)
                    { }
                }
                else
                {
                    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                    _MOBILEBANKING_RESP.ResponseDesc =  "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused.";//CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                    return;
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                _MOBILEBANKING_RESP.ResponseDesc =  "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused.";//CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessBTBroadBandPostpaidPaymentStatus(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, ref BTPOSTPAIDRESPHEADER _BTPOSTPAIDRESPHEADER)
        {
            try
            {
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                //    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain,
                //    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{
                //    return true;
                //};
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                string strSHA512 = SHA512(_MOBILEBANKING_REQ.MSGID.Substring(4) + "-" + _MOBILEBANKING_RESP.TXREFID);
                HttpClient clientSave = null;
                CreateBtPostPaidWebRequest(ref clientSave, strSHA512);
                BTPOSTPAIDDETAIL _BTPOSTPAIDDETAIL = new BTPOSTPAIDDETAIL
                {
                    OPERATION = "CHECKSTATUS",
                    REQTYPE = 30,
                    REFID = _MOBILEBANKING_REQ.MSGID.Substring(4),
                    REQTIME = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    SRCMSISDN = BTConfigurationData.SourceNumber,
                 //   USID = _MOBILEBANKING_REQ.USID,  //archana 
                    TXREFID = _MOBILEBANKING_RESP.TXREFID,
                    DESCRIPTION = "test",
                };
                BTPOSTPAIDREQ _BTPOSTPAIDREQ = new BTPOSTPAIDREQ
                {
                    MFS = _BTPOSTPAIDDETAIL,
                };
                #region Loger
                try
                {
                    string MobileResponseData = string.Empty;
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDREQ));
                            _serelized.Serialize(xmlWriter, _BTPOSTPAIDREQ);
                        }
                        MobileResponseData = stringWriter.ToString();

                    }
                    XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Broad Band PostPaid Payment Status Transaction Request Send to BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                    FormattedXML.ToString() + Environment.NewLine));
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion
                string EmpSaveResponse = string.Empty;
                var Saveresponse = clientSave.PostAsJsonAsync("/fcgi-bin/MFS_CHECKSTATUS/", _BTPOSTPAIDREQ).Result;
                Saveresponse.EnsureSuccessStatusCode();
                if (Saveresponse.IsSuccessStatusCode)
                {
                    EmpSaveResponse = Saveresponse.Content.ReadAsStringAsync().Result;
                    try
                    {
                        _BTPOSTPAIDRESP = JsonConvert.DeserializeObject<BTPOSTPAIDRESPHEADER>(EmpSaveResponse);

                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_BTPOSTPAIDRESPHEADER.MFS.RESPONSE.RESPCODE);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_BTPOSTPAIDRESPHEADER.MFS.RESPONSE.RESPCODE);
                      
                        _MOBILEBANKING_RESP.BTStatusCode = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE.ToString();
                        _MOBILEBANKING_RESP.BTTransStatus = _BTPOSTPAIDRESP.MFS.RESPTYPE.ToString();
                        _MOBILEBANKING_RESP.BTStatusDiscription = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPDESC.ToString();
                       
                        if (Convert.ToInt32(_MOBILEBANKING_RESP.ResponseCode) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            _MOBILEBANKING_RESP.BTOUTSTANDINGDETAILS = _BTPOSTPAIDRESP;
                            _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                            _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BTBroadBandPostPaidSuccess);
                            //_MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BTBroadBandPostPaidSuccess).Replace("@Mobile", _MOBILEBANKING_REQ.RechargeMobileNumber).Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString("N2")).Replace("@RRN", _CREATETRANSACTION_FSFS_RES.FCUBS_BODY.TransactionDetails.FCCREF); ;
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BTBroadBandPostPaidSuccess).Replace("@Mobile", _MOBILEBANKING_REQ.RechargeMobileNumber).Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString("N2")).Replace("@RRN", _MOBILEBANKING_REQ.ReferenceNumber);
                            
                            try
                            {
                                int status = -1;
                                DataTable DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA_FORMAIL(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, out status);
                                _CommanDetails.SystemLogger.WriteTransLog(this, "Account Number :" + _MOBILEBANKING_REQ.CUST_AC_NO);
                                _CommanDetails.SystemLogger.WriteTransLog(this, "Mobile Number  :" + _MOBILEBANKING_REQ.MobileNumber);
                                _CommanDetails.SystemLogger.WriteTransLog(this, "Varify Account From CBS :" + status);
                                if (status == 0)
                                {
                                    _MOBILEBANKING_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();
                                    _ProcessMessage.ProcessSendApproveTransaction(_MOBILEBANKING_REQ, _MOBILEBANKING_RESP.ResponseDesc);
                                }
                            }
                            catch { }
                        }

                        else if (Convert.ToInt32(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE) != Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            string _Action = CommanDetails.GetMFSResponseCodeHost(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                            if (_Action == "1")
                            {
                                MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = _MOBILEBANKING_RESP;
                                var task = Task.Factory.StartNew(() =>
                                {
                                    _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For BT POSTPAID For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                                    // _ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                                    if (CONFIGURATIONCONFIGDATA.ISBTreversal)
                                    {
                                        _ProcessHost.ProcessReversalTransactionToHost(ref _MOBILEBANKING_RESP1, _MOBILEBANKING_REQ);
                                    }
                                });
                            }
                            //System.Threading.Thread.Sleep(8000);
                            if (_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE == "2144")
                            {
                                _MOBILEBANKING_RESP.ResponseCode = "00";
                                _MOBILEBANKING_RESP.ResponseDesc = "Your transaction is pendding.Kindly contact BT at 1600 for support.Sorry for inconvenience caused.";
                            }
                            else
                            {
                                _MOBILEBANKING_RESP.ResponseCode = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE;
                                _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused.";
                            }
                                   //_ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, null, null, null, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        }
                        //else
                        //{
                        //    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        //    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        //    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        //    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        //    var task = Task.Factory.StartNew(() =>
                        //    {
                        //        _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For BT BROAD BAND POSTPAID For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                        //        _ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                        //    });
                        //}

                        #region Loger
                        try
                        {
                            string MobileResponseData = string.Empty;
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDRESPHEADER));
                                    _serelized.Serialize(xmlWriter, _BTPOSTPAIDRESPHEADER);
                                }
                                MobileResponseData = stringWriter.ToString();

                            }
                            XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Broad Band PostPaid Payment Status Transaction Response Recieved from BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                            FormattedXML.ToString() + Environment.NewLine));
                        }
                        catch (Exception ex)
                        {
                            _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    { }
                }
                else
                {
                    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                    _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused.";//CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                    return;
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused.";//CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessBTBroadBandPrepaid(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient)
        {
            try
            {
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                //    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain,
                //    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{
                //    return true;
                //};
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                int result = -1;
                string Amount = string.Empty;
                if (int.TryParse(_MOBILEBANKING_REQ.TXNAMT.ToString(), out result))
                {
                    Amount = Convert.ToString(Convert.ToInt32(_MOBILEBANKING_REQ.TXNAMT.ToString()));
                }
                else
                {
                    Amount = Convert.ToString(_MOBILEBANKING_REQ.TXNAMT);
                }
                string strSHA512 = SHA512(_MOBILEBANKING_REQ.MSGID.Substring(4) + "-" + BTConfigurationData.SourceNumber + "-" + Amount);
                HttpClient clientSave = null;
                CreateBtPostPaidWebRequest(ref clientSave, strSHA512);
                BTPOSTPAIDDETAIL _BTPOSTPAIDDETAIL = new BTPOSTPAIDDETAIL
                {
                    OPERATION = "TRANSFER",
                    REQTYPE = 20,
                    REFID = _MOBILEBANKING_REQ.MSGID.Substring(4),
                    REQTIME = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    SERVICEID = 36,
                    AMOUNT = Amount,
                    SRCMSISDN = BTConfigurationData.SourceNumber,
                    //SRCMPIN = BTConfigurationData.TPIN,
                    SRCMPIN = "",
                    TRANSFERTYPE = "3",
                    SERVICEDETAILS = new SERVICEDETAILSPARAM { SERVICEREFID = "", SERVICENO = "", BILLNO = _MOBILEBANKING_REQ.BILLNO, ACCOUNTNO = _MOBILEBANKING_REQ.RechargeMobileNumber, PPTYPE = "1" },
                    COMMENT = _MOBILEBANKING_REQ.Remark,
                    DESCRIPTION = "Recharge",
                    DESTNOTIFYNO = "",
                    DESTNOTIFYLAN = ""

                };
                BTPOSTPAIDREQ _BTPOSTPAIDREQ = new BTPOSTPAIDREQ
                {
                    MFS = _BTPOSTPAIDDETAIL,
                };
                #region Loger
                try
                {
                    string MobileResponseData = string.Empty;
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDREQ));
                            _serelized.Serialize(xmlWriter, _BTPOSTPAIDREQ);
                        }
                        MobileResponseData = stringWriter.ToString();

                    }
                    XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Broad Band Prepaid Payment Transaction Request Send to BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                    FormattedXML.ToString() + Environment.NewLine));
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion
                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, null, null, null, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                   
                string EmpSaveResponse = string.Empty;
                var Saveresponse = clientSave.PostAsJsonAsync("/fcgi-bin/MFS_TRANSFER/", _BTPOSTPAIDREQ).Result;
                Saveresponse.EnsureSuccessStatusCode();
                if (Saveresponse.IsSuccessStatusCode)
                {
                    EmpSaveResponse = Saveresponse.Content.ReadAsStringAsync().Result;
                    try
                    {
                        _BTPOSTPAIDRESP = JsonConvert.DeserializeObject<BTPOSTPAIDRESPHEADER>(EmpSaveResponse);

                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                        _MOBILEBANKING_REQ.TXNID = _BTPOSTPAIDRESP.MFS.TXID;

                        _MOBILEBANKING_RESP.BTStatusCode = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE.ToString();
                        _MOBILEBANKING_RESP.BTTransStatus = _BTPOSTPAIDRESP.MFS.RESPTYPE.ToString();
                        _MOBILEBANKING_RESP.BTStatusDiscription = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPDESC.ToString();

                        #region Loger
                        try
                        {
                            string MobileResponseData = string.Empty;
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDRESPHEADER));
                                    _serelized.Serialize(xmlWriter, _BTPOSTPAIDRESP);
                                }
                                MobileResponseData = stringWriter.ToString();

                            }
                            XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Broad Band PrePaid Payment Transaction Response Recieved from BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                            FormattedXML.ToString() + Environment.NewLine));
                        }
                        catch (Exception ex)
                        {
                            _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        }
                        #endregion

                        if (Convert.ToInt32(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            _MOBILEBANKING_RESP.TXREFID = _BTPOSTPAIDRESP.MFS.REFID;
                            ProcessBTBroadBandPrePaidStatus(ref _MOBILEBANKING_RESP, _MOBILEBANKING_REQ, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, ref _BTPOSTPAIDRESP);
                        }
                        else if (Convert.ToInt32(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE) != Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            string _Action = CommanDetails.GetMFSResponseCodeHost(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                            if (_Action == "1")
                            {
                                MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = _MOBILEBANKING_RESP;
                                var task = Task.Factory.StartNew(() =>
                                {
                                    _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For BT POSTPAID For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                                    // _ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                                    if (CONFIGURATIONCONFIGDATA.ISBTreversal)
                                    {
                                        _ProcessHost.ProcessReversalTransactionToHost(ref _MOBILEBANKING_RESP1, _MOBILEBANKING_REQ);
                                    }
                                });
                            }
                            if (_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE == "2144")
                            {
                                _MOBILEBANKING_RESP.ResponseCode = "00";
                                _MOBILEBANKING_RESP.ResponseDesc = "Your transaction is pendding.Kindly contact BT at 1600 for support.Sorry for inconvenience caused.";
                            }
                            else
                            {
                                _MOBILEBANKING_RESP.ResponseCode = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE;
                                _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused.";
                            }
                        }
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, null, null, null, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);
                    }
                    catch (Exception ex)
                    { }
                }
                else
                {
                    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                    _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused.";//CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                    return;
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused.";//CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessBTBroadBandPrePaidStatus(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, ref BTPOSTPAIDRESPHEADER _BTPOSTPAIDRESPHEADER)
        {
            try
            {
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                //    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain,
                //    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{
                //    return true;
                //};
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                string strSHA512 = SHA512(_MOBILEBANKING_REQ.MSGID.Substring(4) + "-" + _MOBILEBANKING_RESP.TXREFID);
                HttpClient clientSave = null;
                CreateBtPostPaidWebRequest(ref clientSave, strSHA512);
                BTPOSTPAIDDETAIL _BTPOSTPAIDDETAIL = new BTPOSTPAIDDETAIL
                {
                    OPERATION = "CHECKSTATUS",
                    REQTYPE = 30,
                    REFID = _MOBILEBANKING_REQ.MSGID.Substring(4),
                    REQTIME = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    SRCMSISDN = BTConfigurationData.SourceNumber,
                    TXREFID = _MOBILEBANKING_RESP.TXREFID,
                   // USID = _MOBILEBANKING_REQ.USID,  //archana 
                    DESCRIPTION = "test",
                };
                BTPOSTPAIDREQ _BTPOSTPAIDREQ = new BTPOSTPAIDREQ
                {
                    MFS = _BTPOSTPAIDDETAIL,
                };
                #region Loger
                try
                {
                    string MobileResponseData = string.Empty;
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDREQ));
                            _serelized.Serialize(xmlWriter, _BTPOSTPAIDREQ);
                        }
                        MobileResponseData = stringWriter.ToString();

                    }
                    XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Broad Band PrePaid Payment Status Transaction Request Send to BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                    FormattedXML.ToString() + Environment.NewLine));
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion
                string EmpSaveResponse = string.Empty;
                var Saveresponse = clientSave.PostAsJsonAsync("/fcgi-bin/MFS_CHECKSTATUS/", _BTPOSTPAIDREQ).Result;
                Saveresponse.EnsureSuccessStatusCode();
                if (Saveresponse.IsSuccessStatusCode)
                {
                    EmpSaveResponse = Saveresponse.Content.ReadAsStringAsync().Result;
                    try
                    {
                        _BTPOSTPAIDRESP = JsonConvert.DeserializeObject<BTPOSTPAIDRESPHEADER>(EmpSaveResponse);

                        #region Loger
                        try
                        {
                            string MobileResponseData = string.Empty;
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDRESPHEADER));
                                    _serelized.Serialize(xmlWriter, _BTPOSTPAIDRESPHEADER);
                                }
                                MobileResponseData = stringWriter.ToString();

                            }
                            XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Broad Band PrePaid Payment Status Transaction Response Recieved from BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                            FormattedXML.ToString() + Environment.NewLine));
                        }
                        catch (Exception ex)
                        {
                            _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        }
                        #endregion

                        _MOBILEBANKING_RESP.BTStatusCode = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE.ToString();
                        _MOBILEBANKING_RESP.BTTransStatus = _BTPOSTPAIDRESP.MFS.RESPTYPE.ToString();
                        _MOBILEBANKING_RESP.BTStatusDiscription = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPDESC.ToString();

                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_BTPOSTPAIDRESPHEADER.MFS.RESPONSE.RESPCODE);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_BTPOSTPAIDRESPHEADER.MFS.RESPONSE.RESPCODE);

                        if (Convert.ToInt32(_MOBILEBANKING_RESP.ResponseCode) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            _MOBILEBANKING_RESP.BTOUTSTANDINGDETAILS = _BTPOSTPAIDRESP;
                            _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                            _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BTBroadBandPrepaidSuccess);
                           // _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BTBroadBandPrepaidSuccess).Replace("@Mobile", _MOBILEBANKING_REQ.RechargeMobileNumber).Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString("N2")).Replace("@RRN", _CREATETRANSACTION_FSFS_RES.FCUBS_BODY.TransactionDetails.FCCREF);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BTBroadBandPrepaidSuccess).Replace("@Mobile", _MOBILEBANKING_REQ.RechargeMobileNumber).Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString("N2")).Replace("@RRN", _MOBILEBANKING_REQ.ReferenceNumber);
                           
                            
                            try
                            {
                                int status = -1;
                                DataTable DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA_FORMAIL(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, out status);
                                _CommanDetails.SystemLogger.WriteTransLog(this, "Account Number :" + _MOBILEBANKING_REQ.CUST_AC_NO);
                                _CommanDetails.SystemLogger.WriteTransLog(this, "Mobile Number  :" + _MOBILEBANKING_REQ.MobileNumber);
                                _CommanDetails.SystemLogger.WriteTransLog(this, "Varify Account From CBS :" + status);
                                if (status == 0)
                                {
                                    _MOBILEBANKING_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();
                                    _ProcessMessage.ProcessSendApproveTransaction(_MOBILEBANKING_REQ, _MOBILEBANKING_RESP.ResponseDesc);
                                }
                            }
                            catch { }
                        }
                        else if (Convert.ToInt32(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE) != Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            string _Action = CommanDetails.GetMFSResponseCodeHost(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                            if (_Action == "1")
                            {
                                MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = _MOBILEBANKING_RESP;
                                var task = Task.Factory.StartNew(() =>
                                {
                                    _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For BT POSTPAID For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                                    // _ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                                    if (CONFIGURATIONCONFIGDATA.ISBTreversal)
                                    {
                                        _ProcessHost.ProcessReversalTransactionToHost(ref _MOBILEBANKING_RESP1, _MOBILEBANKING_REQ);
                                    }
                                });
                            }
                            if (_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE == "2144")
                            {
                                _MOBILEBANKING_RESP.ResponseCode = "00";
                                _MOBILEBANKING_RESP.ResponseDesc = "Your transaction is pendding.Kindly contact BT at 1600 for support.Sorry for inconvenience caused.";
                            }
                            else
                            {
                                _MOBILEBANKING_RESP.ResponseCode = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE;
                                _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused.";
                            }
                        }
                        //else
                        //{
                        //    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        //    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        //    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        //    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        //    var task = Task.Factory.StartNew(() =>
                        //    {
                        //        _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For BT BROAD BAND PREPAID For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                        //        _ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                        //    });
                        //}


                    }
                    catch (Exception ex)
                    { }
                }
                else
                {
                    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                    _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused.";//CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                    return;
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused.";//CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessBTLandLinePayment(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient)
        {
            try
            {
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                //    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain,
                //    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{
                //    return true;
                //};
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                int result = -1;
                string Amount = string.Empty;
                if (int.TryParse(_MOBILEBANKING_REQ.TXNAMT.ToString(), out result))
                {
                    Amount = Convert.ToString(Convert.ToInt32(_MOBILEBANKING_REQ.TXNAMT.ToString()));
                }
                else
                {
                    Amount = Convert.ToString(_MOBILEBANKING_REQ.TXNAMT);
                }
                string strSHA512 = SHA512(_MOBILEBANKING_REQ.MSGID.Substring(4) + "-" + BTConfigurationData.SourceNumber + "-" + Amount);
                HttpClient clientSave = null;
                CreateBtPostPaidWebRequest(ref clientSave, strSHA512);
                BTPOSTPAIDDETAIL _BTPOSTPAIDDETAIL = new BTPOSTPAIDDETAIL
                {
                    OPERATION = "TRANSFER",
                    REQTYPE = 20,
                    REFID = _MOBILEBANKING_REQ.MSGID.Substring(4),
                    SESSIONID = _MOBILEBANKING_REQ.MSGID,
                    REQTIME = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    SRCMSISDN = BTConfigurationData.SourceNumber,
                    SERVICEID = 11,
                    AMOUNT = Amount,
                    //SRCMPIN = BTConfigurationData.TPIN,
                    SRCMPIN = "",
                    SERVICENO = _MOBILEBANKING_REQ.RechargeMobileNumber,
                    TRANSFERTYPE = "3",
                    SERVICEDETAILS = new SERVICEDETAILSPARAM { SERVICEREFID = "", SERVICENO = "", ACCOUNTNO = _MOBILEBANKING_REQ.RechargeMobileNumber, BILLNO = _MOBILEBANKING_REQ.BILLNO, USID = _MOBILEBANKING_REQ.USID, PPTYPE = "" },
                    COMMENT = _MOBILEBANKING_REQ.Remark,
                    DESCRIPTION = "Payment",
                    DESTNOTIFYNO = "",
                    DESTNOTIFYLAN = ""

                };
                BTPOSTPAIDREQ _BTPOSTPAIDREQ = new BTPOSTPAIDREQ
                {
                    MFS = _BTPOSTPAIDDETAIL,
                };
                #region Loger
                try
                {
                    string MobileResponseData = string.Empty;
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDREQ));
                            _serelized.Serialize(xmlWriter, _BTPOSTPAIDREQ);
                        }
                        MobileResponseData = stringWriter.ToString();

                    }
                    XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Land Line Payment Transaction Request Send to BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                    FormattedXML.ToString() + Environment.NewLine));
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion

                _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, null, null, null, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                string EmpSaveResponse = string.Empty;
                var Saveresponse = clientSave.PostAsJsonAsync("/fcgi-bin/MFS_TRANSFER/", _BTPOSTPAIDREQ).Result;
                Saveresponse.EnsureSuccessStatusCode();

                       
                if (Saveresponse.IsSuccessStatusCode)
                {
                    EmpSaveResponse = Saveresponse.Content.ReadAsStringAsync().Result;
                    try
                    {
                        _BTPOSTPAIDRESP = JsonConvert.DeserializeObject<BTPOSTPAIDRESPHEADER>(EmpSaveResponse);

                        _MOBILEBANKING_RESP.BTStatusCode = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE.ToString();
                        _MOBILEBANKING_RESP.BTTransStatus = _BTPOSTPAIDRESP.MFS.RESPTYPE.ToString();
                        _MOBILEBANKING_RESP.BTStatusDiscription = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPDESC.ToString();

                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                        _MOBILEBANKING_REQ.TXNID = _BTPOSTPAIDRESP.MFS.TXID;
                       // _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);

                        //if (Convert.ToInt32(_MOBILEBANKING_RESP.ResponseCode) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        //{
                        //    _MOBILEBANKING_RESP.BTOUTSTANDINGDETAILS = _BTPOSTPAIDRESP;
                        //    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        //    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        //    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        //    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        //    _MOBILEBANKING_RESP.MobileNumber = _MOBILEBANKING_REQ.RechargeMobileNumber;
                        //    //_MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved);
                        //    //_MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.Approved);
                        //}
                        if (Convert.ToInt32(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            _MOBILEBANKING_RESP.TXREFID = _BTPOSTPAIDRESP.MFS.REFID;
                            ProcessBTLandLinePaymentStatus(ref _MOBILEBANKING_RESP, _MOBILEBANKING_REQ, _CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, ref _BTPOSTPAIDRESP);
                        }
                        else if (Convert.ToInt32(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE) != Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            string _Action = CommanDetails.GetMFSResponseCodeHost(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                            if (_Action == "1")
                            {
                                MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = _MOBILEBANKING_RESP;
                                var task = Task.Factory.StartNew(() =>
                                {
                                    _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For BT POSTPAID For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                                    // _ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                                    if (CONFIGURATIONCONFIGDATA.ISBTreversal)
                                    {
                                        _ProcessHost.ProcessReversalTransactionToHost(ref _MOBILEBANKING_RESP1, _MOBILEBANKING_REQ);
                                    }
                                });
                            }
                            //System.Threading.Thread.Sleep(8000); 
                            if (_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE == "2144")
                            {
                                _MOBILEBANKING_RESP.ResponseCode = "00";
                                _MOBILEBANKING_RESP.ResponseDesc = "Your transaction is pendding.Kindly contact BT at 1600 for support.Sorry for inconvenience caused.";
                            }
                            else
                            {
                                _MOBILEBANKING_RESP.ResponseCode = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE;
                                _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused.";
                            }
                            _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, null, null, null, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        }
                        //else
                        //{
                        //    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        //    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        //    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        //    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        //    _MOBILEBANKING_RESP.ResponseCode = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE;
                        //    _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_MOBILEBANKING_RESP.ResponseCode);

                        //    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        //}
                        _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, null, null, null, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        #region Loger
                        try
                        {
                            string MobileResponseData = string.Empty;
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDRESPHEADER));
                                    _serelized.Serialize(xmlWriter, _BTPOSTPAIDRESP);
                                }
                                MobileResponseData = stringWriter.ToString();

                            }
                            XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Land Line Payment Transaction Response Recieved from BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                            FormattedXML.ToString() + Environment.NewLine));
                        }
                        catch (Exception ex)
                        {
                            _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    { }
                }
                else
                {
                    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                    _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused.";//CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                    return;
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused.";//CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public void ProcessBTLandLinePaymentStatus(ref MOBILEBANKING_RESP _MOBILEBANKING_RESP, MOBILEBANKING_REQ _MOBILEBANKING_REQ, CREATEUPTRANSACTION_FSFS_REQ _CREATETRANSACTION_FSFS_REQ, CREATEUPTRANSACTION_FSFS_RES _CREATETRANSACTION_FSFS_RES, IMPSTransactionRouter.FCUBSUPService.FCUBS_HEADERType _FCUBS_HEADERType, FCUBSUPService.FCUBSUPServiceSEIClient _FCUBSRTServiceSEIClient, ref BTPOSTPAIDRESPHEADER _BTPOSTPAIDRESPHEADER)
        {
            try
            {
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                //    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain,
                //    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{
                //    return true;
                //};
                string strSHA512 = SHA512(_MOBILEBANKING_REQ.MSGID.Substring(4) + "-" + _MOBILEBANKING_RESP.TXREFID);
                HttpClient clientSave = null;
                CreateBtPostPaidWebRequest(ref clientSave, strSHA512);
                BTPOSTPAIDDETAIL _BTPOSTPAIDDETAIL = new BTPOSTPAIDDETAIL
                {
                    OPERATION = "CHECKSTATUS",
                    REQTYPE = 30,
                    REFID = _MOBILEBANKING_REQ.MSGID.Substring(4),
                    REQTIME = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    SRCMSISDN = BTConfigurationData.SourceNumber,
                    TXREFID = _MOBILEBANKING_RESP.TXREFID,
                  //  USID = _MOBILEBANKING_REQ.USID,  //archana 
                    DESCRIPTION = "test",
                };
                BTPOSTPAIDREQ _BTPOSTPAIDREQ = new BTPOSTPAIDREQ
                {
                    MFS = _BTPOSTPAIDDETAIL,
                };
                #region Loger
                try
                {
                    string MobileResponseData = string.Empty;
                    using (var stringWriter = new StringWriter())
                    {
                        using (var xmlWriter = XmlWriter.Create(stringWriter))
                        {
                            XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDREQ));
                            _serelized.Serialize(xmlWriter, _BTPOSTPAIDREQ);
                        }
                        MobileResponseData = stringWriter.ToString();

                    }
                    XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                    _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Land Line Payment Status Transaction Request Send to BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                    FormattedXML.ToString() + Environment.NewLine));
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                }
                #endregion
                string EmpSaveResponse = string.Empty;
                var Saveresponse = clientSave.PostAsJsonAsync("/fcgi-bin/MFS_CHECKSTATUS/", _BTPOSTPAIDREQ).Result;
                Saveresponse.EnsureSuccessStatusCode();
                if (Saveresponse.IsSuccessStatusCode)
                {
                    EmpSaveResponse = Saveresponse.Content.ReadAsStringAsync().Result;
                    try
                    {
                        _BTPOSTPAIDRESP = JsonConvert.DeserializeObject<BTPOSTPAIDRESPHEADER>(EmpSaveResponse);

                        _MOBILEBANKING_RESP.BTStatusCode = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE.ToString();
                        _MOBILEBANKING_RESP.BTTransStatus = _BTPOSTPAIDRESP.MFS.RESPTYPE.ToString();
                        _MOBILEBANKING_RESP.BTStatusDiscription = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPDESC.ToString();
                        #region Loger
                        try
                        {
                            string MobileResponseData = string.Empty;
                            using (var stringWriter = new StringWriter())
                            {
                                using (var xmlWriter = XmlWriter.Create(stringWriter))
                                {
                                    XmlSerializer _serelized = new XmlSerializer(typeof(BTPOSTPAIDRESPHEADER));
                                    _serelized.Serialize(xmlWriter, _BTPOSTPAIDRESPHEADER);
                                }
                                MobileResponseData = stringWriter.ToString();

                            }
                            XDocument FormattedXML = XDocument.Parse(MobileResponseData);
                            _CommanDetails.SystemLogger.WriteTransLog(this, string.Format("***** BT Land Line Payment Status Transaction Response Recieved from BT Server For Reference Number : " + _CREATETRANSACTION_FSFS_REQ.FCUBS_HEADER.MSGID + Environment.NewLine + Environment.NewLine +
                                                                            FormattedXML.ToString() + Environment.NewLine));
                        }
                        catch (Exception ex)
                        {
                            _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                        }
                        #endregion

                        _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(_BTPOSTPAIDRESPHEADER.MFS.RESPONSE.RESPCODE);
                        _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(_BTPOSTPAIDRESPHEADER.MFS.RESPONSE.RESPCODE);

                        if (Convert.ToInt32(_MOBILEBANKING_RESP.ResponseCode) == Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            _MOBILEBANKING_RESP.BTOUTSTANDINGDETAILS = _BTPOSTPAIDRESP;
                            _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                            _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                            _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                            _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                            _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BTLandLineSuccess);
                            _MOBILEBANKING_RESP.ResponseDesc = CommanDetails.GetResponseCodeDescription(ConstResponseCode.BTLandLineSuccess).Replace("@Mobile", _MOBILEBANKING_REQ.RechargeMobileNumber).Replace("@Amount", _MOBILEBANKING_REQ.TXNAMT.ToString("N2")).Replace("@RRN", _MOBILEBANKING_REQ.ReferenceNumber); ;
                            try
                            {
                                int status = -1;
                                DataTable DTCustomerdata = IMPSTransactions.VERIFYCUSTOMERDATA_FORMAIL(_MOBILEBANKING_REQ.CUST_AC_NO, _MOBILEBANKING_REQ.MobileNumber, out status);
                                _CommanDetails.SystemLogger.WriteTransLog(this, "Account Number :" + _MOBILEBANKING_REQ.CUST_AC_NO);
                                _CommanDetails.SystemLogger.WriteTransLog(this, "Mobile Number  :" + _MOBILEBANKING_REQ.MobileNumber);
                                _CommanDetails.SystemLogger.WriteTransLog(this, "Varify Account From CBS :" + status);
                                if (status == 0)
                                {
                                    //_MOBILEBANKING_REQ.MailID = DTCustomerdata.Rows[0]["e_mail"].ToString();
                                    //_ProcessMessage.ProcessSendApproveTransaction(_MOBILEBANKING_REQ, _MOBILEBANKING_RESP.ResponseDesc);
                                }
                            }
                            catch { }
                        }
                        //else
                        //{
                        //    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                        //    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                        //    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                        //    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                        //    var task = Task.Factory.StartNew(() =>
                        //    {
                        //        _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For BT Land Line For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                        //        _ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                        //    });
                        //}
                        else if (Convert.ToInt32(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE) != Convert.ToInt32(ConstResponseCode.BTApproved))
                        {
                            string _Action = CommanDetails.GetMFSResponseCodeHost(_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE);
                            if (_Action == "1")
                            {
                                MOBILEBANKING_RESP _MOBILEBANKING_RESP1 = _MOBILEBANKING_RESP;
                                var task = Task.Factory.StartNew(() =>
                                {
                                    _CommanDetails.SystemLogger.WriteTransLog(this, "**** Reversal Generated For BT POSTPAID For Reference Number " + _MOBILEBANKING_REQ.MSGID + Environment.NewLine + Environment.NewLine);
                                    // _ProcessHost.ProcessReversalToHost(_CREATETRANSACTION_FSFS_REQ, _CREATETRANSACTION_FSFS_RES, _FCUBS_HEADERType, _FCUBSRTServiceSEIClient, _MOBILEBANKING_REQ);
                                    if (CONFIGURATIONCONFIGDATA.ISBTreversal)
                                    {
                                        _ProcessHost.ProcessReversalTransactionToHost(ref _MOBILEBANKING_RESP1, _MOBILEBANKING_REQ);
                                    }
                                });
                            }
                            //System.Threading.Thread.Sleep(8000);
                            if (_BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE == "2144")
                            {
                                _MOBILEBANKING_RESP.ResponseCode = "00";
                                _MOBILEBANKING_RESP.ResponseDesc = "Your transaction is pendding.Kindly contact BT at 1600 for support.Sorry for inconvenience caused.";
                            }
                            else
                            {
                                _MOBILEBANKING_RESP.ResponseCode = _BTPOSTPAIDRESP.MFS.RESPONSE.RESPCODE;
                                _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused.";
                            }
                           // _ProcessMessage.TransactionPayment((int)enumCommandTypeEnum.AuthorizationResponseMessage, enumSource.Unknown, null, null, null, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 7);

                        }

                    }
                    catch (Exception ex)
                    { }
                }
                else
                {
                    _MOBILEBANKING_RESP.MSGID = _MOBILEBANKING_REQ.MSGID;
                    _MOBILEBANKING_RESP.DeviceID = _MOBILEBANKING_REQ.DeviceID;
                    _MOBILEBANKING_RESP.ReferenceNumber = _MOBILEBANKING_REQ.ReferenceNumber;
                    _MOBILEBANKING_RESP.NationalID = _MOBILEBANKING_REQ.NationalID;
                    _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                    _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused.";//CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                    return;
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(null, ex);
                _MOBILEBANKING_RESP.ResponseCode = CommanDetails.GetResponseCodeHost(ConstResponseCode.BPCTIMEOUT);
                _MOBILEBANKING_RESP.ResponseDesc = "The deducted amount will be reversed soon by the bank. Sorry for inconvenience caused.";//CommanDetails.GetResponseCodeDescription(ConstResponseCode.BPCTIMEOUT);
                _MOBILEBANKING_RESP.ResponseData = null;
            }
        }

        public static HttpWebRequest CreateWebRequest()
        {
            //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
            //         System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            //         System.Security.Cryptography.X509Certificates.X509Chain chain,
            //         System.Net.Security.SslPolicyErrors sslPolicyErrors)
            //{
            //    return true;
            //};
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["TCELL"].ToString());
            //WebProxy myproxy = new WebProxy("172.30.10.222", 3128);
            //myproxy.BypassProxyOnLocal = false;
            //webRequest.Proxy = myproxy;
            webRequest.KeepAlive = false;
            webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Method = "POST";
            return webRequest;
        }

        public static HttpWebRequest CreateTcellPrepaidWebRequest()
        {
            //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
            //         System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            //         System.Security.Cryptography.X509Certificates.X509Chain chain,
            //         System.Net.Security.SslPolicyErrors sslPolicyErrors)
            //{
            //    return true;
            //};
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["TCELLPREPAID"].ToString());
            ////WebProxy myproxy = new WebProxy("172.30.10.222", 3128);
            ////myproxy.BypassProxyOnLocal = false;
            ////webRequest.Proxy = myproxy;
            webRequest.KeepAlive = false;
            webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Method = "POST";
            return webRequest;
        }

        public static HttpWebRequest CreateWebRequestBPC()
        {


            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                     System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                     System.Security.Cryptography.X509Certificates.X509Chain chain,
                     System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["BPC_PAYMENT"].ToString());
            //WebProxy myproxy = new WebProxy("172.30.10.222", 3128);
            //myproxy.BypassProxyOnLocal = true;
            //webRequest.Proxy = myproxy;
            webRequest.KeepAlive = false;
            webRequest.Credentials = new NetworkCredential("ws_testusr", "bpc12345");
            webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Method = "POST";
            return webRequest;
        }

        public static string CreateWaterWebRequest(string WaterAccountNumber)
        {
            string EmpResponse = string.Empty;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                     System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                     System.Security.Cryptography.X509Certificates.X509Chain chain,
                     System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };
            WebProxy myproxy = new WebProxy("172.19.10.222", 3128);
            myproxy.BypassProxyOnLocal = false;

            var request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["WATER"].ToString() + "/isAccountNoExistsForWaterPayment/" + WaterAccountNumber);
            request.Proxy = myproxy;
            var response = (HttpWebResponse)request.GetResponse();
            EmpResponse = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return EmpResponse;
        }

        public static string CreateRicbLoanWebRequest(string CustomerData, string FunctionName)
        {
            string EmpResponse = string.Empty;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                     System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                     System.Security.Cryptography.X509Certificates.X509Chain chain,
                     System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };
            WebProxy myproxy = new WebProxy("172.19.10.222", 3128);
            myproxy.BypassProxyOnLocal = false;

            var request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["RICB"].ToString() + "/" + FunctionName + ".php?cid=" + CustomerData);
            request.Proxy = myproxy;
            var response = (HttpWebResponse)request.GetResponse();
            EmpResponse = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return EmpResponse;
        }

        public static string CreateRicbPaymentWebRequest(string SerialNumber, string ReferenceNumber, string RemitterName, string RemitterAcc, string RemitterMob, string Remark, string FunctionName, string amount)
        {
            string EmpResponse = string.Empty;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                     System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                     System.Security.Cryptography.X509Certificates.X509Chain chain,
                     System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };
            WebProxy myproxy = new WebProxy("172.19.10.222", 3128);
            myproxy.BypassProxyOnLocal = false;

            var request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["RICB"].ToString() + "/" + FunctionName + ".php?serial_no=" + SerialNumber + "&refno=" + ReferenceNumber + "&rem_name=" + RemitterName + "&rem_acc_no=" + RemitterAcc + "&rem_mob_no=" + RemitterMob + "&tran_date=" + DateTime.Now.ToString("dd/MM/yyyy") + "&remarks=" + Remark + "&amount=" + amount + "&tran_status=success");
            request.Proxy = myproxy;
            var response = (HttpWebResponse)request.GetResponse();
            EmpResponse = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return EmpResponse;
        }

        public static void CreateBtPostPaidWebRequest(ref HttpClient clientSave, string strSHA512)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                //Proxy = new WebProxy(string.Format("{0}:{1}", "172.30.10.222", 3128), false),  ////Old proxy

                //Proxy = new WebProxy(string.Format("{0}:{1}", "172.19.10.222", 3128), false),
                //PreAuthenticate = true,
                //UseDefaultCredentials = false,
            };

            httpClientHandler.Credentials = new NetworkCredential();
            clientSave = new HttpClient(httpClientHandler);
            clientSave.BaseAddress = new Uri(ConfigurationManager.AppSettings["BTPOSTPAID"].ToString());
            clientSave.DefaultRequestHeaders.Accept.Clear();
            clientSave.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //clientSave.DefaultRequestHeaders.Add("USERNAME", Base64Encode("DPNB"));
            //clientSave.DefaultRequestHeaders.Add("PASSWD", Base64Encode("DPNB@123"));
            //clientSave.DefaultRequestHeaders.Add("TOKENID", MaximusAESEncryption.EncryptStringBT("DPNB@123", strSHA512.Substring(0, 16)));

            //clientSave.DefaultRequestHeaders.Add("USERNAME", Base64Encode("BNB"));
            //clientSave.DefaultRequestHeaders.Add("PASSWD", Base64Encode("bnb@btl2018"));
            //clientSave.DefaultRequestHeaders.Add("TOKENID", MaximusAESEncryption.EncryptStringBT("bnb@btl2018", strSHA512.Substring(0, 16)));

        

            //// UAT Details
            //clientSave.DefaultRequestHeaders.Add("USERNAME", Base64Encode("TAYANAAGGR"));
            //clientSave.DefaultRequestHeaders.Add("PASSWD", Base64Encode("Tayana@1"));
            //clientSave.DefaultRequestHeaders.Add("TOKENID", MaximusAESEncryption.EncryptStringBT("Tayana@1", strSHA512.Substring(0, 16)));

            // Live details
            clientSave.DefaultRequestHeaders.Add("USERNAME", Base64Encode(ConfigurationManager.AppSettings["BTUSERID"].ToString()));
            clientSave.DefaultRequestHeaders.Add("PASSWD", Base64Encode(ConfigurationManager.AppSettings["BTUSERPASS"].ToString()));
            clientSave.DefaultRequestHeaders.Add("TOKENID", MaximusAESEncryption.EncryptStringBT(ConfigurationManager.AppSettings["BTUSERPASS"].ToString(), strSHA512.Substring(0, 16)));

        }

        public static void CreateBtLeaseLineWebRequest(ref HttpClient clientSave, string strSHA512)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                //Proxy = new WebProxy(string.Format("{0}:{1}", "172.30.10.222", 3128), false),  ////Old proxy

                //Proxy = new WebProxy(string.Format("{0}:{1}", "172.19.10.222", 3128), false),
                //PreAuthenticate = true,
                //UseDefaultCredentials = false,
            };
            httpClientHandler.Credentials = new NetworkCredential();
            clientSave = new HttpClient(httpClientHandler);
            clientSave.BaseAddress = new Uri(ConfigurationManager.AppSettings["BTPOSTPAID"].ToString());
            clientSave.DefaultRequestHeaders.Accept.Clear();
            clientSave.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //// UAT Details
            //clientSave.DefaultRequestHeaders.Add("USERNAME", Base64Encode("TAYANAAGGR"));
            //clientSave.DefaultRequestHeaders.Add("PASSWD", Base64Encode("Tayana@1"));
            //clientSave.DefaultRequestHeaders.Add("TOKENID", MaximusAESEncryption.EncryptStringBT("Tayana@1", strSHA512.Substring(0, 16)));

            //// UAT Details
            //clientSave.DefaultRequestHeaders.Add("USERNAME", Base64Encode("TAYANAAGGR"));
            //clientSave.DefaultRequestHeaders.Add("PASSWD", Base64Encode("Tayana@1"));
            //clientSave.DefaultRequestHeaders.Add("TOKENID", MaximusAESEncryption.EncryptStringBT("Tayana@1", strSHA512.Substring(0, 16)));

            clientSave.DefaultRequestHeaders.Add("USERNAME", Base64Encode(ConfigurationManager.AppSettings["BTUSERID"].ToString()));
            clientSave.DefaultRequestHeaders.Add("PASSWD", Base64Encode(ConfigurationManager.AppSettings["BTUSERPASS"].ToString()));
            clientSave.DefaultRequestHeaders.Add("TOKENID", MaximusAESEncryption.EncryptStringBT(ConfigurationManager.AppSettings["BTUSERPASS"].ToString(), strSHA512.Substring(0, 16)));

        }

        public static void CreateBNgulWebRequest(ref HttpClient clientSave, string strSHA512)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                //Proxy = new WebProxy(string.Format("{0}:{1}", "172.30.10.222", 3128), false),  ////Old proxy

                //Proxy = new WebProxy(string.Format("{0}:{1}", "172.19.10.222", 3128), false),
                //PreAuthenticate = true,
                //UseDefaultCredentials = false,
            };
            httpClientHandler.Credentials = new NetworkCredential();
            clientSave = new HttpClient(httpClientHandler);
            clientSave.BaseAddress = new Uri(ConfigurationManager.AppSettings["BTCASHIN"].ToString());
            clientSave.DefaultRequestHeaders.Accept.Clear();
            clientSave.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //// UAT Details
            clientSave.DefaultRequestHeaders.Add("USERNAME", Base64Encode("TAYANAAGGR"));
            clientSave.DefaultRequestHeaders.Add("PASSWD", Base64Encode("Tayana@1"));
            clientSave.DefaultRequestHeaders.Add("TOKENID", MaximusAESEncryption.EncryptStringBT("Tayana@1", strSHA512.Substring(0, 16)));
        }

        public double RoundUp(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }

        static string NullToString(object Value)
        {
            return Value == null ? "" : Value.ToString();
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string SHA512(string input)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);
                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2").ToLower());
                return hashedInputStringBuilder.ToString();
            }
        }

        public string GetRandomOTP()
        {
            string text = GetUniqueKeyCHARECTER(0);
            string Number = GetUniqueNUMBER(24);
            string Message = text + Number;
            return Message;
        }

        public static string GetUniqueKeyCHARECTER(int maxSize)
        {
            // Main();
            //string s = Main();
            char[] chars = new char[62];
            chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[maxSize];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        public static string GetUniqueNUMBER(int maxSize)
        {
            // Main();
            //string s = Main();
            char[] chars = new char[62];
            chars = "1234567890".ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[maxSize];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        Dictionary<string, string> Months = new Dictionary<string, string>()
         {
                { "JANUARY", "1"},
                { "FEBRUARY", "2"},
                { "MARCH", "3"},
                { "APRIL", "4"},
                { "MAY", "5"},
                { "JUNE", "6"},
                { "JULY", "7"},
                { "AUGUST", "8"},
                { "SEPTEMBER", "9"},
                { "OCTOBER", "10"},
                { "NOVEMBER", "11"},
                { "DECEMBER", "12"},
         };
    }
    public class WATERREQ
    {
        public List<string> LedgerEntryId { get; set; }
        public string AutharizationNo { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PenaltyAmount { get; set; }
        public string bfs_bfsTxnId { get; set; }
        public int bankId { get; set; }
        public string accountNo { get; set; }
        public string orderNo { get; set; }
    }
    public class BTRESPONSE
    {
        public string Status;
        public string Reqtype;
        public string Respid;
        public string Reqid;
        public string Reqcode;
        public string Datetime;

    }
    public class WaterResponse
    {
        public string CID { get; set; }
        public string TaxpayerCode { get; set; }
        public string tpn { get; set; }
        public string name { get; set; }
        public string TaxName { get; set; }
        public string cAddress { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string NewCID { get; set; }
        public string TaxID { get; set; }
        public string AdjustmentAmount { get; set; }
        public string sumTotalAmount { get; set; }
        public string ExemptionAmount { get; set; }
        public string PenaltyAmount { get; set; }
        public string TransactionId { get; set; }
        public string DemandNoticeNo { get; set; }
        public string TaxPayerId { get; set; }
        public string TaxId { get; set; }
        public string DemandAmount { get; set; }
        public string TaxPayerName { get; set; }
        public string TotalAmount { get; set; }
        public string IsPaymentMade { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentAmount { get; set; }
        public string DepositedOn { get; set; }
        public string AutharizationNo { get; set; }
        public string sts { get; set; }
        public string LedgerEntryID { get; set; }
        public string TransactionTypeID { get; set; }
        public string fyrId { get; set; }
        public string demandDate { get; set; }
        public string PenaltyAmountList { get; set; }
        public string LedgerEntryId { get; set; }

        public string bfs_bfsTxnId { get; set; }
        public string orderNo { get; set; }
        public string buildingId { get; set; }
        public string unitId { get; set; }
        public string landId { get; set; }
        public string ModifiedReasonID { get; set; }
        public string bankId { get; set; }
        public string receiptNo { get; set; }
        public string accountNo { get; set; }
        public string billMonth { get; set; }
        public string days { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string receipt { get; set; }
        public string calYear { get; set; }
        public string BuildingTypeID { get; set; }
        public string cid { get; set; }
        public string taxPayerId { get; set; }
        public string ledgerEntryID { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string totalAmount { get; set; }
        public string paymentMade { get; set; }
        public string paymentMode { get; set; }
        public string depositedOn { get; set; }
        public string penaltyAmount { get; set; }
        public string modifiedReasonID { get; set; }
        public string autharizationNo { get; set; }
        public string ledgerEntryId { get; set; }
        public string transactionId { get; set; }
        public string demandNoticeNo { get; set; }
        public string buildingTypeID { get; set; }
        public string taxName { get; set; }
        public string taxpayerCode { get; set; }
        public string taxId { get; set; }
        public string taxID { get; set; }
        public string transactionTypeID { get; set; }
        public string adjustmentAmount { get; set; }
        public string exemptionAmount { get; set; }
        public string penaltyAmountList { get; set; }
        public string email { get; set; }
        public string newCID { get; set; }
        public string mobileNo { get; set; }
        public string demandAmount { get; set; }
        public string taxPayerName { get; set; }
    }

    //////// BT Recharge & Bill Payment
    public class BTPOSTPAIDREQ
    {
        public BTPOSTPAIDDETAIL MFS { get; set; }
    }
    public class BTPOSTPAIDDETAIL
    {
        public string OPERATION { get; set; }
        public int REQTYPE { get; set; }
        public string REFID { get; set; }
        public string SESSIONID { get; set; }
        public string REQTIME { get; set; }
        public string SRCMSISDN { get; set; }
        public int SERVICEID { get; set; }
        public string AMOUNT { get; set; }
        public string SRCMPIN { get; set; }
        public string SERVICENO { get; set; }
        public string INCLUDEACCOUNTDETAILS { get; set; }
        public string INCLUDEBILLDETAILS { get; set; }
        public string DESCRIPTION { get; set; }

        public string TRANSFERTYPE { get; set; }
        public string COMMENT { get; set; }
        public SERVICEDETAILSPARAM SERVICEDETAILS { get; set; }
        public WALLETDETAILS WALLETDETAILS { get; set; }
        public string DESTNOTIFYNO { get; set; }
        public string DESTNOTIFYLAN { get; set; }
        public string TXREFID { get; set; }
        public string USID { get; set; }
        
    }
    public class BTPOSTPAIDRESPHEADER
    {
        public BTPOSTPAIDRESP MFS { get; set; }
    }
    public class BTPREPAIDRESPHEADER
    {
        public MFS MFS { get; set; }
    }
    public class MFS
    {

        public string RESPTIME { get; set; }
        public string OPERATION { get; set; }
        public int RESPTYPE { get; set; }
        public string TXID { get; set; }
        public string VALIDATION { get; set; }
        public RESPONSE RESPONSE { get; set; }
        public BILLDETAILS BILLDETAILS { get; set; }
        public string REFID { get; set; }
    }
    public class RESPONSE
    {
        public string RESPDESC { get; set; }
        public string RESPCODE { get; set; }
    }

    public class BTPOSTPAIDRESP
    {
        public string RESPTIME;
        public string OPERATION;
        public string RESPTYPE;
        public string TXID;
        public RESPONCECODEBT RESPONSE { get; set; }
        public BILLDETAILS BILLDETAILS { get; set; }
        public string REFID { get; set; }
    }
    public class RESPONCECODEBT
    {
        public string RESPDESC { get; set; }
        public string RESPCODE { get; set; }
    }
    public class BILLDETAILS
    {
        public BILLSUMMARY BILLSUMMARY { get; set; }
        public List<PACKAGEDETAILS> PACKAGEDETAILS { get; set; }
    }
    public class PACKAGEDETAILS
    {
        public string PACKAGEID { get; set; }
        public string PACKAGENAME { get; set; }
        public string PACKAGEAMOUNT { get; set; }
        public string PACKAGECOMMENT { get; set; }

    }
    public class BILLSUMMARY
    {
        public string OUTSTANDING { get; set; }
        public string DUEDATE { get; set; }
        public string SESSIONSERVICEID { get; set; }
        public string PROVISIONDET { get; set; }
        public List<BILLDETAILSFINAL> BILLDETAILS { get; set; }
    }

    public class BILLDETAILSFINAL
    {
        public string BILLDATE { get; set; }
        public string BILLNO { get; set; }
        public string DUEDATE { get; set; }
        public string BILLAMOUNT { get; set; }
        public string BILLOUTSTANDING { get; set; }
    }
    public class SERVICEDETAILSPARAM
    {
        public string SERVICEREFID;
        public string SERVICENO;
        public string ACCOUNTNO;
        public string BILLNO;
        public string USID { get; set; }
        public string PPTYPE { get; set; }
        public string SERVICETYPE { get; set; }
    }
    public class WALLETDETAILS
    {
        public string WALLETTYPE{ get; set; }
        public string DESTMSISDN{ get; set; }
        public string DESTMPIN{ get; set; }
        public string TRANSTYPE { get; set; }
       
    }
    public class BTConfigurationData
    {
        public const string SourceNumber = "17001094"; //prod//"17001094";//uat//"17566666";
        public const string SourceNumberLeaseLine = "17001094";//"17566666";
        public const string SourceNumberBngul = "17566666";  //live 17001198 UAT 17566666
        public const string PIN = "01";
        public const string SuppliedParameteresWrong = "04";
        public const string InvalidNationalID = "08";
        public const string BranchIsNotOperational = "12";
        public const string TxnDateIsNotAllined = "13";
        public const string InsuficiantFund = "17";
        public const string Unabletopeocess = "99";
        public const string TPIN = "9880"; //2688
        public const string LeasePIN = "";
        public const string BngulPIN = ""; //Live 5200 UAT 2688
        //public const string BTEloadUsername = "tayana";// uat
        //public const string BTEloadPassword = "tayana@1021";//uat
        public const string BTEloadUsername = "dpnb";// prod
        public const string BTEloadPassword = "dpnb@10952021";//prod

    }
}