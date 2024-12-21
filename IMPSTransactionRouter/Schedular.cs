using System.Timers;
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

using DbNetLink.Data;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using System.Net;
using Newtonsoft.Json.Linq;
using Microsoft.Web.Administration;
using Google.Apis.Auth.OAuth2;

namespace IMPSTransactionRouter
{
    public class Schedular
    {

        public Schedular()
        {
            CommanDetails _CommanDetails = new CommanDetails();
            _CommanDetails.SystemLogger.WriteTransLog(null, "Scheduler Constructed On. ");

            System.Timers.Timer tmrSchedulePayment = null;
            tmrSchedulePayment = new System.Timers.Timer(60000);
            tmrSchedulePayment.Elapsed += new System.Timers.ElapsedEventHandler(_SchedulerPaymentTimer_Elapsed);
            tmrSchedulePayment.Start();
            //  Console.ReadLine();
            // for refresh token
            System.Timers.Timer tmrRefreshToken = null;
            tmrRefreshToken = new System.Timers.Timer(60000);
            tmrRefreshToken.Elapsed += new System.Timers.ElapsedEventHandler(_RefreshTokenTimer_Elapsed);
            tmrRefreshToken.Start();

            // soundbox token

            System.Timers.Timer tmrsoundboxRefreshToken = null;
            tmrsoundboxRefreshToken = new System.Timers.Timer(60000);
            tmrsoundboxRefreshToken.Elapsed += new System.Timers.ElapsedEventHandler(_RefreshsoundboxTokenTimer_Elapsed);
            tmrsoundboxRefreshToken.Start();


            // soundbox token

            System.Timers.Timer ApplicationPoolRefreshToken = null;
            ApplicationPoolRefreshToken = new System.Timers.Timer(20000);
            ApplicationPoolRefreshToken.Elapsed += new System.Timers.ElapsedEventHandler(_RefreshPoolTimer_Elapsed);
            ApplicationPoolRefreshToken.Start();


            // for firebase refresh token
            System.Timers.Timer tmrFBRefreshToken = null;
            tmrFBRefreshToken = new System.Timers.Timer(60000);
            tmrFBRefreshToken.Elapsed += new System.Timers.ElapsedEventHandler(_RefreshFBTokenTimer_Elapsed);
            tmrFBRefreshToken.Start();

        }

        public static void _SchedulerPaymentTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CommanDetails _CommanDetails = new CommanDetails();
            try
            {
                string _CurrentTime = String.Format("{0:hh\\:mm tt}", DateTime.Now);
                DateTime now = DateTime.Now;
                if ((_CurrentTime == Convert.ToString(ConfigurationManager.AppSettings["ScheduerPaymentTimer"])))
                {
                    _CommanDetails.SystemLogger.WriteTransLog(null, "Scheduler time started at : " + _CurrentTime.ToString());
                    SchedulerPayment();

                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(null, "Exception occured at _SchedulerPaymentTimer_Elapsed  " + ex.ToString());
            }
        }

