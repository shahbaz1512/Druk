using MaxiSwitch.DALC.ConsumerTransactions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Script.Serialization;

namespace IMPSTransactionRouter.Models
{
    public class TashiCellToken
    {
        CommanDetails _CommanDetails = new CommanDetails();
        JavaScriptSerializer JS = new JavaScriptSerializer();
        public DataTable TCellToken( ref TCELLAPI_RESP _TcellResp , TCELLAPI_REQ _TcellReq)
        {
            DataTable TashiToken = null;
            TashiToken = IMPSTransactions.GetTashiToken();
            if (TashiToken != null && TashiToken.Rows.Count > 0)
            {
                _TcellResp.AccessToken = TashiToken.Rows[0][1].ToString();
                _TcellResp.RefreshToken = TashiToken.Rows[0][2].ToString();
            }
            else
            {
                GenerateTashiCellToken(ref _TcellResp, _TcellReq);
            }

            return TashiToken;
        }

        public void GenerateTashiCellToken(ref TCELLAPI_RESP _TcellResp, TCELLAPI_REQ _TcellReq)
        {

            _TcellReq.userName = ConfigurationManager.AppSettings["TAPIUser"].ToString();
            _TcellReq.password = ConfigurationManager.AppSettings["TAPIPWD"].ToString();
            string TCELLAPILink = ConfigurationManager.AppSettings["TCELLGenarateToken_URL"].ToString();
            HttpClient clientSave = new HttpClient();
            clientSave.BaseAddress = new Uri(TCELLAPILink);
            clientSave.DefaultRequestHeaders.Accept.Clear();
            clientSave.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //// Live Details
            string EmpSaveResponse = string.Empty;
            var Saveresponse = clientSave.PostAsJsonAsync("", _TcellReq).Result;
            string TcellAPIResp = Saveresponse.Content.ReadAsStringAsync().Result;

            try
            {

                if (string.IsNullOrEmpty(TcellAPIResp))
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, "Response Does Not Received From TCELL " + TcellAPIResp);
                    return ;
                }
                else
                {
                    _CommanDetails.SystemLogger.WriteTransLog(this, "Response  Received From TCELL " + TcellAPIResp);
                    return ;
                }
            }
            catch (Exception ex)
            {
                _CommanDetails.SystemLogger.WriteErrorLog(this, ex);
                return ;
            }

           
        }
    }
}