        public static void SchedulerPayment()
        {
            MOBILEBANKING_REQ _MOBILEBANKING_REQ = new MOBILEBANKING_REQ();
            MOBILEBANKING_RESP _MOBILEBANKING_RESP = new MOBILEBANKING_RESP();
            DataTable dsSchedulePayment = new DataTable();
            CommanDetails _CommanDetails = new CommanDetails();
            ProcessMessage _ProcessMessage = new ProcessMessage();

            string FileName = string.Empty;
            string SummFileName = string.Empty;
            try
            {
                dsSchedulePayment = IMPSTransactions.GetSchedulePayment();
                if (dsSchedulePayment.Rows.Count > 0)
                {
                    for (int j = 0; j < dsSchedulePayment.Rows.Count; j++)
                    {
                        _MOBILEBANKING_REQ.REMITTERACC = dsSchedulePayment.Rows[j]["FromAccountNumber"].ToString();
                        _MOBILEBANKING_REQ.BENIFICIARYACC = dsSchedulePayment.Rows[j]["ToAccountNumber"].ToString();
                        _MOBILEBANKING_REQ.MobileNumber = dsSchedulePayment.Rows[j]["MobileNumber"].ToString();
                        _MOBILEBANKING_REQ.DeviceID = dsSchedulePayment.Rows[j]["DeviceId"].ToString();
                        _MOBILEBANKING_REQ.TXNAMT = Convert.ToDecimal(dsSchedulePayment.Rows[j]["Amount"]);
                        _MOBILEBANKING_REQ.ReferenceNumber = dsSchedulePayment.Rows[j]["ReferenceNumber"].ToString();
                        string bankid = dsSchedulePayment.Rows[j]["BankCode"].ToString();
                        _MOBILEBANKING_REQ.TransType = dsSchedulePayment.Rows[j]["TransactionType"].ToString();
                        _MOBILEBANKING_REQ.IsAccountFT = true;

                        if (bankid == ConfigurationManager.AppSettings["BankCode"].ToString())
                        {
                            _ProcessMessage.TransactionIntraFundTransforMobile((int)Models.enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 1);
                            _ProcessMessage.ProcessFundTransfer(ref _MOBILEBANKING_RESP, _MOBILEBANKING_REQ);
                        }
                        else
                        {
                            _ProcessMessage.TransactionOutwardFundTransfer((int)Models.enumCommandTypeEnum.AuthorizationRequestMessage, _MOBILEBANKING_REQ, _MOBILEBANKING_RESP, 1);
                            _ProcessMessage.ProcessOutwardTransaction(ref _MOBILEBANKING_RESP, _MOBILEBANKING_REQ);
                        }
                        IMPSTransactions.UpdateScheduleTxnDetails(_MOBILEBANKING_REQ.BENIFICIARYACC, _MOBILEBANKING_REQ.ReferenceNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(null, "SchedulerPayment() " + ex.ToString() + ex.Message.ToString());
            }
        }

        public void processfundTransfer()
        {
            #region old
            /*  MOBILEBANKING_REQ request = new MOBILEBANKING_REQ
              {
                  ReferenceNumber = strReferenceNumber,
                  REMITTERACC = strREMITTERACC,
                  BENIFICIARYACC = strBENIFICIARYACC,
                  TXNAMT = Convert.ToDecimal(strAmount),
                  UNDPBankCode = strBankCode,
                  ProcessType = strProcessType,
              };
              string json = URI.PostJson(Convert.ToString(ConfigurationManager.AppSettings["IntraFundTransfer"]), request);
              MOBILEBANKING_RESP objRegUser = new MOBILEBANKING_RESP();



              using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
              {
                  DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(MOBILEBANKING_RESP));
                  objRegUser = (MOBILEBANKING_RESP)deserializer.ReadObject(ms);
              }
              if (objRegUser != null)
              {
                  if (objRegUser.ResponseCode == CommanDetails.GetResponseCodeHost(ConstResponseCode.Approved))
                  {
                      MobilePortalProcess.UNDPProcessed(request.ReferenceNumber, Convert.ToString(request.TXNAMT), request.REMITTERACC, request.BENIFICIARYACC);
                  }
                  else
                  {
                      MobilePortalProcess.UNDPRetry(request.ReferenceNumber, Convert.ToString(request.TXNAMT), request.REMITTERACC, request.BENIFICIARYACC);
                  }
              }*/
            #endregion



        }

        public class reqImpsReport
        {
            public string ACCOUNTNUMBER { get; set; }
            public string MOBILENUMBER { get; set; }
            public string MSGID { get; set; }
            public string CollerID { get; set; }
            public string FromAccount { get; set; }
            public string ToAccount { get; set; }
            public string ResponseCode { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public string TxnType { get; set; }

            public string BankCode { get; set; }
            public string Status { get; set; }
            public string ReferenceNumber { get; set; }
            public string LogedInUserID { get; set; }
            public string ShowID { get; set; }
            public string ContestantID { get; set; }
            public string flag { get; set; }
            public string ShowName { get; set; }
            public string IsRemoved { get; set; }
        }

        #region ScheduleTokenRefreshAPI

        public void _RefreshTokenTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CommanDetails _CommanDetails = new CommanDetails();
            try
            {
                string _CurrentTime = String.Format("{0:hh\\:mm tt}", DateTime.Now);
                DateTime now = DateTime.Now;
                DataTable TashiToken = null;
                TashiToken = IMPSTransactions.GetTashiToken();
                if (TashiToken == null || TashiToken.Rows.Count <= 0)
                {
                    RefreshToken();
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(null, "Exception occured at _RefreshTokenTimer_Elapsed  " + ex.ToString());
            }
        }




        public void RefreshToken()
        {
            CommanDetails _CommanDetails = new CommanDetails();
            TCELLAPI_REQ _TcellReq = new TCELLAPI_REQ();
            TCELLAPI_RESP _TcellResp = new TCELLAPI_RESP();
            JavaScriptSerializer JS = new JavaScriptSerializer();
            try
            {
                _TcellReq.userName = ConfigurationManager.AppSettings["TAPIUser"].ToString();
                _TcellReq.password = ConfigurationManager.AppSettings["TAPIPWD"].ToString();
                string TCELLAPILink = ConfigurationManager.AppSettings["TCELLGenarateToken_URL"].ToString();
                HttpClient clientSave = new HttpClient();
                clientSave.BaseAddress = new Uri(TCELLAPILink);
                clientSave.DefaultRequestHeaders.Accept.Clear();
                clientSave.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _CommanDetails.SystemLogger.WriteTransLog(this, "_TcellReq.userName " + _TcellReq.userName);
                _CommanDetails.SystemLogger.WriteTransLog(this, "_TcellReq.userName " + _TcellReq.password);
                //// Live Details
                var Saveresponse = clientSave.PostAsJsonAsync("", _TcellReq).Result;
                string TcellAPIResp = Saveresponse.Content.ReadAsStringAsync().Result;

                try
                {
                    if (string.IsNullOrEmpty(TcellAPIResp))
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(this, "Response Does Not Received From TCELL " + TcellAPIResp);
                        return;
                    }
                    else
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(this, "Response  Received From TCELL " + TcellAPIResp);
                        Dictionary<string, string> Response = JS.Deserialize<Dictionary<string, string>>(TcellAPIResp);
                        _TcellResp.AccessToken = Response["accesToken"];
                        _TcellResp.RefreshToken = Response["refreshToken"];
                        _CommanDetails.SystemLogger.WriteTransLog(this, "_TcellResp.AccessToken " + _TcellResp.AccessToken);
                        _CommanDetails.SystemLogger.WriteTransLog(this, "_TcellResp.RefreshToken " + _TcellResp.RefreshToken);
                        #region InsertUpdateTokens
                        IMPSTransactions.InsertUpdateTcellTokens(_TcellResp.AccessToken, _TcellResp.RefreshToken);
                        #endregion InsertUpdateTokens
                        return;
                    }
                }
                catch (Exception ex)
                {
                    _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
                    return;
                }

            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(null, "RefreshToken() " + ex.ToString() + ex.Message.ToString());
            }
        }

        #endregion ScheduleTokenRefreshAPI

        #region SchedulesoundboxTokenRefreshAPI

        public void _RefreshsoundboxTokenTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CommanDetails _CommanDetails = new CommanDetails();
            try
            {
                string _CurrentTime = String.Format("{0:hh\\:mm tt}", DateTime.Now);
                DateTime now = DateTime.Now;
                DataTable soundboxtoken = null;
                soundboxtoken = IMPSTransactions.GetsoundboxToken();
                if (soundboxtoken == null || soundboxtoken.Rows.Count <= 0)
                {
                    RefreshsoundboxToken();
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(null, "Exception occured at _RefreshTokenTimer_Elapsed  " + ex.ToString());
            }
        }

        public void RefreshsoundboxToken()
        {
            CommanDetails _CommanDetails = new CommanDetails();
            SOUNDBOX_REQ _SoundboxReq = new SOUNDBOX_REQ();
            SOUNDBOX_RESP _SoundboxResp = new SOUNDBOX_RESP();
            JavaScriptSerializer JS = new JavaScriptSerializer();
            string Token = string.Empty;
            string RefreshToken = string.Empty;
            try
            {
                _SoundboxReq.USERNAME = CONFIGURATIONCONFIGDATA.QSUsername;
                _SoundboxReq.PASSWORD = CONFIGURATIONCONFIGDATA.QSPassword;
                string SoundAPILink = ConfigurationManager.AppSettings["SoundboxGenarateToken_URL"].ToString();
                var httpWebRequest_Auth = (HttpWebRequest)WebRequest.Create(SoundAPILink);
                httpWebRequest_Auth.ContentType = "application/x-amz-json-1.1";
                httpWebRequest_Auth.ServicePoint.Expect100Continue = false;
                httpWebRequest_Auth.Method = "POST";
                httpWebRequest_Auth.Headers.Add("X-Amz-Target", "AWSCognitoIdentityProviderService.InitiateAuth");
                httpWebRequest_Auth.PreAuthenticate = true;
                _CommanDetails.SystemLogger.WriteTransLog(this, "_SoundboxReq.userName " + _SoundboxReq.USERNAME);
                _CommanDetails.SystemLogger.WriteTransLog(this, "_SoundboxReq.userName " + _SoundboxReq.PASSWORD);
                //// Live Details
                string json_Auth = string.Empty;
                var result_Auth = "";
                using (var streamWriter = new StreamWriter(httpWebRequest_Auth.GetRequestStream()))
                {

                    json_Auth = "{\"AuthParameters\":{\"USERNAME\":\"" + CONFIGURATIONCONFIGDATA.QSUsername + "\"," +
                                "\"PASSWORD\":\"" + CONFIGURATIONCONFIGDATA.QSPassword + "\"}," +
                                "\"AuthFlow\":\"USER_PASSWORD_AUTH\"," +
                                "\"ClientId\":\"" + CONFIGURATIONCONFIGDATA.QSSecretKey + "\"}";
                    streamWriter.Write(json_Auth);
                    _CommanDetails.SystemLogger.WriteTransLog(this, "Json :" + json_Auth);
                }
                var httpResponse_Auth = (HttpWebResponse)httpWebRequest_Auth.GetResponse();
                SoundBox _SoundBoxResponse = new SoundBox();
                using (var streamReader = new StreamReader(httpResponse_Auth.GetResponseStream()))
                {
                    result_Auth = streamReader.ReadToEnd();
                    _CommanDetails.SystemLogger.WriteTransLog(this, "Json Response Received :" + result_Auth);
                    if (httpResponse_Auth.StatusCode.ToString() == "OK")
                    {
                        _SoundBoxResponse = JsonConvert.DeserializeObject<SoundBox>(result_Auth);
                        _SoundboxResp.AccessToken = _SoundBoxResponse.AuthenticationResult.IdToken;
                        _SoundboxResp.RefreshToken = _SoundBoxResponse.AuthenticationResult.RefreshToken;
                        var userObj = JObject.Parse(result_Auth);
                        #region InsertUpdateTokens
                        IMPSTransactions.InsertUpdatesoundboxTokens(_SoundboxResp.AccessToken, _SoundboxResp.RefreshToken);
                        #endregion InsertUpdateTokens
                    }
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(null, "RefreshToken() " + ex.ToString() + ex.Message.ToString());
            }
        }

        #endregion ScheduleTokenRefreshAPI

        #region ScheduleApplicationPool


        public void _RefreshPoolTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CommanDetails _CommanDetails = new CommanDetails();
            try
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "_RefreshPoolTimer_Elapsed: ");
                using (ServerManager serverManager = new ServerManager())
                {
                    string appPoolName = ConfigurationManager.AppSettings["appPoolName"];
                    ApplicationPool appPool = serverManager.ApplicationPools[appPoolName];
                    if (appPool != null)
                    {
                        Console.WriteLine("Refreshing application pool: " + appPoolName);
                        _CommanDetails.SystemLogger.WriteTransLog(this, "Refreshing application pool: ");
                         appPool.Recycle();
                        _CommanDetails.SystemLogger.WriteTransLog(this, "Application pool Refreshed Successsfully: ");
                    }
                    else
                    {
                        _CommanDetails.SystemLogger.WriteTransLog(this, "Application pool not found: ");
                    }
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(this, "Error In pool timer elapsed: " + ex);
            }
        }


        #endregion ScheduleApplicationPool

        #region RefreshFireBaseToken

        public void _RefreshFBTokenTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CommanDetails _CommanDetails = new CommanDetails();
            try
            {
                string _CurrentTime = String.Format("{0:hh\\:mm tt}", DateTime.Now);
                DateTime now = DateTime.Now;
                DataTable FbToken = null;
                FbToken = IMPSTransactions.GetTashiToken();
                if (FbToken == null || FbToken.Rows.Count <= 0)
                {
                    RefreshFBTokenIOS();
                    RefreshFBTokenAndroid();
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(null, "Exception occured at _RefreshFBTokenTimer_Elapsed  " + ex.ToString());
            }
        }
        public void RefreshFBTokenAndroid()
        {
            CommanDetails _CommanDetails = new CommanDetails();
            try
            {
                string path = ConfigurationManager.AppSettings["ANDROIDJSON"].ToString();
                string fileName = System.Web.Hosting.HostingEnvironment.MapPath(path);
                string scopes = ConfigurationManager.AppSettings["TOKENURL"].ToString();
                var bearertoken = ""; // Bearer Token in this variable
                using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    bearertoken = GoogleCredential
                      .FromStream(stream) // Loads key file
                      .CreateScoped(scopes) // Gathers scopes requested
                      .UnderlyingCredential // Gets the credentials
                      .GetAccessTokenForRequestAsync().Result; // Gets the Access Token
                }
                #region InsertUpdateTokens
                IMPSTransactions.InsertUpdateFbTokens(bearertoken, bearertoken,"ANDROID");
                #endregion InsertUpdateTokens

                _CommanDetails.SystemLogger.WriteTransLog(this, "bearertokenAndroid : " + bearertoken);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(null, "RefreshFBTokenAndroid() " + ex.ToString() + ex.Message.ToString());
            }
        }
        public void RefreshFBTokenIOS()
        {
            CommanDetails _CommanDetails = new CommanDetails();
            try
            {
                string path = ConfigurationManager.AppSettings["IOSJSON"].ToString();
                string fileName = System.Web.Hosting.HostingEnvironment.MapPath(path);

                string scopes = ConfigurationManager.AppSettings["TOKENURL"].ToString();//https://www.googleapis.com/auth/firebase.messaging";
                var bearertoken = ""; // Bearer Token in this variable
                using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    bearertoken = GoogleCredential
                      .FromStream(stream) // Loads key file
                      .CreateScoped(scopes) // Gathers scopes requested
                      .UnderlyingCredential // Gets the credentials
                      .GetAccessTokenForRequestAsync().Result; // Gets the Access Token
                }
                #region InsertUpdateTokens
                IMPSTransactions.InsertUpdateFbTokens(bearertoken, bearertoken, "IOS");
                #endregion InsertUpdateTokens

                _CommanDetails.SystemLogger.WriteTransLog(this, "bearertokenIOS : " + bearertoken);
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteTransLog(null, "RefreshFBTokenIOS() " + ex.ToString() + ex.Message.ToString());
            }
        }


        #endregion RefreshFireBaseToken

    }
}
